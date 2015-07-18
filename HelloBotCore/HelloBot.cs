using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using HelloBotCore.Entities;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
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
        public readonly double Version = 0.4;
        
        private List<ModuleInfoBase> _allModules = new List<ModuleInfoBase>();

        private readonly string _moduleDllmask;
        private readonly string _botCommandPrefix;
        private readonly string _moduleFolderPath;
        private readonly int _commandTimeoutSec;
        private readonly Dictionary<Guid, object> _commandDictLocks;
        private readonly string _settingsFolderAbsolutePath;
        private Language _currentLanguage = Language.English;

        private readonly Dictionary<Guid, BotContextBase> _commandContexts;
        private readonly object _commandContextLock = new object();
        public delegate void OnErrorOccuredDelegate(Exception ex, ModuleInfoBase module);
        
        /// <param name="clientCommandContext">Can be null</param>
        public delegate void OnMessageRecievedDelegate(Guid commandToken,AnswerInfo answer,ClientCommandContext clientCommandContext);
        public event OnErrorOccuredDelegate OnErrorOccured;
        public event OnMessageRecievedDelegate OnMessageRecieved;
        private readonly double _currentUIClientVersion;

        public delegate void TrayIconSetupRequiredDelegate(Guid moduleId, Icon icon, string title);
        public event TrayIconSetupRequiredDelegate OnTrayIconSetupRequired;

        public delegate void TrayIconStateChangeRequestedDelegate(Guid moduleId, Icon originalIcon, string text, Color? backgroundColor = null);
        public event TrayIconStateChangeRequestedDelegate OnTrayIconStateChangeRequested;

        /// <summary>
        /// Bot costructor
        /// </summary>
        /// <param name="settingsFolderAbsolutePath">folder for module settings, will be created if not exist</param>
        /// <param name="moduleDllmask">File mask for retrieving client command dlls</param>
        /// <param name="botCommandPrefix">Prefix for bot commands. Only messages with that prefix will be handled</param>
        public HelloBot(string settingsFolderAbsolutePath,double currentUIClientVersion,string moduleDllmask = "*.dll", string botCommandPrefix = "!", string moduleFolderPath = ".")
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

            _commandContexts = new Dictionary<Guid, BotContextBase>();
            
        }

        public void Start(List<string> disabledModules=null)
        {
            RegisterModules(disabledModules);
            RunEventModuleTimers();
            RunTrayModuleTimers();
        }

        private void RunEventModuleTimers()
        {
            foreach (var ev in Events)
            {
                var tEv = ev;
                new Thread(() =>
                {
                    
                    Guid commandToken = Guid.NewGuid();
                    AddNewCommandContext(commandToken, new BotCommandContext()
                    {
                        CommandName = tEv.GetModuleName(),
                        ModuleType = ModuleType.Event,
                        ModuleId = tEv.Id
                    });

                    while (true)
                    {
                        try
                        {
                            if (tEv.IsEnabled)
                            {
                                tEv.CallEvent(commandToken);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (OnErrorOccured != null)
                            {
                                OnErrorOccured(ex,tEv);
                            }
                        }
                        
                        Thread.Sleep(tEv.EventRunEvery);
                    }

                }).Start();
            }
        }

        private void RunTrayModuleTimers()
        {
            foreach (var tm in TrayModules)
            {
                var tTm = tm;
                new Thread(() =>
                {

                    Guid commandToken = Guid.NewGuid();
                    AddNewCommandContext(commandToken, new BotTrayModuleContext()
                    {
                        ModuleType = ModuleType.Tray,
                        ModuleId = tTm.Id,
                        TrayIcon = tTm.TrayIcon
                    });
                    if (tTm.IsEnabled)
                    {
                        if (OnTrayIconSetupRequired!=null)
                            OnTrayIconSetupRequired(tTm.Id,tTm.TrayIcon,tTm.GetModuleName());
                    }
                    while (true)
                    {
                        try
                        {
                            if (tTm.IsEnabled)
                            {
                                tTm.CallEvent(commandToken);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (OnErrorOccured != null)
                            {
                                OnErrorOccured(ex, tTm);
                            }
                        }

                        Thread.Sleep(tTm.EventRunEvery);
                    }

                }).Start();
            }
        }

        private void AddNewCommandContext(Guid commandToken, BotContextBase botContext)
        {
            lock (_commandContextLock)
            {
                _commandContexts.Add(commandToken, botContext);
            }
        }

        private void RegisterModules(List<string> disabledModules=null)
        {

            var allModules = LoadModules(disabledModules);

            var handlerModules = allModules.OfType<ModuleCommandInfo>().Where(x => x.CallCommandList.Any()).ToList();
            var baseList = ExtendAliases(handlerModules).Select(x => (ModuleInfoBase)x).ToList();//extend aliases for autocomplete wrong keyboard layout search
            baseList.AddRange(allModules.OfType<ModuleEventInfo>().Select(x => (ModuleInfoBase)x));
            baseList.AddRange(allModules.OfType<ModuleTrayInfo>().Select(x => (ModuleInfoBase)x));
            AllModules.AddRange(baseList);
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

        protected virtual List<ModuleInfoBase> LoadModules(List<string> disabledModules)
        {
            List<ModuleInfoBase> toReturn = new List<ModuleInfoBase>();
            var dlls = Directory.GetFiles(_moduleFolderPath, _moduleDllmask);
            var i = typeof(ModuleRegister);
            var settingsAttr = typeof (ModuleSettingsForAttribute);
            
            if (disabledModules == null) disabledModules = new List<string>();
            foreach (var dll in dlls)
            {
                var ass = Assembly.LoadFile(dll);
                var fi = new FileInfo(dll);
                //get types from assembly
                var types = ass.GetTypes();
                var typesInAssembly = types.Where(type => i.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).ToList();
                var settingForModules = types.Where(t => t.IsDefined(settingsAttr, false)).Select(x =>
                    new
                    {
                        moduleSettingsClass = x,
                        moduleForParentClass = ((ModuleSettingsForAttribute)(Attribute.GetCustomAttribute(x, settingsAttr))).ModuleType
                    }).ToList();

                foreach (Type type in typesInAssembly)
                {
                    object obj = Activator.CreateInstance(type);
                    
                    var modules = ((ModuleRegister)obj).GetModules().Select(module =>
                    {
                        var settingClass = settingForModules.FirstOrDefault(x => x.moduleForParentClass == module.GetType());
                        var tModule = new ModuleCommandInfo();
                        _commandDictLocks.Add(tModule.Id, new object());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), module, this, ((ModuleRegister)obj).AuthorInfo);
                        tModule.IsEnabled = !disabledModules.Contains(tModule.ModuleSystemName);
                        if (settingClass != null)
                            tModule.ModuleSettingsType = settingClass.moduleSettingsClass;
                        return (ModuleInfoBase)tModule;
                    });

                    var events = ((ModuleRegister)obj).GetEvents().Select(ev =>
                    {
                        var settingClass = settingForModules.FirstOrDefault(x => x.moduleForParentClass == ev.GetType());
                        var tModule = new ModuleEventInfo();
                        _commandDictLocks.Add(tModule.Id, new object());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), ev, this, ((ModuleRegister)obj).AuthorInfo);
                        tModule.IsEnabled = !disabledModules.Contains(tModule.ModuleSystemName);
                        if (settingClass != null)
                            tModule.ModuleSettingsType = settingClass.moduleSettingsClass;
                        return (ModuleInfoBase)tModule;
                    });

                    var trayModules = ((ModuleRegister)obj).GetTrayModules().Select(ev =>
                    {
                        var settingClass = settingForModules.FirstOrDefault(x => x.moduleForParentClass == ev.GetType());
                        var tModule = new ModuleTrayInfo();
                        _commandDictLocks.Add(tModule.Id, new object());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), ev, this, ((ModuleRegister)obj).AuthorInfo);
                        tModule.IsEnabled = !disabledModules.Contains(tModule.ModuleSystemName);
                        if (settingClass != null)
                            tModule.ModuleSettingsType = settingClass.moduleSettingsClass;
                        return (ModuleInfoBase)tModule;
                    });

                    toReturn.AddRange(modules);
                    toReturn.AddRange(events);
                    toReturn.AddRange(trayModules);
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
                        ModuleCommandInfo foundModuleCommand = FindModule(command, out command, out args);
                        if (foundModuleCommand != null)
                        {
                            ModuleCommandInfo hnd = foundModuleCommand;
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
                                            CommandName = !string.IsNullOrEmpty(hnd.ProvidedTitle) ? hnd.ProvidedTitle : command,
                                            ModuleType = ModuleType.Handler,
                                            ModuleId = hnd.Id
                                        });

                                        
                                        hnd.HandleMessage(command, args, commandTempGuid);
                                        
                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!(ex is ThreadAbortException))
                                        {
                                            if (OnErrorOccured != null)
                                            {
                                                OnErrorOccured(ex,hnd);
                                            }
                                            
                                        }
                                    }
                                }, TimeSpan.FromSeconds(_commandTimeoutSec)))
                                {
                                    ShowInternalMessage(command,"модуль сломался. Причина : время модуля на выполнение команды истекло");
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
        

        public ModuleCommandInfo FindModule(string phrase, out string command, out string args)
        {
            ModuleCommandInfo toReturn = null;
            command = string.Empty;
            List<string> foundCommands = new List<string>();
            args = string.Empty;
            foreach (var module in EnabledCommands)
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
                foreach (var module in EnabledCommands)
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
                toReturn = EnabledCommands.FirstOrDefault(x => x.CallCommandList.Select(y=>y.Command).Contains(foundCommand,StringComparer.OrdinalIgnoreCase));
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
            foreach (var commandList in EnabledCommands.Select(x=>x.CallCommandList))
            {
                toReturn.AddRange(commandList.Select(x=>x.Command));
            }
            return toReturn;
        }
        
        public List<CallCommandInfo> FindCommands(string incCommand)
        {
            return (
                from module in EnabledCommands
                from cmd in module.CallCommandList where
                cmd.Command.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase) ||
                cmd.Aliases.Any(y => y.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase))
                let descr = !string.IsNullOrEmpty(cmd.Description) ? cmd.Description : module.CommandDescription.Description
                select new CallCommandInfo(cmd.Command, descr)).ToList();
        }

        #region methods for modules
        
        public void SaveSettings<T>(ModuleInfoBase info, T serializableSettingObject) where T : class
        {
            lock (_commandDictLocks[info.Id])
            {
                var settings = new ModuleSettings<T>(info.Version, serializableSettingObject);
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(info.GetSettingFileFullPath(_settingsFolderAbsolutePath), json);
            }
        }

        public T GetSettings<T>(ModuleInfoBase info) where T : class
        {
            lock (_commandDictLocks[info.Id])
            {
                string fullPath = info.GetSettingFileFullPath(_settingsFolderAbsolutePath);
                if (!File.Exists(fullPath)) return null;
                string data = File.ReadAllText(fullPath);
                var settings = JsonConvert.DeserializeObject<ModuleSettings<T>>(data);
                return settings.ModuleData;
            }
        }

        public void ShowMessage(Guid commandToken, ModuleInfoBase info, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            BotContextBase commandBaseContext;
            //todo:refactoring required (should be cleared time to time)
            lock (_commandContexts)
            {
                if (_commandContexts.TryGetValue(commandToken, out commandBaseContext))
                {
                    //_commandContexts.Remove(commandToken);
                }
            }
            BotCommandContext commandContext = commandBaseContext as BotCommandContext;
            if (commandContext != null)
            {
                if (OnMessageRecieved != null)
                {
                    Color? bodyBackgroundColor = null;
                    Color? headerBackgroundColor = null;
                    var eventInfo = info as ModuleEventInfo;
                    if (eventInfo != null)
                    {
                        bodyBackgroundColor = eventInfo.BodyBackgroundColor;
                        headerBackgroundColor = eventInfo.HeaderBackgroundColor;
                    }

                    var commandInfo = info as ModuleCommandInfo;
                    if (commandInfo != null)
                    {
                        bodyBackgroundColor = commandInfo.BodyBackgroundColor;
                        headerBackgroundColor = commandInfo.HeaderBackgroundColor;
                    }

                    OnMessageRecieved(commandToken, new AnswerInfo()
                    {
                        Answer = content,
                        Title = title,
                        CommandName = commandContext.CommandName,
                        AnswerType = answerType,
                        MessageSourceType = commandContext.ModuleType,
                        Icon = info.Icon,
                        BodyBackgroundColor = bodyBackgroundColor,
                        HeaderBackgroundColor = headerBackgroundColor
                    }, commandContext.ClientCommandContext);


                }

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

            BotCommandContext commandContext = GetCommandContextByToken(commandToken) as BotCommandContext;
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

        private BotContextBase GetCommandContextByToken(Guid commandToken)
        {
            BotContextBase botContextBase;
            lock (_commandContexts)
            {
                _commandContexts.TryGetValue(commandToken, out botContextBase);
            }
            return botContextBase;
        }

        public void HandleUserReactionToCommand(Guid commandToken, UserReactionToCommandType reactionType)
        {
            BotCommandContext commandContext = GetCommandContextByToken(commandToken) as BotCommandContext;

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

        public void SetTrayText(Guid trayToken, string text)
        {
            var trayModuleContext = GetCommandContextByToken(trayToken) as BotTrayModuleContext;
            if (trayModuleContext != null)
            {
                if (OnTrayIconStateChangeRequested != null)
                    OnTrayIconStateChangeRequested(trayModuleContext.ModuleId,trayModuleContext.TrayIcon, text);
            }
        }

        public void SetTrayColor(Guid trayToken, Color color)
        {
            var trayModuleContext = GetCommandContextByToken(trayToken) as BotTrayModuleContext;
            if (trayModuleContext != null)
            {
                if (OnTrayIconStateChangeRequested != null)
                    OnTrayIconStateChangeRequested(trayModuleContext.ModuleId, trayModuleContext.TrayIcon, null, color);
            }
        }

        public IEnumerable<ModuleEventInfo> Events { get { return _allModules.OfType<ModuleEventInfo>(); } }
        public IEnumerable<ModuleCommandInfo> EnabledCommands { get { return _allModules.OfType<ModuleCommandInfo>().Where(x=>x.IsEnabled); } }
        public IEnumerable<ModuleTrayInfo> TrayModules { get { return _allModules.OfType<ModuleTrayInfo>(); } }

        public List<ModuleInfoBase> AllModules { get { return _allModules; } }


        public void DisableModule(string moduleSystemName)
        {
            AllModules.Single(x => x.ModuleSystemName == moduleSystemName).IsEnabled = false;
        }

        public void EnableModule(string moduleSystemName)
        {
            AllModules.Single(x => x.ModuleSystemName == moduleSystemName).IsEnabled = true;
        }
    }
}
