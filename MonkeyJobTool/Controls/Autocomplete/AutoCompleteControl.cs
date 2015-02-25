using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Forms;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutoCompleteControl : UserControl
    {
        private AutocompletePopupControl popup = new AutocompletePopupControl();
        public Form ParentForm { get; set; }

        public delegate List<string> GetItemsFromSource(string term);
        public GetItemsFromSource DataFilterFunc;

        public delegate void OnCommandReceivedDelegate(string command);
        public event OnCommandReceivedDelegate OnCommandReceived;  

        public AutoCompleteControl()
        {
            InitializeComponent();
        }

        private void AutoCompleteControl_Load(object sender, EventArgs e)
        {

        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            string term = txtCommand.Text;
            if (!string.IsNullOrEmpty(term))
            {
                var actualItems = DataFilterFunc(term);
                if (actualItems.Any())
                {
                    var popupModel = new AutocompletePopupInfo();
                    foreach (var item in actualItems)
                    {
                        popupModel.Items.Add(new AutocompletePopupItem()
                        {
                            WordParts = GetWordParts(item, term)
                        });
                    }
                    popup.Model = popupModel;
                    popup.ShowItems();
                    ParentForm.ToTop();//restore focus
                    popup.Top = ParentForm.Top-popup.Height;
                    popup.Left = ParentForm.Left;
                    popup.Width = ParentForm.Width;
                }
                else
                {
                    popup.Hide();
                }
            }
            else
            {
                popup.Hide();
            }
        }

        private List<SelectableWordPart> GetWordParts(string word, string term)
        {
            List<SelectableWordPart> toReturn = new List<SelectableWordPart>();

            int foundIndex = word.IndexOf(term, System.StringComparison.InvariantCultureIgnoreCase);
            if (foundIndex != -1)
            {
                string foundPart = word.Substring(foundIndex, term.Length);

                if (foundIndex > 0)
                {
                    var prefixPart = word.Substring(0, foundIndex);
                    toReturn.Add(new SelectableWordPart()
                    {
                        WordPart = prefixPart
                    });
                }

                string postFixPart = word.Substring(foundIndex + foundPart.Length);
                toReturn.Add(new SelectableWordPart()
                {
                    WordPart = foundPart,
                    IsSelected = true
                });
                toReturn.Add(new SelectableWordPart()
                {
                    WordPart = postFixPart
                });
            }
            else
            {
                toReturn.Add(new SelectableWordPart()
                {
                    WordPart = word
                });
            }
            
            return toReturn;
        }

        private void txtCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (OnCommandReceived != null)
                {
                    OnCommandReceived(txtCommand.Text);
                }
            }
        }
    }
}
