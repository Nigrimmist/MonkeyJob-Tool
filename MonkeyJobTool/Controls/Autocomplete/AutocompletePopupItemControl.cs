using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Entities;

namespace MonkeyJobTool.Forms.Autocomplete
{
    public partial class AutocompletePopupItemControl : UserControl
    {
        private List<SelectableWordPart> wordParts;

        public AutocompletePopupItemControl(List<SelectableWordPart> wordParts)
        {
            this.wordParts = wordParts;
            InitializeComponent();
        }

        private void AutocompleteItemControl_Load(object sender, EventArgs e)
        {
            RichTextLabel rtl = new RichTextLabel();
            rtl.Font = new Font("MS Reference Sans Serif", 15.57F);
            //rtl.BackColor = Color.LemonChiffon;            
            StringBuilder sb = new StringBuilder();
            sb.Append(@"{\rtf1\ansi ");
            foreach (var wordPart in wordParts)
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

            rtl.Rtf = sb.ToString();
            rtl.Width = this.Width;
            rtl.Left = 5;
            rtl.Top = 10;
            this.Controls.Add(rtl);
            
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
            //float x = 10;
            //float y = 0;

            //using (Graphics formGraphics = e.Graphics)
            //{
            //    for (int i = 0; i < wordParts.Count; i++)
            //    {
            //        var wordPart = wordParts[i];
            //        FontStyle fontStyle = wordPart.IsSelected ? FontStyle.Regular : FontStyle.Regular;
            //        using (Font drawFont = new Font("MS Reference Sans Serif", 15.57F, fontStyle))
            //        {
            //            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            //            {
            //                HtmlRenderer.Render(formGraphics, "test <b>sss</b>", new PointF(x, y), 200);
                            
            //                //if (wordPart.IsSelected && i!=0) x -= 6; //correction regarding bold
            //                //formGraphics.DrawString(wordPart.WordPart, drawFont, drawBrush, x, y);
            //                SizeF size = formGraphics.MeasureString(wordPart.WordPart, drawFont);
                            
            //                //if (wordPart.IsSelected) x -= 6; //correction regarding bold
            //                x += (size.Width); //correction regarding bold
            //            }
            //        }
            //    }
            //}
        }

        
    }
}
