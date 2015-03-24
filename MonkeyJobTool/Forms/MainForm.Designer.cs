﻿namespace MonkeyJobTool.Forms
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
            this.tsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainIcon = new System.Windows.Forms.PictureBox();
            this.trayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSettings,
            this.tsExit});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(135, 48);
            // 
            // tsSettings
            // 
            this.tsSettings.Name = "tsSettings";
            this.tsSettings.Size = new System.Drawing.Size(134, 22);
            this.tsSettings.Text = "Настройки";
            this.tsSettings.Click += new System.EventHandler(this.tsSettings_Click);
            // 
            // tsExit
            // 
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(134, 22);
            this.tsExit.Text = "Выход";
            this.tsExit.Click += new System.EventHandler(this.tsExit_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "MonkeyJob Tool";
            this.trayIcon.Visible = true;
            this.trayIcon.Click += new System.EventHandler(this.trayIcon_Click);
            this.trayIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDown);
            // 
            // MainIcon
            // 
            this.MainIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainIcon.Image = global::MonkeyJobTool.Properties.Resources.monkey_small;
            this.MainIcon.Location = new System.Drawing.Point(12, 12);
            this.MainIcon.Name = "MainIcon";
            this.MainIcon.Size = new System.Drawing.Size(25, 26);
            this.MainIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MainIcon.TabIndex = 1;
            this.MainIcon.TabStop = false;
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
    }
}
