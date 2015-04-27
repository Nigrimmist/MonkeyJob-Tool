using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MonkeyJobTool.Forms;
using Timer = System.Windows.Forms.Timer;


namespace MonkeyJobTool.Managers
{
    public class HelpPopupManager
    {
        private static HelpPopup CurrentHelpForm { get; set; }
        private static Timer CurrentHelpTimer { get; set; }

        public static void BindHelpForm(PictureBox target, string body)
        {
            target.MouseEnter += new EventHandler(target_MouseEnter);
            target.MouseLeave += new EventHandler(target_MouseLeave);
            target.AccessibleDescription = body;
        }

        private static void target_MouseLeave(object sender, EventArgs e)
        {
            if (CurrentHelpForm != null)
            {
                if (CurrentHelpForm.InvokeRequired)
                {
                    CurrentHelpForm.Invoke(new MethodInvoker(CloseHelpForm));
                }
                else
                {
                    CloseHelpForm();
                }
            }

        }

        private static void CloseHelpForm()
        {
            if (CurrentHelpForm != null)
            {
                CurrentHelpForm.Close();
                CurrentHelpForm.Dispose();
                CurrentHelpForm = null;
                if (CurrentHelpTimer != null)
                {
                    CurrentHelpTimer.Stop();
                }
            }
        }

        private static void ShowHelpForm()
        {
            CurrentHelpForm.MouseCoords = new Point(Cursor.Position.X, Cursor.Position.Y);
            CurrentHelpForm.Show();
            CurrentHelpForm.SetupCoords();
        }

        private static void target_MouseEnter(object sender, EventArgs e)
        {
            string body = ((PictureBox)sender).AccessibleDescription;

            HelpPopup postForm = new HelpPopup
            {
                FormType = PopupFormType.HelpForm,
                HelpData = new HelpInfo() {Body = body},
            };

            postForm.Init();
            
            CurrentHelpForm = postForm;
            if (CurrentHelpTimer != null)
            {
                CurrentHelpTimer.Stop();
            }

            if (CurrentHelpTimer == null)
            {
                CurrentHelpTimer = new Timer {Interval = 300};
                CurrentHelpTimer.Tick += CurrentHelpTimerOnTick;
            }

            CurrentHelpTimer.Start();
        }

        private static void CurrentHelpTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (CurrentHelpForm.InvokeRequired)
            {
                CurrentHelpForm.Invoke(new MethodInvoker(ShowHelpForm));
            }
            else
            {
                ShowHelpForm();
            }
            CurrentHelpTimer.Stop();
        }
    }
}
