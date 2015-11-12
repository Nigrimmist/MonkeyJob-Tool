using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Forms;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutoCompleteControl : UserControl
    {
        
        private AutocompletePopupControl _popup = new AutocompletePopupControl(title: "Команды");
        private CommandArgumentSuggester _commandArgumentSuggester;

        public Form ParentForm { get; set; }
        
        public delegate DataFilterInfo GetItemsFromSource(string term);
        public GetItemsFromSource DataFilterFunc;
        private bool _isPopupOpen;
        private string _lastPreselectCommand = string.Empty;
        

        public delegate void OnCommandReceivedDelegate(string command);
        public event OnCommandReceivedDelegate OnCommandReceived;

        //public delegate void OnTextChangeDelegate(string text);
        //public event OnTextChangeDelegate OnTextChanged;

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

        public void Init()
        {
            //_commandArgumentSuggester = new CommandArgumentSuggester(this.txtCommand, ParentForm);
            //_commandArgumentSuggester.Init();
        }

        private void AutoCompleteControl_Load(object sender, EventArgs e)
        {
            _popup.OnItemHighlighted += popup_OnItemHighlighted;
            _popup.OnNoOneSelected += _popup_OnNoOneSelected;
            _popup.OnMouseClicked += _popup_OnMouseClicked;
            txtCommand.CommandSuggestRequired += txtCommand_CommandSuggestRequired;
            txtCommand.OnCommandBlured += txtCommand_OnCommandBlured;
        }

        void txtCommand_CommandSuggestRequired(string commandText)
        {
            string term = commandText;

            if (_popup.IsAnyitemHighlighted || (IsPopupOpen && _popup.Model.Term==term)) return; //no any suggestions if selectMode enabled
            //_commandArgumentSuggester.Hide();
            //if (OnTextChanged != null)
            //    OnTextChanged(term);
            _lastPreselectCommand = term;
            if (term.Length >= StartSuggestFrom)
            {

                var filterResult = DataFilterFunc(term);

                if (filterResult.FoundItems.Any())
                {
                    var popupModel = new AutocompletePopupInfo();
                    foreach (var item in filterResult.FoundItems)
                    {
                        popupModel.Items.Add(new AutocompletePopupItem()
                        {
                            ClearText = item
                        });
                    }
                    popupModel.Term = term;
                    _popup.Model = popupModel;
                    _popup.ShowItems();
                    _popup.Top = ParentForm.Top - _popup.Height;
                    _popup.Left = ParentForm.Left + 1;
                    _popup.Width = ParentForm.Width - 3;
                    _isPopupOpen = true;

                    if (filterResult.FoundItems.Count == 1)
                    {
                        //todo : apply command continue (right key eqiuvalent up)
                        //txtCommand.SetCommand(filterResult.FoundItems.First());
                    }
                }
                else
                {
                    _popup.Hide();
                    _isPopupOpen = false;
                }
                txtCommand.NotifyAboutAvailableCommandSuggests(filterResult.FoundItems);
            }
        }

        void txtCommand_OnCommandBlured()
        {
            if (_isPopupOpen)
            {
                _popup.Hide();
                _isPopupOpen = false;
            }
        }

        void _popup_OnMouseClicked(string clickedItem)
        {
            SetCommand(clickedItem);
        }

        void _popup_OnNoOneSelected()
        {
            //txtCommand.Text = _lastPreSelectText;
            //txtCommand.SelectionStart = txtCommand.Text.Length;
        }

        private void SetCommand(string command)
        {
            //txtCommand.Text = command;
            //txtCommand.SelectionStart = txtCommand.Text.Length;
            
            txtCommand.SetCommand(command);
        }
        void popup_OnItemHighlighted(string highlightedItem,bool usingMouse)
        {
            if (!usingMouse)
            {
                SetCommand(highlightedItem);
            }
        }

        
        private void txtCommand_KeyDown(object sender, KeyEventArgs e)
        {
            //bool arrowNavigationEnabled = !_commandArgumentSuggester.IsPopupOpen;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    SendCommand();
                    
                    e.SuppressKeyPress = true;

                    break;
                }
                case Keys.Up:
                {
                    _popup.HighlightUp();
                    e.Handled = true;
                    //if (arrowNavigationEnabled)
                    //{
                    //    _popup.HighlightUp();
                    //    e.Handled = true;
                    //}
                    break;
                }
                case Keys.Down:
                {
                    _popup.HighlightDown();
                    e.Handled = true;
                    //if (arrowNavigationEnabled)
                    //{
                    //    _popup.HighlightDown();
                    //    e.Handled = true;
                    //}
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
            //_commandArgumentSuggester.Hide();
        }

        public void PopupToTop()
        {
            _popup.ToTop();
            //_commandArgumentSuggester.PopupToTop();
        }

        public int GetPopupHeight()
        {
            return _popup.Height;
        }

        public void SelectAllText()
        {
            //txtCommand.SelectAll();
        }

        public bool IsTextSelected()
        {
            return !string.IsNullOrEmpty(txtCommand.SelectedText);
        }

        private void pbEnter_Click(object sender, EventArgs e)
        {
            SendCommand();
        }

        private void ChangeEnterIconColor(bool defaultIcon, TimeSpan? showTime=null)
        {
            this.pbEnter.BackgroundImage = defaultIcon?Properties.Resources.enter5:Properties.Resources.enter5_gray;
        }

        private void pbEnter_MouseEnter(object sender, EventArgs e)
        {
            ChangeEnterIconColor(false);
        }

        private void pbEnter_MouseLeave(object sender, EventArgs e)
        {
            ChangeEnterIconColor(true);
        }

        private void pnlEnterIconHolder_MouseEnter(object sender, EventArgs e)
        {
            ChangeEnterIconColor(false);
        }

        private void pnlEnterIconHolder_MouseLeave(object sender, EventArgs e)
        {
            ChangeEnterIconColor(true);
        }

        private void pnlEnterIconHolder_Click(object sender, EventArgs e)
        {
            SendCommand();
        }

        private void SendCommand()
        {
            if (OnCommandReceived != null)
            {
                OnCommandReceived(txtCommand.Text);
                timerEnterIconChange.Stop();
                timerEnterIconChange.Interval = 200;
                ChangeEnterIconColor(false);
                timerEnterIconChange.Start();
            }
        }

        private void timerEnterIconChange_Tick(object sender, EventArgs e)
        {
            ChangeEnterIconColor(true);
            timerEnterIconChange.Stop();
        }

        public void Clear()
        {
            txtCommand.Clear();
        }

        public void ShowArguments(List<AutoSuggestItem> args)
        {
            //_commandArgumentSuggester.ShowItems(args);
        }
    }
}
