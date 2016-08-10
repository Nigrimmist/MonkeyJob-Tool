using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HelloBotCore.Entities;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;
using Newtonsoft.Json;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;
using IntegrationClientBase = HelloBotCore.Entities.IntegrationClientBase;

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
        public readonly double Version = 0.6;
        private const int RunTraceSaveEveryMin = 5;

        private List<ComponentInfoBase> _modules = new List<ComponentInfoBase>();
        private readonly string _moduleDllmask;
        private readonly string _botCommandPrefix;
        private readonly string _moduleFolderPath;
        private readonly int _commandTimeoutSec;
        private readonly Dictionary<Guid, ModuleLocker> _commandDictLocks;
        private readonly string _settingsFolderAbsolutePath;
        private readonly string _logsFolderAbsolutePath;
        private Language _currentLanguage = Language.English;

        private readonly Dictionary<Guid, BotContextBase> _commandContexts;
        private readonly object _commandContextLock = new object();

        public delegate void OnModuleErrorOccuredDelegate(Exception ex, ModuleInfoBase module);
        public delegate void OnGeneralErrorOccuredDelegate(Exception ex);

        /// <param name="clientCommandContext">Can be null</param>
        public delegate void OnMessageRecievedDelegate(Guid? commandToken, AnswerInfo answer, ClientCommandContext clientCommandContext);
        public delegate void OnMessageHandledDelegate();

        public event OnGeneralErrorOccuredDelegate OnErrorOccured;
        public event OnModuleErrorOccuredDelegate OnModuleErrorOccured;
        public event OnMessageRecievedDelegate OnMessageRecieved;
        public event OnMessageHandledDelegate OnMessageHandled;
        private readonly double _currentUIClientVersion;
        private List<IntegrationClientBase> _integrationClients;


        public delegate void TrayIconSetupRequiredDelegate(Guid moduleId, Icon icon, string title);

        public event TrayIconSetupRequiredDelegate OnTrayIconSetupRequired;

        public delegate void TrayIconStateChangeRequestedDelegate(Guid moduleId, Icon originalIcon, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null);

        public event TrayIconStateChangeRequestedDelegate OnTrayIconStateChangeRequested;

        public delegate void OnTrayPopupShowRequestedDelegate(Guid moduleId, string title, string body, TimeSpan timeout, TooltipType tooltipType);

        public event OnTrayPopupShowRequestedDelegate OnTrayBalloonTipRequested;
        

        public event Action<List<AutoSuggestItem>> OnSuggestRecieved;
        public delegate bool IsClientEnabledForModuleDelegate(string client,string module, ModuleType moduleType);

        private IsClientEnabledForModuleDelegate _isClientEnabledForModuleFunc = null;

        /// <summary>
        /// Bot costructor
        /// </summary>
        /// <param name="settingsFolderAbsolutePath">folder for module settings, will be created if not exist</param>
        /// <param name="moduleDllmask">File mask for retrieving client command dlls</param>
        /// <param name="botCommandPrefix">Prefix for bot commands. Only messages with that prefix will be handled</param>
        public HelloBot(string settingsFolderAbsolutePath, string logsFolderAbsolutePath,double currentUIClientVersion, string moduleDllmask = "*.dll", string botCommandPrefix = "!", string moduleFolderPath = ".")
        {
            _currentUIClientVersion = currentUIClientVersion;
            _settingsFolderAbsolutePath = settingsFolderAbsolutePath;
            _logsFolderAbsolutePath = logsFolderAbsolutePath;
            if (!Directory.Exists(settingsFolderAbsolutePath))
                Directory.CreateDirectory(settingsFolderAbsolutePath);
            if (!Directory.Exists(logsFolderAbsolutePath))
                Directory.CreateDirectory(logsFolderAbsolutePath);
            
            _moduleDllmask = moduleDllmask;
            _botCommandPrefix = botCommandPrefix;
            _moduleFolderPath = moduleFolderPath;
            _commandTimeoutSec = 30;
            _commandDictLocks = new Dictionary<Guid, ModuleLocker>();
            _commandContexts = new Dictionary<Guid, BotContextBase>();
            
            new Thread(SaveModuleTraces).Start();
        }

        public void RunEventBasedModules()
        {
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
                            if (OnModuleErrorOccured != null)
                            {
                                OnModuleErrorOccured(ex, tEv);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(30));
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
                        TrayIcon = tTm.TrayIcon,
                        CommandName = tTm.GetModuleName(),
                    });
                    if (tTm.IsEnabled)
                    {
                        if (OnTrayIconSetupRequired != null)
                            OnTrayIconSetupRequired(tTm.Id, tTm.TrayIcon, tTm.GetModuleName());
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
                            if (OnModuleErrorOccured != null)
                            {
                                OnModuleErrorOccured(ex, tTm);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(30));
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

        public void RegisterModules(List<string> enabledModules = null, List<string> disabledModules = null)
        {
            var allModules = LoadModules(enabledModules,disabledModules);

            var handlerModules = allModules.OfType<ModuleCommandInfo>().Where(x => x.CallCommandList.Any()).ToList();
            var baseList = ExtendAliases(handlerModules).Select(x => (ModuleInfoBase) x).ToList(); //extend aliases for autocomplete wrong keyboard layout search
            baseList.AddRange(allModules.OfType<ModuleEventInfo>().Select(x => (ModuleInfoBase) x));
            baseList.AddRange(allModules.OfType<ModuleTrayInfo>().Select(x => (ModuleInfoBase) x));
            Modules.AddRange(baseList);
        }

        public void RegisterIntegrationClients(List<string> enabledClients, IsClientEnabledForModuleDelegate isClientEnabledForModuleFunc)
        {
            _isClientEnabledForModuleFunc = isClientEnabledForModuleFunc;
            var allClients = LoadIntegrationClients(enabledClients);
            _integrationClients = allClients;
        }

        public List<IntegrationClientBase> LoadIntegrationClients(List<string> enabledClients = null)
        {
            List<IntegrationClientBase> toReturn = new List<IntegrationClientBase>();
            var dlls = Directory.GetFiles(_moduleFolderPath, _moduleDllmask);
            var baseType = typeof(IntegrationClientRegisterBase);
            var settingsAttr = typeof(ModuleSettingsForAttribute);

            if (enabledClients == null) enabledClients = new List<string>();
            foreach (var dll in dlls)
            {
                var ass = Assembly.LoadFile(dll);
                var fi = new FileInfo(dll);
                //get types from assembly
                var types = ass.GetTypes();
                var typesInAssembly = types.Where(type => baseType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).ToList();
                var settingForClients = types.Where(t => t.IsDefined(settingsAttr, false)).Select(x =>
                    new
                    {
                        moduleSettingsClass = x,
                        moduleForParentClass = ((ModuleSettingsForAttribute)(Attribute.GetCustomAttribute(x, settingsAttr))).ModuleType
                    }).ToList();

                foreach (Type type in typesInAssembly)
                {
                    object obj = Activator.CreateInstance(type);

                    var clients = ((IntegrationClientRegisterBase)obj).GetIntegrationClients().Select(module =>
                    {
                        var settingClass = settingForClients.FirstOrDefault(x => x.moduleForParentClass == module.GetType());
                        var tModule = new IntegrationClientInfo(_settingsFolderAbsolutePath, _logsFolderAbsolutePath);
                        _commandDictLocks.Add(tModule.Id, new ModuleLocker());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), module, this,((IntegrationClientRegisterBase)obj).AuthorInfo);
                        tModule.IsEnabled = enabledClients.Contains(tModule.SystemName);
                        if (settingClass != null)
                            tModule.SettingsType = settingClass.moduleSettingsClass;

                        var mainModuleSettings = tModule.GetSettings<IntegrationClientSettings>();
                        if (mainModuleSettings == null)
                        {
                            mainModuleSettings = new IntegrationClientSettings();
                            tModule.SaveSettings(mainModuleSettings);
                        }

                        for (var i = 0; i < mainModuleSettings.InstanceCount; i++)
                        {
                            var clonedClient = tModule.Clone();
                            clonedClient.InstanceId = i;
                            tModule.Instances.Add(clonedClient);
                        }

                        return (IntegrationClientBase)tModule;
                    }).ToList();

                    toReturn.AddRange(clients);
                }
            }

            return toReturn;
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

        protected virtual List<ModuleInfoBase> LoadModules(List<string> enabledModules, List<string> disabledModules)
        {
            List<ModuleInfoBase> toReturn = new List<ModuleInfoBase>();
            var dlls = Directory.GetFiles(_moduleFolderPath, _moduleDllmask);
            var i = typeof (ModuleRegisterBase);
            var settingsAttr = typeof (ModuleSettingsForAttribute);

            if (enabledModules == null) enabledModules = new List<string>();
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
                        moduleForParentClass = ((ModuleSettingsForAttribute) (Attribute.GetCustomAttribute(x, settingsAttr))).ModuleType
                    }).ToList();

                foreach (Type type in typesInAssembly)
                {
                    object obj = Activator.CreateInstance(type);

                    var modules = ((ModuleRegisterBase) obj).GetModules().Select(module =>
                    {
                        var settingClass = settingForModules.FirstOrDefault(x => x.moduleForParentClass == module.GetType());
                        var tModule = new ModuleCommandInfo(_settingsFolderAbsolutePath,_logsFolderAbsolutePath);
                        _commandDictLocks.Add(tModule.Id, new ModuleLocker());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), module, this, ((ModuleRegisterBase) obj).AuthorInfo);
                        tModule.IsEnabled = !disabledModules.Contains(tModule.SystemName);
                        if (settingClass != null)
                            tModule.SettingsType = settingClass.moduleSettingsClass;
                        return (ModuleInfoBase) tModule;
                    }).ToList();

                    var events = ((ModuleRegisterBase) obj).GetEvents().Select(ev =>
                    {
                        var settingClass = settingForModules.FirstOrDefault(x => x.moduleForParentClass == ev.GetType());
                        var tModule = new ModuleEventInfo(_settingsFolderAbsolutePath, _logsFolderAbsolutePath);
                        _commandDictLocks.Add(tModule.Id, new ModuleLocker());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), ev, this, ((ModuleRegisterBase) obj).AuthorInfo);
                        tModule.IsEnabled = enabledModules.Contains(tModule.SystemName);
                        if (settingClass != null)
                            tModule.SettingsType = settingClass.moduleSettingsClass;
                        return (ModuleInfoBase) tModule;
                    }).ToList();

                    var trayModules = ((ModuleRegisterBase) obj).GetTrayModules().Select(ev =>
                    {
                        var settingClass = settingForModules.FirstOrDefault(x => x.moduleForParentClass == ev.GetType());
                        var tModule = new ModuleTrayInfo(_settingsFolderAbsolutePath, _logsFolderAbsolutePath);
                        _commandDictLocks.Add(tModule.Id, new ModuleLocker());
                        tModule.Init(Path.GetFileNameWithoutExtension(fi.Name), ev, this, ((ModuleRegisterBase) obj).AuthorInfo);
                        tModule.IsEnabled = enabledModules.Contains(tModule.SystemName);
                        if (settingClass != null)
                            tModule.SettingsType = settingClass.moduleSettingsClass;
                        return (ModuleInfoBase) tModule;
                    }).ToList();

                    toReturn.AddRange(modules);
                    toReturn.AddRange(events);
                    toReturn.AddRange(trayModules);
                }
            }

            return toReturn;
        }

        public bool HandleMessage(string incomingMessage, ClientCommandContext clientCommandContext, bool runWithTimeout)
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

                                    hnd.HandleMessage(command, args.TrimStart(), commandTempGuid);

                                    if (OnMessageHandled != null) 
                                        OnMessageHandled();
                                }
                                catch (Exception ex)
                                {
                                    if (!(ex is ThreadAbortException))
                                    {
                                        if (OnModuleErrorOccured != null)
                                        {
                                            OnModuleErrorOccured(ex, hnd);
                                        }

                                    }
                                }
                            }, TimeSpan.FromSeconds(_commandTimeoutSec), runWithTimeout))
                            {
                                ShowInternalMessage(command, CommunicationMessage.FromString("модуль сломался. Причина : время модуля на выполнение команды истекло"));
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

        private static bool RunWithTimeout(ThreadStart threadStart, TimeSpan timeout, bool timeoutEnabled)
        {
            Thread workerThread = new Thread(threadStart);
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();
            
            if (timeoutEnabled)
            {
                var finished = workerThread.Join(timeout);
                if (!finished)
                    workerThread.Abort();
                return finished;
            }
            return true;
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
                toReturn = EnabledCommands.FirstOrDefault(x => x.CallCommandList.Select(y => y.Command).Contains(foundCommand, StringComparer.OrdinalIgnoreCase));
                if (toReturn != null)
                {
                    command = foundCommand;
                }
            }
            
            return toReturn;
        }

        public List<string> GetUserDefinedCommandList()
        {
            List<string> toReturn = new List<string>();
            foreach (var commandList in EnabledCommands.Select(x => x.CallCommandList))
            {
                toReturn.AddRange(commandList.Select(x => x.Command));
            }
            return toReturn;
        }

        public List<CallCommandInfo> FindCommands(string incCommand)
        {
            return (
                from module in EnabledCommands
                from cmd in module.CallCommandList
                where
                    cmd.Command.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase) ||
                    cmd.Aliases.Any(y => y.StartsWith(incCommand, StringComparison.InvariantCultureIgnoreCase))
                select cmd).ToList();
        }

        #region methods for modules

        public void SaveSettings<T>(ComponentInfoBase info, T serializableSettingObject) where T : class
        {
            lock (_commandDictLocks[info.Id].SettingsLock)
            {
                info.SaveSettings(serializableSettingObject);
            }
        }

        

        public T GetSettings<T>(ComponentInfoBase module) where T : class
        {
            lock (_commandDictLocks[module.Id].SettingsLock)
            {
                return module.GetSettings<T>();
            }
        }

        
        public void ShowMessage(ComponentInfoBase moduleInfo, CommunicationMessage content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default, Guid? commandToken = null, bool useBaseClient = false)
        {
            BotContextBase commandBaseContext = null;
            //todo:refactoring required (should be cleared time to time)
            if (commandToken.HasValue)
            {
                lock (_commandContexts)
                {
                    if (_commandContexts.TryGetValue(commandToken.Value, out commandBaseContext))
                    {
                        //_commandContexts.Remove(commandToken);
                    }
                }
            }
            
            BotCommandContext commandContext = commandBaseContext as BotCommandContext;
            if (commandContext != null || !commandToken.HasValue)
            {
                var enabledIntegrationClients = IntegrationClients.Where(x => x.IsEnabled && _isClientEnabledForModuleFunc(x.SystemName, moduleInfo.SystemName, moduleInfo.ModuleType)).ToList();
                
                if (enabledIntegrationClients.Any() && !useBaseClient)
                {
                    foreach (IntegrationClientBase client in enabledIntegrationClients)
                    {
                        Guid token = Guid.NewGuid();
                        AddNewCommandContext(token, new BotCommandContext()
                        {
                            CommandName = !string.IsNullOrEmpty(client.ProvidedTitle) ? client.ProvidedTitle : "",
                            ModuleType = ModuleType.IntegrationClient,
                            ModuleId = client.Id
                        });

                        client.SendMessageToClient(token, new CommunicationClientMessage(content)
                        {
                            FromModule = moduleInfo.ProvidedTitle ?? ""
                        });
                    }
                }
                else
                {
                    if (OnMessageRecieved != null)
                    {
                        Color? bodyBackgroundColor = null;
                        Color? headerBackgroundColor = null;
                        var eventInfo = moduleInfo as ModuleEventInfo;
                        if (eventInfo != null)
                        {
                            bodyBackgroundColor = eventInfo.BodyBackgroundColor;
                            headerBackgroundColor = eventInfo.HeaderBackgroundColor;
                        }

                        var commandInfo = moduleInfo as ModuleCommandInfo;
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
                            Icon = moduleInfo.Icon,
                            BodyBackgroundColor = bodyBackgroundColor,
                            HeaderBackgroundColor = headerBackgroundColor
                        }, commandContext.ClientCommandContext);
                    }
                }

                if (OnMessageHandled != null)
                    OnMessageHandled();

            }
        }

        //todo : should be refactoring to show error (remove method and call error event)
        [Obsolete]
        private void ShowInternalMessage(string commandname, CommunicationMessage message, string title = null, ClientCommandContext clientCommandContext = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            Guid systemGuid = Guid.Empty;
            if (OnMessageRecieved != null)
                OnMessageRecieved(systemGuid, new AnswerInfo()
                {
                    Answer = message,
                    Title = title,
                    AnswerType = answerType,
                    CommandName = commandname,
                    MessageSourceType = ModuleType.Handler
                }, clientCommandContext);
        }


        public void RegisterUserReactionCallback(Guid commandToken, UserReactionToCommandType reactionType, Action reactionCallback)
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
                        commandContext.OnClickAction =reactionCallback;
                        break;
                }

            }
        }

        public void UpdateTrayText(Guid token, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null)
        {
            var trayModuleContext = GetCommandContextByToken(token) as BotTrayModuleContext;
            if (trayModuleContext != null)
            {
                if (OnTrayIconStateChangeRequested != null)
                    OnTrayIconStateChangeRequested(trayModuleContext.ModuleId, trayModuleContext.TrayIcon, text, textColor, backgroundColor, fontSize, fontName, iconBorderColor);
            }
        }

        public void ShowTrayBalloonTip(Guid token, string text, TimeSpan? timeout = null, TooltipType? tooltipType = null)
        {
            var trayModuleContext = GetCommandContextByToken(token) as BotTrayModuleContext;
            if (trayModuleContext != null)
            {
                string title = trayModuleContext.CommandName;

                if (OnTrayBalloonTipRequested != null)
                    OnTrayBalloonTipRequested(trayModuleContext.ModuleId, title, text, timeout ?? TimeSpan.FromSeconds(10), tooltipType ?? TooltipType.None);
            }
        }

        public void LogModuleTraceRequest(ComponentInfoBase moduleInfo, string message)
        {
            lock (_commandDictLocks[moduleInfo.Id].LogTraceLock)
            {
                moduleInfo.Trace.AddMessage(message);
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
                            //commandContext.ClearHandlers();
                        }
                        break;
                    case UserReactionToCommandType.Closed:
                        if (commandContext.OnClosedAction != null)
                        {
                            commandContext.OnClosedAction();
                            //commandContext.ClearHandlers();
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
        
        public IEnumerable<ModuleEventInfo> Events
        {
            get { return _modules.OfType<ModuleEventInfo>(); }
        }

        public IEnumerable<ModuleCommandInfo> EnabledCommands
        {
            get { return _modules.OfType<ModuleCommandInfo>().Where(x => x.IsEnabled); }
        }

        public IEnumerable<ModuleTrayInfo> TrayModules
        {
            get { return _modules.OfType<ModuleTrayInfo>(); }
        }

        public List<ComponentInfoBase> Modules
        {
            get { return _modules; }
        }

        public List<IntegrationClientBase> IntegrationClients
        {
            get { return _integrationClients; }
        }


        public void DisableModule(string moduleSystemName)
        {
            Modules.Union(_integrationClients).Single(x => x.SystemName == moduleSystemName).IsEnabled = false;
        }

        public void EnableModule(string moduleSystemName)
        {
            Modules.Union(_integrationClients).Single(x => x.SystemName == moduleSystemName).IsEnabled = true;
        }

        public List<ComponentInfoBase> GetIncompatibleSettingModules()
        {
            List<ComponentInfoBase> toReturn = new List<ComponentInfoBase>();
            foreach (ComponentInfoBase module in Modules.Union(IntegrationClients).Select(x => (ComponentInfoBase)x).Union(IntegrationClients).Where(x => x.SettingsType != null))
            {
                lock (_commandDictLocks[module.Id].SettingsLock)
                {
                    string fullPath = module.GetSettingFileFullPath();
                    if (File.Exists(fullPath))
                    {
                        string data = File.ReadAllText(fullPath);
                        var settings = JsonConvert.DeserializeObject<ModuleSettings>(data);
                        if (settings.SettingsVersion < module.SettingsModuleVersion)
                        {
                            toReturn.Add(module);
                        }
                    }

                }
            }
            return toReturn;
        }


        private void SaveModuleTraces()
        {
            while (true)
            {
                Thread.Sleep(RunTraceSaveEveryMin*1000*60);
                try
                {
                    foreach (var module in Modules)
                    {
                        lock (_commandDictLocks[module.Id].LogTraceLock)
                        {
                            module.Trace.Save();
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (OnErrorOccured != null)
                        OnErrorOccured(ex);
                }
            }
        }


        //public void GetArgumentSuggestions(CallCommandInfo command, string commandAlias, string args)
        //{
        //    if (command.CommandArgumentSuggestions != null)
        //    {

        //        //todo : thread pool required
        //        new Thread(() =>
        //        {
        //            try
        //            {
        //                foreach (var comSuggestion in command.CommandArgumentSuggestions)
        //                {
        //                    foreach (var argSuggestion in comSuggestion.TemplateParseInfo.Where(x=>x.Order==0))
        //                    {
        //                        var firstLevelSuggestions = string.IsNullOrEmpty(argSuggestion.RegexpPart) && args.Trim() == "";
                                
        //                        if (firstLevelSuggestions || (!string.IsNullOrEmpty(argSuggestion.RegexpPart) && Regex.IsMatch(args, argSuggestion.RegexpPart)))
        //                        {
        //                            ShowSuggestionsToClient(comSuggestion.Details.Where(x=>x.Key==argSuggestion.Key).Select(x=>x.GetSuggestionFunc).Single()());
        //                            return;
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //if (OnModuleErrorOccured != null)
        //                //    OnModuleErrorOccured(ex, command);
        //            }
        //        }).Start();
        //    }
        //}

        public void ShowSuggestionsToClient(List<AutoSuggestItem> items)
        {
            if (OnSuggestRecieved != null)
            {
                OnSuggestRecieved(items);
            }
        }

        
    }
}
