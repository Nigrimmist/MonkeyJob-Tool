using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore;
using HelloBotCore.Entities;
using HelloDesktopAssistant;
using HelloDesktopAssistant.Forms;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Utilities;

namespace MonkeyJobTool.Forms
{
    
    public partial class MainForm : Form
    {
        private HelloBot _bot;
        private const string _copyToBufferPostFix = " в буфер";
        private AutoCompleteControl _autocomplete;
        private Bitmap defaultIcon = Properties.Resources.monkey_highres;
        private Bitmap loadingIcon = Properties.Resources.loading;
        private List<InfoPopup> _openedPopups = new List<InfoPopup>();

        public MainForm()
        {
           InitializeComponent();
           this.MainIcon.Image =
           this.tsExit.Image = defaultIcon;
           this.tsSettings.Image = Properties.Resources.settings;
          
           try
           {
               _bot = new HelloBot(botCommandPrefix: "");
               App.Init(openFormHotKeyRaised);
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString());
           }
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
            this.ShowInTaskbar = false;
            _bot.OnErrorOccured += BotOnOnErrorOccured;
            

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
        }

        
        
        void autocomplete_OnCommandReceived(string command)
        {
            
            bool toBuffer = false;
            if (command.Trim().EndsWith(_copyToBufferPostFix, StringComparison.InvariantCultureIgnoreCase))
            {
                command = command.Substring(0, command.Length - _copyToBufferPostFix.Length);
                toBuffer = true;
            }
            SetLoading(true);
            
            _bot.HandleMessage(command, delegate(AnswerInfo answerInfo)
            {
                string answer = answerInfo.Answer;
                var answerType = answerInfo.Type;
                SetLoading(false);
                if (toBuffer)
                {
                    this.Invoke(new MethodInvoker(() => Clipboard.SetText(answer)));
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
                            this.Invoke((MethodInvoker) (() => ShowPopup(answerInfo.Command, answer,null)));
                        }
                    }
                }
            }, null);
        }

        
        private void ShowPopup(string title, string text,TimeSpan? displayTime)
        {
            CloseAllPopups();
            InfoPopup popup = new InfoPopup(title, text, displayTime);
            popup.Width = this.Width;
            popup.ToTop();
            var totalPopupY = _openedPopups.Sum(x => x.Height);
            popup.Location = new Point(this.Location.X, this.Location.Y - popup.Height - totalPopupY);
            popup.FormClosed += popup_FormClosed;
            this.ToTop();
            
            _openedPopups.Add(popup);
        }

        void popup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _openedPopups.Remove((InfoPopup) sender);
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
            if (_openedPopups.Any())
            {
                _openedPopups.ForEach(p=>p.ToTop());
            }
            this.ToTop();
        }

        private void BotOnOnErrorOccured(Exception exception)
        {
           
        }
        
        private void tsExit_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void tsSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }

        private void SetLoading(bool isLoading)
        {
            MainIcon.Image = isLoading ? loadingIcon : defaultIcon;
        }

        void _autocomplete_OnKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                    {
                        //if (_openedPopups.Any())
                        //{
                        //    _openedPopups.Last().Close();
                        //}
                        //else
                        {
                            this.Hide();
                            _autocomplete.HidePopup();
                            CloseAllPopups();
                        }
                        break;
                    }
            }
        }

        private void CloseAllPopups()
        {
            while (_openedPopups.Any())
            {
                _openedPopups.First().Close();
            }
        }
    }
}
