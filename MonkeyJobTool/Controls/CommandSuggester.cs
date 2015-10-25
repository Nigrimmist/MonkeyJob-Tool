using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication;
using MonkeyJobTool.Controls.Autocomplete;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;

namespace MonkeyJobTool.Controls
{
    public class CommandArgumentSuggester
    {
        private readonly AutoCompleteTextBox _boundTextbox;
        private readonly Form _parentForm;

        private AutocompletePopupControl _popup = new AutocompletePopupControl(ColorTranslator.FromHtml("#FFDB99"), Color.DarkOrange, "Аргументы");
        
        private bool _isPopupOpen;

        public delegate void OnCommandReceivedDelegate(string command);
        public event OnCommandReceivedDelegate OnCommandReceived;

        
        public int StartSuggestFrom = 1;
        int _argListLeftMargin = 40;

        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
        }


        public CommandArgumentSuggester(AutoCompleteTextBox boundTextbox, Form parentForm)
        {
            _boundTextbox = boundTextbox;
            _parentForm = parentForm;
        }

        public void Init()
        {
            _popup.OnItemHighlighted += popup_OnItemHighlighted;
            _popup.OnNoOneSelected += _popup_OnNoOneSelected;
            _popup.OnMouseClicked += _popup_OnMouseClicked;
            _boundTextbox.KeyDown += boundTextbox_KeyDown;
            _boundTextbox.TextChanged += boundTextbox_TextChanged;
        }

        void boundTextbox_TextChanged(object sender, EventArgs e)
        {
            preArgChangeText = _boundTextbox.Text;
        }

        void boundTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
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
            
        }

        void _popup_OnMouseClicked(string clickedItem)
        {
            SetArg(clickedItem);
        }

        void _popup_OnNoOneSelected()
        {
            _boundTextbox.SetArgumentText(preArgChangeText);
            _boundTextbox.SelectionStart = _boundTextbox.Text.Length;
        }

        private string preArgChangeText = null;
        private void SetArg(string arg)
        {
            var indexToReplace = getIndexToInsertArgument(preArgChangeText??_boundTextbox.Text, arg);

            _boundTextbox.SetArgumentText((preArgChangeText ?? _boundTextbox.Text).Substring(0, indexToReplace + 1) + arg);
            _boundTextbox.SelectionStart = _boundTextbox.Text.Length;
        }
        
        private int getIndexToInsertArgument(string text, string argument)
        {
            
            int corr = 0;
            for (var i = argument.Length - 1; i >= 0; i--)
            {
                if (argument[i] == text[text.Length - 1 - corr])
                {
                    corr++;
                }
                else
                {
                    corr = 0;
                }
            }
            corr = text.Length - 1 - corr;
            return corr;
        }

        void popup_OnItemHighlighted(string highlightedItem,bool usingMouse)
        {
            if (!usingMouse)
            {
                SetArg(highlightedItem);
            }
        }

        public void ShowItems(List<AutoSuggestItem> items)
        {
            

            var popupModel = new AutocompletePopupInfo();
            foreach (var item in items)
            {
                var sp = new SelectableWordPart() {IsSelected = false, WordPart = item.DisplayedKey};
                popupModel.Items.Add(new AutocompletePopupItem()
                {
                    WordParts = new List<SelectableWordPart>() { sp },
                    ClearText = item.DisplayedKey,
                    Value = item.Value
                });
            }

            if (_isPopupOpen && _popup.Model.Items.All(x => popupModel.Items.Any(y => y.Value == x.Value))) return;

            _popup.Model = popupModel;
            _popup.ShowItems();
            _popup.Top = _parentForm.Top - _popup.Height;
            _popup.Left = _parentForm.Left + _argListLeftMargin;
            _popup.Width = _parentForm.Width - _argListLeftMargin;
            _isPopupOpen = true;
        }

        public void Hide()
        {
            _popup.Hide();
            _isPopupOpen = false;
        }

        public void PopupToTop()
        {
            _popup.ToTop();
        }

        public int GetPopupHeight()
        {
            return _popup.Height;
        }

        public void SelectAllText()
        {
            _boundTextbox.SelectAll();
        }

        public bool IsTextSelected()
        {
            return !string.IsNullOrEmpty(_boundTextbox.SelectedText);
        }

        private void pbEnter_Click(object sender, EventArgs e)
        {
            SendCommand();
        }

        private void pnlEnterIconHolder_Click(object sender, EventArgs e)
        {
            SendCommand();
        }

        private void SendCommand()
        {
            if (OnCommandReceived != null)
            {
                OnCommandReceived(_boundTextbox.Text);
                
            }
        }
        
    }
}
