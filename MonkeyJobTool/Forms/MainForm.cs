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
        

        public MainForm()
        {
           InitializeComponent();
           this.MainIcon.Image =
           this.tsExit.Image = defaultIcon;
           this.tsSettings.Image = Properties.Resources.settings;
          
           try
           {
               _bot = new HelloBot(botCommandPrefix: "");
               App.Init(openFormHotKeyRaised,this);
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

            if (!_bot.HandleMessage(command, delegate(AnswerInfo answerInfo)
            {
                string answer = answerInfo.Answer;
                var answerType = answerInfo.Type;
                SetLoading(false);
                if (toBuffer)
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
                                App.Instance.ShowPopup(answerInfo.Command, answer, null);
                                this.ToTop();
                            }));

                        }
                    }
                }
            }, null))
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
            this.ToTop();
        }

        
    }
}
