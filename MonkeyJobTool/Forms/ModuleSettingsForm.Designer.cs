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
            this.pnlSettingControls = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlSettingControls
            // 
            this.pnlSettingControls.Location = new System.Drawing.Point(13, 13);
            this.pnlSettingControls.Name = "pnlSettingControls";
            this.pnlSettingControls.Size = new System.Drawing.Size(458, 287);
            this.pnlSettingControls.TabIndex = 0;
            // 
            // ModuleSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(483, 312);
            this.Controls.Add(this.pnlSettingControls);
            this.Name = "ModuleSettingsForm";
            this.Text = "Настройки модуля";
            this.Load += new System.EventHandler(this.ModuleSettingsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSettingControls;
    }
}