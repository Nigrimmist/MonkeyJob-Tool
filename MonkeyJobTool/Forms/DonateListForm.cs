using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Helpers;
using MonkeyJobTool.Managers;
using MonkeyJobTool.Properties;

namespace MonkeyJobTool.Forms
{
    public partial class DonateListForm : Form
    {
        public DonateListForm()
        {
            InitializeComponent();
        }

        private void gwFavorites_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    string link = ((DataGridView)sender).Rows[e.RowIndex].ErrorText;
                    if (!string.IsNullOrEmpty(link))
                        Process.Start(link);
                }
                catch { }
            }
        }

        private void DonateForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DonateForm_Load(object sender, EventArgs e)
        {
            DataBindDonateGrid();
        }

        private List<DonateItem> _donateItems = null; 
        private void DataBindDonateGrid()
        {
            lblUpdateInProgress.Visible = false;
            gwDonate.Rows.Clear();

            if (_donateItems != null)
            {
                foreach (DonateItem item in _donateItems)
                {
                    DataGridViewRow r = new DataGridViewRow();
                    
                    r.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = item.From,
                        Style =
                            new DataGridViewCellStyle() {Alignment = DataGridViewContentAlignment.MiddleCenter}
                    });
                    r.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    r.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = item.USDCount + " $.",
                        Style = new DataGridViewCellStyle() {Alignment = DataGridViewContentAlignment.MiddleCenter}
                    });
                    r.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = !string.IsNullOrEmpty(item.Comment)?(item.Comment.Length > 20 ? item.Comment.Substring(0, 20) + "..." : item.Comment):"",
                        Style =
                            new DataGridViewCellStyle() {Alignment = DataGridViewContentAlignment.MiddleCenter}
                    });
                    r.Cells.Add(new DataGridViewTextBoxCell() {Value = item.CreateDate});

                    gwDonate.Rows.Add(r);
                }
            }
            else
            {
                lblUpdateInProgress.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnDonateClick_Click(object sender, EventArgs e)
        {
            Hide();
            DonateForm d = new DonateForm();
            d.ShowDialog();
            Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _donateItems = DonateManager.GetDonateList();
                this.Invoke(new MethodInvoker(DataBindDonateGrid));
            }
            catch
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    lblUpdateInProgress.Text = "Ошибка. Попробуйте чуть позже.";
                }));
            }
        }

        private void gwDonate_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            
        }

        private void gwDonate_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
        }

        private void DonateForm_Activated(object sender, EventArgs e)
        {
            
        }

        private void DonateListForm_Shown(object sender, EventArgs e)
        {
            new Thread(GoogleAnalytics.LogOpenDonateListForm).Start();
        }

        
    }
}
