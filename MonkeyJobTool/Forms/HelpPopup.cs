using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MonkeyJobTool.Properties;

namespace MonkeyJobTool.Forms
{
    
    public enum PopupFormType
    {
        HelpForm,
        CommandInfo,
        SuggestCommandInfo
    }
    
    public partial class HelpPopup : Form
    {
        public Point MouseCoords { get; set; }
        public PopupFormType FormType { get; set; }
        public HelpInfo HelpData { get; set; }
        
        public HelpPopup()
        {
            InitializeComponent();
            this.Paint += this.messageForm_Paint;
        }

      
        
        public void Init()
        {

            fldBody.Text = string.Empty;
            switch (FormType)
            {
                case PopupFormType.HelpForm:
                {
                    HandleHelp();
                    break;
                }
                case PopupFormType.CommandInfo:
                {
                    HandleCommandInfo();
                    break;
                }
                case PopupFormType.SuggestCommandInfo:
                {
                    HandleSuggestCommandInfo();
                    break;
                }
            }
            Font tempFont = ( (fldBody)).Font;
            int textLength = ((fldBody)).Text.Length;
            int textLines = ((fldBody)).GetLineFromCharIndex(textLength) + 1;
            int margin = ((fldBody)).Bounds.Height - ((fldBody)).ClientSize.Height;
            ((RichTextBox)(fldBody)).Height = (TextRenderer.MeasureText(" ", tempFont).Height * textLines) + margin + 3 + rtbTitle.Height;
            this.Height = fldBody.Height+28;
        }

        private void HandleSuggestCommandInfo()
        {
            lblTitle.Text = HelpData.Title;
            this.Width = 260;
            fldBody.Width = 200;
            rtbTitle.Width = this.Width-2;
            fldBody.Location = new Point(50, fldBody.Location.Y);
            PictureBox iconBox = new PictureBox();
            iconBox.Image = Resources.help1;
            fldBody.Text = HelpData.Body;
            iconBox.Size = new Size(36, 36);
            iconBox.Top = 25;
            iconBox.Left = 8;
            iconBox.Height = Resources.help1.Height;
            iconBox.Width = Resources.help1.Width;
            this.Controls.Add(iconBox);

        }

        private void HandleCommandInfo()
        {
            lblTitle.Text = HelpData.Title;
            
            fldBody.Width = 400;
            fldBody.Location = new Point(50, fldBody.Location.Y);
            PictureBox iconBox = new PictureBox();
            iconBox.Image = HelpData.Icon;
            fldBody.Text = HelpData.Body;
            iconBox.Size = new Size(36, 36);
            iconBox.Top = 25;
            iconBox.Left = 8;
            iconBox.Height = Resources.help1.Height;
            iconBox.Width = Resources.help1.Width;
            this.Controls.Add(iconBox);
            
        }
        
        private void HandleHelp()
        {
            lblTitle.Text = "Полезная информация";
            fldBody.Width = 400;
            fldBody.Location = new Point(50, fldBody.Location.Y);
            PictureBox iconBox = new PictureBox();
            iconBox.Image = Resources.help1;
            fldBody.Text = HelpData.Body;
            iconBox.Size = new Size(36, 36);
            iconBox.Top = 25;
            iconBox.Left = 8;
            iconBox.Height = Resources.help1.Height;
            iconBox.Width = Resources.help1.Width;
            this.Controls.Add(iconBox);
        }

        public void SetupCoords()
        {
            this.Left = MouseCoords.X + 10;

            Screen screen = Screen.FromPoint(new Point(MouseCoords.X, MouseCoords.Y));
            int currentFormBottomPosition = screen.WorkingArea.Bottom - (MouseCoords.Y + this.Height);

            if (currentFormBottomPosition < 0)
            {
                this.Top = MouseCoords.Y + 3 - (-currentFormBottomPosition) - 10;
            }
            else
            {
                this.Top = MouseCoords.Y + 3;
            }

        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void messageForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

    }

    public class HelpInfo
    {
        public string Body { get; set; }
        public Image Icon { get; set; }
        public string Title { get; set; }
        public string ForCommand { get; set; }
    }
}
