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
            this.TCSettings = new System.Windows.Forms.TabControl();
            this.TPGeneral = new System.Windows.Forms.TabPage();
            this.chkIsShowCommandHelp = new System.Windows.Forms.CheckBox();
            this.chkDenyErrorInfoSend = new System.Windows.Forms.CheckBox();
            this.chkIsDenyCollectingStats = new System.Windows.Forms.CheckBox();
            this.chkIsHideDonateBtn = new System.Windows.Forms.CheckBox();
            this.TPHotkeys = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.TPReplaces = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlCommandReplaces = new System.Windows.Forms.FlowLayoutPanel();
            this.TPModuleSettings = new System.Windows.Forms.TabPage();
            this.pnlRunModule = new System.Windows.Forms.Panel();
            this.btnModuleRun = new System.Windows.Forms.Button();
            this.btnShowLogs = new System.Windows.Forms.Button();
            this.btnEnabledDisableModule = new System.Windows.Forms.Button();
            this.gridModules = new System.Windows.Forms.DataGridView();
            this.TPClients = new System.Windows.Forms.TabPage();
            this.btnShowClientLogs = new System.Windows.Forms.Button();
            this.btnEnabledDisableClient = new System.Windows.Forms.Button();
            this.gridClients = new System.Windows.Forms.DataGridView();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.collsInstCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.hsShowHelpForCommands = new MonkeyJobTool.Controls.HelpTooltip();
            this.hsSendErrorReport = new MonkeyJobTool.Controls.HelpTooltip();
            this.htDonateBtn = new MonkeyJobTool.Controls.HelpTooltip();
            this.htStatsCollect = new MonkeyJobTool.Controls.HelpTooltip();
            this.htHotKey = new MonkeyJobTool.Controls.HelpTooltip();
            this.htReplace = new MonkeyJobTool.Controls.HelpTooltip();
            this.htModuleTest = new MonkeyJobTool.Controls.HelpTooltip();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsEnabledClients = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.collsInstCountClients = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsColClients = new System.Windows.Forms.DataGridViewImageColumn();
            this.TCSettings.SuspendLayout();
            this.TPGeneral.SuspendLayout();
            this.TPHotkeys.SuspendLayout();
            this.TPReplaces.SuspendLayout();
            this.TPModuleSettings.SuspendLayout();
            this.pnlRunModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModules)).BeginInit();
            this.TPClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridClients)).BeginInit();
            this.SuspendLayout();
            // 
            // chkIsWithWindowsStart
            // 
            this.chkIsWithWindowsStart.AutoSize = true;
            this.chkIsWithWindowsStart.Location = new System.Drawing.Point(8, 18);
            this.chkIsWithWindowsStart.Margin = new System.Windows.Forms.Padding(4);
            this.chkIsWithWindowsStart.Name = "chkIsWithWindowsStart";
            this.chkIsWithWindowsStart.Size = new System.Drawing.Size(235, 21);
            this.chkIsWithWindowsStart.TabIndex = 0;
            this.chkIsWithWindowsStart.Text = "Запускать при старте Windows";
            this.chkIsWithWindowsStart.UseVisualStyleBackColor = true;
            // 
            // cmbKey1
            // 
            this.cmbKey1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey1.FormattingEnabled = true;
            this.cmbKey1.Location = new System.Drawing.Point(164, 14);
            this.cmbKey1.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKey1.Name = "cmbKey1";
            this.cmbKey1.Size = new System.Drawing.Size(72, 24);
            this.cmbKey1.TabIndex = 1;
            // 
            // cmbKey2
            // 
            this.cmbKey2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey2.FormattingEnabled = true;
            this.cmbKey2.Location = new System.Drawing.Point(245, 14);
            this.cmbKey2.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKey2.Name = "cmbKey2";
            this.cmbKey2.Size = new System.Drawing.Size(72, 24);
            this.cmbKey2.TabIndex = 2;
            // 
            // cmbKey3
            // 
            this.cmbKey3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKey3.FormattingEnabled = true;
            this.cmbKey3.Location = new System.Drawing.Point(327, 14);
            this.cmbKey3.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKey3.Name = "cmbKey3";
            this.cmbKey3.Size = new System.Drawing.Size(72, 24);
            this.cmbKey3.TabIndex = 3;
            // 
            // TCSettings
            // 
            this.TCSettings.Controls.Add(this.TPGeneral);
            this.TCSettings.Controls.Add(this.TPHotkeys);
            this.TCSettings.Controls.Add(this.TPReplaces);
            this.TCSettings.Controls.Add(this.TPModuleSettings);
            this.TCSettings.Controls.Add(this.TPClients);
            this.TCSettings.Location = new System.Drawing.Point(17, 16);
            this.TCSettings.Margin = new System.Windows.Forms.Padding(4);
            this.TCSettings.Name = "TCSettings";
            this.TCSettings.SelectedIndex = 0;
            this.TCSettings.Size = new System.Drawing.Size(773, 295);
            this.TCSettings.TabIndex = 4;
            // 
            // TPGeneral
            // 
            this.TPGeneral.Controls.Add(this.hsShowHelpForCommands);
            this.TPGeneral.Controls.Add(this.chkIsShowCommandHelp);
            this.TPGeneral.Controls.Add(this.hsSendErrorReport);
            this.TPGeneral.Controls.Add(this.chkDenyErrorInfoSend);
            this.TPGeneral.Controls.Add(this.htDonateBtn);
            this.TPGeneral.Controls.Add(this.htStatsCollect);
            this.TPGeneral.Controls.Add(this.chkIsDenyCollectingStats);
            this.TPGeneral.Controls.Add(this.chkIsHideDonateBtn);
            this.TPGeneral.Controls.Add(this.chkIsWithWindowsStart);
            this.TPGeneral.Location = new System.Drawing.Point(4, 25);
            this.TPGeneral.Margin = new System.Windows.Forms.Padding(4);
            this.TPGeneral.Name = "TPGeneral";
            this.TPGeneral.Padding = new System.Windows.Forms.Padding(4);
            this.TPGeneral.Size = new System.Drawing.Size(765, 266);
            this.TPGeneral.TabIndex = 0;
            this.TPGeneral.Text = "Общие";
            this.TPGeneral.UseVisualStyleBackColor = true;
            // 
            // chkIsShowCommandHelp
            // 
            this.chkIsShowCommandHelp.AutoSize = true;
            this.chkIsShowCommandHelp.Location = new System.Drawing.Point(8, 132);
            this.chkIsShowCommandHelp.Margin = new System.Windows.Forms.Padding(4);
            this.chkIsShowCommandHelp.Name = "chkIsShowCommandHelp";
            this.chkIsShowCommandHelp.Size = new System.Drawing.Size(261, 21);
            this.chkIsShowCommandHelp.TabIndex = 7;
            this.chkIsShowCommandHelp.Text = "Показывать подсказки для команд";
            this.chkIsShowCommandHelp.UseVisualStyleBackColor = true;
            // 
            // chkDenyErrorInfoSend
            // 
            this.chkDenyErrorInfoSend.AutoSize = true;
            this.chkDenyErrorInfoSend.Location = new System.Drawing.Point(8, 103);
            this.chkDenyErrorInfoSend.Margin = new System.Windows.Forms.Padding(4);
            this.chkDenyErrorInfoSend.Name = "chkDenyErrorInfoSend";
            this.chkDenyErrorInfoSend.Size = new System.Drawing.Size(413, 21);
            this.chkDenyErrorInfoSend.TabIndex = 5;
            this.chkDenyErrorInfoSend.Text = "Запретить отсылать информацию об ошибках программы";
            this.chkDenyErrorInfoSend.UseVisualStyleBackColor = true;
            // 
            // chkIsDenyCollectingStats
            // 
            this.chkIsDenyCollectingStats.AutoSize = true;
            this.chkIsDenyCollectingStats.Location = new System.Drawing.Point(8, 75);
            this.chkIsDenyCollectingStats.Margin = new System.Windows.Forms.Padding(4);
            this.chkIsDenyCollectingStats.Name = "chkIsDenyCollectingStats";
            this.chkIsDenyCollectingStats.Size = new System.Drawing.Size(468, 21);
            this.chkIsDenyCollectingStats.TabIndex = 2;
            this.chkIsDenyCollectingStats.Text = "Запретить собирать статистику запусков (через Google Analytics)";
            this.chkIsDenyCollectingStats.UseVisualStyleBackColor = true;
            // 
            // chkIsHideDonateBtn
            // 
            this.chkIsHideDonateBtn.AutoSize = true;
            this.chkIsHideDonateBtn.Location = new System.Drawing.Point(8, 47);
            this.chkIsHideDonateBtn.Margin = new System.Windows.Forms.Padding(4);
            this.chkIsHideDonateBtn.Name = "chkIsHideDonateBtn";
            this.chkIsHideDonateBtn.Size = new System.Drawing.Size(306, 21);
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
            this.TPHotkeys.Location = new System.Drawing.Point(4, 25);
            this.TPHotkeys.Margin = new System.Windows.Forms.Padding(4);
            this.TPHotkeys.Name = "TPHotkeys";
            this.TPHotkeys.Padding = new System.Windows.Forms.Padding(4);
            this.TPHotkeys.Size = new System.Drawing.Size(765, 266);
            this.TPHotkeys.TabIndex = 1;
            this.TPHotkeys.Text = "Хоткеи";
            this.TPHotkeys.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Вызов программы : ";
            // 
            // TPReplaces
            // 
            this.TPReplaces.Controls.Add(this.htReplace);
            this.TPReplaces.Controls.Add(this.label3);
            this.TPReplaces.Controls.Add(this.label2);
            this.TPReplaces.Controls.Add(this.pnlCommandReplaces);
            this.TPReplaces.Location = new System.Drawing.Point(4, 25);
            this.TPReplaces.Margin = new System.Windows.Forms.Padding(4);
            this.TPReplaces.Name = "TPReplaces";
            this.TPReplaces.Padding = new System.Windows.Forms.Padding(4);
            this.TPReplaces.Size = new System.Drawing.Size(765, 266);
            this.TPReplaces.TabIndex = 2;
            this.TPReplaces.Text = "Сокращения команд";
            this.TPReplaces.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(348, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Команда";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(87, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Сокращение";
            // 
            // pnlCommandReplaces
            // 
            this.pnlCommandReplaces.AutoScroll = true;
            this.pnlCommandReplaces.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlCommandReplaces.Location = new System.Drawing.Point(8, 44);
            this.pnlCommandReplaces.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCommandReplaces.Name = "pnlCommandReplaces";
            this.pnlCommandReplaces.Size = new System.Drawing.Size(568, 197);
            this.pnlCommandReplaces.TabIndex = 0;
            this.pnlCommandReplaces.WrapContents = false;
            // 
            // TPModuleSettings
            // 
            this.TPModuleSettings.Controls.Add(this.pnlRunModule);
            this.TPModuleSettings.Controls.Add(this.btnShowLogs);
            this.TPModuleSettings.Controls.Add(this.btnEnabledDisableModule);
            this.TPModuleSettings.Controls.Add(this.gridModules);
            this.TPModuleSettings.Location = new System.Drawing.Point(4, 25);
            this.TPModuleSettings.Margin = new System.Windows.Forms.Padding(4);
            this.TPModuleSettings.Name = "TPModuleSettings";
            this.TPModuleSettings.Padding = new System.Windows.Forms.Padding(4);
            this.TPModuleSettings.Size = new System.Drawing.Size(765, 266);
            this.TPModuleSettings.TabIndex = 3;
            this.TPModuleSettings.Text = "Управление модулями";
            this.TPModuleSettings.UseVisualStyleBackColor = true;
            // 
            // pnlRunModule
            // 
            this.pnlRunModule.Controls.Add(this.htModuleTest);
            this.pnlRunModule.Controls.Add(this.btnModuleRun);
            this.pnlRunModule.Location = new System.Drawing.Point(581, 105);
            this.pnlRunModule.Margin = new System.Windows.Forms.Padding(4);
            this.pnlRunModule.Name = "pnlRunModule";
            this.pnlRunModule.Size = new System.Drawing.Size(157, 55);
            this.pnlRunModule.TabIndex = 10;
            this.pnlRunModule.Visible = false;
            // 
            // btnModuleRun
            // 
            this.btnModuleRun.BackColor = System.Drawing.Color.Moccasin;
            this.btnModuleRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnModuleRun.ForeColor = System.Drawing.Color.Black;
            this.btnModuleRun.Location = new System.Drawing.Point(0, 4);
            this.btnModuleRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnModuleRun.Name = "btnModuleRun";
            this.btnModuleRun.Size = new System.Drawing.Size(157, 48);
            this.btnModuleRun.TabIndex = 8;
            this.btnModuleRun.Text = "Тест модуля";
            this.btnModuleRun.UseVisualStyleBackColor = false;
            this.btnModuleRun.Click += new System.EventHandler(this.btnModuleRun_Click);
            // 
            // btnShowLogs
            // 
            this.btnShowLogs.BackColor = System.Drawing.Color.Moccasin;
            this.btnShowLogs.Enabled = false;
            this.btnShowLogs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowLogs.ForeColor = System.Drawing.Color.Black;
            this.btnShowLogs.Location = new System.Drawing.Point(581, 52);
            this.btnShowLogs.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowLogs.Name = "btnShowLogs";
            this.btnShowLogs.Size = new System.Drawing.Size(157, 48);
            this.btnShowLogs.TabIndex = 7;
            this.btnShowLogs.Text = "Просмотреть лог активности";
            this.btnShowLogs.UseVisualStyleBackColor = false;
            this.btnShowLogs.Click += new System.EventHandler(this.btnShowLogs_Click);
            // 
            // btnEnabledDisableModule
            // 
            this.btnEnabledDisableModule.BackColor = System.Drawing.Color.Moccasin;
            this.btnEnabledDisableModule.Enabled = false;
            this.btnEnabledDisableModule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEnabledDisableModule.ForeColor = System.Drawing.Color.Black;
            this.btnEnabledDisableModule.Location = new System.Drawing.Point(581, 7);
            this.btnEnabledDisableModule.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnabledDisableModule.Name = "btnEnabledDisableModule";
            this.btnEnabledDisableModule.Size = new System.Drawing.Size(157, 37);
            this.btnEnabledDisableModule.TabIndex = 6;
            this.btnEnabledDisableModule.Text = "Выключить модуль";
            this.btnEnabledDisableModule.UseVisualStyleBackColor = false;
            this.btnEnabledDisableModule.Click += new System.EventHandler(this.btnEnabledDisableComponent_Click);
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
            this.collsInstCount,
            this.settingsCol});
            this.gridModules.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridModules.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridModules.Location = new System.Drawing.Point(8, 7);
            this.gridModules.Margin = new System.Windows.Forms.Padding(4);
            this.gridModules.MultiSelect = false;
            this.gridModules.Name = "gridModules";
            this.gridModules.ReadOnly = true;
            this.gridModules.RowHeadersVisible = false;
            this.gridModules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridModules.Size = new System.Drawing.Size(565, 256);
            this.gridModules.TabIndex = 0;
            this.gridModules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellClick);
            this.gridModules.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseMove);
            this.gridModules.SelectionChanged += new System.EventHandler(this.gridModules_SelectionChanged);
            this.gridModules.MouseLeave += new System.EventHandler(this.grid_MouseLeave);
            this.gridModules.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grid_MouseMove);
            // 
            // TPClients
            // 
            this.TPClients.Controls.Add(this.btnShowClientLogs);
            this.TPClients.Controls.Add(this.btnEnabledDisableClient);
            this.TPClients.Controls.Add(this.gridClients);
            this.TPClients.Location = new System.Drawing.Point(4, 25);
            this.TPClients.Margin = new System.Windows.Forms.Padding(4);
            this.TPClients.Name = "TPClients";
            this.TPClients.Padding = new System.Windows.Forms.Padding(4);
            this.TPClients.Size = new System.Drawing.Size(765, 266);
            this.TPClients.TabIndex = 4;
            this.TPClients.Text = "Интеграция с клиентами";
            this.TPClients.UseVisualStyleBackColor = true;
            // 
            // btnShowClientLogs
            // 
            this.btnShowClientLogs.BackColor = System.Drawing.Color.Moccasin;
            this.btnShowClientLogs.Enabled = false;
            this.btnShowClientLogs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowClientLogs.ForeColor = System.Drawing.Color.Black;
            this.btnShowClientLogs.Location = new System.Drawing.Point(581, 52);
            this.btnShowClientLogs.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowClientLogs.Name = "btnShowClientLogs";
            this.btnShowClientLogs.Size = new System.Drawing.Size(157, 48);
            this.btnShowClientLogs.TabIndex = 10;
            this.btnShowClientLogs.Text = "Просмотреть лог активности";
            this.btnShowClientLogs.UseVisualStyleBackColor = false;
            this.btnShowClientLogs.Click += new System.EventHandler(this.btnShowLogs_Click);
            // 
            // btnEnabledDisableClient
            // 
            this.btnEnabledDisableClient.BackColor = System.Drawing.Color.Moccasin;
            this.btnEnabledDisableClient.Enabled = false;
            this.btnEnabledDisableClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEnabledDisableClient.ForeColor = System.Drawing.Color.Black;
            this.btnEnabledDisableClient.Location = new System.Drawing.Point(581, 7);
            this.btnEnabledDisableClient.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnabledDisableClient.Name = "btnEnabledDisableClient";
            this.btnEnabledDisableClient.Size = new System.Drawing.Size(157, 37);
            this.btnEnabledDisableClient.TabIndex = 9;
            this.btnEnabledDisableClient.Text = "Отключить клиент";
            this.btnEnabledDisableClient.UseVisualStyleBackColor = false;
            this.btnEnabledDisableClient.Click += new System.EventHandler(this.btnEnabledDisableComponent_Click);
            // 
            // gridClients
            // 
            this.gridClients.AllowUserToAddRows = false;
            this.gridClients.AllowUserToDeleteRows = false;
            this.gridClients.AllowUserToResizeRows = false;
            this.gridClients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridClients.BackgroundColor = System.Drawing.Color.White;
            this.gridClients.CausesValidation = false;
            this.gridClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClient,
            this.colIsEnabledClients,
            this.collsInstCountClients,
            this.settingsColClients});
            this.gridClients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridClients.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridClients.Location = new System.Drawing.Point(8, 7);
            this.gridClients.Margin = new System.Windows.Forms.Padding(4);
            this.gridClients.MultiSelect = false;
            this.gridClients.Name = "gridClients";
            this.gridClients.ReadOnly = true;
            this.gridClients.RowHeadersVisible = false;
            this.gridClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridClients.Size = new System.Drawing.Size(565, 256);
            this.gridClients.TabIndex = 8;
            this.gridClients.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellClick);
            this.gridClients.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseMove);
            this.gridClients.SelectionChanged += new System.EventHandler(this.gridModules_SelectionChanged);
            this.gridClients.MouseLeave += new System.EventHandler(this.grid_MouseLeave);
            this.gridClients.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grid_MouseMove);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.Moccasin;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Location = new System.Drawing.Point(691, 319);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(100, 28);
            this.btnSaveConfig.TabIndex = 5;
            this.btnSaveConfig.Text = "Сохранить";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
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
            // collsInstCount
            // 
            this.collsInstCount.HeaderText = "Кол-во";
            this.collsInstCount.Name = "collsInstCount";
            this.collsInstCount.ReadOnly = true;
            // 
            // settingsCol
            // 
            this.settingsCol.HeaderText = "Настройки";
            this.settingsCol.Name = "settingsCol";
            this.settingsCol.ReadOnly = true;
            this.settingsCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.settingsCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // hsShowHelpForCommands
            // 
            this.hsShowHelpForCommands.AutoSize = true;
            this.hsShowHelpForCommands.Location = new System.Drawing.Point(293, 132);
            this.hsShowHelpForCommands.Margin = new System.Windows.Forms.Padding(5);
            this.hsShowHelpForCommands.Name = "hsShowHelpForCommands";
            this.hsShowHelpForCommands.Size = new System.Drawing.Size(25, 23);
            this.hsShowHelpForCommands.TabIndex = 8;
            // 
            // hsSendErrorReport
            // 
            this.hsSendErrorReport.AutoSize = true;
            this.hsSendErrorReport.Location = new System.Drawing.Point(445, 103);
            this.hsSendErrorReport.Margin = new System.Windows.Forms.Padding(5);
            this.hsSendErrorReport.Name = "hsSendErrorReport";
            this.hsSendErrorReport.Size = new System.Drawing.Size(25, 23);
            this.hsSendErrorReport.TabIndex = 6;
            // 
            // htDonateBtn
            // 
            this.htDonateBtn.AutoSize = true;
            this.htDonateBtn.Location = new System.Drawing.Point(340, 47);
            this.htDonateBtn.Margin = new System.Windows.Forms.Padding(5);
            this.htDonateBtn.Name = "htDonateBtn";
            this.htDonateBtn.Size = new System.Drawing.Size(25, 23);
            this.htDonateBtn.TabIndex = 4;
            // 
            // htStatsCollect
            // 
            this.htStatsCollect.AutoSize = true;
            this.htStatsCollect.Location = new System.Drawing.Point(493, 75);
            this.htStatsCollect.Margin = new System.Windows.Forms.Padding(5);
            this.htStatsCollect.Name = "htStatsCollect";
            this.htStatsCollect.Size = new System.Drawing.Size(25, 23);
            this.htStatsCollect.TabIndex = 3;
            // 
            // htHotKey
            // 
            this.htHotKey.AutoSize = true;
            this.htHotKey.Location = new System.Drawing.Point(424, 17);
            this.htHotKey.Margin = new System.Windows.Forms.Padding(5);
            this.htHotKey.Name = "htHotKey";
            this.htHotKey.Size = new System.Drawing.Size(25, 23);
            this.htHotKey.TabIndex = 5;
            // 
            // htReplace
            // 
            this.htReplace.AutoSize = true;
            this.htReplace.Location = new System.Drawing.Point(527, 53);
            this.htReplace.Margin = new System.Windows.Forms.Padding(5);
            this.htReplace.Name = "htReplace";
            this.htReplace.Size = new System.Drawing.Size(25, 23);
            this.htReplace.TabIndex = 6;
            // 
            // htModuleTest
            // 
            this.htModuleTest.AutoSize = true;
            this.htModuleTest.Location = new System.Drawing.Point(133, 6);
            this.htModuleTest.Margin = new System.Windows.Forms.Padding(5);
            this.htModuleTest.Name = "htModuleTest";
            this.htModuleTest.Size = new System.Drawing.Size(21, 20);
            this.htModuleTest.TabIndex = 9;
            // 
            // colClient
            // 
            this.colClient.FillWeight = 150F;
            this.colClient.HeaderText = "Клиент";
            this.colClient.Name = "colClient";
            this.colClient.ReadOnly = true;
            // 
            // colIsEnabledClients
            // 
            this.colIsEnabledClients.HeaderText = "Статус";
            this.colIsEnabledClients.Name = "colIsEnabledClients";
            this.colIsEnabledClients.ReadOnly = true;
            // 
            // collsInstCountClients
            // 
            this.collsInstCountClients.HeaderText = "Кол-во";
            this.collsInstCountClients.Name = "collsInstCountClients";
            this.collsInstCountClients.ReadOnly = true;
            // 
            // settingsColClients
            // 
            this.settingsColClients.HeaderText = "Настройки";
            this.settingsColClients.Name = "settingsColClients";
            this.settingsColClients.ReadOnly = true;
            this.settingsColClients.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.settingsColClients.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 364);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.TCSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.TCSettings.ResumeLayout(false);
            this.TPGeneral.ResumeLayout(false);
            this.TPGeneral.PerformLayout();
            this.TPHotkeys.ResumeLayout(false);
            this.TPHotkeys.PerformLayout();
            this.TPReplaces.ResumeLayout(false);
            this.TPReplaces.PerformLayout();
            this.TPModuleSettings.ResumeLayout(false);
            this.pnlRunModule.ResumeLayout(false);
            this.pnlRunModule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModules)).EndInit();
            this.TPClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridClients)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsWithWindowsStart;
        private System.Windows.Forms.ComboBox cmbKey1;
        private System.Windows.Forms.ComboBox cmbKey2;
        private System.Windows.Forms.ComboBox cmbKey3;
        private System.Windows.Forms.TabControl TCSettings;
        private System.Windows.Forms.TabPage TPGeneral;
        private System.Windows.Forms.TabPage TPHotkeys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.TabPage TPReplaces;
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
        private Controls.HelpTooltip hsSendErrorReport;
        private System.Windows.Forms.CheckBox chkDenyErrorInfoSend;
        private System.Windows.Forms.Button btnShowLogs;
        private Controls.HelpTooltip hsShowHelpForCommands;
        private System.Windows.Forms.CheckBox chkIsShowCommandHelp;
        private System.Windows.Forms.TabPage TPClients;
        private System.Windows.Forms.Button btnEnabledDisableClient;
        private System.Windows.Forms.DataGridView gridClients;
        private System.Windows.Forms.Button btnShowClientLogs;
        private Controls.HelpTooltip htModuleTest;
        private System.Windows.Forms.Button btnModuleRun;
        private System.Windows.Forms.Panel pnlRunModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn collsInstCount;
        private System.Windows.Forms.DataGridViewImageColumn settingsCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsEnabledClients;
        private System.Windows.Forms.DataGridViewTextBoxColumn collsInstCountClients;
        private System.Windows.Forms.DataGridViewImageColumn settingsColClients;
    }
}