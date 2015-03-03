﻿namespace MonkeyJobTool.Forms
{
    partial class InfoPopup
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.IconPic = new System.Windows.Forms.PictureBox();
            this.txtMessage = new MonkeyJobTool.Controls.Autocomplete.RichTextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.IconPic)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(30, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(43, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            // 
            // IconPic
            // 
            this.IconPic.Image = global::MonkeyJobTool.Properties.Resources.monkey_small;
            this.IconPic.Location = new System.Drawing.Point(4, 2);
            this.IconPic.Name = "IconPic";
            this.IconPic.Size = new System.Drawing.Size(27, 26);
            this.IconPic.TabIndex = 1;
            this.IconPic.TabStop = false;
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Location = new System.Drawing.Point(12, 34);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(236, 57);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.TabStop = false;
            this.txtMessage.Text = "";
            this.txtMessage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtMessage_MouseUp);
            // 
            // InfoPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(260, 102);
            this.ControlBox = false;
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.IconPic);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InfoPopup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "InfoPopup";
            this.Load += new System.EventHandler(this.InfoPopup_Load);
            this.Click += new System.EventHandler(this.InfoPopup_Click);
            this.MouseEnter += new System.EventHandler(this.InfoPopup_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.InfoPopup_MouseLeave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InfoPopup_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.IconPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox IconPic;
        private Controls.Autocomplete.RichTextLabel txtMessage;
    }
}