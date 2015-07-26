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
            this.TPCommandReplace = new System.Windows.Forms.TabControl();
            this.TPGeneral = new System.Windows.Forms.TabPage();
            this.hsSendErrorReport = new MonkeyJobTool.Controls.HelpTooltip();
            this.chkDenyErrorInfoSend = new System.Windows.Forms.CheckBox();
            this.htDonateBtn = new MonkeyJobTool.Controls.HelpTooltip();
            this.htStatsCollect = new MonkeyJobTool.Controls.HelpTooltip();
            this.chkIsDenyCollectingStats = new System.Windows.Forms.CheckBox();
            this.chkIsHideDonateBtn = new System.Windows.Forms.CheckBox();
            this.TPHotkeys = new System.Windows.Forms.TabPage();
            this.htHotKey = new MonkeyJobTool.Controls.HelpTooltip();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.htReplace = new MonkeyJobTool.Controls.HelpTooltip();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlCommandReplaces = new System.Windows.Forms.FlowLayoutPanel();
            this.TPModuleSettings = new System.Windows.Forms.TabPage();
            this.btnEnabledDisableModule = new System.Windows.Forms.Button();
            this.gridModules = new System.Windows.Forms.DataGridView();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.TPCommandReplace.SuspendLayout();
            this.TPGeneral.SuspendLayout();
            this.TPHotkeys.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.TPModuleSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModules)).BeginInit();
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
            // TPCommandReplace
            // 
            this.TPCommandReplace.Controls.Add(this.TPGeneral);
            this.TPCommandReplace.Controls.Add(this.TPHotkeys);
            this.TPCommandReplace.Controls.Add(this.tabPage3);
            this.TPCommandReplace.Controls.Add(this.TPModuleSettings);
            this.TPCommandReplace.Location = new System.Drawing.Point(13, 13);
            this.TPCommandReplace.Name = "TPCommandReplace";
            this.TPCommandReplace.SelectedIndex = 0;
            this.TPCommandReplace.Size = new System.Drawing.Size(580, 240);
            this.TPCommandReplace.TabIndex = 4;
            // 
            // TPGeneral
            // 
            this.TPGeneral.Controls.Add(this.hsSendErrorReport);
            this.TPGeneral.Controls.Add(this.chkDenyErrorInfoSend);
            this.TPGeneral.Controls.Add(this.htDonateBtn);
            this.TPGeneral.Controls.Add(this.htStatsCollect);
            this.TPGeneral.Controls.Add(this.chkIsDenyCollectingStats);
            this.TPGeneral.Controls.Add(this.chkIsHideDonateBtn);
            this.TPGeneral.Controls.Add(this.chkIsWithWindowsStart);
            this.TPGeneral.Location = new System.Drawing.Point(4, 22);
            this.TPGeneral.Name = "TPGeneral";
            this.TPGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.TPGeneral.Size = new System.Drawing.Size(572, 214);
            this.TPGeneral.TabIndex = 0;
            this.TPGeneral.Text = "Общие";
            this.TPGeneral.UseVisualStyleBackColor = true;
            // 
            // hsSendErrorReport
            // 
            this.hsSendErrorReport.AutoSize = true;
            this.hsSendErrorReport.Location = new System.Drawing.Point(334, 84);
            this.hsSendErrorReport.Name = "hsSendErrorReport";
            this.hsSendErrorReport.Size = new System.Drawing.Size(19, 19);
            this.hsSendErrorReport.TabIndex = 6;
            // 
            // chkDenyErrorInfoSend
            // 
            this.chkDenyErrorInfoSend.AutoSize = true;
            this.chkDenyErrorInfoSend.Location = new System.Drawing.Point(6, 84);
            this.chkDenyErrorInfoSend.Name = "chkDenyErrorInfoSend";
            this.chkDenyErrorInfoSend.Size = new System.Drawing.Size(322, 17);
            this.chkDenyErrorInfoSend.TabIndex = 5;
            this.chkDenyErrorInfoSend.Text = "Запретить отсылать информацию об ошибках программы";
            this.chkDenyErrorInfoSend.UseVisualStyleBackColor = true;
            // 
            // htDonateBtn
            // 
            this.htDonateBtn.AutoSize = true;
            this.htDonateBtn.Location = new System.Drawing.Point(255, 38);
            this.htDonateBtn.Name = "htDonateBtn";
            this.htDonateBtn.Size = new System.Drawing.Size(19, 19);
            this.htDonateBtn.TabIndex = 4;
            // 
            // htStatsCollect
            // 
            this.htStatsCollect.AutoSize = true;
            this.htStatsCollect.Location = new System.Drawing.Point(370, 61);
            this.htStatsCollect.Name = "htStatsCollect";
            this.htStatsCollect.Size = new System.Drawing.Size(19, 19);
            this.htStatsCollect.TabIndex = 3;
            // 
            // chkIsDenyCollectingStats
            // 
            this.chkIsDenyCollectingStats.AutoSize = true;
            this.chkIsDenyCollectingStats.Location = new System.Drawing.Point(6, 61);
            this.chkIsDenyCollectingStats.Name = "chkIsDenyCollectingStats";
            this.chkIsDenyCollectingStats.Size = new System.Drawing.Size(358, 17);
            this.chkIsDenyCollectingStats.TabIndex = 2;
            this.chkIsDenyCollectingStats.Text = "Запретить собирать статистику запусков (через Google Analytics)";
            this.chkIsDenyCollectingStats.UseVisualStyleBackColor = true;
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
            // TPHotkeys
            // 
            this.TPHotkeys.Controls.Add(this.htHotKey);
            this.TPHotkeys.Controls.Add(this.label1);
            this.TPHotkeys.Controls.Add(this.cmbKey1);
            this.TPHotkeys.Controls.Add(this.cmbKey3);
            this.TPHotkeys.Controls.Add(this.cmbKey2);
            this.TPHotkeys.Location = new System.Drawing.Point(4, 22);
            this.TPHotkeys.Name = "TPHotkeys";
            this.TPHotkeys.Padding = new System.Windows.Forms.Padding(3);
            this.TPHotkeys.Size = new System.Drawing.Size(572, 214);
            this.TPHotkeys.TabIndex = 1;
            this.TPHotkeys.Text = "Хоткеи";
            this.TPHotkeys.UseVisualStyleBackColor = true;
            // 
            // htHotKey
            // 
            this.htHotKey.AutoSize = true;
            this.htHotKey.Location = new System.Drawing.Point(318, 14);
            this.htHotKey.Name = "htHotKey";
            this.htHotKey.Size = new System.Drawing.Size(19, 19);
            this.htHotKey.TabIndex = 5;
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
            this.tabPage3.Controls.Add(this.htReplace);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.pnlCommandReplaces);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(572, 214);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Сокращения команд";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // htReplace
            // 
            this.htReplace.AutoSize = true;
            this.htReplace.Location = new System.Drawing.Point(395, 43);
            this.htReplace.Name = "htReplace";
            this.htReplace.Size = new System.Drawing.Size(19, 19);
            this.htReplace.TabIndex = 6;
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
            // pnlCommandReplaces
            // 
            this.pnlCommandReplaces.AutoScroll = true;
            this.pnlCommandReplaces.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlCommandReplaces.Location = new System.Drawing.Point(6, 36);
            this.pnlCommandReplaces.Name = "pnlCommandReplaces";
            this.pnlCommandReplaces.Size = new System.Drawing.Size(426, 160);
            this.pnlCommandReplaces.TabIndex = 0;
            this.pnlCommandReplaces.WrapContents = false;
            // 
            // TPModuleSettings
            // 
            this.TPModuleSettings.Controls.Add(this.btnEnabledDisableModule);
            this.TPModuleSettings.Controls.Add(this.gridModules);
            this.TPModuleSettings.Location = new System.Drawing.Point(4, 22);
            this.TPModuleSettings.Name = "TPModuleSettings";
            this.TPModuleSettings.Padding = new System.Windows.Forms.Padding(3);
            this.TPModuleSettings.Size = new System.Drawing.Size(572, 214);
            this.TPModuleSettings.TabIndex = 3;
            this.TPModuleSettings.Text = "Управление модулями";
            this.TPModuleSettings.UseVisualStyleBackColor = true;
            // 
            // btnEnabledDisableModule
            // 
            this.btnEnabledDisableModule.BackColor = System.Drawing.Color.Moccasin;
            this.btnEnabledDisableModule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEnabledDisableModule.ForeColor = System.Drawing.Color.Black;
            this.btnEnabledDisableModule.Location = new System.Drawing.Point(436, 6);
            this.btnEnabledDisableModule.Name = "btnEnabledDisableModule";
            this.btnEnabledDisableModule.Size = new System.Drawing.Size(118, 30);
            this.btnEnabledDisableModule.TabIndex = 6;
            this.btnEnabledDisableModule.Text = "Выключить модуль";
            this.btnEnabledDisableModule.UseVisualStyleBackColor = false;
            this.btnEnabledDisableModule.Click += new System.EventHandler(this.btnEnabledDisableModule_Click);
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
            this.moduleTypeColumn,
            this.colIsEnabled,
            this.settingsCol});
            this.gridModules.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridModules.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridModules.Location = new System.Drawing.Point(6, 6);
            this.gridModules.MultiSelect = false;
            this.gridModules.Name = "gridModules";
            this.gridModules.ReadOnly = true;
            this.gridModules.RowHeadersVisible = false;
            this.gridModules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridModules.Size = new System.Drawing.Size(424, 208);
            this.gridModules.TabIndex = 0;
            this.gridModules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridModules_CellClick);
            this.gridModules.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridModules_CellMouseLeave);
            this.gridModules.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridModules_CellMouseMove);
            this.gridModules.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridModules_RowEnter);
            this.gridModules.MouseLeave += new System.EventHandler(this.gridModules_MouseLeave);
            this.gridModules.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridModules_MouseMove);
            // 
            // ModuleName
            // 
            this.ModuleName.FillWeight = 150F;
            this.ModuleName.HeaderText = "Модуль";
            this.ModuleName.Name = "ModuleName";
            this.ModuleName.ReadOnly = true;
            // 
            // moduleTypeColumn
            // 
            this.moduleTypeColumn.FillWeight = 85F;
            this.moduleTypeColumn.HeaderText = "Тип";
            this.moduleTypeColumn.Name = "moduleTypeColumn";
            this.moduleTypeColumn.ReadOnly = true;
            // 
            // colIsEnabled
            // 
            this.colIsEnabled.HeaderText = "Статус";
            this.colIsEnabled.Name = "colIsEnabled";
            this.colIsEnabled.ReadOnly = true;
            // 
            // settingsCol
            // 
            this.settingsCol.HeaderText = "Настройки";
            this.settingsCol.Name = "settingsCol";
            this.settingsCol.ReadOnly = true;
            this.settingsCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.settingsCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.Moccasin;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Location = new System.Drawing.Point(518, 259);
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
            this.ClientSize = new System.Drawing.Size(602, 296);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.TPCommandReplace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.TPCommandReplace.ResumeLayout(false);
            this.TPGeneral.ResumeLayout(false);
            this.TPGeneral.PerformLayout();
            this.TPHotkeys.ResumeLayout(false);
            this.TPHotkeys.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.TPModuleSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridModules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsWithWindowsStart;
        private System.Windows.Forms.ComboBox cmbKey1;
        private System.Windows.Forms.ComboBox cmbKey2;
        private System.Windows.Forms.ComboBox cmbKey3;
        private System.Windows.Forms.TabControl TPCommandReplace;
        private System.Windows.Forms.TabPage TPGeneral;
        private System.Windows.Forms.TabPage TPHotkeys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FlowLayoutPanel pnlCommandReplaces;
        private System.Windows.Forms.CheckBox chkIsHideDonateBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage TPModuleSettings;
        private System.Windows.Forms.DataGridView gridModules;
        private System.Windows.Forms.Button btnEnabledDisableModule;
        private System.Windows.Forms.CheckBox chkIsDenyCollectingStats;
        private Controls.HelpTooltip htStatsCollect;
        private Controls.HelpTooltip htDonateBtn;
        private Controls.HelpTooltip htHotKey;
        private Controls.HelpTooltip htReplace;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsEnabled;
        private System.Windows.Forms.DataGridViewImageColumn settingsCol;
        private Controls.HelpTooltip hsSendErrorReport;
        private System.Windows.Forms.CheckBox chkDenyErrorInfoSend;
    }
}