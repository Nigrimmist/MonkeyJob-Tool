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
        private AutocompletePopupControl _popup = new AutocompletePopupControl();
        public Form ParentForm { get; set; }
        
        public delegate DataFilterInfo GetItemsFromSource(string term);
        public GetItemsFromSource DataFilterFunc;
        private bool _isPopupOpen;
        private string _lastPreSelectText = string.Empty;

        public delegate void OnCommandReceivedDelegate(string command);
        public event OnCommandReceivedDelegate OnCommandReceived;

        public delegate void OnTextChangeDelegate(string text);
        public event OnTextChangeDelegate OnTextChanged;

        public delegate void OnKeyPressedDelegate(Keys key, KeyEventArgs e);
        public event OnKeyPressedDelegate OnKeyPressed;
        public int StartSuggestFrom = 1;
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
            _popup.OnItemHighlighted += popup_OnItemHighlighted;
            _popup.OnNoOneSelected += _popup_OnNoOneSelected;
            _popup.OnMouseClicked += _popup_OnMouseClicked;
        }

        void _popup_OnMouseClicked(string clickedItem)
        {
            SetCommand(clickedItem);
        }

        void _popup_OnNoOneSelected()
        {
            txtCommand.Text = _lastPreSelectText;
            txtCommand.SelectionStart = txtCommand.Text.Length;
        }

        private void SetCommand(string command)
        {
            txtCommand.Text = command + " ";
            txtCommand.SelectionStart = txtCommand.Text.Length;
        }
        void popup_OnItemHighlighted(string highlightedItem,bool usingMouse)
        {
            if (!usingMouse)
            {
                SetCommand(highlightedItem);
            }
        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            string term = txtCommand.Text;
            if (OnTextChanged != null)
                OnTextChanged(term);

            if (_popup.IsAnyitemHighlighted) return; //no any suggestions if selectMode enabled
            
            
            _lastPreSelectText = term;
            if (!string.IsNullOrEmpty(term) && term.Length >= StartSuggestFrom)
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
                    _popup.Model = popupModel;
                    _popup.ShowItems();
                    _popup.Top = ParentForm.Top-_popup.Height;
                    _popup.Left = ParentForm.Left;
                    _popup.Width = ParentForm.Width;
                    _isPopupOpen = true;
                }
                else
                {
                    _popup.Hide();
                    _isPopupOpen = false;
                }
            }
            else
            {
                _popup.Hide();
                _isPopupOpen = false;
            }
            //App.Instance.CloseAllPopups();
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
                    
                    e.SuppressKeyPress = true;

                    break;
                }
                case Keys.Up:
                {
                    _popup.HighlightUp();
                    e.Handled = true;
                    break;
                }
                case Keys.Down:
                {
                    _popup.HighlightDown();
                    e.Handled = true;
                    break;
                }
                default:
                {
                    _popup.ResetHighlightIndex();

                    break;
                }
            }
            if (OnKeyPressed != null)
                OnKeyPressed(e.KeyCode,e);
        }

        public void HidePopup()
        {
            _popup.Hide();
        }

        public void PopupToTop()
        {
            _popup.ToTop();
        }

        public void SelectAllText()
        {
            txtCommand.SelectAll();
        }

        public bool IsTextSelected()
        {
            return !string.IsNullOrEmpty(txtCommand.SelectedText);
        }

        private void pbEnter_Click(object sender, EventArgs e)
        {
            if (OnCommandReceived != null)
            {
                OnCommandReceived(txtCommand.Text);
            }
        }
    }
}
