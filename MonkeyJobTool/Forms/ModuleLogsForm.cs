using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore.Entities;

namespace MonkeyJobTool.Forms
{
    public partial class ModuleLogsForm : Form
    {
        public List<LogMessage> LogMessages { get;set; }

        public ModuleLogsForm()
        {
            InitializeComponent();
        }

        private void ModuleLogs_Load(object sender, EventArgs e)
        {
            FillData();
            fldMessages.SelectionStart = 0;
        }

        private void updateLogTimer_Tick(object sender, EventArgs e)
        {
            FillData();
        }

        private void FillData()
        {
            fldMessages.Text = String.Join(Environment.NewLine, LogMessages.Select(x => x.Date.ToString("yy/MM/dd hh:mm:ss") + " : " + x.Message).ToArray());
        }
    }
}
