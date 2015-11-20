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

       private AutocompleteText.TryResolveCommandFromStringDelegate _tryResolveCommandFromStringFunc;

        public AutoCompleteTextBox()
        {
        }

        public void Init(AutocompleteText.TryResolveCommandFromStringDelegate tryResolveCommandFromStringFunc)
        {
            _textWrapper = new AutocompleteText(this, tryResolveCommandFromStringFunc);
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
        }

        private bool _disableTextChangeFiring { get; set; }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!_disableTextChangeFiring)
                base.OnTextChanged(e);
        }

        public void SetArgumentText(string text)
        {
            _disableTextChangeFiring = true;
            this.Text = text;
            _disableTextChangeFiring = false;
        }

        public void SetCommand(CallCommandInfo command)
        {
            _textWrapper.SetCommand(command);
            RefreshText();
        }

        public void Clear()
        {
            _textWrapper.Clear();
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
