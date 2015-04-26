using System;
using System.Drawing;
using System.Windows.Forms;
using MonkeyJobTool.Properties;

namespace MonkeyJobTool.Forms
{
    
    public enum PopupFormType
    {
        HelpForm,
    }
    
    public partial class HelpPopup : Form
    {
        public Point MouseCoords { get; set; }
        public PopupFormType FormType { get; set; }
        public HelpInfo HelpData { get; set; }
        


        public HelpPopup()
        {
            InitializeComponent();
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.messageForm_Paint);
        }

      
        
        public void Init()
        {

            fldBody.Text = string.Empty;
            switch (FormType)
            {
                
                    case PopupFormType.HelpForm:
                    {
                        HandleHelp(HelpData);
                        break;
                    }
            }
            Font tempFont = ((RichTextBox) (fldBody)).Font;
            int textLength = ((RichTextBox) (fldBody)).Text.Length;
            int textLines = ((RichTextBox) (fldBody)).GetLineFromCharIndex(textLength) + 1;
            int margin = ((RichTextBox) (fldBody)).Bounds.Height - ((RichTextBox) (fldBody)).ClientSize.Height;
            ((RichTextBox)(fldBody)).Height = (TextRenderer.MeasureText(" ", tempFont).Height * textLines) + margin + 3 + richTextBox2.Height;
            this.Height = fldBody.Height+28;
        }

        
        private void HandleHelp(HelpInfo helpData)
        {
            lblTitle.Text = "Полезная информация";
            fldBody.Width = 400;
            fldBody.Location = new Point(50, fldBody.Location.Y); 
            PictureBox iconBox = new PictureBox();
            iconBox.Image = Resources.help1;
            fldBody.Text = helpData.Body;
            iconBox.Size = new Size(36, 36);
            this.Controls.Add(iconBox);
            iconBox.Top = 25;
            iconBox.Left = 8;
        }

        public void SetupCoords()
        {
            this.Left = MouseCoords.X + 10;

            Screen screen = Screen.FromPoint(new Point(MouseCoords.X, MouseCoords.Y));
            int currentFormBottomPosition = screen.WorkingArea.Bottom - (MouseCoords.Y + this.Height);

            if (currentFormBottomPosition < 0)
            {
                //this.Height -= -currentFormBottomPosition;
                //((RichTextBox)(richTextBox1)).Height -= -currentFormBottomPosition;


                this.Top = MouseCoords.Y + 3 - (-currentFormBottomPosition)-10;
                //if (screen.WorkingArea.Height - this.Top + (-currentFormBottomPosition) <= screen.WorkingArea.Height)
                //{
                //    this.Top = MouseCoords.Y + 3 - (-currentFormBottomPosition);
                //}
            }
            else
            {
                
                this.Top = MouseCoords.Y + 3;
            }

            //lblTitle.Text = currentFormBottomPosition.ToString();
        }
        


        private void FullPostForm_Load(object sender, EventArgs e)
        {
            
        }



        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            MouseCoords = new Point(Cursor.Position.X + 10, Cursor.Position.Y);
            SetupCoords();
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void messageForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void richTextBox2_MouseMove(object sender, MouseEventArgs e)
        {
            this.Close();
        }


    }

    public class HelpInfo
    {
        public string Body { get; set; }
    }
}
