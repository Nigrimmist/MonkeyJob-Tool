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
        private HelloBot bot;
        private string copyToBufferPostFix = " в буфер";
        private AutoCompleteControl autocomplete;

        public MainForm()
        {
           InitializeComponent();
           this.pictureBox1.Image = Properties.Resources.chimp;
           this.tsExit.Image = Properties.Resources.opened33;
           this.tsSettings.Image = Properties.Resources.settings49;
          
           try
           {
               bot = new HelloBot(botCommandPrefix: "");
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
            bot.OnErrorOccured += BotOnOnErrorOccured;
            

            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);

            autocomplete = new AutoCompleteControl()
            {
                ParentForm = this,
                DataFilterFunc = GetCommandListByTerm,
                Left = 43,
                Top = 8
            };
            autocomplete.OnCommandReceived += autocomplete_OnCommandReceived;
            this.Controls.Add(autocomplete);
            this.ToTop();
        }

        void autocomplete_OnCommandReceived(string command)
        {
            
            bool toBuffer = false;
            if (command.Trim().EndsWith(copyToBufferPostFix, StringComparison.InvariantCultureIgnoreCase))
            {
                command = command.Substring(0, command.Length - copyToBufferPostFix.Length);
                toBuffer = true;
            }

            bot.HandleMessage(command, delegate(string answer, AnswerBehaviourType answerType)
            {
                if (toBuffer)
                {
                    //copy to buffer in separate thread
                    new Thread(() => this.Invoke(new MethodInvoker(() => Clipboard.SetText(answer)))).Start();
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
                        if (!string.IsNullOrEmpty(answer)) MessageBox.Show(answer);
                    }
                }
            }, null);
        }

        private DataFilterInfo GetCommandListByTerm(string term)
        {
            var foundItems = bot.FindCommands(term).Select(x => x.Command.ToLower()).ToList();
            
            return new DataFilterInfo()
            {
                FoundByTerm = term,
                FoundItems = foundItems
            };
        }

        void openFormHotKeyRaised(object sender, KeyPressedEventArgs e)
        {
            if (autocomplete != null && autocomplete.IsPopupOpen)
            {
                autocomplete.PopupToTop();
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

        


        

    }
}
