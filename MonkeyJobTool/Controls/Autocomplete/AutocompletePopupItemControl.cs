using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Entities;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutocompletePopupItemControl : UserControl
    {
        
        private Label _rtl;
        public string Value;
        public int Index { get; set; }

        public delegate void OnChildMouseEnterDelegate();
        public event OnChildMouseEnterDelegate OnChildMouseEnter;
        public event OnChildMouseEnterDelegate OnChildMouseLeave;

        public AutocompletePopupItemControl( string value)
        {
            
            this.Value = value;
            InitializeComponent();
            
            
            Label rtl = new Label()
            {
                AutoSize = true,
                MaximumSize = new Size(this.Width-5, 0)
            };
            rtl.Font = new Font("MS Reference Sans Serif", 15.57F);
                      
            
            rtl.Text = Value;
            rtl.Left = 5;
            rtl.Top = 10;
            
            _rtl = rtl;
            rtl.MouseEnter += rtl_MouseEnter;
            rtl.MouseLeave += rtl_MouseLeave;
            
            this.Controls.Add(rtl);
        }

        void rtl_MouseLeave(object sender, EventArgs e)
        {
            if (OnChildMouseLeave != null)
            {
                OnChildMouseLeave();
            }
        }

        void rtl_MouseEnter(object sender, EventArgs e)
        {
            if (OnChildMouseEnter != null)
            {
                OnChildMouseEnter();
            }
        }

        private void AutocompleteItemControl_Load(object sender, EventArgs e)
        {
            
            
        }
       
        private void AutocompletePopupItemControl_Paint(object sender, PaintEventArgs e)
        {
            
        }

        public void SetBackColor(Color color)
        {
            this.BackColor = color;
            this._rtl.BackColor = color;
        }
    }
}
