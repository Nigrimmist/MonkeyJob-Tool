namespace MonkeyJobTool.Forms
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
            this.components = new System.ComponentModel.Container();
            this.closeTimer = new System.Windows.Forms.Timer(this.components);
            this.IconPic = new System.Windows.Forms.PictureBox();
            this.lblCloseHint = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.rtTitle = new MonkeyJobTool.Controls.Autocomplete.RichTextLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlCloseHint = new System.Windows.Forms.Panel();
            this.txtMessage = new MonkeyJobTool.Controls.Autocomplete.RichTextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.IconPic)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlCloseHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeTimer
            // 
            this.closeTimer.Interval = 1000;
            this.closeTimer.Tick += new System.EventHandler(this.closeTimer_Tick);
            // 
            // IconPic
            // 
            this.IconPic.BackgroundImage = global::MonkeyJobTool.Properties.Resources.monkey_highres_img;
            this.IconPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IconPic.Location = new System.Drawing.Point(3, 1);
            this.IconPic.Margin = new System.Windows.Forms.Padding(0);
            this.IconPic.Name = "IconPic";
            this.IconPic.Size = new System.Drawing.Size(26, 26);
            this.IconPic.TabIndex = 1;
            this.IconPic.TabStop = false;
            // 
            // lblCloseHint
            // 
            this.lblCloseHint.AutoSize = true;
            this.lblCloseHint.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCloseHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCloseHint.ForeColor = System.Drawing.Color.DarkGray;
            this.lblCloseHint.Location = new System.Drawing.Point(30, 0);
            this.lblCloseHint.Name = "lblCloseHint";
            this.lblCloseHint.Size = new System.Drawing.Size(157, 13);
            this.lblCloseHint.TabIndex = 10;
            this.lblCloseHint.Text = "*Правый клик - для закрытия";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.rtTitle);
            this.pnlHeader.Controls.Add(this.IconPic);
            this.pnlHeader.ForeColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Location = new System.Drawing.Point(1, 1);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(258, 38);
            this.pnlHeader.TabIndex = 12;
            // 
            // rtTitle
            // 
            this.rtTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rtTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtTitle.Location = new System.Drawing.Point(33, 4);
            this.rtTitle.Name = "rtTitle";
            this.rtTitle.ReadOnly = true;
            this.rtTitle.Size = new System.Drawing.Size(221, 21);
            this.rtTitle.TabIndex = 11;
            this.rtTitle.TabStop = false;
            this.rtTitle.Text = "Title";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlCloseHint);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Controls.Add(this.txtMessage);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(259, 104);
            this.pnlMain.TabIndex = 13;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            // 
            // pnlCloseHint
            // 
            this.pnlCloseHint.Controls.Add(this.lblCloseHint);
            this.pnlCloseHint.Location = new System.Drawing.Point(62, 69);
            this.pnlCloseHint.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCloseHint.Name = "pnlCloseHint";
            this.pnlCloseHint.Size = new System.Drawing.Size(187, 19);
            this.pnlCloseHint.TabIndex = 13;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(14, 42);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(236, 17);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.TabStop = false;
            this.txtMessage.Text = "";
            // 
            // InfoPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(261, 146);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMain);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InfoPopup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "InfoPopup";
            this.Deactivate += new System.EventHandler(this.InfoPopup_Deactivate);
            this.Load += new System.EventHandler(this.InfoPopup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.IconPic)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlCloseHint.ResumeLayout(false);
            this.pnlCloseHint.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox IconPic;
        private Controls.Autocomplete.RichTextLabel txtMessage;
        private System.Windows.Forms.Timer closeTimer;
        private System.Windows.Forms.Label lblCloseHint;
        private Controls.Autocomplete.RichTextLabel rtTitle;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlCloseHint;
    }
}