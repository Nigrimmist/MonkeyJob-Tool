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
            this.components = new System.ComponentModel.Container();
            
            this.pbEnter = new System.Windows.Forms.PictureBox();
            this.pnlEnterIconHolder = new System.Windows.Forms.Panel();
            this.timerEnterIconChange = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbEnter)).BeginInit();
            this.pnlEnterIconHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCommand
            // 
            this.txtCommand = new AutoCompleteTextBox();
            this.txtCommand.BackColor = System.Drawing.SystemColors.Control;
            this.txtCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCommand.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCommand.Font = new System.Drawing.Font("MS Reference Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommand.Location = new System.Drawing.Point(3, 3);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(179, 26);
            this.txtCommand.TabIndex = 0;
            
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommand_KeyDown);
            // 
            // pbEnter
            // 
            this.pbEnter.BackgroundImage = global::MonkeyJobTool.Properties.Resources.enter5;
            this.pbEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbEnter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEnter.Location = new System.Drawing.Point(4, 8);
            this.pbEnter.Name = "pbEnter";
            this.pbEnter.Size = new System.Drawing.Size(16, 16);
            this.pbEnter.TabIndex = 1;
            this.pbEnter.TabStop = false;
            this.pbEnter.Click += new System.EventHandler(this.pbEnter_Click);
            this.pbEnter.MouseEnter += new System.EventHandler(this.pbEnter_MouseEnter);
            this.pbEnter.MouseLeave += new System.EventHandler(this.pbEnter_MouseLeave);
            // 
            // pnlEnterIconHolder
            // 
            this.pnlEnterIconHolder.Controls.Add(this.pbEnter);
            this.pnlEnterIconHolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlEnterIconHolder.Location = new System.Drawing.Point(185, 0);
            this.pnlEnterIconHolder.Name = "pnlEnterIconHolder";
            this.pnlEnterIconHolder.Size = new System.Drawing.Size(22, 33);
            this.pnlEnterIconHolder.TabIndex = 2;
            this.pnlEnterIconHolder.Click += new System.EventHandler(this.pnlEnterIconHolder_Click);
            this.pnlEnterIconHolder.MouseEnter += new System.EventHandler(this.pnlEnterIconHolder_MouseEnter);
            this.pnlEnterIconHolder.MouseLeave += new System.EventHandler(this.pnlEnterIconHolder_MouseLeave);
            // 
            // timerEnterIconChange
            // 
            this.timerEnterIconChange.Tick += new System.EventHandler(this.timerEnterIconChange_Tick);
            // 
            // AutoCompleteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlEnterIconHolder);
            this.Controls.Add(this.txtCommand);
            this.Name = "AutoCompleteControl";
            this.Size = new System.Drawing.Size(209, 33);
            this.Load += new System.EventHandler(this.AutoCompleteControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEnter)).EndInit();
            this.pnlEnterIconHolder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AutoCompleteTextBox txtCommand;
        private System.Windows.Forms.PictureBox pbEnter;
        private System.Windows.Forms.Panel pnlEnterIconHolder;
        private System.Windows.Forms.Timer timerEnterIconChange;
    }
}
