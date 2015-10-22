using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Helpers;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutocompletePopupControl : Form
    {
        public AutocompletePopupInfo Model { get; set; }
        private Color _highlightColor = Color.LightSkyBlue;
        private Color _defaultColor ;

        public delegate void OnHighlightedDelegate(string highlightedItem, bool usingMouse);
        public event OnHighlightedDelegate OnItemHighlighted;

        public delegate void OnMouseClickedDelegate(string clickedItem);
        public event OnMouseClickedDelegate OnMouseClicked;

        public delegate void OnNoOneSelectedDelegate();
        public event OnNoOneSelectedDelegate OnNoOneSelected;

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
                AutocompletePopupItemControl itemControl = new AutocompletePopupItemControl(item.WordParts,item.ClearText)
                {
                    Top = totalHeght,
                    Index = Model.Items.Count-1-i
                };
                totalHeght += itemControl.Height;
                items.Insert(0,itemControl);
                itemControl.MouseEnter += c_MouseEnter;
                itemControl.MouseLeave += itemControl_MouseLeave;
                itemControl.Cursor = Cursors.Hand;
                itemControl.Click += c_Click;
                itemControl.OnChildMouseEnter += () => {c_MouseEnter(itemControl,null);};
                itemControl.OnChildMouseLeave += () => { itemControl_MouseLeave(itemControl, null); };
                foreach (var c in FormHelper.IterateControls(itemControl.Controls))
                {
                    c.Cursor = Cursors.Hand;
                    c.Click += c_Click;
                }
                this.Controls.Add(itemControl);
            }
            this.Height = totalHeght;
            this.ToTop();
        }

        void itemControl_MouseLeave(object sender, System.EventArgs e)
        {
            if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                ResetHighlight();
            }
        }

        void c_Click(object sender, System.EventArgs e)
        {
            if (OnMouseClicked != null)
                OnMouseClicked(items[_HighlightedItemIndex].Value);
            this.Hide();
        }

        
        private void HighlightCurrentItem(int prevSelectedIndex,bool usingMouse)
        {
            if (items.Any() && prevSelectedIndex != _HighlightedItemIndex)
            {
                if (prevSelectedIndex != -1)
                    items[prevSelectedIndex].SetBackColor(_defaultColor);

                if (_HighlightedItemIndex != -1)
                {
                    items[_HighlightedItemIndex].SetBackColor(_highlightColor);
                    if (OnItemHighlighted != null)
                        OnItemHighlighted(items[_HighlightedItemIndex].Value, usingMouse);
                }
                else
                {
                    if (OnNoOneSelected != null)
                        OnNoOneSelected();
                }

            }

        }

        public void HighlightUp()
        {
            var prev = _HighlightedItemIndex;
            _HighlightedItemIndex++;
            HighlightCurrentItem(prev,false);
        }

        public void HighlightDown()
        {
            var prev = _HighlightedItemIndex;
            _HighlightedItemIndex--;
            HighlightCurrentItem(prev,false);
        }

        private void AutocompletePopupControl_Load(object sender, System.EventArgs e)
        {
            
        }

        void c_MouseEnter(object sender, System.EventArgs e)
        {
            var index = ((AutocompletePopupItemControl) sender).Index;
            var tSelected = _HighlightedItemIndex;
            _HighlightedItemIndex = index;
            HighlightCurrentItem(tSelected,true);
        }



        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        public bool IsAnyitemHighlighted {get { return _HighlightedItemIndex!=-1; }}

        public void ResetHighlight()
        {
            var tSelected = _HighlightedItemIndex;
            _HighlightedItemIndex = -1;
            HighlightCurrentItem(tSelected,true);
        }


        public void ResetHighlightIndex()
        {
            _highlightedItemIndex = -1;
        }
    }
}
