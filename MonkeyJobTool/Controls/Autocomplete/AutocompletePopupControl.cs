using System.Windows.Forms;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Forms.Autocomplete;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutocompletePopupControl : Form
    {
        private bool _isOpened;
        public AutocompletePopupInfo Model { get; set; }

        public AutocompletePopupControl()
        {
            InitializeComponent();
        }

       

        public void ShowItems()
        {
            this.Controls.Clear();
            int totalHeght = 0;
            foreach (var item in Model.Items)
            {
                AutocompletePopupItemControl itemControl = new AutocompletePopupItemControl(item.WordParts)
                {
                    Top = totalHeght
                };
                totalHeght += itemControl.Height;
                this.Controls.Add(itemControl);
            }
            this.Height = totalHeght;
            this.Show();
            this.ToTop();

        }

        private void AutocompletePopupControl_Load(object sender, System.EventArgs e)
        {

        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
    }
}
