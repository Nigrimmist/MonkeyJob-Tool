namespace MonkeyJobTool.Forms.Autocomplete
{
    partial class AutocompletePopupItemControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AutocompletePopupItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AutocompletePopupItemControl";
            this.Size = new System.Drawing.Size(262, 39);
            this.Load += new System.EventHandler(this.AutocompleteItemControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AutocompletePopupItemControl_Paint);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
