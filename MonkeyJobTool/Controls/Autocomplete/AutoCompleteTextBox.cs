using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Entities.Autocomplete;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public class AutoCompleteTextBox : TextBox
    {
        private AutocompleteText _textWrapper;

        public delegate void CommandSuggestRequiredDelegate(string commandText);
        public event CommandSuggestRequiredDelegate CommandSuggestRequired;

        public delegate void CommandBluredDelegate();
        public event CommandBluredDelegate OnCommandBlured;

        public AutoCompleteTextBox()
        {
            _textWrapper = new AutocompleteText(this);
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

        public void SetCommand(string command)
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
    }
}
