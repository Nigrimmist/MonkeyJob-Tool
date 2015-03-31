namespace MonkeyJobTool.Forms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.chkIsWithWindowsStart = new System.Windows.Forms.CheckBox();
            this.cmbKey1 = new System.Windows.Forms.ComboBox();
            this.cmbKey2 = new System.Windows.Forms.ComboBox();
            this.cmbKey3 = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkIsHideDonateBtn = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pnlCommandReplaces = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIsWithWindowsStart
            // 
            this.chkIsWithWindowsStart.AutoSize = true;
            this.chkIsWithWindowsStart.Location = new System.Drawing.Point(6, 15);
            this.chkIsWithWindowsStart.Name = "chkIsWithWindowsStart";
            this.chkIsWithWindowsStart.Size = new System.Drawing.Size(191, 17);
            this.chkIsWithWindowsStart.TabIndex = 0;
            this.chkIsWithWindowsStart.Text = "Запускать при запуске Windows";
            this.chkIsWithWindowsStart.UseVisualStyleBackColor = true;
            this.chkIsWithWindowsStart.CheckedChanged += new System.EventHandler(this.chkIsWithWindowsStart_CheckedChanged);
            // 
            // cmbKey1
            // 
            this.cmbKey1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey1.FormattingEnabled = true;
            this.cmbKey1.Location = new System.Drawing.Point(123, 11);
            this.cmbKey1.Name = "cmbKey1";
            this.cmbKey1.Size = new System.Drawing.Size(55, 21);
            this.cmbKey1.TabIndex = 1;
            // 
            // cmbKey2
            // 
            this.cmbKey2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey2.FormattingEnabled = true;
            this.cmbKey2.Location = new System.Drawing.Point(184, 11);
            this.cmbKey2.Name = "cmbKey2";
            this.cmbKey2.Size = new System.Drawing.Size(55, 21);
            this.cmbKey2.TabIndex = 2;
            // 
            // cmbKey3
            // 
            this.cmbKey3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey3.FormattingEnabled = true;
            this.cmbKey3.Location = new System.Drawing.Point(245, 11);
            this.cmbKey3.Name = "cmbKey3";
            this.cmbKey3.Size = new System.Drawing.Size(55, 21);
            this.cmbKey3.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(444, 261);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkIsHideDonateBtn);
            this.tabPage1.Controls.Add(this.chkIsWithWindowsStart);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(436, 235);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Общие";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkIsHideDonateBtn
            // 
            this.chkIsHideDonateBtn.AutoSize = true;
            this.chkIsHideDonateBtn.Location = new System.Drawing.Point(6, 38);
            this.chkIsHideDonateBtn.Name = "chkIsHideDonateBtn";
            this.chkIsHideDonateBtn.Size = new System.Drawing.Size(243, 17);
            this.chkIsHideDonateBtn.TabIndex = 1;
            this.chkIsHideDonateBtn.Text = "Скрыть кнопку \"Сказать спасибо\" из трея";
            this.chkIsHideDonateBtn.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.cmbKey1);
            this.tabPage2.Controls.Add(this.cmbKey3);
            this.tabPage2.Controls.Add(this.cmbKey2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(436, 235);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Хоткеи";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Вызов программы : ";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pnlCommandReplaces);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(436, 235);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Сокращения команд";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pnlCommandReplaces
            // 
            this.pnlCommandReplaces.AutoScroll = true;
            this.pnlCommandReplaces.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlCommandReplaces.Location = new System.Drawing.Point(6, 6);
            this.pnlCommandReplaces.Name = "pnlCommandReplaces";
            this.pnlCommandReplaces.Size = new System.Drawing.Size(424, 223);
            this.pnlCommandReplaces.TabIndex = 0;
            this.pnlCommandReplaces.WrapContents = false;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.Moccasin;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Location = new System.Drawing.Point(382, 280);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 5;
            this.btnSaveConfig.Text = "Сохранить";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 315);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsWithWindowsStart;
        private System.Windows.Forms.ComboBox cmbKey1;
        private System.Windows.Forms.ComboBox cmbKey2;
        private System.Windows.Forms.ComboBox cmbKey3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FlowLayoutPanel pnlCommandReplaces;
        private System.Windows.Forms.CheckBox chkIsHideDonateBtn;
    }
}