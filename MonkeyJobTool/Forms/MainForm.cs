using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore;
using HelloBotCore.DAL.StorageServices;
using HelloBotCore.Entities;
using Microsoft.Win32;
using MonkeyJobTool.Controls;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Helpers;
using MonkeyJobTool.Managers;
using MonkeyJobTool.Managers.Interfaces;
using MonkeyJobTool.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharedHelper;
using Language = HelloBotCore.Entities.Language;
using LogManager = MonkeyJobTool.Managers.LogManager;

namespace MonkeyJobTool.Forms
{
    
    public partial class MainForm : Form
    {
        private HelloBot _bot;
        private const string _copyToBufferPostFix = " в буфер";
        private AutoCompleteControl _autocomplete;
        
        private readonly Bitmap _defaultIcon = Resources.monkey_highres_img;
        private readonly Bitmap _loadingIcon = Resources.loading;
        private bool _isFirstRun;
        private object _trayIconLocker = new object();
        private Dictionary<Guid, NotifyIcon> _trayModuleIcons = new Dictionary<Guid, NotifyIcon>();
        private IStorageManager _storageManager;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {


            try
            {
                var fs = new FileStorage(App.Instance.ExecutionFolder, App.Instance.DefaultDateTimeConverter);
                _storageManager = new StorageManager(fs);

                App.Instance.Init(openFormHotKeyRaised, this, _storageManager);
                LogManager.Trace("Main load started. inited");
                App.Instance.OnSettingsChanged += Instance_OnSettingsChanged;
                App.Instance.OnNotificationCountChanged += Instance_OnNotificationCountChanged;

                RegistryKey appRegistry = Registry.CurrentUser.CreateSubKey(AppConstants.AppName);
                if (appRegistry != null)
                {
                    var firstRunKeyVal = appRegistry.GetValue(AppConstants.Registry.FirstRun);
                    _isFirstRun = firstRunKeyVal == null;
                    if (_isFirstRun)
                    {
                        LogManager.Trace("First run");
                        App.Instance.UserID = Guid.NewGuid();
                        appRegistry.SetValue(AppConstants.Registry.FirstRun, App.Instance.UserID);
                        LogManager.Trace("Value to registry has been set");
                    }
                    else
                    {
                        LogManager.Trace("Second or more run. id : " + firstRunKeyVal);
                        App.Instance.UserID = new Guid(firstRunKeyVal.ToString());
                    }
                }

                var screen = Screen.FromPoint(this.Location);
                this.tsCheckAllAsDisplayed.Visible = false;
                this.ShowInTaskbar = false;
                this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);

                InitBot((continueClbck) =>
                {
                    var changedSettingComponents = _bot.GetIncompatibleSettingComponents();
                    LogManager.Trace("changed component count : " + changedSettingComponents.Count);
                    this.Invoke(new MethodInvoker(delegate
                    {
                        if (_isFirstRun || changedSettingComponents.Any())
                        {
                            var settingForm = new SettingsForm();
                            settingForm.ChangedComponents = changedSettingComponents;
                            settingForm.Closed += (s, ev) =>
                            {
                                Init();
                                continueClbck();
                            };
                            settingForm.ShowDialog();
                        }
                        else
                        {
                            Init();
                            continueClbck();
                        }
                    }));

                });
                SetupNotifyOffMode();

            }
            catch (Exception ex)
            {
                LogManager.Error(ex, "MainForm_Load error");
            }
        }

        private void Init()
        {
            this.Deactivate += MainForm_Deactivate;
            this.Activated += MainForm_Activated;
            this.tsDonate.Visible = App.Instance.AppConf.ShowDonateButton;

            _autocomplete = new AutoCompleteControl()
            {
                ParentForm = this,
                DataFilterFunc = GetCommandListByTerm,
                Left = 43,
                Top = 9,
                StartSuggestFrom = 1
            };
            _autocomplete.OnKeyPressed += _autocomplete_OnKeyPressed;
            _autocomplete.OnCommandReceived += autocomplete_OnCommandReceived;
            _autocomplete.HelpShouldBeShown += autocomplete_HelpShouldBeShown;
            _autocomplete.OnTextEmpty += _autocomplete_OnTextEmpty;
            this.Controls.Add(_autocomplete);

            
            this.ToTop(true);
            LogAnalytic();
        }

        void _autocomplete_OnTextEmpty()
        {
            App.Instance.CloseFixedPopup();
            _autocomplete.HidePopup();
            CloseHelpInfo();
        }

        

        void MainForm_Activated(object sender, EventArgs e)
        {
            Debug.WriteLine("MainForm_Activated");
        }

        void autocomplete_HelpShouldBeShown(bool exist, bool forcedByUser, string command)
        {
            if (exist)
            {
                string fcommand;
                string args;
                var foundCommand = _bot.FindModule(command, out fcommand, out args);
                bool closeHelpInfo=true;
                if (forcedByUser)
                {
                    ShowHelpInfo(foundCommand);
                    closeHelpInfo = false;
                }
                else
                {
                    if (App.Instance.AppConf.ShowCommandHelp)
                    {
                        ShowHelpInfo(foundCommand);
                        closeHelpInfo = false;
                    }
                }
                if (closeHelpInfo)
                    CloseHelpInfo();
            }
            else
            {
                CloseHelpInfo();
            }
            
        }
        void autocomplete_OnTextChanged(string text)
        {
            //string args;
            //bool commandReplaceCountExceed;
            //bool closeHelpInfo = true;
            //string command = TryToReplaceCommand(text, out commandReplaceCountExceed);
            
            ////_commandSuggester.Hide();
            //var foundCommand = _bot.FindModule(command, out command, out args);
            //if (foundCommand != null)
            //{
            //    if (foundCommand.Icon != null)
            //        MainIcon.Image = foundCommand.Icon;
                
            //    if (!string.IsNullOrEmpty(args))
            //        _bot.GetArgumentSuggestions(foundCommand, command, args);

            //    if (string.IsNullOrEmpty(args))
            //    {
            //        if (App.Instance.AppConf.ShowCommandHelp)
            //        {
            //            ShowHelpInfo(foundCommand);
            //            closeHelpInfo = false;
            //        }
            //    }
            //    else
            //    {
            //        if (args.Trim() == "?")
            //        {
            //            ShowHelpInfo(foundCommand);
            //            closeHelpInfo = false;
            //        }
            //    }
            //}
            //else
            //{
            //    MainIcon.Image = _defaultIcon;
            //}

            //if (closeHelpInfo)
            //    CloseHelpInfo();

            //if (string.IsNullOrEmpty(text))
            //{
            //    App.Instance.CloseFixedPopup();
            //    CloseHelpInfo();
            //}
        }


        private HelpPopup _helpPopupForm = null;

        private void ShowHelpInfo(ModuleInfoBase command)
        {
            {
                if (_helpPopupForm != null && _helpPopupForm.HelpData.ForCommand == command.SystemName)
                {
                    return;
                }
                CloseHelpInfo();

                var helpForm = new HelpPopup()
                {
                    HelpData = new HelpInfo
                    {
                        Body = command.ToString(false),
                        Icon = command.Icon ?? Resources.monkey_highres_img,
                        Title = "Подсказка по команде \"" + command.GetComponentName() + "\"",
                        ForCommand = command.SystemName
                    }
                };
                _helpPopupForm = helpForm;
                helpForm.FormType = PopupFormType.SuggestCommandInfo;
                helpForm.Init();

                int top = this.Top + this.Height - helpForm.Height; //(_autocomplete.IsPopupOpen?_autocomplete.GetPopupHeight():0);
                helpForm.Top = top;
                helpForm.Left = this.Left - this.Width;
                helpForm.ToTop();

            }
        }

        private void CloseHelpInfo()
        {
            if (_helpPopupForm != null)
            {
                _helpPopupForm.Close();
                _helpPopupForm = null;
            }
        }

        private void ShowHelpInfo()
        {
            LogManager.Trace("Start ShowHelpInfo");
            if (_helpPopupForm != null)
            {
                _helpPopupForm.ToTop();
            }
            LogManager.Trace("End ShowHelpInfo");

        }
        private void HideHelpInfo()
        {
            if (_helpPopupForm != null)
            {
                _helpPopupForm.Hide();
            }
        }
        private void InitBot(Action<Action> afterInitActionClbck=null)
        {
            new Thread(() =>
            {
                try
                {
                    LogManager.Trace("Start InitBot()");

                    SetLoading(true);
                    _bot = new HelloBot(LogManager.Trace,AppConstants.AppVersion, botCommandPrefix: "", rootFolder: App.Instance.ExecutionFolder);
                    
                    _bot.OnModuleErrorOccured +=BotOnModuleOnModuleErrorOccured;
                    _bot.OnErrorOccured+= BotOnGeneralErrorOccured;
                    _bot.OnMessageRecieved += BotOnMessageRecieved;
                    _bot.OnMessageHandled += BotOnMessageHandled;
                    _bot.OnTrayIconSetupRequired += OnTrayIconSetupRequired;
                    _bot.OnTrayIconStateChangeRequested += OnTrayIconStateChangeRequested;
                    _bot.OnTrayBalloonTipRequested += BotOnOnTrayBalloonTipRequested;
                    _bot.SetCurrentLanguage((Language) (int) App.Instance.AppConf.Language);
                    _bot.RegisterComponents(App.Instance.AppConf.SystemData.EnabledModules, App.Instance.AppConf.SystemData.DisabledModules);
                    _bot.RegisterIntegrationClients(App.Instance.AppConf.SystemData.EnabledModules);
                    _bot.OnSuggestRecieved += BotOnSuggestRecieved;
                    _bot.OnComponentRemoved += _bot_OnModuleRemoved;
                    _bot.CanNotifyClient += () => !App.Instance.AppConf.SystemData.DoNotNotify;
                    

                    App.Instance.Bot = _bot;
                    SetLoading(false);
                    if (afterInitActionClbck != null)
                    {
                        afterInitActionClbck(() =>
                        {
                            _bot.RunEventBasedModules();
                            if (App.Instance.AppConf.InstalledAppVersion < AppConstants.AppVersion)
                            {
                                App.Instance.AppConf.InstalledAppVersion = AppConstants.AppVersion;
                                App.Instance.AppConf.Save();
                                App.Instance.ShowFixedPopup("Обновление", "MonkeyJob Tool успешно обновилась до версии " + App.Instance.AppConf.InstalledAppVersion+".\r\n\r\nПриятного пользования, камрад!",null);
                            }
                            ApplyDebugMode(true);
                        });
                    }
                    else
                    {
                        _bot.RunEventBasedModules();
                    }
                    
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex, "InitBot error");
                }
                LogManager.Trace("End InitBot()");

            }).Start();
        }

        private void OnDebugStateChanged(bool changedTo)
        {
            App.Instance.AppConf.DebugModeEnabled = changedTo;
            App.Instance.AppConf.Save();
            ApplyDebugMode(false);
        }

        private void ApplyDebugMode(bool startCheck)
        {
            var enabled = App.Instance.AppConf.DebugModeEnabled;
            this.BackColor = enabled ? Color.OrangeRed : SystemColors.Control;
            _autocomplete.SetBackColor(this.BackColor);

            string msg = string.Format("Режим отладки {0}ключен", enabled ? "в" : "вы");
            if (enabled)
            {
                msg +=
                    ". \r\n\r\nНе забудьте выключить этот режим командой 'debug off', когда он вам перестанет быть нужен, т.к он пишет все события, происходящие в программе в лог файл, что может повлиять на производительность как программы, так и вашей операционной системы.\r\n\r\nЯркая подсветка будет сигнализировать о включенном режиме отладке.";
            }
            else
            {
                msg += ". \r\n\r\nФайл лога лежит в папке с программой и называется log.txt, скиньте его разработчику на Nigrimmist@gmail.com. Спасибо!";
            }
            if (!(!enabled && startCheck))
                App.Instance.ShowFixedPopup("Отладка",msg , null, titleBackgroundColor: Color.OrangeRed);
        }

        void _bot_OnModuleRemoved(string moduleSystemName)
        {
            App.Instance.RemoveModuleFromConfig(moduleSystemName);
        }

        private void BotOnGeneralErrorOccured(Exception ex)
        {
            LogManager.Error(ex);
        }

        private void BotOnOnTrayBalloonTipRequested(Guid moduleId, string title, string body, TimeSpan timeout, TooltipType tooltipType)
        {
            LogManager.Trace("Start BotOnOnTrayBalloonTipRequested()");

            NotifyIcon notifyIcon;
            lock (_trayIconLocker)
            {
                _trayModuleIcons.TryGetValue(moduleId, out notifyIcon);
            }
            if (notifyIcon != null)
            {
                MultithreadHelper.ThreadSafeCall(this, () =>
                {
                    notifyIcon.ShowBalloonTip((int) timeout.TotalMilliseconds, title, body, tooltipType.ToTooltipType());
                });
            }
            LogManager.Trace("End BotOnOnTrayBalloonTipRequested()");

        }


        private void OnTrayIconStateChangeRequested(Guid moduleId, Icon originalIcon, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null)
        {
            LogManager.Trace("Start OnTrayIconStateChangeRequested()");

            if (!string.IsNullOrEmpty(text))
            {
                NotifyIcon notifyIcon;
                lock (_trayIconLocker)
                {
                    _trayModuleIcons.TryGetValue(moduleId, out notifyIcon);
                }
                if (notifyIcon != null)
                {
                    MultithreadHelper.ThreadSafeCall(this, () =>
                    {
                        notifyIcon.Icon = ImageHelper.GetIconWithNotificationCount(text, originalIcon, textColor, backgroundColor, fontSize, fontName, iconBorderColor);
                    });
                }
            }
            LogManager.Trace("End OnTrayIconStateChangeRequested()");

            
        }

        private void OnTrayIconSetupRequired(Guid moduleId, Icon icon, string title)
        {
            LogManager.Trace("Start OnTrayIconSetupRequired()");

            lock (_trayIconLocker)
            {
                _trayModuleIcons.Add(moduleId, new NotifyIcon
                {
                    Text = title,
                    Icon = icon,
                    Visible = true
                });
            }
            LogManager.Trace("End OnTrayIconSetupRequired()");

        }

        private void BotOnModuleOnModuleErrorOccured(Exception exception, ModuleInfoBase module)
        {
            string errorMessage = "Неизвестная ошибка.";
            bool logError = true;
            if (module.ComponentType == ComponentType.Handler)
            {
                errorMessage = "Увы, что-то пошло не так и модуль сломался, попробуйте как-нибудь по другому.";

                if (exception is WebException)
                {
                    logError = false;
                    if (!InternetChecker.IsInternetEnabled())
                    {
                        errorMessage = "Ошибка. Модуль требует подключения к интернета. Пожалуйста, попробуйте ещё раз через некоторое время, когда интернет-соединение появится.";
                    }
                    else
                    {
                        errorMessage = "Увы, какой-то из интернет-сервисов, необходимых для модуля - в данный момент не работает. Пожалуйста, попробуйте чуть позже.";
                    }
                }

                MultithreadHelper.ThreadSafeCall(this, () =>
                {
                    SetLoading(false);
                    App.Instance.ShowInternalPopup("Ошибка модуля", errorMessage, TimeSpan.FromSeconds(10));
                });
               
            }
            else
            {
                if (exception is WebException)
                {
                    if (!InternetChecker.IsInternetEnabled())
                    {
                        logError = false;
                    }
                }
            }

            if (logError && !string.IsNullOrEmpty(module.Author.EmailForLogs) && !string.IsNullOrEmpty(errorMessage))
                EmailHelper.SendModuleErrorEmail(exception, module.Author.EmailForLogs,"MonkeyJob error report for "+module.GetComponentName()+" module");
        }

        
        void Instance_OnNotificationCountChanged(int notificationCount)
        {
            LogManager.Trace(string.Format("Start Instance_OnNotificationCountChanged() notificationCount : {0} ", notificationCount));
            UpdateTrayicon(notificationCount);
            tsCheckAllAsDisplayed.Visible = notificationCount > 0;
            LogManager.Trace(string.Format("End Instance_OnNotificationCountChanged() notificationCount : {0} ", notificationCount));

        }

        private void UpdateTrayicon(int? notificationCount = null)
        {
            LogManager.Trace(string.Format("Start UpdateTrayicon() notificationCount : {0} ", notificationCount));

            if (!notificationCount.HasValue)
                notificationCount = App.Instance.NotificationCount;

            MultithreadHelper.ThreadSafeCall(this, () =>
            {
                trayIcon.Icon =
                        notificationCount > 0
                            ? ImageHelper.GetIconWithNotificationCount(notificationCount.Value.ToString(), GetCurrentClearTrayIcon(), Color.White, Color.OrangeRed, 6, useEllipseAsBackground: true)
                            : GetCurrentClearTrayIcon();
            });
            LogManager.Trace(string.Format("End UpdateTrayicon() notificationCount : {0} ", notificationCount));
        }

        private Icon GetCurrentClearTrayIcon()
        {
            return App.Instance.AppConf.SystemData.DoNotNotify ? Resources.MonkeyJob_16x16_gray : Resources.MonkeyJob_16x16;
        }

        void Instance_OnSettingsChanged()
        {
            tsDonate.Visible = App.Instance.AppConf.ShowDonateButton;
        }

        private DateTime? _lastDeactivateFormDate;
        private bool _isHelpBalloonDisplayed;
        void MainForm_Deactivate(object sender, EventArgs e)
        {
            Debug.WriteLine("MainForm_Deactivate");
            //hack check. Required in case when user click right click to popup to close it. We not hide main form. But hide if another application get focus.
            if (!App.ApplicationIsActivated())
            {
                HideMain();
                App.Instance.HideAllPopupsAvailableForHiding();
                //App.Instance.CloseFixedPopup();
                App.Instance.ReorderPopupsPositions();
                _lastDeactivateFormDate = DateTime.Now;
            }

            if (_isFirstRun && !_isHelpBalloonDisplayed)
            {
                _isHelpBalloonDisplayed = true;
                ShowBalloon(string.Format("{0} рядом.", AppConstants.AppName), string.Format("Как только захочешь увидеть меня, нажми {0} или кликни по иконке в трее. Через правый клик можно зайти в настройки.", App.Instance.AppConf.HotKeys.ProgramOpen));
            }
        }

        void BotOnMessageHandled()
        {
            SetLoading(false);
        }
        void BotOnMessageRecieved(Guid? commandToken,AnswerInfo answerInfo, ClientCommandContext clientCommandContext)
        {
            LogManager.Trace(string.Format("Start BotOnMessageRecieved() commandToken : {0} ", commandToken));

                var answer = answerInfo.Answer.ToString();
                var answerType = answerInfo.AnswerType;
                
                string title = string.IsNullOrEmpty(answerInfo.Title) ? answerInfo.CommandName : answerInfo.Title;
                if (answerInfo.MessageSourceType == ComponentType.Event)
                {
                    //do not show already exist popup for events
                    if (App.Instance.NotificationPopupExist(answerInfo.Answer.ToString(), answerInfo.Title))
                    {
                        return;
                    }
                }

                if (answerType == AnswerBehaviourType.CopyToClipBoard || (clientCommandContext != null && clientCommandContext.IsToBuffer))
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        App.Instance.CopyToClipboard(title, answer);
                    }));
                }
                else
                {
                    if (answerType == AnswerBehaviourType.OpenLink)
                    {
                        if (answer.StartsWith("http://") || answer.StartsWith("https://"))
                        {
                            LogManager.Trace(string.Format("Start BotOnMessageRecieved().openLink commandToken : {0}, answer : {1} ", commandToken, answer));

                            Process.Start(answer);
                            //close fixed popup with previous answer if exist
                            this.Invoke(new MethodInvoker(delegate
                            {
                                App.Instance.CloseFixedPopup();
                            }));
                        }
                    }
                    else if (answerType == AnswerBehaviourType.ShowText)
                    {
                        if (!string.IsNullOrEmpty(answer))
                        {
                            
                            this.Invoke((MethodInvoker) (delegate
                            {

                                var mainFormActive = App.ApplicationIsActivated();
                                if (answerInfo.MessageSourceType == ComponentType.Handler)
                                {
                                    if (mainFormActive)
                                        App.Instance.ShowFixedPopup(title, answer, commandToken, answerInfo.Icon, answerInfo.HeaderBackgroundColor, answerInfo.BodyBackgroundColor);
                                    else
                                        App.Instance.ShowNotification(title, answer, commandToken, answerInfo.Icon, answerInfo.HeaderBackgroundColor, answerInfo.BodyBackgroundColor);
                                }
                                else if (answerInfo.MessageSourceType == ComponentType.Event || answerInfo.MessageSourceType == ComponentType.IntegrationClient)
                                {
                                    App.Instance.ShowNotification(title, answer, commandToken, answerInfo.Icon, answerInfo.HeaderBackgroundColor, answerInfo.BodyBackgroundColor);
                                }
                                else
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                            }));

                        }
                    }
                }
                LogManager.Trace(string.Format("End BotOnMessageRecieved() commandToken : {0} ", commandToken));

        }

        
        private string TryToReplaceCommand(string command, out bool replaceCountExceed)
        {
            LogManager.Trace(string.Format("Start TryToReplaceCommand(). command : {0}", command));

            int replaceChainCount = 0;
            var commandReplacesToUse = new List<CommandReplace>();
            commandReplacesToUse.AddRange(App.Instance.AppConf.CommandReplaces);
            replaceCountExceed = false;
            while (true)
            {
                string toReturn = command;
                var bestMatchReplace = commandReplacesToUse.Where(x => command.StartsWith(x.From + " ") || (command.StartsWith(x.From) && command.Length == x.From.Length)).OrderByDescending(x => x.From.Length).FirstOrDefault();
                if (bestMatchReplace != null)
                {
                    commandReplacesToUse.Remove(bestMatchReplace);
                    var args = command.Substring(bestMatchReplace.From.Length);
                    if (args.Length == 0 || args.StartsWith(" ")) //not part of other word
                    {
                        var commandArgs = args.Trim().Split(new[] {"=>"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                        var replaceCount = Regex.Matches(bestMatchReplace.To, @"({\d+})").Count;

                        if (commandArgs.Count < replaceCount)
                        {
                            commandArgs.AddRange(Enumerable.Repeat("", replaceCount - commandArgs.Count));
                        }

                        toReturn = string.Format(bestMatchReplace.To, commandArgs.ToArray());

                        if (replaceCount == 0)
                        {
                            toReturn = bestMatchReplace.To + args;
                        }
                    }
                    command = toReturn;
                    if (replaceChainCount++ < 20)
                        continue;
                    else
                        replaceCountExceed = true;
                }
                LogManager.Trace(string.Format("End TryToReplaceCommand(). command : {0}", command));

                return toReturn;
            }

        }

        void autocomplete_OnCommandReceived(string command)
        {
            LogManager.Trace(string.Format("Start autocomplete_OnCommandReceived() command : {0}", command));

            if (!_formBusy)
            {
                _autocomplete.HidePopup();
                bool commandReplaceCountExceed;

                command = TryToReplaceCommand(command, out commandReplaceCountExceed);

                if (commandReplaceCountExceed)
                {
                    App.Instance.ShowFixedPopup(AppConstants.AppName, "Обнаружено зацикливание в заменах команд, проверьте их на корректность.", null);
                }
                else
                {
                    if (command.ToLower().Trim() == "debug on" || command.ToLower().Trim() == "debug off")
                    {
                        OnDebugStateChanged(command.ToLower().Trim().EndsWith("on"));
                        return;
                    }

                    bool toBuffer = false;
                    if (command.Trim().EndsWith(_copyToBufferPostFix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        command = command.Substring(0, command.Length - _copyToBufferPostFix.Length);
                        toBuffer = true;
                    }
                    SetLoading(true);


                    if (!_bot.HandleMessage(command, new ClientCommandContext() { IsToBuffer = toBuffer }, !App.Instance.AppConf.DevelopmentModeEnabled))
                    {
                        App.Instance.ShowFixedPopup(AppConstants.AppName, "Команда не найдена", null);

                        SetLoading(false);
                    }

                }
            }
            LogManager.Trace(string.Format("End autocomplete_OnCommandReceived() command : {0}", command));

        }

        private DataFilterInfo GetCommandListByTerm(string term, bool strongSearch)
        {
            LogManager.Trace(string.Format("Start GetCommandListByTerm(). term : {0}, strongSearch : {1}", term, strongSearch));

            var foundItems = _bot.FindCommands(term, strongSearch);
            LogManager.Trace(string.Format("End GetCommandListByTerm(). term : {0}, strongSearch : {1}", term, strongSearch));
            
            return new DataFilterInfo()
            {
                FoundByTerm = term,
                FoundCommands = foundItems
            };
        }

        void openFormHotKeyRaised(object sender, KeyPressedEventArgs e)
        {
            ShowMain();
        }

        
        
        private void tsExit_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Environment.Exit(Environment.ExitCode);
            //todo : do not terminate all child threads. think about how to wait about few secs after notify them and emulate programm close
        }

        private void tsSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }

        private Image _preLoadingImg;
        private bool _formBusy = false;
        private void SetLoading(bool isLoading)
        {
            LogManager.Trace("Start SetLoading()");

            if (isLoading)
                _preLoadingImg = MainIcon.Image;
            _formBusy = isLoading;
            MainIcon.Image = isLoading ? _loadingIcon : _preLoadingImg ?? _defaultIcon;
            LogManager.Trace("End SetLoading()");

        }

        private void _autocomplete_OnKeyPressed(Keys key,KeyEventArgs e)
        {
            switch (key)
            {
                case Keys.Escape:
                {
                    HideMain();
                    App.Instance.CloseFixedPopup();
                    App.Instance.ReorderPopupsPositions();
                    break;
                }
                default:
                {
                    break;
                }
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                if (!_autocomplete.IsTextSelected())
                {
                    var fixedAnswerPopup = App.Instance.GetOpenedFixedPopup();
                    if (fixedAnswerPopup != null)
                    {
                        App.Instance.CopyToClipboard("Буфер обмена", fixedAnswerPopup.Text);
                        e.SuppressKeyPress = true;
                    }
                    
                }
            }
        }

        private void HideMain()
        {
            LogManager.Trace("Start HideMain()");
            this.Hide();
            _autocomplete.HidePopup();
            App.Instance.HideAllPopupsAvailableForHiding();
            HideHelpInfo();
            LogManager.Trace("End HideMain()");
        }
        private void trayIcon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowMain();
            }
        }

        void ShowMain()
        {
            LogManager.Trace("Start ShowMain()");
            //do not show old commands after min of inactivity
            if (_lastDeactivateFormDate.HasValue && DateTime.Now - _lastDeactivateFormDate.Value > App.Instance.AppConf.SystemData.ClearCommandAfterMinOfInactivity)
            {
                LogManager.Trace("Clearing autocomplete");

                _autocomplete.Clear();
                App.Instance.CloseFixedPopup();
                _lastDeactivateFormDate = null;
            }

            
            App.Instance.AllPopupsToTop();
            ShowHelpInfo();
            this.ToTop(true);
            App.Instance.ReorderPopupsPositions();
            
            if (!App.Instance.AnyPopupExist())
            {
                if (_autocomplete.IsOneOfPopupsOpen)
                {
                    _autocomplete.PopupToTop();
                }
            }
            
            LogManager.Trace("End ShowMain()");

        }

        private void LogAnalytic()
        {
            if (App.Instance.AppConf.AllowUsingGoogleAnalytics)
            {
                new Thread(() =>
                {
                    try
                    {
                        LogManager.Trace("Start log analytics");
                        DateTime lastLogDate = DateTime.Today;

                        RegistryKey appRegistry = Registry.CurrentUser.CreateSubKey(AppConstants.AppName);
                        var lastStatsCollectedKey = appRegistry.GetValue(AppConstants.Registry.LastStatsCollectedDateKey);
                        if (_isFirstRun)
                        {
                            GoogleAnalytics.LogFirstUse();
                            GoogleAnalytics.LogRun();
                        }
                        if (lastStatsCollectedKey != null)
                        {
                            lastLogDate = DateTime.ParseExact(lastStatsCollectedKey.ToString(), AppConstants.DateTimeFormat, null);
                        }

                        if (DateTime.Today > lastLogDate)
                            GoogleAnalytics.LogRun();


                        appRegistry.SetValue(AppConstants.Registry.LastStatsCollectedDateKey, DateTime.Now.Date.ToString(AppConstants.DateTimeFormat));
                        LogManager.Trace("End log analytics");
                    }
                    catch (Exception ex)
                    {
                        LogManager.Error(ex,"LogAnalytic");
                    }
                }).Start();
            }
        }

        public void HandleCommandInfoPopupClose(Guid? commandToken, ClosePopupReasonType closeReason)
        {
            if(commandToken!=null)
                _bot.HandleUserReactionToCommand(commandToken.Value, closeReason.ToUserReactonType());
        }


        private void ShowBalloon(string title, string body)
        {
            if (title != null)
            {
                trayIcon.BalloonTipTitle = title;
            }

            if (body != null)
            {
                trayIcon.BalloonTipText = body;
            }

            trayIcon.ShowBalloonTip(30000);
        }

        

        private void tsCheckAllAsDisplayed_Click(object sender, EventArgs e)
        {
            App.Instance.CheckAllNotificationAsRead();
        }

        private void tsDonate_Click(object sender, EventArgs e)
        {
            new DonateListForm().ShowDialog();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            LogManager.Trace("Start MainForm_Paint()");
            int borderSize = 1;

            using (Pen p = new Pen(Color.Black, borderSize))
            {
                e.Graphics.DrawLine(p, 0, 0, 0, this.ClientSize.Height);
                e.Graphics.DrawLine(p, 0, 0, this.ClientSize.Width, 0);
                e.Graphics.DrawLine(p, this.ClientSize.Width - borderSize, 0, this.ClientSize.Width - borderSize, this.ClientSize.Height - borderSize);
                e.Graphics.DrawLine(p, this.ClientSize.Width - borderSize, this.ClientSize.Height - borderSize, 0, this.ClientSize.Height - borderSize);
            }
            LogManager.Trace("End MainForm_Paint()");
        }
        

        private void tsNotificationOff_Click(object sender, EventArgs e)
        {
            App.Instance.AppConf.SystemData.DoNotNotify = !App.Instance.AppConf.SystemData.DoNotNotify;
            App.Instance.AppConf.Save();
            SetupNotifyOffMode();
        }

        private void SetupNotifyOffMode()
        {
            if (App.Instance.AppConf.SystemData.DoNotNotify)
            {
                tsNotificationOff.Image = Resources.NotificationOn;
                tsNotificationOff.Text = "Беспокоить";
                App.Instance.SetAllPopupsAsNotified();
                App.Instance.HideAllPopupsAvailableForHiding();
            }
            else
            {
                tsNotificationOff.Image = Resources.NotificationOff;
                tsNotificationOff.Text = "Не беспокоить";
            }
            UpdateTrayicon();
        }

        void BotOnSuggestRecieved(List<AutoSuggestItem> obj)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                _autocomplete.ShowArguments(obj);
            }));
            
        }
    }
}
