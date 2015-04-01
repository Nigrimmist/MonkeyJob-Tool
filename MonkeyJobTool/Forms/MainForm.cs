using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
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
using MonkeyJobTool.Properties;
using MonkeyJobTool.Utilities;
using Newtonsoft.Json;
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

                var screen = Screen.FromPoint(this.Location);
                this.tsCheckAllAsDisplayed.Visible = false;
                this.ShowInTaskbar = false;
                this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
                this.Deactivate += MainForm_Deactivate;
                this.tsDonate.Visible = App.Instance.AppConf.ShowDonateButton;

                RegistryKey appRegistry = Registry.CurrentUser.CreateSubKey(AppConstants.AppName);
                if (appRegistry != null)
                {
                    var lastStatsCollectedKey = appRegistry.GetValue(AppConstants.Registry.LastStatsCollectedDateKey);
                    _isFirstRun = lastStatsCollectedKey == null;
                }
                
                _bot = new HelloBot(App.Instance.ExecutionFolder + "ModuleSettings", AppConstants.AppVersion, botCommandPrefix: "", moduleFolderPath: App.Instance.ExecutionFolder);
                _bot.OnErrorOccured += BotOnOnErrorOccured;
                _bot.OnMessageRecieved += BotOnMessageRecieved;
                _bot.SetCurrentLanguage((Language)(int)App.Instance.AppConf.Language);

                _autocomplete = new AutoCompleteControl()
                {
                    ParentForm = this,
                    DataFilterFunc = GetCommandListByTerm,
                    Left = 43,
                    Top = 9
                };
                _autocomplete.OnKeyPressed += _autocomplete_OnKeyPressed;
                _autocomplete.OnCommandReceived += autocomplete_OnCommandReceived;

                this.Controls.Add(_autocomplete);
                this.ToTop(true);
                
                LogAnalytic();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        void Instance_OnNotificationCountChanged(int notificationCount)
        {
            trayIcon.Icon = notificationCount > 0 ? GetIconWithNotificationCount(notificationCount) : Resources.MonkeyJob_ico;
            tsCheckAllAsDisplayed.Visible = notificationCount > 0;
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
                App.Instance.CloseFixedPopup();
                App.Instance.ReorderPopupsPositions();
            }

            if (_isFirstRun && !_isHelpBalloonDisplayed)
            {
                _isHelpBalloonDisplayed = true;
                ShowBalloon(string.Format("{0} рядом.", AppConstants.AppName), string.Format("Как только захочешь увидеть меня, нажми {0} или кликни по иконке в трее.", App.Instance.AppConf.HotKeys.ProgramOpen));
            }
        }


        void BotOnMessageRecieved(Guid commandToken,AnswerInfo answerInfo, ClientCommandContext clientCommandContext)
        {
                string answer = answerInfo.Answer;
                var answerType = answerInfo.AnswerType;
                SetLoading(false);
                if (clientCommandContext != null && clientCommandContext.IsToBuffer)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Clipboard.SetText(answer);
                        App.Instance.ShowInternalPopup("Результат команды скопирован в буфер обмена", TimeSpan.FromSeconds(2));
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
                            App.Instance.CloseFixedPopup();
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
                                        App.Instance.ShowFixedPopup(answerInfo.CommandName, answer, commandToken);
                                        break;
                                    case ModuleType.Event:
                                        App.Instance.ShowNotification(answerInfo.CommandName, answer,commandToken);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }));

                        }
                    }
                }
        }

        private string TryToReplaceCommand(string command)
        {
            string toReturn = command;
            var bestMatchReplace = App.Instance.AppConf.CommandReplaces.Where(x => command.StartsWith(x.From)).OrderByDescending(x=>x.From.Length).FirstOrDefault();
            if (bestMatchReplace != null)
            {
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
            }
            return toReturn;
        }
        
        void autocomplete_OnCommandReceived(string command)
        {
            command = TryToReplaceCommand(command);

            bool toBuffer = false;
            if (command.Trim().EndsWith(_copyToBufferPostFix, StringComparison.InvariantCultureIgnoreCase))
            {
                command = command.Substring(0, command.Length - _copyToBufferPostFix.Length);
                toBuffer = true;
            }
            SetLoading(true);

            if (!_bot.HandleMessage(command, new ClientCommandContext(){IsToBuffer = toBuffer}))
            {
                SetLoading(false);
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

        private void BotOnOnErrorOccured(Exception exception)
        {
           
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

        private void SetLoading(bool isLoading)
        {
            MainIcon.Image = isLoading ? _loadingIcon : _defaultIcon;
        }

        private void _autocomplete_OnKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                {
                    HideMain();
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
            if (_autocomplete != null && _autocomplete.IsPopupOpen)
            {
                _autocomplete.PopupToTop();
            }
            App.Instance.AllPopupsToTop();
            this.ToTop(true);

            if (_autocomplete != null)
                _autocomplete.SelectAllText();

            App.Instance.ReorderPopupsPositions();
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

        public static Icon GetIconWithNotificationCount(int notificationCount)
        {
            Icon createdIcon;

            using (Bitmap bitmap = new Bitmap(32, 32))
            {
                Icon icon = Resources.MonkeyJob_ico;
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (SolidBrush drawBrush = new SolidBrush(Color.White))
                    {
                        using (Font drawFont = new Font("Comic Sans MS", 14, FontStyle.Regular))
                        {
                            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                            graphics.DrawIcon(icon, 0, 0);
                            graphics.FillEllipse(new SolidBrush(Color.OrangeRed), 10,12,22,21);
                            graphics.DrawString(notificationCount.ToString(), drawFont, drawBrush, 14, 10);
                            createdIcon = Icon.FromHandle(bitmap.GetHicon());
                        }
                    }
                }
            }

            return createdIcon;
        }

        private void tsCheckAllAsDisplayed_Click(object sender, EventArgs e)
        {
            App.Instance.CheckAllNotificationAsRead();
        } 
    }
}
