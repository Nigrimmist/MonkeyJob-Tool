using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Managers;

namespace MonkeyJobTool.Helpers
{
    public class EmailHelper
    {
        public static void SendModuleErrorEmail(Exception ex, string toEmail, string subj)
        {
            if (!App.Instance.AppConf.DevelopmentModeEnabled)
            {
                new Thread(() =>
                {
                    string email = "monkeyjobsender@gmail.com";
                    string password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("ZnBtampxdXl5endkdndocGF3ZXNvbWVzYWx0LWRqYXNoZGxod2JAKjcwMTIzNw==")).Replace("awesomesalt-djashdlhwb@*701237", string.Empty);

                    var fromAddress = new MailAddress(email);
                    var toAddress = new MailAddress(toEmail);

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(fromAddress.Address, password),
                        Timeout = 20000
                    };
                    var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subj,
                        Body = ex + Environment.NewLine + LogManager.CollectSystemInfo()
                    };
                    smtp.Send(message);
                }).Start();
            }
            else
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
