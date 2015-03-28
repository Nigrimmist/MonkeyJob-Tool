using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutocompletePopupControl : Form
    {
        public AutocompletePopupInfo Model { get; set; }
        private Color _highlightColor = Color.LemonChiffon;
        private Color _defaultColor ;

        public delegate void OnHighlightedDelegate(string highlightedItem);

        public event OnHighlightedDelegate OnItemHighlighted;

        private int _HighlightedItemIndex
        {
            get { return _highlightedItemIndex; }
            set
            {
                if (value >= items.Count) value = items.Count - 1;
                if (value < -1) value = -1;
                _highlightedItemIndex = value;
            }
        }


        private List<AutocompletePopupItemControl> items = new List<AutocompletePopupItemControl>();
        private int _highlightedItemIndex = -1;


        public AutocompletePopupControl()
        {
            InitializeComponent();
            _defaultColor = this.BackColor;
        }

        public void ShowItems()
        {
            this.Controls.Clear();
            this.items.Clear();
            _highlightedItemIndex = -1;
            int totalHeght = 0;
            for (int i = 0; i < Model.Items.Count; i++)
            {
                var item = Model.Items[i];
                AutocompletePopupItemControl itemControl = new AutocompletePopupItemControl(item.WordParts,item.Value)
                {
                    Top = totalHeght,
                };
                totalHeght += itemControl.Height;
                items.Insert(0,itemControl);
                this.Controls.Add(itemControl);
            }
            this.Height = totalHeght;
            this.ToTop();
        }

        private void HighlightCurrentItem(int prevSelectedIndex)
        {
            if (items.Any() && prevSelectedIndex != _HighlightedItemIndex)
            {
                if (prevSelectedIndex != -1)
                    items[prevSelectedIndex].SetBackColor(_defaultColor);

                if (_HighlightedItemIndex != -1)
                {
                    items[_HighlightedItemIndex].SetBackColor(_highlightColor);
                    if (OnItemHighlighted != null)
                        OnItemHighlighted(items[_HighlightedItemIndex].Value);
                }

            }

        }

        public void HighlightUp()
        {
            var prev = _HighlightedItemIndex;
            _HighlightedItemIndex++;
            HighlightCurrentItem(prev);
        }

        public void HighlightDown()
        {
            var prev = _HighlightedItemIndex;
            _HighlightedItemIndex--;
            HighlightCurrentItem(prev);
        }

        private void AutocompletePopupControl_Load(object sender, System.EventArgs e)
        {

        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        public bool IsInSelectMode {get { return _HighlightedItemIndex!=-1; }}

        public void ResetHighlight()
        {
            _highlightedItemIndex = -1;
        }
    }
}
