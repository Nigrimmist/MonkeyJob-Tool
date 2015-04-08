using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using HelloBotCore.Entities;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;
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
        public readonly double Version = 0.2;
        private List<ModuleCommandInfo> _handlerModules = new List<ModuleCommandInfo>();
        private List<ModuleEventInfo> _eventModules = new List<ModuleEventInfo>();
        
        private readonly string _moduleDllmask;
        private readonly string _botCommandPrefix;
        private readonly string _moduleFolderPath;
        private readonly int _commandTimeoutSec;
        private readonly Dictionary<Guid, object> _commandDictLocks;
        private readonly string _settingsFolderAbsolutePath;
        private Language _currentLanguage = Language.English;

        private readonly Dictionary<Guid, BotCommandContext> _commandContexts;
        private readonly object _commandContextLock = new object();
        public delegate void OnErrorOccuredDelegate(Exception ex);
        
        /// <param name="clientCommandContext">Can be null</param>
        public delegate void OnMessageRecievedDelegate(Guid commandToken,AnswerInfo answer,ClientCommandContext clientCommandContext);
        public event OnErrorOccuredDelegate OnErrorOccured;
        public event OnMessageRecievedDelegate OnMessageRecieved;
        private readonly double _currentUIClientVersion;

        /// <summary>
        /// Bot costructor
        /// </summary>
        /// <param name="settingsFolderAbsolutePath">folder for module settings, will be created if not exist</param>
        /// <param name="moduleDllmask">File mask for retrieving client command dlls</param>
        /// <param name="botCommandPrefix">Prefix for bot commands. Only messages with that prefix will be handled</param>
        public HelloBot(string settingsFolderAbsolutePath,double currentUIClientVersion,string moduleDllmask = "*.dll", string botCommandPrefix = "!", string moduleFolderPath = "." )
        {
            _currentUIClientVersion = currentUIClientVersion;
            _settingsFolderAbsolutePath = settingsFolderAbsolutePath;
            if (!Directory.Exists(settingsFolderAbsolutePath))
                Directory.CreateDirectory(settingsFolderAbsolutePath);

            _moduleDllmask = moduleDllmask;
            _botCommandPrefix = botCommandPrefix;
            _moduleFolderPath = moduleFolderPath;
            _commandTimeoutSec = 30;
            _commandDictLocks = new Dictionary<Guid, object>();
            
            _commandContexts = new Dictionary<Guid, BotCommandContext>();
            
        }

        public void Start()
        {
            RegisterModules();
            RunEventModuleTimers();
        }

        private void RunEventModuleTimers()
        {
            foreach (var ev in _eventModules)
            {
                var tEv = ev;
                new Thread(() =>
                {
                    //Thread.Sleep((int)TimeSpan.FromSeconds(3).TotalMilliseconds);
                    Guid commandToken = Guid.NewGuid();
                    AddNewCommandContext(commandToken, new BotCommandContext()
                    {
                        CommandName = string.IsNullOrEmpty(tEv.DisplayName)?tEv.ModuleName:tEv.DisplayName,
                        CommandType = ModuleType.Event
                    });

                    while (true)
                    {
                        try
                        {
                            tEv.CallEvent(commandToken);
                        }
                        catch (Exception ex)
                        {
                            ShowInternalMessage("Ошибка",ex.ToString());
                            //todo:log module exception to module exception file
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
            var allModules = GetModules();
            
            _handlerModules = allModules.OfType<ModuleCommandInfo>().Where(x=>x.CallCommandList.Any()).ToList();
            _handlerModules = ExtendAliases(_handlerModules);//extend aliases for autocomplete wrong keyboard layout search
            _eventModules = allModules.OfType<ModuleEventInfo>().ToList();
        }

        private List<ModuleCommandInfo> ExtendAliases(List<ModuleCommandInfo> modules)
        {
            foreach (var module in modules)
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
            
            return modules;
        }

        protected virtual List<ModuleCommandInfoBase> GetModules()
        {
            List<ModuleCommandInfoBase> toReturn = new List<ModuleCommandInfoBase>();
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
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), module, this);
                        return (ModuleCommandInfoBase)tModule;
                    });

                    var events = ((ModuleRegister)obj).GetEvents().Select(ev =>
                    {
                        var tModule = new ModuleEventInfo();
                        _commandDictLocks.Add(tModule.Id, new object());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), ev, this);
                        return (ModuleCommandInfoBase)tModule;
                    });

                    toReturn.AddRange(modules);
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
                                            CommandName = !string.IsNullOrEmpty(hnd.DisplayName) ? hnd.DisplayName : command,
                                            CommandType = ModuleType.Handler
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
                                            ShowInternalMessage(command,"модуль сломался");
                                        }
                                    }
                                }, TimeSpan.FromSeconds(_commandTimeoutSec)))
                                {
                                    ShowInternalMessage(command,"модуль сломался. Причина : время на выполнение команды истекло");
                                }
                            }).Start();
                            return true;
                        }
                        return false;
                    
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
            foreach (var module in _handlerModules)
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
                foreach (var module in _handlerModules)
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
                toReturn = _handlerModules.FirstOrDefault(x => x.CallCommandList.Select(y=>y.Command).Contains(foundCommand,StringComparer.OrdinalIgnoreCase));
                if (toReturn != null)
                {
                    command = foundCommand;
                }
            }
            args = args.Trim();
            return toReturn;
        }

        public List<string> GetUserDefinedCommandList()
        {
            List<string> toReturn = new List<string>();
            foreach (var commandList in _handlerModules.Select(x=>x.CallCommandList))
            {
                toReturn.AddRange(commandList.Select(x=>x.Command));
            }
            return toReturn;
        }
        
        public List<CallCommandInfo> FindCommands(string incCommand)
        {
            return (
                from module in _handlerModules
                from cmd in module.CallCommandList where
                cmd.Command.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase) ||
                cmd.Aliases.Any(y => y.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase))
                let descr = !string.IsNullOrEmpty(cmd.Description) ? cmd.Description : module.CommandDescription
                select new CallCommandInfo(cmd.Command, descr)).ToList();
        }

        #region methods for modules
        
        public void SaveSettings<T>(ModuleCommandInfoBase commandInfo, T serializableSettingObject) where T : class
        {
            lock (_commandDictLocks[commandInfo.Id])
            {
                var settings = new ModuleSettings<T>(commandInfo.Version, serializableSettingObject);
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                string moduleFileName = commandInfo.ModuleName + ".json";
                File.WriteAllText(_settingsFolderAbsolutePath + "/" + moduleFileName, json);
            }
        }

        public T GetSettings<T>(ModuleCommandInfoBase commandInfo) where T : class
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

        public void ShowMessage(Guid commandToken, ModuleCommandInfoBase commandInfo, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
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
            
            if (commandContext != null)
            {
                if (OnMessageRecieved != null)
                    OnMessageRecieved(commandToken,new AnswerInfo()
                    {
                        Answer = content,
                        Title = title,
                        CommandName = commandContext.CommandName,
                        AnswerType = answerType,
                        MessageSourceType = commandContext.CommandType,
                        Icon = commandInfo.Icon,
                        BodyBackgroundColor = commandInfo.BodyBackgroundColor,
                        HeaderBackgroundColor = commandInfo.HeaderBackgroundColor
                    },  commandContext.ClientCommandContext);
            }
        }

        //todo : should be refactoring to show error (remove method and call error event)
        [Obsolete]
        private void ShowInternalMessage(string commandname, string content, string title = null, ClientCommandContext clientCommandContext=null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            Guid systemGuid = Guid.Empty;
            if (OnMessageRecieved != null)
                OnMessageRecieved(systemGuid, new AnswerInfo()
                {
                    Answer = content,
                    Title = title,
                    AnswerType = answerType,
                    CommandName = commandname,
                    MessageSourceType = ModuleType.Handler
                },clientCommandContext);
        }
        

        public void RegisterUserReactionCallback(Guid commandToken,UserReactionToCommandType reactionType, Action reactionCallback)
        {
            BotCommandContext commandContext = GetCommandContextByToken(commandToken);

            if (commandContext != null)
            {
                switch (reactionType)
                {
                    case UserReactionToCommandType.Closed:
                        commandContext.OnClosedAction = reactionCallback;
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
                    case UserReactionToCommandType.HidedByTimeout:
                    {
                        break;
                    }
                    case UserReactionToCommandType.Clicked:
                        if (commandContext.OnClickAction != null)
                        {
                            commandContext.OnClickAction();
                            commandContext.ClearHandlers();
                        }
                        break;
                    case UserReactionToCommandType.Closed:
                        if (commandContext.OnClosedAction != null)
                        {
                            commandContext.OnClosedAction();
                            commandContext.ClearHandlers();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("reactionType");
                }
            }
        }

        
        public void SetCurrentLanguage(Language lang)
        {
            _currentLanguage = lang;
        }

        public Language GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public double GetCurrentVersion()
        {
            return Version;
        }

        public double GetUIClientVersion()
        {
            return _currentUIClientVersion;
        }
    }

   
}
