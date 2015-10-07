namespace MonkeyJobTool.Forms
{
    partial class ModuleLogsForm
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
            this.fldMessages = new System.Windows.Forms.TextBox();
            this.updateLogTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // fldMessages
            // 
            this.fldMessages.Location = new System.Drawing.Point(12, 12);
            this.fldMessages.Multiline = true;
            this.fldMessages.Name = "fldMessages";
            this.fldMessages.Size = new System.Drawing.Size(446, 284);
            this.fldMessages.TabIndex = 0;
            // 
            // updateLogTimer
            // 
            this.updateLogTimer.Enabled = true;
            this.updateLogTimer.Interval = 1000;
            this.updateLogTimer.Tick += new System.EventHandler(this.updateLogTimer_Tick);
            // 
            // ModuleLogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 308);
            this.Controls.Add(this.fldMessages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModuleLogsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Логи модуля";
            this.Load += new System.EventHandler(this.ModuleLogs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fldMessages;
        private System.Windows.Forms.Timer updateLogTimer;
    }
}