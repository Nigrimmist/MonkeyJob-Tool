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
            this.tsCommands = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkIsHideDonateBtn = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pnlCommandReplaces = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.gridModules = new System.Windows.Forms.DataGridView();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.tsCommands.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModules)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            // tsCommands
            // 
            this.tsCommands.Controls.Add(this.tabPage1);
            this.tsCommands.Controls.Add(this.tabPage2);
            this.tsCommands.Controls.Add(this.tabPage3);
            this.tsCommands.Controls.Add(this.tabPage4);
            this.tsCommands.Location = new System.Drawing.Point(13, 13);
            this.tsCommands.Name = "tsCommands";
            this.tsCommands.SelectedIndex = 0;
            this.tsCommands.Size = new System.Drawing.Size(444, 384);
            this.tsCommands.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkIsHideDonateBtn);
            this.tabPage1.Controls.Add(this.chkIsWithWindowsStart);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(436, 358);
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
            this.tabPage2.Size = new System.Drawing.Size(436, 358);
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
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.pnlCommandReplaces);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(436, 358);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Сокращения команд";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pnlCommandReplaces
            // 
            this.pnlCommandReplaces.AutoScroll = true;
            this.pnlCommandReplaces.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlCommandReplaces.Location = new System.Drawing.Point(6, 36);
            this.pnlCommandReplaces.Name = "pnlCommandReplaces";
            this.pnlCommandReplaces.Size = new System.Drawing.Size(424, 307);
            this.pnlCommandReplaces.TabIndex = 0;
            this.pnlCommandReplaces.WrapContents = false;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.Moccasin;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Location = new System.Drawing.Point(382, 403);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 5;
            this.btnSaveConfig.Text = "Сохранить";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(65, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Сокращение";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(261, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Команда";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.gridModules);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(436, 358);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Управление модулями";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // gridModules
            // 
            this.gridModules.AllowUserToAddRows = false;
            this.gridModules.AllowUserToDeleteRows = false;
            this.gridModules.AllowUserToResizeRows = false;
            this.gridModules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridModules.BackgroundColor = System.Drawing.Color.White;
            this.gridModules.CausesValidation = false;
            this.gridModules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ModuleName,
            this.ModuleType,
            this.isEnabled});
            this.gridModules.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridModules.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridModules.Location = new System.Drawing.Point(6, 6);
            this.gridModules.Name = "gridModules";
            this.gridModules.ReadOnly = true;
            this.gridModules.RowHeadersVisible = false;
            this.gridModules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridModules.Size = new System.Drawing.Size(424, 168);
            this.gridModules.TabIndex = 0;
            this.gridModules.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridModules_RowEnter);
            // 
            // ModuleName
            // 
            this.ModuleName.FillWeight = 67.11845F;
            this.ModuleName.HeaderText = "Модуль";
            this.ModuleName.Name = "ModuleName";
            this.ModuleName.ReadOnly = true;
            // 
            // ModuleType
            // 
            this.ModuleType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModuleType.FillWeight = 49.08036F;
            this.ModuleType.HeaderText = "Тип";
            this.ModuleType.Name = "ModuleType";
            this.ModuleType.ReadOnly = true;
            // 
            // isEnabled
            // 
            this.isEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.isEnabled.FillWeight = 49.08036F;
            this.isEnabled.HeaderText = "Статус";
            this.isEnabled.Name = "isEnabled";
            this.isEnabled.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDescr);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(6, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 163);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Описание и настройка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(228, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Описание";
            // 
            // txtDescr
            // 
            this.txtDescr.BackColor = System.Drawing.Color.White;
            this.txtDescr.Location = new System.Drawing.Point(231, 33);
            this.txtDescr.Multiline = true;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.ReadOnly = true;
            this.txtDescr.Size = new System.Drawing.Size(187, 104);
            this.txtDescr.TabIndex = 2;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 438);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.tsCommands);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tsCommands.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridModules)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsWithWindowsStart;
        private System.Windows.Forms.ComboBox cmbKey1;
        private System.Windows.Forms.ComboBox cmbKey2;
        private System.Windows.Forms.ComboBox cmbKey3;
        private System.Windows.Forms.TabControl tsCommands;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FlowLayoutPanel pnlCommandReplaces;
        private System.Windows.Forms.CheckBox chkIsHideDonateBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView gridModules;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn isEnabled;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDescr;
    }
}