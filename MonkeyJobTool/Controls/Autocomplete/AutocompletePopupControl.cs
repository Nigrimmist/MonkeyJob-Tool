using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelloBotCommunication;
using MonkeyJobTool.Entities.Autocomplete;
using MonkeyJobTool.Extensions;
using MonkeyJobTool.Helpers;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public partial class AutocompletePopupControl : Form
    {
        public AutocompletePopupInfo Model { get; set; }
        private Color _highlightColor = Color.LightSkyBlue;
        private Color _defaultColor ;

        public delegate void OnHighlightedDelegate(AutocompleteItem highlightedItem, bool usingMouse);
        public event OnHighlightedDelegate OnItemHighlighted;

        public delegate void OnMouseClickedDelegate(AutocompleteItem clickedItem);
        public event OnMouseClickedDelegate OnMouseClicked;

        public delegate void OnNoOneSelectedDelegate();
        public event OnNoOneSelectedDelegate OnNoOneSelected;
        public string Title { get; set; }

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

        public int ShowCount = 10;

        private List<AutocompletePopupItemControl> items = new List<AutocompletePopupItemControl>();
        private int _highlightedItemIndex = -1;


        public AutocompletePopupControl(Color? backColor=null, Color? highlightColor=null, string title=null)
        {
            InitializeComponent();
            if (backColor.HasValue)
                this.BackColor = backColor.Value;
            if (highlightColor.HasValue)
                _highlightColor = highlightColor.Value;
            if (!string.IsNullOrEmpty(title))
                Title = title;
            _defaultColor = this.BackColor;
        }

        private int displayedItemsFromIndex = 0;
        private int displayedItemsToIndex = 0;

        public void ShowItems()
        {
            
            pnlItems.Controls.Clear();
            this.items.Clear();
            _highlightedItemIndex = -1;
            int totalHeght = 0;
            Label lbl = null;
            if (!string.IsNullOrEmpty(Title))
            {
                lbl = new Label()
                {
                    Text = Title,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font(DefaultFont,FontStyle.Bold),
                    Margin = new Padding(0)
                };
                
                this.Controls.Add(lbl);
                
            }
            pnlItems.Top = lbl!=null?lbl.Height:0;
            totalHeght = 0;
            displayedItemsFromIndex = 0;
            displayedItemsToIndex = Model.Items.Count > ShowCount ? ShowCount-1 : Model.Items.Count-1;
            for (int i = 0; i <= displayedItemsToIndex; i++)
            {
                var item = Model.Items[i];
                var itemControl = CreateItemControl(item,ref totalHeght,i);
                pnlItems.Controls.Add(itemControl);
                itemControl.Height += 10;
                totalHeght += itemControl.Height;
            }
            SetupPageArrows();
            this.Height = 0; //reset max height because of autosize
            this.ToTop();
            lbl.Width = this.Width;
        }


        private void SetupPageArrows()
        {
            if (items.Count > ShowCount)
            {
                if (displayedItemsToIndex < ShowCount - 1)
                {
                    pnlItems.Top = 10;
                }
            }
        }

        private AutocompletePopupItemControl CreateItemControl(AutocompletePopupItem item, ref int totalHeght, int index)
        {
            AutocompletePopupItemControl itemControl = new AutocompletePopupItemControl(item.Value)
            {
                Top = totalHeght,
                Index = Model.Items.Count - 1 - index
            };
            
            items.Insert(0, itemControl);
            itemControl.MouseEnter += c_MouseEnter;
            itemControl.MouseLeave += itemControl_MouseLeave;
            itemControl.Cursor = Cursors.Hand;
            itemControl.Click += c_Click;
            itemControl.OnChildMouseEnter += () => { c_MouseEnter(itemControl, null); };
            itemControl.OnChildMouseLeave += () => { itemControl_MouseLeave(itemControl, null); };
            foreach (var c in FormHelper.IterateControls(itemControl.Controls))
            {
                c.Cursor = Cursors.Hand;
                c.Click += c_Click;
            }
            itemControl.SetBackColor(this.BackColor);
            return itemControl;
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
