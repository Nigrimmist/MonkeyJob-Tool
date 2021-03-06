﻿using System;
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
using HelloBotCore.Entities;
using Microsoft.Win32;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Helpers;
using MonkeyJobTool.Managers;
using MonkeyJobTool.Properties;
using Newtonsoft.Json;
using SharedHelper;
using Language = HelloBotCore.Entities.Language;

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

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                App.Instance.Init(openFormHotKeyRaised, this);
                App.Instance.OnSettingsChanged += Instance_OnSettingsChanged;
                App.Instance.OnNotificationCountChanged += Instance_OnNotificationCountChanged;

                RegistryKey appRegistry = Registry.CurrentUser.CreateSubKey(AppConstants.AppName);
                if (appRegistry != null)
                {
                    var firstRunKeyVal = appRegistry.GetValue(AppConstants.Registry.FirstRun);
                    _isFirstRun = firstRunKeyVal == null;
                    if (_isFirstRun)
                    {
                        App.Instance.UserID = Guid.NewGuid();
                        appRegistry.SetValue(AppConstants.Registry.FirstRun, App.Instance.UserID);
                    }
                    else
                    {
                        App.Instance.UserID = new Guid(firstRunKeyVal.ToString());
                    }
                }

                var screen = Screen.FromPoint(this.Location);
                this.tsCheckAllAsDisplayed.Visible = false;
                this.ShowInTaskbar = false;
                this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);

                InitBot((continueClbck) =>
                {
                    var changedSettingModules = _bot.GetIncompatibleSettingModules();
                    this.Invoke(new MethodInvoker(delegate
                    {
                        if (_isFirstRun || changedSettingModules.Any())
                        {
                            var settingForm = new SettingsForm();
                            settingForm.ChangedModules = changedSettingModules;
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
            _autocomplete.OnTextChanged += _autocomplete_OnTextChanged;

            this.Controls.Add(_autocomplete);
            this.ToTop(true);
            LogAnalytic();
        }

        void _autocomplete_OnTextChanged(string text)
        {
            string args;
            bool commandReplaceCountExceed;
            string command = TryToReplaceCommand(text, out commandReplaceCountExceed);

            var foundCommand = _bot.FindModule(command, out command, out args);
            if (foundCommand != null && foundCommand.Icon!=null)
            {
                MainIcon.Image = foundCommand.Icon;
            }
            else
            {
                MainIcon.Image = _defaultIcon;
            }
            if (string.IsNullOrEmpty(text))
            {
                App.Instance.CloseFixedPopup();
            }
        }


        private void InitBot(Action<Action> afterInitActionClbck=null)
        {
            new Thread(() =>
            {
                try
                {   
                    SetLoading(true);
                    _bot = new HelloBot(App.Instance.FolderSettingPath, AppConstants.AppVersion, botCommandPrefix: "", moduleFolderPath: App.Instance.ExecutionFolder);
                    _bot.OnErrorOccured +=BotOnOnErrorOccured;
                    _bot.OnMessageRecieved += BotOnMessageRecieved;
                    _bot.OnTrayIconSetupRequired += OnTrayIconSetupRequired;
                    _bot.OnTrayIconStateChangeRequested += OnTrayIconStateChangeRequested;
                    _bot.OnTrayBalloonTipRequested += BotOnOnTrayBalloonTipRequested;
                    _bot.SetCurrentLanguage((Language) (int) App.Instance.AppConf.Language);
                    _bot.RegisterModules(App.Instance.AppConf.SystemData.DisabledModules);
                    
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
            }).Start();
        }

        private void BotOnOnTrayBalloonTipRequested(Guid moduleId, string title, string body, TimeSpan timeout, TooltipType tooltipType)
        {
            NotifyIcon notifyIcon;
            lock (_trayIconLocker)
            {
                _trayModuleIcons.TryGetValue(moduleId, out notifyIcon);
            }
            if (notifyIcon != null)
            {
                Action act = () => notifyIcon.ShowBalloonTip((int)timeout.TotalMilliseconds, title, body, tooltipType.ToTooltipType());
                if (this.InvokeRequired)
                    this.Invoke(act);
                else
                    act();
            }
        }


        private void OnTrayIconStateChangeRequested(Guid moduleId, Icon originalIcon, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null)
        {
            if (!string.IsNullOrEmpty(text))
            {
                NotifyIcon notifyIcon;
                lock (_trayIconLocker)
                {
                    _trayModuleIcons.TryGetValue(moduleId, out notifyIcon);
                }
                if (notifyIcon != null)
                {
                    Action act = () => notifyIcon.Icon = ImageHelper.GetIconWithNotificationCount(text, originalIcon,textColor,backgroundColor,fontSize,fontName,iconBorderColor);
                    if (this.InvokeRequired)
                        this.Invoke(act);
                    else
                        act();
                }
            }
            
        }

        private void OnTrayIconSetupRequired(Guid moduleId, Icon icon, string title)
        {
            lock (_trayIconLocker)
            {
                _trayModuleIcons.Add(moduleId, new NotifyIcon
                {
                    Text = title,
                    Icon = icon,
                    Visible = true
                });
            }
        }

        private void BotOnOnErrorOccured(Exception exception, ModuleInfoBase module)
        {
            string errorMessage = "Неизвестная ошибка.";
            bool logError = true;
            if (module.ModuleType == ModuleType.Handler)
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
                Action act = () =>
                {
                    SetLoading(false);
                    App.Instance.ShowInternalPopup("Ошибка модуля", errorMessage, TimeSpan.FromSeconds(10));

                };
                if (this.InvokeRequired)
                {
                    this.Invoke(act);
                }
                else act();
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
                EmailHelper.SendModuleErrorEmail(exception, module.Author.EmailForLogs,"MonkeyJob error report for "+module.GetModuleName()+" module");
        }

        
        void Instance_OnNotificationCountChanged(int notificationCount)
        {
            UpdateTrayicon(notificationCount);
            tsCheckAllAsDisplayed.Visible = notificationCount > 0;
        }

        private void UpdateTrayicon(int? notificationCount = null)
        {
            if (!notificationCount.HasValue)
                notificationCount = App.Instance.NotificationCount;
            Action act =
                () =>
                    trayIcon.Icon =
                        notificationCount > 0
                            ? ImageHelper.GetIconWithNotificationCount(notificationCount.Value.ToString(), GetCurrentClearTrayIcon(), Color.White, Color.OrangeRed, 6, useEllipseAsBackground: true)
                            : GetCurrentClearTrayIcon();
            if (InvokeRequired)
                Invoke(act);
            else
                act();


        }

        private Icon GetCurrentClearTrayIcon()
        {
            return App.Instance.AppConf.SystemData.DoNotNotify ? Resources.MonkeyJob_16x16_gray : Resources.MonkeyJob_16x16;
        }

        void Instance_OnSettingsChanged()
        {
            tsDonate.Visible = App.Instance.AppConf.ShowDonateButton;
        }

        private bool _isHelpBalloonDisplayed;
        void MainForm_Deactivate(object sender, EventArgs e)
        {
            //hack check. Required in case when user click right click to popup to close it. We not hide main form. But hide if another application get focus.
            if (!App.ApplicationIsActivated())
            {
                HideMain();
                App.Instance.HideAllPopupsAvailableForHiding();
                //App.Instance.CloseFixedPopup();
                App.Instance.ReorderPopupsPositions();
            }

            if (_isFirstRun && !_isHelpBalloonDisplayed)
            {
                _isHelpBalloonDisplayed = true;
                ShowBalloon(string.Format("{0} рядом.", AppConstants.AppName), string.Format("Как только захочешь увидеть меня, нажми {0} или кликни по иконке в трее. Через правый клик можно зайти в настройки.", App.Instance.AppConf.HotKeys.ProgramOpen));
            }
        }

        void BotOnMessageRecieved(Guid commandToken,AnswerInfo answerInfo, ClientCommandContext clientCommandContext)
        {
                string answer = answerInfo.Answer;
                var answerType = answerInfo.AnswerType;
                SetLoading(false);
                string title = string.IsNullOrEmpty(answerInfo.Title) ? answerInfo.CommandName : answerInfo.Title;
                if (answerInfo.MessageSourceType == ModuleType.Event)
                {
                    //do not show already exist popup for events
                    if (App.Instance.NotificationPopupExist(answerInfo.Answer, answerInfo.Title))
                    {
                        return;
                    }
                }

                if (answerType == AnswerBehaviourType.CopyToClipBoard || (clientCommandContext != null && clientCommandContext.IsToBuffer))
                {
                    
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Clipboard.SetText(answer);
                        App.Instance.ShowInternalPopup(title,"Результат команды скопирован в буфер обмена", TimeSpan.FromSeconds(3));
                    }));
                }
                else
                {
                    if (answerType == AnswerBehaviourType.OpenLink)
                    {
                        if (answer.StartsWith("http://") || answer.StartsWith("https://"))
                        {
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
                                switch (answerInfo.MessageSourceType)
                                {
                                    case ModuleType.Handler:
                                        App.Instance.ShowFixedPopup(title, answer, commandToken, answerInfo.Icon, answerInfo.HeaderBackgroundColor, answerInfo.BodyBackgroundColor);
                                        break;
                                    case ModuleType.Event:
                                        App.Instance.ShowNotification(title, answer, commandToken, answerInfo.Icon, answerInfo.HeaderBackgroundColor, answerInfo.BodyBackgroundColor);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }));

                        }
                    }
                }
        }

        private string TryToReplaceCommand(string command, out bool replaceCountExceed)
        {
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
                return toReturn;
                break;
            }
        }

        void autocomplete_OnCommandReceived(string command)
        {
            Debug.WriteLine(_formBusy);
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
                    bool toBuffer = false;
                    if (command.Trim().EndsWith(_copyToBufferPostFix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        command = command.Substring(0, command.Length - _copyToBufferPostFix.Length);
                        toBuffer = true;
                    }
                    SetLoading(true);


                    if (!_bot.HandleMessage(command, new ClientCommandContext() {IsToBuffer = toBuffer}))
                    {
                        App.Instance.ShowFixedPopup(AppConstants.AppName, "Команда не найдена", null);

                        SetLoading(false);
                    }

                }
            }
        }

        

        private DataFilterInfo GetCommandListByTerm(string term)
        {
           var foundItems = _bot.FindCommands(term).Select(x => x.Command.ToLower()).ToList();
            
            return new DataFilterInfo()
            {
                FoundByTerm = term,
                FoundItems = foundItems
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
            if (isLoading)
                _preLoadingImg = MainIcon.Image;
            _formBusy = isLoading;
            MainIcon.Image = isLoading ? _loadingIcon : _preLoadingImg ?? _defaultIcon;
        }

        private void _autocomplete_OnKeyPressed(Keys key)
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
        }

        private void HideMain()
        {
            this.Hide();
            _autocomplete.HidePopup();
            App.Instance.HideAllPopupsAvailableForHiding();
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
            App.Instance.AllPopupsToTop();
            this.ToTop(true);
            App.Instance.ReorderPopupsPositions();

            if (!App.Instance.AnyPopupExist())
            {
                if (_autocomplete.IsPopupOpen)
                {
                    _autocomplete.PopupToTop();
                }
            }
            _autocomplete.SelectAllText();
        }

        private void LogAnalytic()
        {
            if (App.Instance.AppConf.AllowUsingGoogleAnalytics)
            {
                new Thread(() =>
                {
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

                }).Start();
            }
        }

        public void HandleCommandInfoPopupClose(Guid? commandToken, ClosePopupReasonType closeReason)
        {
            if(commandToken!=null)
                _bot.HandleUserReactionToCommand(commandToken.Value, (UserReactionToCommandType)((int)closeReason));
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
            int borderSize = 1;

            using (Pen p = new Pen(Color.Black, borderSize))
            {
                e.Graphics.DrawLine(p, 0, 0, 0, this.ClientSize.Height);
                e.Graphics.DrawLine(p, 0, 0, this.ClientSize.Width, 0);
                e.Graphics.DrawLine(p, this.ClientSize.Width - borderSize, 0, this.ClientSize.Width - borderSize, this.ClientSize.Height - borderSize);
                e.Graphics.DrawLine(p, this.ClientSize.Width - borderSize, this.ClientSize.Height - borderSize, 0, this.ClientSize.Height - borderSize);
            }
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
    }
}
