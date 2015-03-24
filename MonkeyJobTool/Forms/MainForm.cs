using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using MonkeyJobTool.Entities.Json;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Helpers;
using MonkeyJobTool.Utilities;
using Newtonsoft.Json;

namespace MonkeyJobTool.Forms
{
    
    public partial class MainForm : Form
    {
        private HelloBot _bot;
        private const string _copyToBufferPostFix = " в буфер";
        private AutoCompleteControl _autocomplete;
        private Bitmap defaultIcon = Properties.Resources.monkey_highres;
        private Bitmap loadingIcon = Properties.Resources.loading;
        

        public MainForm()
        {
           InitializeComponent();
           this.MainIcon.Image =
           this.tsExit.Image = defaultIcon;
           this.tsSettings.Image = Properties.Resources.settings;
           
            //Process[] processlist = Process.GetProcesses();

            //foreach (Process process in processlist)
            //{
            //    if (!String.IsNullOrEmpty(process.MainWindowTitle))
            //    {
            //        Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
            //    }
            //}

        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            
            try
            {
                App.Instance.Init(openFormHotKeyRaised, this);
                _bot = new HelloBot(App.Instance.ExecutionFolder+"ModuleSettings",botCommandPrefix: "",moduleFolderPath : App.Instance.ExecutionFolder);
                //MessageBox.Show(App.Instance.AppConf.SystemData.LastUpdateCheckDate.ToString());
                this.ShowInTaskbar = false;
                _bot.OnErrorOccured += BotOnOnErrorOccured;
                _bot.OnMessageRecieved += BotOnMessageRecieved;

                var screen = Screen.FromPoint(this.Location);
                this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);

                _autocomplete = new AutoCompleteControl()
                {
                    ParentForm = this,
                    DataFilterFunc = GetCommandListByTerm,
                    Left = 43,
                    Top = 8
                };
                _autocomplete.OnKeyPressed += _autocomplete_OnKeyPressed;
                _autocomplete.OnCommandReceived += autocomplete_OnCommandReceived;
                this.Controls.Add(_autocomplete);
                this.ToTop();

                CheckNewVersion();
                LogAnalytic();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                        App.Instance.ShowPopup("Скопировано в буфер обмена", TimeSpan.FromSeconds(2));
                    }));
                }
                else
                {
                    if (answerType == AnswerBehaviourType.OpenLink)
                    {
                        if (answer.StartsWith("http://") || answer.StartsWith("https://"))
                            Process.Start(answer);
                    }
                    else if (answerType == AnswerBehaviourType.ShowText)
                    {
                        if (!string.IsNullOrEmpty(answer))
                        {
                            this.Invoke((MethodInvoker) (delegate
                            {
                                App.Instance.ShowPopup(answerInfo.CommandName, answer, null, commandToken);
                                this.ToTop();
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
            if (_autocomplete != null && _autocomplete.IsPopupOpen)
            {
                _autocomplete.PopupToTop();
            }
            App.Instance.AllPopupsToTop();
            this.ToTop();
            if (_autocomplete != null)
                _autocomplete.SelectAllText();
        }

        private void BotOnOnErrorOccured(Exception exception)
        {
           
        }
        
        private void tsExit_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Environment.Exit(Environment.ExitCode);
            //Application.Exit();
        }

        private void tsSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }

        private void SetLoading(bool isLoading)
        {
            MainIcon.Image = isLoading ? loadingIcon : defaultIcon;
        }

        private void _autocomplete_OnKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                {
                    this.Hide();
                    _autocomplete.HidePopup();
                    App.Instance.CloseAllPopups();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        
        private void trayIcon_Click(object sender, EventArgs e)
        {
            
        }

        private void trayIcon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.ToTop();
            }
        }

        //todo : rewrite to module
        private void CheckNewVersion()
        {
            if (App.Instance.AppConf.SystemData.LastUpdateCheckDate.AddDays(3) <= DateTime.Now)
            {
                App.Instance.AppConf.SystemData.LastUpdateCheckDate = DateTime.Now.Date;
                App.Instance.AppConf.Save();

                HtmlReaderManager hrm = new HtmlReaderManager();
                hrm.Get(string.Format(AppConstants.Urls.LatestVersionFileUrlFormat, App.Instance.AppConf.Language));
                string versionJson = hrm.Html;
                AppVersionInfo info = JsonConvert.DeserializeObject<AppVersionInfo>(versionJson);
                var versions = info.Versions.OrderBy(x => x.Version);
                var latestVersion = versions.Last();
                if (latestVersion.Version > App.Instance.AppConf.SystemData.Version)
                {
                    App.Instance.ShowPopup("Вышла новая версия!", "Версия v" + latestVersion.Version + " доступна для скачивания. В новой версии :" + Environment.NewLine + Environment.NewLine + latestVersion.WhatsNew, null);
                }

                new Thread(() =>
                {
                    

                }).Start();
            }

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
                    bool isFirstRun = lastStatsCollectedKey == null;
                    if (isFirstRun)
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

                    appRegistry.SetValue(AppConstants.Registry.LastStatsCollectedDateKey, lastLogDate.ToString(AppConstants.DateTimeFormat));

                }).Start();
            }
        }

        public void HandleCommandInfoPopupClose(Guid? commandToken, ClosePopupReasonType closeReason)
        {
            if(commandToken!=null)
                _bot.HandleUserReactionToCommand(commandToken.Value, (UserReactionToCommandType)((int)closeReason));
        }
    }
}
