namespace MonkeyJobTool.Forms
{
    partial class ModuleSettingsForm
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
            this.pnlSettings = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlSettings
            // 
            this.pnlSettings.AutoScroll = true;
            this.pnlSettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlSettings.Location = new System.Drawing.Point(12, 12);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(459, 280);
            this.pnlSettings.TabIndex = 0;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BackColor = System.Drawing.Color.Moccasin;
            this.btnSaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveConfig.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Location = new System.Drawing.Point(396, 298);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 6;
            this.btnSaveConfig.Text = "Сохранить";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // ModuleSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(483, 328);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.pnlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModuleSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки модуля";
            this.Load += new System.EventHandler(this.ModuleSettingsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlSettings;
        private System.Windows.Forms.Button btnSaveConfig;


    }
}