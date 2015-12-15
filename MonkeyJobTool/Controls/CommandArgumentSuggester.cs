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

        private AutocompletePopupControl _popup;
        
        private bool _isPopupOpen;
        //private bool _isPopupShouldBeOpen;

        public delegate void OnItemSelectedDelegate(AutocompleteItem item);
        public event OnItemSelectedDelegate OnItemSelected;

        
        public int StartSuggestFrom = 1;
        int _argListLeftMargin = 0;

        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
        }


        //public bool WasOpenedBeforeHide
        //{
        //    get { return _isPopupShouldBeOpen; }
        //}
        


        public CommandArgumentSuggester(AutoCompleteTextBox boundTextbox, Form parentForm, Color? backColor = null, Color? highlightColor = null, string title = null)
        {
            _boundTextbox = boundTextbox;
            _parentForm = parentForm;
            _popup = new AutocompletePopupControl(backColor,highlightColor,title);

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
            if (_isPopupOpen)
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
        }

        void _popup_OnMouseClicked(AutocompleteItem clickedItem)
        {
            SelectItem(clickedItem);
        }

        void _popup_OnNoOneSelected()
        {
            //_boundTextbox.SetArgumentText(preArgChangeText);
            //_boundTextbox.SelectionStart = _boundTextbox.Text.Length;
        }

        private string preArgChangeText = null;
        private void SelectItem(AutocompleteItem item)
        {
            //var indexToReplace = getIndexToInsertArgument(preArgChangeText??_boundTextbox.Text, arg);

            //_boundTextbox.SetArgumentText((preArgChangeText ?? _boundTextbox.Text).Substring(0, indexToReplace + 1) + arg);
            //_boundTextbox.SelectionStart = _boundTextbox.Text.Length;
            if (OnItemSelected != null)
                OnItemSelected(item);
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

        void popup_OnItemHighlighted(AutocompleteItem highlightedItem, bool usingMouse)
        {
            if (!usingMouse)
            {
                SelectItem(highlightedItem);
            }
        }

        public void ShowItems(List<AutocompleteItem> items)
        {
            var popupModel = new AutocompletePopupInfo();
            foreach (var item in items)
            {
                popupModel.Items.Add(new AutocompletePopupItem()
                {
                    Value = item
                });
            }

            if (_isPopupOpen && _popup.Model.Items.Any() && _popup.Model.Items.Count == popupModel.Items.Count && _popup.Model.Items.All(x => popupModel.Items.Any(y => y.Value.DisplayedValue == x.Value.DisplayedValue))) return;

            _popup.Model = popupModel;
            _popup.ShowItems();
            _popup.Top = _parentForm.Top - _popup.Height;
            _popup.Left = _parentForm.Left + _argListLeftMargin + 1;
            _popup.Width = _parentForm.Width - _argListLeftMargin - 3;
            _isPopupOpen = true;
        }

        public void Hide(bool rememberOpenedPosition)
        {
            _popup.Hide();
            if (!rememberOpenedPosition)
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

        
        
    }
}
