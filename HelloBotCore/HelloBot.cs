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
using HelloBotCommunication.Interfaces;
using Newtonsoft.Json;

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

    public class HelloBot : IModuleClientHandler
    {
        private List<ModuleCommandInfo> _modules = new List<ModuleCommandInfo>();
        private readonly IDictionary<string, SystemCommandInfo> _systemCommands;
        private readonly string _moduleDllmask;
        private readonly string _botCommandPrefix;
        private readonly string _moduleFolderPath;
        private readonly int _commandTimeoutSec;
        private Dictionary<Guid, object> _commandDictLocks;
        private string _settingsFolderAbsolutePath;
        private JsonSerializer _serializer;
        private Dictionary<Guid, BotCommandContext> _commandContexts;
        private object _commandContextLock = new object();
        public delegate void OnErrorOccuredDelegate(Exception ex);
        
        /// <param name="clientCommandContext">Can be null</param>
        public delegate void OnMessageRecievedDelegate(Guid commandToken,AnswerInfo answer,ClientCommandContext clientCommandContext);
        public event OnErrorOccuredDelegate OnErrorOccured;
        public event OnMessageRecievedDelegate OnMessageRecieved;


        /// <summary>
        /// Bot costructor
        /// </summary>
        /// <param name="settingsFolderAbsolutePath">folder for module settings, will be created if not exist</param>
        /// <param name="moduleDllmask">File mask for retrieving client command dlls</param>
        /// <param name="botCommandPrefix">Prefix for bot commands. Only messages with that prefix will be handled</param>
        public HelloBot(string settingsFolderAbsolutePath,string moduleDllmask = "*.dll", string botCommandPrefix = "!", string moduleFolderPath = ".")
        {
            _settingsFolderAbsolutePath = settingsFolderAbsolutePath;
            if (!Directory.Exists(settingsFolderAbsolutePath))
                Directory.CreateDirectory(settingsFolderAbsolutePath);

            _moduleDllmask = moduleDllmask;
            _botCommandPrefix = botCommandPrefix;
            _moduleFolderPath = moduleFolderPath;
            _commandTimeoutSec = 30;
            _commandDictLocks = new Dictionary<Guid, object>();
            _serializer = new JsonSerializer();
            _systemCommands = new Dictionary<string, SystemCommandInfo>()
            {
                {"?", new SystemCommandInfo("список системных команд", GetSystemCommands)},
                {"modules", new SystemCommandInfo("список модулей", GetUserDefinedCommands)},
            };
            _commandContexts = new Dictionary<Guid, BotCommandContext>();
            RegisterModules();
            RunEventModuleTimers();
        }

        private void RunEventModuleTimers()
        {
            var events = _modules.Where(x => x.ModuleType == ModuleType.Event).ToList();
            foreach (var ev in events)
            {
                var tEv = ev;
                new Thread(() =>
                {
                    Guid commandToken = Guid.NewGuid();
                    AddNewCommandContext(commandToken, new BotCommandContext()
                    {
                        CommandName = tEv.ModuleName
                    });

                    while (true)
                    {
                        try
                        {
                            tEv.CallEvent(commandToken);
                        }
                        catch (Exception ex)
                        {
                            //todo:log module exception
                        }
                        
                        Thread.Sleep(tEv.EventRunEvery);
                    }
                    

                }).Start();
            }
        }

        private void AddNewCommandContext(Guid commandToken, BotCommandContext botContext)
        {
            lock (_commandContextLock)
            {
                _commandContexts.Add(commandToken, botContext);
            }
        }

        private void RegisterModules()
        {
            var modules = GetModules();
            modules = ExtendAliases(modules).ToList();//extend aliases for autocomplete wrong keyboard layout search
            _modules = modules;
        }

        private List<ModuleCommandInfo> ExtendAliases(List<ModuleCommandInfo> modules)
        {
            foreach (var module in modules)
            {
                if (module.ModuleType == ModuleType.Handler)
                {
                    foreach (var command in module.CallCommandList)
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
            }
            
            return modules;
        }

        protected virtual List<ModuleCommandInfo> GetModules()
        {
            List<ModuleCommandInfo> toReturn = new List<ModuleCommandInfo>();
            var dlls = Directory.GetFiles(_moduleFolderPath, _moduleDllmask);
            var i = typeof(ModuleRegister);
            foreach (var dll in dlls)
            {
                var ass = Assembly.LoadFile(dll);
                var fi = new FileInfo(dll);
                //get types from assembly
                var typesInAssembly = ass.GetTypes().Where(type => i.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).ToList();

                foreach (Type type in typesInAssembly)
                {
                    object obj = Activator.CreateInstance(type);
                    var modules = ((ModuleRegister)obj).GetModules().Select(module =>
                    {
                        var tModule = new ModuleCommandInfo();
                        _commandDictLocks.Add(tModule.Id, new object());
                        tModule.InitModule(Path.GetFileNameWithoutExtension(fi.Name), module, this);
                        return tModule;
                    });

                    var events = ((ModuleRegister)obj).GetEvents().Select(ev =>
                    {
                        var tModule = new ModuleCommandInfo();
                        _commandDictLocks.Add(tModule.Id, new object());
                        tModule.InitEvent(Path.GetFileNameWithoutExtension(fi.Name), ev, this);
                        return tModule;
                    });

                    toReturn.AddRange(modules.Where(x=>x.CallCommandList.Any()));
                    toReturn.AddRange(events);
                }
            }
            
            return toReturn;
        }
        
        public bool HandleMessage(string incomingMessage, ClientCommandContext clientCommandContext)
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
                        ShowMessage(systemComand.Value.Callback(), systemComand.Key);
                        return true;
                    }
                    else
                    {
                        string args;
                        ModuleCommandInfo foundModule = FindModule(command, out command, out args);
                        if (foundModule != null)
                        {
                            ModuleCommandInfo hnd = foundModule;
                            new Thread(() => //running in separate thread
                            {
                                if (!RunWithTimeout(() => //check for timing
                                {
                                    try
                                    {
                                        var commandTempGuid = Guid.NewGuid();

                                        AddNewCommandContext(commandTempGuid, new BotCommandContext()
                                        {
                                            ClientCommandContext = clientCommandContext,
                                            CommandName = command
                                        });
                                        
                                        hnd.HandleMessage(command, args, commandTempGuid);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex is ThreadAbortException))
                                        {
                                            if (OnErrorOccured != null)
                                            {
                                                OnErrorOccured(ex);
                                            }
                                            ShowMessage(command,"модуль сломался");
                                        }
                                    }
                                }, TimeSpan.FromSeconds(_commandTimeoutSec)))
                                {
                                    ShowMessage(command,"модуль сломался. Причина : время на выполнение команды истекло");
                                }
                            }).Start();
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            return false;
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
        

        private ModuleCommandInfo FindModule(string phrase, out string command, out string args)
        {
            ModuleCommandInfo toReturn = null;
            command = string.Empty;
            List<string> foundCommands = new List<string>();
            args = string.Empty;
            foreach (var module in _modules)
            {
                foreach (var com in module.CallCommandList)
                {
                    if (phrase.StartsWith(com.Command, StringComparison.OrdinalIgnoreCase))
                    {
                        args = phrase.Substring(com.Command.Length);
                        if (string.IsNullOrEmpty(args) || args.StartsWith(" "))
                        foundCommands.Add(com.Command);
                    }
                }
            }
            if (!foundCommands.Any())
            {
                //trying search by aliases
                foreach (var module in _modules)
                {
                    foreach (var com in module.CallCommandList)
                    {
                        foreach (var alias in com.Aliases)
                        {
                            if (phrase.StartsWith(alias, StringComparison.OrdinalIgnoreCase))
                            {
                                args = phrase.Substring(alias.Length);
                                if (string.IsNullOrEmpty(args) || args.StartsWith(" "))
                                    foundCommands.Add(com.Command);
                            }
                        }
                    }
                }
            }
            if (foundCommands.Any())
            {
                string foundCommand = foundCommands.OrderByDescending(x => x).First();
                toReturn = _modules.FirstOrDefault(x => x.CallCommandList.Select(y=>y.Command).Contains(foundCommand,StringComparer.OrdinalIgnoreCase));
                if (toReturn != null)
                {
                    command = foundCommand;
                }
            }
            args = args.Trim();
            return toReturn;
        }

        private string GetSystemCommands()
        {
            return String.Join(Environment.NewLine, _systemCommands.Select(x => String.Format(_botCommandPrefix+"{0} - {1}", x.Key, x.Value.Description)).ToArray());
        }

        public List<string> GetUserDefinedCommandList()
        {
            List<string> toReturn = new List<string>();
            foreach (var commandList in _modules.Select(x=>x.CallCommandList))
            {
                toReturn.AddRange(commandList.Select(x=>x.Command));
            }
            return toReturn;
        }
        private string GetUserDefinedCommands()
        {
            StringBuilder sb = new StringBuilder();
            var modules = _modules.Select(x => String.Format("{0} - {1}", string.Join(" / ", x.CallCommandList.Select(y => _botCommandPrefix + y.Command).ToArray()), x.CommandDescription)).ToArray();
            sb.Append(String.Join(Environment.NewLine,modules));
            sb.AppendLine("");
            sb.AppendLine("Запили свой модуль : https://github.com/Nigrimmist/MonkeyJob-Tool");

            return sb.ToString();
        }

        public List<CallCommandInfo> FindCommands(string incCommand)
        {
            return (
                from module in _modules from cmd in module.CallCommandList
                where cmd.Command.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase) ||
                cmd.Aliases.Any(y => y.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase))
                let descr = !string.IsNullOrEmpty(cmd.Description) ? cmd.Description : module.CommandDescription
                select new CallCommandInfo(cmd.Command, descr)).ToList();
        }

        #region methods for modules
        
        public void SaveSettings<T>(ModuleCommandInfo commandInfo, T serializableSettingObject) where T : class
        {
            lock (_commandDictLocks[commandInfo.Id])
            {
                var settings = new ModuleSettings<T>(commandInfo.Version, serializableSettingObject);
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                string moduleFileName = commandInfo.ModuleName + ".json";
                File.WriteAllText(_settingsFolderAbsolutePath + "/" + moduleFileName, json);
            }
        }

        public T GetSettings<T>(ModuleCommandInfo commandInfo) where T : class
        {
            lock (_commandDictLocks[commandInfo.Id])
            {
                string moduleFileName = commandInfo.ModuleName + ".json";
                string fullPath = _settingsFolderAbsolutePath+"/" + moduleFileName;
                if (!File.Exists(fullPath)) return default(T);
                string data = File.ReadAllText(fullPath);
                var settings = JsonConvert.DeserializeObject<ModuleSettings<T>>(data);
                return settings.ModuleData;
            }
        }

        public void ShowMessage(Guid commandToken, ModuleCommandInfo commandInfo, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            BotCommandContext commandContext;
            //todo:refactoring required (should be cleared time to time)
            lock (_commandContexts)
            {
                if (_commandContexts.TryGetValue(commandToken, out commandContext))
                {
                    //_commandContexts.Remove(commandToken);
                }
            }
            //one command = one answer for now
            if (commandContext != null)
            {
                if (OnMessageRecieved != null)
                    OnMessageRecieved(commandToken,new AnswerInfo()
                    {
                        Answer = content,
                        Title = title,
                        CommandName = commandContext.CommandName,
                        AnswerType = answerType
                    },  commandContext.ClientCommandContext );
            }
        }
        private void ShowMessage(string commandname, string content, string title = null, ClientCommandContext clientCommandContext=null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            Guid systemGuid = Guid.Empty;
            if (OnMessageRecieved != null)
                OnMessageRecieved(systemGuid, new AnswerInfo()
                {
                    Answer = content,
                    Title = title,
                    AnswerType = answerType,
                    CommandName = commandname
                },clientCommandContext);

        }
        

        public void RegisterUserReactionCallback(Guid commandToken,UserReactionToCommandType reactionType, Action reactionCallback)
        {
            BotCommandContext commandContext = GetCommandContextByToken(commandToken);

            if (commandContext != null)
            {
                switch (reactionType)
                {
                    case UserReactionToCommandType.Notified:
                        commandContext.OnNotifiedAction = reactionCallback;
                        break;
                    case UserReactionToCommandType.Ignored:
                        commandContext.OnIgnoreAction = reactionCallback;
                        break;
                    case UserReactionToCommandType.Clicked:
                        commandContext.OnClickAction = reactionCallback;
                        break;
                    
                }
                
            }
        }
        #endregion

        private BotCommandContext GetCommandContextByToken(Guid commandToken)
        {
            BotCommandContext commandContext;
            lock (_commandContexts)
            {
                _commandContexts.TryGetValue(commandToken, out commandContext);
            }
            return commandContext;
        }

        public void HandleUserReactionToCommand(Guid commandToken, UserReactionToCommandType reactionType)
        {
            BotCommandContext commandContext = GetCommandContextByToken(commandToken);

            if (commandContext != null)
            {
                switch (reactionType)
                {
                    case UserReactionToCommandType.Ignored:
                        if (commandContext.OnIgnoreAction != null)
                        {
                            commandContext.OnIgnoreAction();
                            commandContext.OnIgnoreAction = null;
                        }
                        break;
                    case UserReactionToCommandType.Clicked:
                        if (commandContext.OnClickAction != null)
                        {
                            commandContext.OnClickAction();
                            commandContext.OnClickAction = null;
                        }
                        break;
                    case UserReactionToCommandType.Notified:
                        if (commandContext.OnNotifiedAction != null)
                        {
                            commandContext.OnNotifiedAction();
                            commandContext.OnNotifiedAction = null;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("reactionType");
                }
            }
        }
    }

   
}
