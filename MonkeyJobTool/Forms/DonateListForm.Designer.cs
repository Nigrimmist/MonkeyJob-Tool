using MonkeyJobTool.Properties;

namespace MonkeyJobTool.Forms
{
    partial class DonateListForm
    {
        // <summary>
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gwDonate = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDonateClick = new System.Windows.Forms.Button();
            this.lblUpdateInProgress = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gwDonate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gwDonate
            // 
            this.gwDonate.AllowUserToAddRows = false;
            this.gwDonate.AllowUserToDeleteRows = false;
            this.gwDonate.AllowUserToResizeRows = false;
            this.gwDonate.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gwDonate.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gwDonate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gwDonate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.gwDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gwDonate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gwDonate.Location = new System.Drawing.Point(12, 47);
            this.gwDonate.MultiSelect = false;
            this.gwDonate.Name = "gwDonate";
            this.gwDonate.ReadOnly = true;
            this.gwDonate.RowHeadersVisible = false;
            this.gwDonate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gwDonate.ShowEditingIcon = false;
            this.gwDonate.Size = new System.Drawing.Size(852, 339);
            this.gwDonate.TabIndex = 47;
            this.gwDonate.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gwFavorites_CellContentClick);
            this.gwDonate.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gwDonate_CellPainting);
            this.gwDonate.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.gwDonate_RowPostPaint);
            // 
            // Column1
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "Кто";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Сколько";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Комментарий";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Когда";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::MonkeyJobTool.Properties.Resources.closeIcon;
            this.pictureBox1.Location = new System.Drawing.Point(843, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 22);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(364, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 20);
            this.label1.TabIndex = 50;
            this.label1.Text = "Друзья-меценаты : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(780, 389);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 51;
            this.label2.Text = "Спасибо!";
            // 
            // btnDonateClick
            // 
            this.btnDonateClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnDonateClick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDonateClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDonateClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDonateClick.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDonateClick.Location = new System.Drawing.Point(238, 509);
            this.btnDonateClick.Name = "btnDonateClick";
            this.btnDonateClick.Size = new System.Drawing.Size(390, 36);
            this.btnDonateClick.TabIndex = 53;
            this.btnDonateClick.Text = "Я тоже хочу поблагодарить автора!";
            this.btnDonateClick.UseVisualStyleBackColor = false;
            this.btnDonateClick.Click += new System.EventHandler(this.btnDonateClick_Click);
            // 
            // lblUpdateInProgress
            // 
            this.lblUpdateInProgress.AutoSize = true;
            this.lblUpdateInProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUpdateInProgress.ForeColor = System.Drawing.Color.DarkRed;
            this.lblUpdateInProgress.Location = new System.Drawing.Point(325, 205);
            this.lblUpdateInProgress.Name = "lblUpdateInProgress";
            this.lblUpdateInProgress.Size = new System.Drawing.Size(230, 17);
            this.lblUpdateInProgress.TabIndex = 59;
            this.lblUpdateInProgress.Text = "Идёт обновление, подождите";
            this.lblUpdateInProgress.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(150, 423);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(717, 17);
            this.label3.TabIndex = 54;
            this.label3.Text = "Спасибо за поддержку, которую вы даёте мне. Много бессонных ночей уже ушло и ещё " +
    "уйдёт";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(150, 440);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(639, 17);
            this.label4.TabIndex = 55;
            this.label4.Text = "на написание monkeyjob tool. С вашей поддержкой это делается легче и приятней.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.DarkRed;
            this.label6.Location = new System.Drawing.Point(19, 423);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 17);
            this.label6.TabIndex = 57;
            this.label6.Text = "Дорогие друзья!";
            // 
            // DonateListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(875, 563);
            this.Controls.Add(this.lblUpdateInProgress);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDonateClick);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gwDonate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DonateListForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Благодарности";
            this.Activated += new System.EventHandler(this.DonateForm_Activated);
            this.Load += new System.EventHandler(this.DonateForm_Load);
            this.Shown += new System.EventHandler(this.DonateListForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DonateForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.gwDonate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gwDonate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDonateClick;
        private System.Windows.Forms.Label lblUpdateInProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;

    }
}