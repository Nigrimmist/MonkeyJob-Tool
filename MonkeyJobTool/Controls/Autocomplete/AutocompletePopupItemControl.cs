using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
            int totalWidth = 0;
            foreach (var wordPart in wordParts)
            {
                var newLabel = new Label();
                //newLabel.AutoSize = false;
                
                newLabel.Text= wordPart.WordPart;
                if (wordPart.IsSelected)
                {
                    newLabel.Font = new Font(newLabel.Font, FontStyle.Bold);
                }
                newLabel.Padding = new Padding(0);
                //using (Graphics g = CreateGraphics())
                //{
                //    SizeF size = g.MeasureString(wordPart.WordPart, newLabel.Font);
                //    newLabel.Width = (int)Math.Ceiling(size.Width);
                //    newLabel.Height = (int)Math.Ceiling(size.Height);
                //}

                newLabel.Left = totalWidth;
                totalWidth += newLabel.Width;
                this.Controls.Add(newLabel);
            }
        }


    }
}
