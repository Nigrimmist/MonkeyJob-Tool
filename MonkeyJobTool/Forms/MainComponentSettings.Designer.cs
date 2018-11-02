namespace MonkeyJobTool.Forms
{
    partial class MainComponentSettings
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
            this.gridClients = new System.Windows.Forms.DataGridView();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnShowModuleCommunication = new System.Windows.Forms.Button();
            this.btnEnabledDisableClient = new System.Windows.Forms.Button();
            this.btnShowClientLogs = new System.Windows.Forms.Button();
            this.btnRemoveClient = new System.Windows.Forms.Button();
            this.btnAddClient = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridClients)).BeginInit();
            this.SuspendLayout();
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
            this.colIsEnabled,
            this.settingsCol});
            this.gridClients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridClients.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridClients.Location = new System.Drawing.Point(16, 15);
            this.gridClients.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridClients.MultiSelect = false;
            this.gridClients.Name = "gridClients";
            this.gridClients.ReadOnly = true;
            this.gridClients.RowHeadersVisible = false;
            this.gridClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridClients.Size = new System.Drawing.Size(565, 256);
            this.gridClients.TabIndex = 9;
            this.gridClients.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridClients_CellClick);
            this.gridClients.SelectionChanged += new System.EventHandler(this.gridClients_SelectionChanged);
            // 
            // colClient
            // 
            this.colClient.FillWeight = 23.34584F;
            this.colClient.HeaderText = "№";
            this.colClient.Name = "colClient";
            this.colClient.ReadOnly = true;
            // 
            // colIsEnabled
            // 
            this.colIsEnabled.FillWeight = 155.6389F;
            this.colIsEnabled.HeaderText = "Статус";
            this.colIsEnabled.Name = "colIsEnabled";
            this.colIsEnabled.ReadOnly = true;
            // 
            // settingsCol
            // 
            this.settingsCol.FillWeight = 36.01523F;
            this.settingsCol.HeaderText = "Настройки";
            this.settingsCol.Name = "settingsCol";
            this.settingsCol.ReadOnly = true;
            this.settingsCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.settingsCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnShowModuleCommunication
            // 
            this.btnShowModuleCommunication.BackColor = System.Drawing.Color.Moccasin;
            this.btnShowModuleCommunication.Enabled = false;
            this.btnShowModuleCommunication.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowModuleCommunication.ForeColor = System.Drawing.Color.Black;
            this.btnShowModuleCommunication.Location = new System.Drawing.Point(589, 114);
            this.btnShowModuleCommunication.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowModuleCommunication.Name = "btnShowModuleCommunication";
            this.btnShowModuleCommunication.Size = new System.Drawing.Size(157, 48);
            this.btnShowModuleCommunication.TabIndex = 12;
            this.btnShowModuleCommunication.Text = "Зависимость от модулей";
            this.btnShowModuleCommunication.UseVisualStyleBackColor = false;
            this.btnShowModuleCommunication.Click += new System.EventHandler(this.btnShowModuleCommunication_Click);
            // 
            // btnEnabledDisableClient
            // 
            this.btnEnabledDisableClient.BackColor = System.Drawing.Color.Moccasin;
            this.btnEnabledDisableClient.Enabled = false;
            this.btnEnabledDisableClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEnabledDisableClient.ForeColor = System.Drawing.Color.Black;
            this.btnEnabledDisableClient.Location = new System.Drawing.Point(589, 15);
            this.btnEnabledDisableClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEnabledDisableClient.Name = "btnEnabledDisableClient";
            this.btnEnabledDisableClient.Size = new System.Drawing.Size(157, 37);
            this.btnEnabledDisableClient.TabIndex = 13;
            this.btnEnabledDisableClient.Text = "Вылючить";
            this.btnEnabledDisableClient.UseVisualStyleBackColor = false;
            this.btnEnabledDisableClient.Click += new System.EventHandler(this.btnEnabledDisableClient_Click);
            // 
            // btnShowClientLogs
            // 
            this.btnShowClientLogs.BackColor = System.Drawing.Color.Moccasin;
            this.btnShowClientLogs.Enabled = false;
            this.btnShowClientLogs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowClientLogs.ForeColor = System.Drawing.Color.Black;
            this.btnShowClientLogs.Location = new System.Drawing.Point(589, 59);
            this.btnShowClientLogs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowClientLogs.Name = "btnShowClientLogs";
            this.btnShowClientLogs.Size = new System.Drawing.Size(157, 48);
            this.btnShowClientLogs.TabIndex = 14;
            this.btnShowClientLogs.Text = "Просмотреть лог активности";
            this.btnShowClientLogs.UseVisualStyleBackColor = false;
            this.btnShowClientLogs.Click += new System.EventHandler(this.btnShowClientLogs_Click);
            // 
            // btnRemoveClient
            // 
            this.btnRemoveClient.BackColor = System.Drawing.Color.Moccasin;
            this.btnRemoveClient.Enabled = false;
            this.btnRemoveClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveClient.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveClient.Location = new System.Drawing.Point(259, 278);
            this.btnRemoveClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveClient.Name = "btnRemoveClient";
            this.btnRemoveClient.Size = new System.Drawing.Size(157, 37);
            this.btnRemoveClient.TabIndex = 15;
            this.btnRemoveClient.Text = "Удалить";
            this.btnRemoveClient.UseVisualStyleBackColor = false;
            this.btnRemoveClient.Click += new System.EventHandler(this.btnRemoveClient_Click);
            // 
            // btnAddClient
            // 
            this.btnAddClient.BackColor = System.Drawing.Color.Moccasin;
            this.btnAddClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddClient.ForeColor = System.Drawing.Color.Black;
            this.btnAddClient.Location = new System.Drawing.Point(424, 278);
            this.btnAddClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddClient.Name = "btnAddClient";
            this.btnAddClient.Size = new System.Drawing.Size(157, 37);
            this.btnAddClient.TabIndex = 16;
            this.btnAddClient.Text = "Добавить новый";
            this.btnAddClient.UseVisualStyleBackColor = false;
            this.btnAddClient.Click += new System.EventHandler(this.btnAddClient_Click);
            // 
            // MainComponentSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 331);
            this.Controls.Add(this.btnAddClient);
            this.Controls.Add(this.btnRemoveClient);
            this.Controls.Add(this.btnShowClientLogs);
            this.Controls.Add(this.btnEnabledDisableClient);
            this.Controls.Add(this.btnShowModuleCommunication);
            this.Controls.Add(this.gridClients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainComponentSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Клиенты";
            this.Load += new System.EventHandler(this.MainComponentSettings_Load);
            this.Shown += new System.EventHandler(this.MainComponentSettings_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridClients)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridClients;
        private System.Windows.Forms.Button btnShowModuleCommunication;
        private System.Windows.Forms.Button btnEnabledDisableClient;
        private System.Windows.Forms.Button btnShowClientLogs;
        private System.Windows.Forms.Button btnRemoveClient;
        private System.Windows.Forms.Button btnAddClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsEnabled;
        private System.Windows.Forms.DataGridViewImageColumn settingsCol;
    }
}