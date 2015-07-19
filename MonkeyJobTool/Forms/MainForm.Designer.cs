using MonkeyJobTool.Properties;

namespace MonkeyJobTool.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsCheckAllAsDisplayed = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNotificationOff = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNotificationOffFiveMin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNotificationOffTenMin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNotificationOffThirtyMin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNotificationOffOneHourMin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNotificationOffBillionYears = new System.Windows.Forms.ToolStripMenuItem();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainIcon = new System.Windows.Forms.PictureBox();
            this.DoNotNotifyTimer = new System.Windows.Forms.Timer(this.components);
            this.tsCancelNotifyOff = new System.Windows.Forms.ToolStripMenuItem();
            this.trayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCheckAllAsDisplayed,
            this.tsNotificationOff,
            this.tsSettings,
            this.tsDonate,
            this.tsExit});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(262, 136);
            // 
            // tsCheckAllAsDisplayed
            // 
            this.tsCheckAllAsDisplayed.Image = global::MonkeyJobTool.Properties.Resources.eye109;
            this.tsCheckAllAsDisplayed.Name = "tsCheckAllAsDisplayed";
            this.tsCheckAllAsDisplayed.Size = new System.Drawing.Size(261, 22);
            this.tsCheckAllAsDisplayed.Text = "Отметить все как просмотренные";
            this.tsCheckAllAsDisplayed.Click += new System.EventHandler(this.tsCheckAllAsDisplayed_Click);
            // 
            // tsSettings
            // 
            this.tsSettings.Image = global::MonkeyJobTool.Properties.Resources.settings;
            this.tsSettings.Name = "tsSettings";
            this.tsSettings.Size = new System.Drawing.Size(261, 22);
            this.tsSettings.Text = "Настройки";
            this.tsSettings.Click += new System.EventHandler(this.tsSettings_Click);
            // 
            // tsDonate
            // 
            this.tsDonate.Image = global::MonkeyJobTool.Properties.Resources.heart72;
            this.tsDonate.Name = "tsDonate";
            this.tsDonate.Size = new System.Drawing.Size(261, 22);
            this.tsDonate.Text = "Сказать спасибо";
            this.tsDonate.Click += new System.EventHandler(this.tsDonate_Click);
            // 
            // tsNotificationOff
            // 
            this.tsNotificationOff.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCancelNotifyOff,
            this.tsNotificationOffFiveMin,
            this.tsNotificationOffTenMin,
            this.tsNotificationOffThirtyMin,
            this.tsNotificationOffOneHourMin,
            this.tsNotificationOffBillionYears});
            this.tsNotificationOff.Image = global::MonkeyJobTool.Properties.Resources.Speaker_Muted_Mode_16;
            this.tsNotificationOff.Name = "tsNotificationOff";
            this.tsNotificationOff.Size = new System.Drawing.Size(261, 22);
            this.tsNotificationOff.Text = "Не тревожить";
            // 
            // tsNotificationOffFiveMin
            // 
            this.tsNotificationOffFiveMin.Name = "tsNotificationOffFiveMin";
            this.tsNotificationOffFiveMin.Size = new System.Drawing.Size(152, 22);
            this.tsNotificationOffFiveMin.Text = "5 минут";
            this.tsNotificationOffFiveMin.Click += new System.EventHandler(this.tsNotificationOffFiveMin_Click);
            // 
            // tsNotificationOffTenMin
            // 
            this.tsNotificationOffTenMin.Name = "tsNotificationOffTenMin";
            this.tsNotificationOffTenMin.Size = new System.Drawing.Size(152, 22);
            this.tsNotificationOffTenMin.Text = "10 минут";
            this.tsNotificationOffTenMin.Click += new System.EventHandler(this.tsNotificationOffTenMin_Click);
            // 
            // tsNotificationOffThirtyMin
            // 
            this.tsNotificationOffThirtyMin.Name = "tsNotificationOffThirtyMin";
            this.tsNotificationOffThirtyMin.Size = new System.Drawing.Size(152, 22);
            this.tsNotificationOffThirtyMin.Text = "30 минут";
            this.tsNotificationOffThirtyMin.Click += new System.EventHandler(this.tsNotificationOffThirtyMin_Click);
            // 
            // tsNotificationOffOneHourMin
            // 
            this.tsNotificationOffOneHourMin.Name = "tsNotificationOffOneHourMin";
            this.tsNotificationOffOneHourMin.Size = new System.Drawing.Size(152, 22);
            this.tsNotificationOffOneHourMin.Text = "1 час";
            this.tsNotificationOffOneHourMin.Click += new System.EventHandler(this.tsNotificationOffOneHourMin_Click);
            // 
            // tsNotificationOffBillionYears
            // 
            this.tsNotificationOffBillionYears.Name = "tsNotificationOffBillionYears";
            this.tsNotificationOffBillionYears.Size = new System.Drawing.Size(152, 22);
            this.tsNotificationOffBillionYears.Text = "Миллион лет";
            this.tsNotificationOffBillionYears.Click += new System.EventHandler(this.tsNotificationOffBillionYears_Click);
            // 
            // tsExit
            // 
            this.tsExit.Image = global::MonkeyJobTool.Properties.Resources.exit;
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(261, 22);
            this.tsExit.Text = "Выход";
            this.tsExit.Click += new System.EventHandler(this.tsExit_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = global::MonkeyJobTool.Properties.Resources.MonkeyJob_16x16;
            this.trayIcon.Text = "MonkeyJob Tool";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDown);
            // 
            // MainIcon
            // 
            this.MainIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainIcon.Image = global::MonkeyJobTool.Properties.Resources.monkey_highres_img;
            this.MainIcon.Location = new System.Drawing.Point(12, 12);
            this.MainIcon.Name = "MainIcon";
            this.MainIcon.Size = new System.Drawing.Size(25, 26);
            this.MainIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MainIcon.TabIndex = 1;
            this.MainIcon.TabStop = false;
            // 
            // DoNotNotifyTimer
            // 
            this.DoNotNotifyTimer.Interval = 1000;
            this.DoNotNotifyTimer.Tick += new System.EventHandler(this.DoNotNotifyTimer_Tick);
            // 
            // tsCancelNotifyOff
            // 
            this.tsCancelNotifyOff.Name = "tsCancelNotifyOff";
            this.tsCancelNotifyOff.Size = new System.Drawing.Size(152, 22);
            this.tsCancelNotifyOff.Text = "Отмена";
            this.tsCancelNotifyOff.Visible = false;
            this.tsCancelNotifyOff.Click += new System.EventHandler(this.tsCancelNotifyOff_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 51);
            this.Controls.Add(this.MainIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MonkeyJob Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.trayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MainIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem tsExit;
        private System.Windows.Forms.ToolStripMenuItem tsSettings;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolStripMenuItem tsDonate;
        private System.Windows.Forms.ToolStripMenuItem tsCheckAllAsDisplayed;
        private System.Windows.Forms.ToolStripMenuItem tsNotificationOff;
        private System.Windows.Forms.ToolStripMenuItem tsNotificationOffFiveMin;
        private System.Windows.Forms.ToolStripMenuItem tsNotificationOffTenMin;
        private System.Windows.Forms.ToolStripMenuItem tsNotificationOffThirtyMin;
        private System.Windows.Forms.ToolStripMenuItem tsNotificationOffOneHourMin;
        private System.Windows.Forms.ToolStripMenuItem tsNotificationOffBillionYears;
        private System.Windows.Forms.Timer DoNotNotifyTimer;
        private System.Windows.Forms.ToolStripMenuItem tsCancelNotifyOff;
    }
}

