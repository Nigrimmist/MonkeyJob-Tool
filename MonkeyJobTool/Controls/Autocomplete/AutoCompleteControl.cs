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
using HelloBotCore.Entities;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Forms;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutoCompleteControl : UserControl
    {
        
        //private AutocompletePopupControl _popup = new AutocompletePopupControl(title: "Команды");
        //ColorTranslator.FromHtml("#FFDB99"), Color.DarkOrange, "Аргументы"
        private CommandArgumentSuggester _commandArgumentSuggester;
        private CommandArgumentSuggester _commandSuggester;


        public Form ParentForm { get; set; }
        
        public delegate DataFilterInfo GetItemsFromSource(string term);
        public GetItemsFromSource DataFilterFunc;
        //private bool _isPopupOpen;
        private string _lastPreselectCommand = string.Empty;
        

        public delegate void OnCommandReceivedDelegate(string command);
        public event OnCommandReceivedDelegate OnCommandReceived;

        //public delegate void OnTextChangeDelegate(string text);
        //public event OnTextChangeDelegate OnTextChanged;

        public delegate void OnKeyPressedDelegate(Keys key, KeyEventArgs e);
        public event OnKeyPressedDelegate OnKeyPressed;
        public int StartSuggestFrom = 1;
        public bool IsOneOfPopupsOpen
        {
            get { return _commandSuggester.IsPopupOpen; }
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
            _commandSuggester = new CommandArgumentSuggester(txtCommand, ParentForm, title: "Команды");
            _commandSuggester.OnItemSelected += _commandSuggester_OnItemSelected;
            txtCommand.Init(command =>
            {
                var commands = DataFilterFunc(command).FoundCommands;
                if (commands.Count == 1)
                    return commands.First();
                return null;
            }, GetArgumentSuggestionsFunc );

            txtCommand.CommandSuggestRequired += txtCommand_CommandSuggestRequired;
            txtCommand.ArgSuggestRequired += txtCommand_ArgSuggestRequired;
            txtCommand.OnCommandBlured += txtCommand_OnCommandBlured;
            txtCommand.OnArgumentBlured += txtCommand_OnArgumentBlured;
        }

        void _commandSuggester_OnItemSelected(AutocompleteItem item)
        {
            SetCommand(item.Value as CallCommandInfo);
        }

        private List<AutoSuggestItem> GetArgumentSuggestionsFunc(CallCommandInfo command, CommandArgumentSuggestionInfo suggest, string key, int order)
        {
            Console.WriteLine("GetArgumentSuggestionsFunc");
            var suggestions =  suggest.Details.Single(x => x.Key == key).GetSuggestionFunc();
            //_commandArgumentSuggester.
            return suggestions;
        }

        void txtCommand_ArgSuggestRequired(string argText)
        {
            
        }

        void txtCommand_OnArgumentBlured()
        {
            //if (_commandArgumentSuggester.IsPopupOpen)
            //{
            //    _commandArgumentSuggester.Hide();
            //}
        }

        void txtCommand_CommandSuggestRequired(string commandText)
        {
            string term = commandText;

            //todo : next line must be migrated to commandArgumentSuggester
            //if (_popup.IsAnyitemHighlighted || (IsOneOfPopupsOpen && _popup.Model.Term==term)) return; //no any suggestions if selectMode enabled
            
            //_commandArgumentSuggester.Hide();
            //if (OnTextChanged != null)
            //    OnTextChanged(term);
            _lastPreselectCommand = term;
            if (term.Length >= StartSuggestFrom)
            {

                var filterResult = DataFilterFunc(term);

                if (filterResult.FoundCommands.Any())
                {

                    //todo cache required : iterate items and check for equall
                    var popupModel = new AutocompletePopupInfo();
                    List<AutocompleteItem> items = new List<AutocompleteItem>();
                    foreach (var item in filterResult.FoundCommands)
                    {
                        items.Add(new AutocompleteItem()
                        {
                            DisplayedValue = item.Command,
                            Value = item
                        });
                    }
                    popupModel.Term = term;
                    _commandSuggester.ShowItems(items);

                    if (filterResult.FoundCommands.Count == 1)
                    {
                        //todo : apply command continue (right key eqiuvalent up)
                        //txtCommand.SetCommand(filterResult.FoundItems.First());
                    }
                }
                else
                {
                    _commandSuggester.Hide();
                }
                //txtCommand.NotifyAboutAvailableCommandSuggests(filterResult.FoundCommands);
            }
        }

        void txtCommand_OnCommandBlured()
        {
            _commandSuggester.Hide();
        }

        void _popup_OnMouseClicked(AutocompleteItem clickedItem)
        {
            SetCommand(clickedItem.Value as CallCommandInfo);
        }

        void _popup_OnNoOneSelected()
        {
            //txtCommand.Text = _lastPreSelectText;
            //txtCommand.SelectionStart = txtCommand.Text.Length;
        }

        private void SetCommand(CallCommandInfo commandInfo)
        {
            //txtCommand.Text = commandInfo.Command;
            //txtCommand.SelectionStart = txtCommand.Text.Length;
            
            txtCommand.SetCommand(commandInfo);
        }
        void popup_OnItemHighlighted(AutocompleteItem highlightedItem, bool usingMouse)
        {
            if (!usingMouse)
            {
                SetCommand(highlightedItem.Value as CallCommandInfo);
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
            }
            if (OnKeyPressed != null)
                OnKeyPressed(e.KeyCode,e);
        }

        public void HidePopup()
        {
            _commandSuggester.Hide();
            //_commandArgumentSuggester.Hide();
        }

        public void PopupToTop()
        {
            _commandSuggester.PopupToTop();
            //_commandArgumentSuggester.PopupToTop();
        }

        public int GetPopupHeight()
        {
            return _commandSuggester.GetPopupHeight();
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
