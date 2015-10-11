namespace MonkeyJobTool.Forms
{
    partial class HelpPopup
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
            this.fldBody = new System.Windows.Forms.RichTextBox();
            this.rtbTitle = new System.Windows.Forms.RichTextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // fldBody
            // 
            this.fldBody.BackColor = System.Drawing.Color.PapayaWhip;
            this.fldBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fldBody.DetectUrls = false;
            this.fldBody.Location = new System.Drawing.Point(8, 27);
            this.fldBody.Name = "fldBody";
            this.fldBody.ReadOnly = true;
            this.fldBody.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.fldBody.Size = new System.Drawing.Size(432, 39);
            this.fldBody.TabIndex = 0;
            this.fldBody.Text = "";
            // 
            // rtbTitle
            // 
            this.rtbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.rtbTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTitle.DetectUrls = false;
            this.rtbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbTitle.Location = new System.Drawing.Point(1, 1);
            this.rtbTitle.Name = "rtbTitle";
            this.rtbTitle.ReadOnly = true;
            this.rtbTitle.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbTitle.Size = new System.Drawing.Size(450, 20);
            this.rtbTitle.TabIndex = 1;
            this.rtbTitle.Text = "";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(45, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "lblTitle";
            // 
            // HelpPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(452, 202);
            this.ControlBox = false;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.rtbTitle);
            this.Controls.Add(this.fldBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HelpPopup";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FullPostForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox fldBody;
        private System.Windows.Forms.RichTextBox rtbTitle;
        private System.Windows.Forms.Label lblTitle;


    }
}