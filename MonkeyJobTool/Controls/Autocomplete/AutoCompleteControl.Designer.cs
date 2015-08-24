namespace MonkeyJobTool.Controls.Autocomplete
{
    partial class AutoCompleteControl
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
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.pbEnter = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnter)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCommand
            // 
            this.txtCommand.BackColor = System.Drawing.SystemColors.Control;
            this.txtCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCommand.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCommand.Font = new System.Drawing.Font("MS Reference Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommand.Location = new System.Drawing.Point(3, 3);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(179, 26);
            this.txtCommand.TabIndex = 0;
            this.txtCommand.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommand_KeyDown);
            // 
            // pbEnter
            // 
            this.pbEnter.BackgroundImage = global::MonkeyJobTool.Properties.Resources.enter5;
            this.pbEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbEnter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEnter.Location = new System.Drawing.Point(187, 7);
            this.pbEnter.Name = "pbEnter";
            this.pbEnter.Size = new System.Drawing.Size(17, 17);
            this.pbEnter.TabIndex = 1;
            this.pbEnter.TabStop = false;
            this.pbEnter.Click += new System.EventHandler(this.pbEnter_Click);
            // 
            // AutoCompleteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbEnter);
            this.Controls.Add(this.txtCommand);
            this.Name = "AutoCompleteControl";
            this.Size = new System.Drawing.Size(209, 33);
            this.Load += new System.EventHandler(this.AutoCompleteControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEnter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.PictureBox pbEnter;
    }
}
