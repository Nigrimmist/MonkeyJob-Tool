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
        
        public delegate DataFilterInfo GetItemsFromSource(string term);
        public GetItemsFromSource DataFilterFunc;
        private bool _isPopupOpen;
        private string _lastPreSelectText = string.Empty;

        public delegate void OnCommandReceivedDelegate(string command);
        public event OnCommandReceivedDelegate OnCommandReceived;

        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
        }

        public AutoCompleteControl()
        {
            InitializeComponent();
        }

        private void AutoCompleteControl_Load(object sender, EventArgs e)
        {
            popup.OnItemHighlighted += popup_OnItemHighlighted;
        }

        void popup_OnItemHighlighted(string highlightedItem)
        {
            txtCommand.Text = highlightedItem+" ";
            txtCommand.SelectionStart = txtCommand.Text.Length;
        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            if (popup.IsInSelectMode) return; //no any suggestions if selectMode enabled
            string term = txtCommand.Text;
            _lastPreSelectText = term;
            if (!string.IsNullOrEmpty(term))
            {
                var filterResult = DataFilterFunc(term);
                term = filterResult.FoundByTerm; //incoming and outgoing term can be different
                if (filterResult.FoundItems.Any())
                {
                    var popupModel = new AutocompletePopupInfo();
                    foreach (var item in filterResult.FoundItems)
                    {
                        popupModel.Items.Add(new AutocompletePopupItem()
                        {
                            WordParts = GetWordParts(item, term),
                            Value = item
                        });
                    }
                    popup.Model = popupModel;
                    popup.ShowItems();
                    ParentForm.ToTop();//restore focus
                    popup.Top = ParentForm.Top-popup.Height;
                    popup.Left = ParentForm.Left;
                    popup.Width = ParentForm.Width;
                    _isPopupOpen = true;
                }
                else
                {
                    popup.Hide();
                    _isPopupOpen = false;
                }
            }
            else
            {
                popup.Hide();
                _isPopupOpen = false;
            }
        }

        private List<SelectableWordPart> GetWordParts(string word, string term)
        {
            var toReturn = new List<SelectableWordPart>();

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
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (OnCommandReceived != null)
                    {
                        OnCommandReceived(txtCommand.Text);
                    }
                    break;
                }
                case Keys.Up:
                {
                    popup.HighlightUp();
                    e.Handled = true;
                    break;
                }
                case Keys.Down:
                {
                    var tIsInSelectMode = popup.IsInSelectMode;
                    popup.HighlightDown();
                    if (!popup.IsInSelectMode && tIsInSelectMode) //from select to non-select
                    {
                        txtCommand.Text = _lastPreSelectText;
                        txtCommand.SelectionStart = txtCommand.Text.Length;
                    }
                    e.Handled = true;
                    break;
                }
                default:
                {
                    popup.ResetHighlight();
                    break;
                }
            }
            
        }

        public void PopupToTop()
        {
            popup.ToTop();
        }
    }
}
