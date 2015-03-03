using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using HelloBotCore.Entities;
using HelloBotCore.Utilities;
using HelloBotCommunication;

namespace HelloBotCore
{
    internal class SystemCommandInfo
    {
        public string Description { get; set; }
        public Func<string> Callback { get; set; }

        public SystemCommandInfo(string description, Func<string> callback)
        {
            Description = description;
            Callback = callback;
        }
    }

    public class HelloBot
    {
        private List<ModuleActionHandler> _handlers = new List<ModuleActionHandler>();
        private readonly IDictionary<string, SystemCommandInfo> _systemCommands;
        private readonly string _moduleDllmask;
        private readonly string _botCommandPrefix;
        private readonly int _commandTimeoutSec;

        /// <summary>
        /// Bot costructor
        /// </summary>
        /// <param name="dllMask">File mask for retrieving client command dlls</param>
        /// <param name="botCommandPrefix">Prefix for bot commands. Only messages with that prefix will be handled</param>
        public HelloBot(string moduleDllmask = "*.dll", string botCommandPrefix = "!")
        {
            this._moduleDllmask = moduleDllmask;
            this._botCommandPrefix = botCommandPrefix;
            this._commandTimeoutSec = 30;

            _systemCommands = new Dictionary<string, SystemCommandInfo>()
            {
                {"help", new SystemCommandInfo("список системных команд", GetSystemCommands)},
                {"modules", new SystemCommandInfo("список кастомных модулей", GetUserDefinedCommands)},
            };
            RegisterModules();
        }

        private void RegisterModules()
        {
            var handlers = GetHandlers();
            handlers = ExtendAliases(handlers).ToList();//extend aliases for autocomplete wrong keyboard layout search
            _handlers = handlers;
        }

        private List<ModuleActionHandler> ExtendAliases(List<ModuleActionHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                foreach (var command in handler.CallCommandList)
                {
                    List<string> tAliases = new List<string>();
                    foreach (string tAlias in command.Aliases)
                    {
                        tAliases.AddRange(tAlias.GetOtherKeyboardLayoutWords(tAlias.DetectLanguage()));
                    }
                    command.Aliases.AddRange(tAliases);
                    command.Aliases.AddRange(command.Command.GetOtherKeyboardLayoutWords(command.Command.DetectLanguage()));
                }
            }
            
            return handlers;
        }

        protected virtual List<ModuleActionHandler> GetHandlers()
        {
            List<ModuleActionHandler> toReturn = new List<ModuleActionHandler>();
            var dlls = Directory.GetFiles(".", _moduleDllmask);
            var i = typeof(IActionHandlerRegister);
            foreach (var dll in dlls)
            {
                var ass = Assembly.LoadFile(Environment.CurrentDirectory + dll);

                //get types from assembly
                var typesInAssembly = ass.GetTypes().Where(i.IsAssignableFrom).ToList();

                foreach (Type type in typesInAssembly)
                {
                    object obj = Activator.CreateInstance(type);
                    var clientHandlers = ((IActionHandlerRegister)obj).GetHandlers();
                    toReturn.AddRange(from handler in clientHandlers where handler.CallCommandList.Any() select new ModuleActionHandler(handler));
                }
            }
            
            return toReturn;
        }
        
        public void HandleMessage(string incomingMessage, Action<AnswerInfo> answerCallback, object data)
        {
            if (incomingMessage.Contains(_botCommandPrefix))
            {
                var command = incomingMessage.Substring(incomingMessage.IndexOf(_botCommandPrefix, StringComparison.InvariantCulture) + _botCommandPrefix.Length);
                if (!string.IsNullOrEmpty(command))
                {
                    
                    var systemCommandList = _systemCommands.Where(x => x.Key.ToLower() == command.ToLower()).ToList();
                    if (systemCommandList.Any())
                    {
                        var systemComand = systemCommandList.First();
                        answerCallback(new AnswerInfo()
                        {
                            Answer = systemComand.Value.Callback(),
                            Type = AnswerBehaviourType.ShowText
                        });
                    }
                    else
                    {

                        ModuleActionHandler handler = FindHandler(command, out command);
                        if (handler != null)
                        {
                            string args = incomingMessage.Substring(incomingMessage.IndexOf(command, StringComparison.InvariantCultureIgnoreCase) + command.Length).Trim();

                            ModuleActionHandler hnd = handler;
                            new Thread(() => //running in separate thread
                            {
                                if (!RunWithTimeout(() => //check for timing
                                {
                                    try
                                    {
                                        hnd.HandleMessage(command, args, data, answerCallback);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex is ThreadAbortException))
                                        {
                                            if (OnErrorOccured != null)
                                            {
                                                OnErrorOccured(ex);
                                            }
                                            answerCallback(new AnswerInfo()
                                            {
                                                Answer = command + " сломался",
                                                Type = AnswerBehaviourType.ShowText
                                            });
                                        }
                                    }
                                }, TimeSpan.FromSeconds(_commandTimeoutSec)))
                                {
                                    answerCallback(new AnswerInfo()
                                    {
                                        Answer = command + " сломан (время на выполнение команды истекло)",
                                        Type = AnswerBehaviourType.ShowText
                                    });
                                }
                            }).Start();
                        }
                    }
                }
            }
        }
        
        static bool RunWithTimeout(ThreadStart threadStart, TimeSpan timeout)
        {
            Thread workerThread = new Thread(threadStart);
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();

            bool finished = workerThread.Join(timeout);
            if (!finished)
                workerThread.Abort();

            return finished;
        }
        public delegate void OnErrorOccuredDelegate(Exception ex);
        public event OnErrorOccuredDelegate OnErrorOccured;

        private ModuleActionHandler FindHandler(string phrase, out string command)
        {
            ModuleActionHandler toReturn = null;
            command = string.Empty;
            List<string> foundCommands = new List<string>();
            foreach (var actionHandler in _handlers)
            {
                foreach (var com in actionHandler.CallCommandList)
                {
                    if (phrase.StartsWith(com.Command, StringComparison.OrdinalIgnoreCase))
                    {
                        var args = phrase.Substring(com.Command.Length);
                        if (string.IsNullOrEmpty(args) || args.StartsWith(" "))
                        foundCommands.Add(com.Command);
                    }
                }
            }

            if (foundCommands.Any())
            {
                string foundCommand = foundCommands.OrderByDescending(x => x).First();
                toReturn = _handlers.FirstOrDefault(x => x.CallCommandList.Select(y=>y.Command).Contains(foundCommand,StringComparer.OrdinalIgnoreCase));
                if (toReturn != null)
                {
                    command = foundCommand;
                }
            }
            
            return toReturn;
        }

        private string GetSystemCommands()
        {
            return String.Join(Environment.NewLine, _systemCommands.Select(x => String.Format(_botCommandPrefix+"{0} - {1}", x.Key, x.Value.Description)).ToArray());
        }

        public List<string> GetUserDefinedCommandList()
        {
            List<string> toReturn = new List<string>();
            foreach (var commandList in _handlers.Select(x=>x.CallCommandList))
            {
                toReturn.AddRange(commandList.Select(x=>x.Command));
            }
            return toReturn;
        }
        private string GetUserDefinedCommands()
        {
            StringBuilder sb = new StringBuilder();
            var modules = _handlers.Select(x => String.Format("{0} - {1}", string.Join(" / ", x.CallCommandList.Select(y => _botCommandPrefix + y.Command).ToArray()), x.CommandDescription)).ToArray();
            sb.Append(String.Join(Environment.NewLine,modules));
            sb.AppendLine("");
            sb.AppendLine("Запили свой модуль : https://github.com/Nigrimmist/MonkeyJob-Tool");

            return sb.ToString();
        }

        public List<CallCommandInfo> FindCommands(string incCommand)
        {
            return (
                from handler in _handlers from cmd in handler.CallCommandList
                where cmd.Command.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase) ||
                cmd.Aliases.Any(y => y.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase)) 
                let descr = !string.IsNullOrEmpty(cmd.Description) ? cmd.Description : handler.CommandDescription
                select new CallCommandInfo(cmd.Command, descr)).ToList();
        }
    }

   
}
