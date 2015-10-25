using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public class AutoCompleteTextBox : TextBox
    {
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
    }
}
