using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using HelloBotCommunication;

namespace HelloBotCore
{
    internal class CommandInfo
    {
        public string Description { get; set; }
        public Func<string> Callback { get; set; }

        public CommandInfo(string description, Func<string> callback)
        {
            Description = description;
            Callback = callback;
        }
    }

    public class HelloBot
    {
        private  List<IActionHandler> handlers = new List<IActionHandler>();
        private IDictionary<string, CommandInfo> systemCommands;
        private string moduleDllmask { get; set; }
        private string botCommandPrefix;
        private int commandTimeoutSec;

        /// <summary>
        /// Bot costructor
        /// </summary>
        /// <param name="dllMask">File mask for retrieving client command dlls</param>
        /// <param name="botCommandPrefix">Prefix for bot commands. Only messages with that prefix will be handled</param>
        public HelloBot(string moduleDllmask = "*.dll", string botCommandPrefix = "!")
        {
            this.moduleDllmask = moduleDllmask;
            this.botCommandPrefix = botCommandPrefix;
            this.commandTimeoutSec = 30;

            systemCommands = new Dictionary<string, CommandInfo>()
            {
                {"help", new CommandInfo("список системных команд", GetSystemCommands)},
                {"modules", new CommandInfo("список кастомных модулей", GetUserDefinedCommands)},
            };
            RegisterModules();
        }

        private void RegisterModules()
        {
            handlers = GetHandlers();
        }

        protected virtual List<IActionHandler> GetHandlers()
        {
            List<IActionHandler> toReturn = new List<IActionHandler>();
            var dlls = Directory.GetFiles(".", moduleDllmask);
            var i = typeof(IActionHandlerRegister);
            foreach (var dll in dlls)
            {
                var ass = Assembly.LoadFile(Environment.CurrentDirectory + dll);

                //get types from assembly
                var typesInAssembly = ass.GetTypes().Where(x => i.IsAssignableFrom(x)).ToList();

                foreach (Type type in typesInAssembly)
                {
                    object obj = Activator.CreateInstance(type);
                    var clientHandlers = ((IActionHandlerRegister)obj).GetHandlers();
                    foreach (IActionHandler handler in clientHandlers)
                    {
                        if (handler.CallCommandList.Any())
                        {
                            toReturn.Add(handler);
                        }
                    }
                }
            }
            
            return toReturn;
        }
        
        public void HandleMessage(string incomingMessage, Action<string,AnswerBehaviourType> answerCallback, object data)
        {
            if (incomingMessage.Contains(botCommandPrefix))
            {
                var command = incomingMessage.Substring(incomingMessage.IndexOf(botCommandPrefix, StringComparison.InvariantCulture) + botCommandPrefix.Length);
                if (!string.IsNullOrEmpty(command))
                {
                    
                    var systemCommandList = systemCommands.Where(x => x.Key.ToLower() == command.ToLower()).ToList();
                    if (systemCommandList.Any())
                    {
                        var systemComand = systemCommandList.First();
                        answerCallback(systemComand.Value.Callback(), AnswerBehaviourType.ShowText);
                    }
                    else
                    {

                        IActionHandler handler = FindHandler(command, out command);
                        if (handler != null)
                        {
                            string args = incomingMessage.Substring(incomingMessage.IndexOf(command, StringComparison.InvariantCultureIgnoreCase) + command.Length).Trim();

                            IActionHandler hnd = handler;

                            if (!RunWithTimeout(() =>
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
                                        answerCallback(command + " сломан :(", AnswerBehaviourType.ShowText);
                                    }
                                }
                            }, TimeSpan.FromSeconds(commandTimeoutSec)))
                            {
                                answerCallback(command + " сломан (время на выполнение команды истекло)", AnswerBehaviourType.ShowText);
                            }

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
        public delegate void onErrorOccuredDelegate(Exception ex);
        public event onErrorOccuredDelegate OnErrorOccured;

        private IActionHandler FindHandler(string phrase, out string command)
        {
            IActionHandler toReturn = null;
            command = string.Empty;
            List<string> foundCommands = new List<string>();
            foreach (var actionHandler in handlers)
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
                toReturn = handlers.FirstOrDefault(x => x.CallCommandList.Select(y=>y.Command).Contains(foundCommand,StringComparer.OrdinalIgnoreCase));
                if (toReturn != null)
                {
                    command = foundCommand;
                }
            }
            
            return toReturn;
        }

        private string GetSystemCommands()
        {
            return String.Join(Environment.NewLine, systemCommands.Select(x => String.Format(botCommandPrefix+"{0} - {1}", x.Key, x.Value.Description)).ToArray());
        }

        public List<string> GetUserDefinedCommandList()
        {
            List<string> toReturn = new List<string>();
            foreach (var commandList in handlers.Select(x=>x.CallCommandList))
            {
                toReturn.AddRange(commandList.Select(x=>x.Command));
            }
            return toReturn;
        }
        private string GetUserDefinedCommands()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(String.Join(Environment.NewLine, handlers.Select(x => String.Format("{0} - {1}", string.Join(" / ", x.CallCommandList.Select(y => botCommandPrefix + y.Command).ToArray()), x.CommandDescription)).ToArray()));
            sb.AppendLine("");
            sb.AppendLine("Запили свой модуль : https://github.com/Nigrimmist/HelloBot");

            return sb.ToString();
        }
    }

   
}
