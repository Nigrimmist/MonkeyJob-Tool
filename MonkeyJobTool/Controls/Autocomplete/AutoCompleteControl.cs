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

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutoCompleteControl : UserControl
    {

        public List<string> testCommands = new List<string>()
        {
            "test1",
            "test2",
            "lala",
            "blalba"
        };

        public delegate List<string> GetItemsFromSource(string term);
        public GetItemsFromSource DataFilterFunc;
        private AutocompletePopupControl popup = new AutocompletePopupControl();


        public AutoCompleteControl()
        {
            DataFilterFunc = term => testCommands.Where(x => x.StartsWith(term)).ToList();
            InitializeComponent();
        }

        

        private void AutoCompleteControl_Load(object sender, EventArgs e)
        {

        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            string term = "te";
            var actualItems = DataFilterFunc(term);
            var popupModel = new AutocompletePopupInfo();
            foreach (var item in actualItems)
            {
                popupModel.Items.Add(new AutocompletePopupItem()
                {
                    WordParts = GetWordParts(item, term)
                });
            }
            popup.Model = popupModel;
            popup.ShowItems();
        }

        private List<SelectableWordPart> GetWordParts(string word, string term)
        {
            List<SelectableWordPart> toReturn = new List<SelectableWordPart>();

            int foundIndex = word.IndexOf(term, System.StringComparison.InvariantCultureIgnoreCase);
            if (foundIndex != -1)
            {
                string foundPart = word.Substring(0, term.Length);
                string otherPart = word.Substring(foundIndex + foundPart.Length);
                toReturn.Add(new SelectableWordPart()
                {
                    WordPart = foundPart,
                    IsSelected = true
                });
                toReturn.Add(new SelectableWordPart()
                {
                    WordPart = otherPart
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
    }
}
