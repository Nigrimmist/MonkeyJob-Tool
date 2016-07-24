namespace MonkeyJobTool.Forms
{
    partial class ClientToModulesForm
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
            this.lstAvailableModules = new System.Windows.Forms.ListBox();
            this.lstCheckedModules = new System.Windows.Forms.ListBox();
            this.btnAddModule = new System.Windows.Forms.Button();
            this.btnRemoveModule = new System.Windows.Forms.Button();
            this.rdByModuleType = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlExceptType = new System.Windows.Forms.Panel();
            this.cmbExceptType = new System.Windows.Forms.ComboBox();
            this.rdExceptType = new System.Windows.Forms.RadioButton();
            this.rdAll = new System.Windows.Forms.RadioButton();
            this.pnlByType = new System.Windows.Forms.Panel();
            this.cmbModuleTypes = new System.Windows.Forms.ComboBox();
            this.pnlListOfModules = new System.Windows.Forms.Panel();
            this.rdExceptModules = new System.Windows.Forms.RadioButton();
            this.rdOnlyModules = new System.Windows.Forms.RadioButton();
            this.lblModuleLabel = new System.Windows.Forms.Label();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.pnlExceptType.SuspendLayout();
            this.pnlByType.SuspendLayout();
            this.pnlListOfModules.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstAvailableModules
            // 
            this.lstAvailableModules.FormattingEnabled = true;
            this.lstAvailableModules.Location = new System.Drawing.Point(3, 23);
            this.lstAvailableModules.Name = "lstAvailableModules";
            this.lstAvailableModules.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailableModules.Size = new System.Drawing.Size(155, 238);
            this.lstAvailableModules.TabIndex = 1;
            // 
            // lstCheckedModules
            // 
            this.lstCheckedModules.FormattingEnabled = true;
            this.lstCheckedModules.Location = new System.Drawing.Point(264, 23);
            this.lstCheckedModules.Name = "lstCheckedModules";
            this.lstCheckedModules.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstCheckedModules.Size = new System.Drawing.Size(155, 238);
            this.lstCheckedModules.TabIndex = 2;
            // 
            // btnAddModule
            // 
            this.btnAddModule.Location = new System.Drawing.Point(173, 64);
            this.btnAddModule.Name = "btnAddModule";
            this.btnAddModule.Size = new System.Drawing.Size(75, 45);
            this.btnAddModule.TabIndex = 3;
            this.btnAddModule.Text = ">>";
            this.btnAddModule.UseVisualStyleBackColor = true;
            this.btnAddModule.Click += new System.EventHandler(this.btnAddModule_Click);
            // 
            // btnRemoveModule
            // 
            this.btnRemoveModule.Location = new System.Drawing.Point(173, 115);
            this.btnRemoveModule.Name = "btnRemoveModule";
            this.btnRemoveModule.Size = new System.Drawing.Size(75, 45);
            this.btnRemoveModule.TabIndex = 4;
            this.btnRemoveModule.Text = "<<";
            this.btnRemoveModule.UseVisualStyleBackColor = true;
            this.btnRemoveModule.Click += new System.EventHandler(this.btnRemoveModule_Click);
            // 
            // rdByModuleType
            // 
            this.rdByModuleType.AutoSize = true;
            this.rdByModuleType.Location = new System.Drawing.Point(6, 40);
            this.rdByModuleType.Name = "rdByModuleType";
            this.rdByModuleType.Size = new System.Drawing.Size(147, 17);
            this.rdByModuleType.TabIndex = 5;
            this.rdByModuleType.TabStop = true;
            this.rdByModuleType.Text = "Активен по типу модуля";
            this.rdByModuleType.UseVisualStyleBackColor = true;
            this.rdByModuleType.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pnlExceptType);
            this.groupBox1.Controls.Add(this.rdExceptType);
            this.groupBox1.Controls.Add(this.rdAll);
            this.groupBox1.Controls.Add(this.pnlByType);
            this.groupBox1.Controls.Add(this.pnlListOfModules);
            this.groupBox1.Controls.Add(this.rdExceptModules);
            this.groupBox1.Controls.Add(this.rdOnlyModules);
            this.groupBox1.Controls.Add(this.rdByModuleType);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 407);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим зависимости";
            // 
            // pnlExceptType
            // 
            this.pnlExceptType.Controls.Add(this.cmbExceptType);
            this.pnlExceptType.Location = new System.Drawing.Point(222, 61);
            this.pnlExceptType.Name = "pnlExceptType";
            this.pnlExceptType.Size = new System.Drawing.Size(237, 27);
            this.pnlExceptType.TabIndex = 12;
            // 
            // cmbExceptType
            // 
            this.cmbExceptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExceptType.FormattingEnabled = true;
            this.cmbExceptType.Location = new System.Drawing.Point(2, 1);
            this.cmbExceptType.Name = "cmbExceptType";
            this.cmbExceptType.Size = new System.Drawing.Size(227, 21);
            this.cmbExceptType.TabIndex = 6;
            // 
            // rdExceptType
            // 
            this.rdExceptType.AutoSize = true;
            this.rdExceptType.Location = new System.Drawing.Point(6, 62);
            this.rdExceptType.Name = "rdExceptType";
            this.rdExceptType.Size = new System.Drawing.Size(139, 17);
            this.rdExceptType.TabIndex = 11;
            this.rdExceptType.TabStop = true;
            this.rdExceptType.Text = "Для всех типов кроме";
            this.rdExceptType.UseVisualStyleBackColor = true;
            this.rdExceptType.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rdAll
            // 
            this.rdAll.AutoSize = true;
            this.rdAll.Location = new System.Drawing.Point(6, 17);
            this.rdAll.Name = "rdAll";
            this.rdAll.Size = new System.Drawing.Size(160, 17);
            this.rdAll.TabIndex = 11;
            this.rdAll.TabStop = true;
            this.rdAll.Text = "Активен для всех модулей";
            this.rdAll.UseVisualStyleBackColor = true;
            this.rdAll.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // pnlByType
            // 
            this.pnlByType.Controls.Add(this.cmbModuleTypes);
            this.pnlByType.Location = new System.Drawing.Point(221, 34);
            this.pnlByType.Name = "pnlByType";
            this.pnlByType.Size = new System.Drawing.Size(238, 26);
            this.pnlByType.TabIndex = 10;
            // 
            // cmbModuleTypes
            // 
            this.cmbModuleTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModuleTypes.FormattingEnabled = true;
            this.cmbModuleTypes.Location = new System.Drawing.Point(3, 3);
            this.cmbModuleTypes.Name = "cmbModuleTypes";
            this.cmbModuleTypes.Size = new System.Drawing.Size(227, 21);
            this.cmbModuleTypes.TabIndex = 6;
            // 
            // pnlListOfModules
            // 
            this.pnlListOfModules.Controls.Add(this.lblModuleLabel);
            this.pnlListOfModules.Controls.Add(this.lstAvailableModules);
            this.pnlListOfModules.Controls.Add(this.btnAddModule);
            this.pnlListOfModules.Controls.Add(this.btnRemoveModule);
            this.pnlListOfModules.Controls.Add(this.lstCheckedModules);
            this.pnlListOfModules.Location = new System.Drawing.Point(6, 125);
            this.pnlListOfModules.Name = "pnlListOfModules";
            this.pnlListOfModules.Size = new System.Drawing.Size(427, 267);
            this.pnlListOfModules.TabIndex = 9;
            // 
            // rdExceptModules
            // 
            this.rdExceptModules.AutoSize = true;
            this.rdExceptModules.Location = new System.Drawing.Point(6, 109);
            this.rdExceptModules.Name = "rdExceptModules";
            this.rdExceptModules.Size = new System.Drawing.Size(156, 17);
            this.rdExceptModules.TabIndex = 8;
            this.rdExceptModules.TabStop = true;
            this.rdExceptModules.Text = "Для всех модулей, кроме";
            this.rdExceptModules.UseVisualStyleBackColor = true;
            this.rdExceptModules.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rdOnlyModules
            // 
            this.rdOnlyModules.AutoSize = true;
            this.rdOnlyModules.Location = new System.Drawing.Point(6, 86);
            this.rdOnlyModules.Name = "rdOnlyModules";
            this.rdOnlyModules.Size = new System.Drawing.Size(192, 17);
            this.rdOnlyModules.TabIndex = 7;
            this.rdOnlyModules.TabStop = true;
            this.rdOnlyModules.Text = "Только для конкретных модулей";
            this.rdOnlyModules.UseVisualStyleBackColor = true;
            this.rdOnlyModules.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // lblModuleLabel
            // 
            this.lblModuleLabel.AutoSize = true;
            this.lblModuleLabel.Location = new System.Drawing.Point(261, 5);
            this.lblModuleLabel.Name = "lblModuleLabel";
            this.lblModuleLabel.Size = new System.Drawing.Size(35, 13);
            this.lblModuleLabel.TabIndex = 13;
            this.lblModuleLabel.Text = "label1";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.Moccasin;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Location = new System.Drawing.Point(475, 430);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 7;
            this.btnSaveConfig.Text = "Сохранить";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // ClientToModulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 465);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ClientToModulesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Зависимость от модулей";
            this.Load += new System.EventHandler(this.ClientToModulesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlExceptType.ResumeLayout(false);
            this.pnlByType.ResumeLayout(false);
            this.pnlListOfModules.ResumeLayout(false);
            this.pnlListOfModules.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstAvailableModules;
        private System.Windows.Forms.ListBox lstCheckedModules;
        private System.Windows.Forms.Button btnAddModule;
        private System.Windows.Forms.Button btnRemoveModule;
        private System.Windows.Forms.RadioButton rdByModuleType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlByType;
        private System.Windows.Forms.ComboBox cmbModuleTypes;
        private System.Windows.Forms.Panel pnlListOfModules;
        private System.Windows.Forms.RadioButton rdExceptModules;
        private System.Windows.Forms.RadioButton rdOnlyModules;
        private System.Windows.Forms.RadioButton rdAll;
        private System.Windows.Forms.Panel pnlExceptType;
        private System.Windows.Forms.ComboBox cmbExceptType;
        private System.Windows.Forms.RadioButton rdExceptType;
        private System.Windows.Forms.Label lblModuleLabel;
        private System.Windows.Forms.Button btnSaveConfig;
    }
}