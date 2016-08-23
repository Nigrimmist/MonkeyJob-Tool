using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore.Entities;
using MonkeyJobTool.Entities.Autocomplete;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public class AutoCompleteTextBox : RichTextBox
    {
        private AutocompleteText _textWrapper;

        public delegate void CommandSuggestRequiredDelegate(string commandText);
        public event CommandSuggestRequiredDelegate CommandSuggestRequired;

        public delegate void CommandBluredDelegate();
        public event CommandBluredDelegate OnCommandBlured;

        public delegate void ArgumentBluredDelegate();
        public event ArgumentBluredDelegate OnArgumentBlured;

        public delegate void OnTextEmptyDelegate();
        public event OnTextEmptyDelegate OnTextEmpty;

        public delegate void ArgSuggestRequiredDelegate(string argText);
        public event ArgSuggestRequiredDelegate ArgSuggestRequired;

        public delegate void HelpShouldBeShownDelegate(bool exist, bool forcedByUser, string command);
        public event HelpShouldBeShownDelegate HelpShouldBeShown;

       private AutocompleteText.TryResolveCommandFromStringDelegate _tryResolveCommandFromStringFunc;

        public AutoCompleteTextBox()
        {
        }

        public void Init(AutocompleteText.TryResolveCommandFromStringDelegate tryResolveCommandFromStringFunc,AutocompleteText.GetSuggestionsDelegate getArgumentSuggestionsFunc)
        {
            _textWrapper = new AutocompleteText(this, tryResolveCommandFromStringFunc, getArgumentSuggestionsFunc);
            _textWrapper.OnCommandFocused += commandText =>
            {
                if (CommandSuggestRequired != null)
                    CommandSuggestRequired(commandText);
            };
            _textWrapper.OnCommandBlured += () =>
            {
                if (OnCommandBlured != null)
                    OnCommandBlured();
            };
            _textWrapper.OnArgumentBlured += () =>
            {
                Console.WriteLine("OnArgumentBlured");
                if (OnArgumentBlured != null)
                    OnArgumentBlured();
            };
            _textWrapper.OnTextEmpty += () =>
            {
                Console.WriteLine("OnTextEmpty");
                if (OnTextEmpty != null)
                    OnTextEmpty();
            };
            _textWrapper.OnArgumentFocused += argumentText =>
            {
                Console.WriteLine("OnArgumentFocused");
                if (ArgSuggestRequired != null)
                    ArgSuggestRequired(argumentText);
            };
            _textWrapper.HelpShouldBeShown += (exist, forcedByUser, command) =>
            {
                if (HelpShouldBeShown != null)
                    HelpShouldBeShown(exist, forcedByUser, command);
                
            };
        }

        //private bool _disableTextChangeFiring { get; set; }

        //protected override void OnTextChanged(EventArgs e)
        //{
        //    //if (!_disableTextChangeFiring)
        //    if (_textWrapper.ChangedEventEnabled)
        //        base.OnTextChanged(e);
        //}

        public void SetArgument(AutoSuggestItem item)
        {
            //_disableTextChangeFiring = true;
            _textWrapper.SetArg(item);
            //_disableTextChangeFiring = false;
        }

        public void SetCommand(CallCommandInfo command)
        {
            _textWrapper.SetCommand(command);
            Console.WriteLine("setCommand.RefreshText");
            
        }

        public void Clear()
        {
            _textWrapper.Clear();
            Console.WriteLine("clear.RefreshText");
            RefreshText();
        }

        private void RefreshText()
        {
            _textWrapper.RefreshText();
        }

        public void NotifyAboutAvailableCommandSuggests(List<string> commands)
        {
            _textWrapper.NotifyAboutAvailableCommandSuggests(commands);
        }
    }
}

