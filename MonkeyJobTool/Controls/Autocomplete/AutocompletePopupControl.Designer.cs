namespace MonkeyJobTool.Controls.Autocomplete
{
    partial class AutocompletePopupControl
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
            this.pnlItems = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlItems
            // 
            this.pnlItems.AutoSize = true;
            this.pnlItems.Location = new System.Drawing.Point(0, 0);
            this.pnlItems.Margin = new System.Windows.Forms.Padding(0);
            this.pnlItems.Name = "pnlItems";
            this.pnlItems.Size = new System.Drawing.Size(202, 21);
            this.pnlItems.TabIndex = 0;
            // 
            // AutocompletePopupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(232, 30);
            this.Controls.Add(this.pnlItems);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutocompletePopupControl";
            this.ShowInTaskbar = false;
            this.Text = "AutocompletePopup";
            this.Load += new System.EventHandler(this.AutocompletePopupControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlItems;

    }
}