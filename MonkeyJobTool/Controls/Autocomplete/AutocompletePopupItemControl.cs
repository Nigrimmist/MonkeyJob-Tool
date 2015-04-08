using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Entities;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutocompletePopupItemControl : UserControl
    {
        private List<SelectableWordPart> _wordParts;
        private RichTextLabel _rtl;
        public string Value;
        public int Index { get; set; }

        public delegate void OnChildMouseEnterDelegate();
        public event OnChildMouseEnterDelegate OnChildMouseEnter;
        public event OnChildMouseEnterDelegate OnChildMouseLeave;

        public AutocompletePopupItemControl(List<SelectableWordPart> wordParts, string value)
        {
            this._wordParts = wordParts;
            this.Value = value;
            InitializeComponent();

            RichTextLabel rtl = new RichTextLabel();
            rtl.Font = new Font("MS Reference Sans Serif", 15.57F);
            //rtl.BackColor = Color.LemonChiffon;            
            StringBuilder sb = new StringBuilder();
            sb.Append(@"{\rtf1\ansi ");
            foreach (var wordPart in _wordParts)
            {
                if (wordPart.IsSelected)
                {
                    sb.Append(@"\b ");
                }
                sb.Append(ConvertString2RTF(wordPart.WordPart));
                if (wordPart.IsSelected)
                {
                    sb.Append(@"\b0 ");
                }
            }
            sb.Append(@"}");
            rtl.Width = this.Width;
            rtl.Rtf = sb.ToString();

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
        private string ConvertString2RTF(string input)
        {
            //first take care of special RTF chars
            StringBuilder backslashed = new StringBuilder(input);
            backslashed.Replace(@"\", @"\\");
            backslashed.Replace(@"{", @"\{");
            backslashed.Replace(@"}", @"\}");

            //then convert the string char by char
            StringBuilder sb = new StringBuilder();
            foreach (char character in backslashed.ToString())
            {
                if (character <= 0x7f)
                    sb.Append(character);
                else
                    sb.Append("\\u" + Convert.ToUInt32(character) + "?");
            }
            return sb.ToString();
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
