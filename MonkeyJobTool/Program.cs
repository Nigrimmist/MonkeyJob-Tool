using System;
using System.Threading;
using System.Windows.Forms;
using MonkeyJobTool.Forms;

namespace MonkeyJobTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool isAlreadyRunning = IsSyncMutexExists();

            if (isAlreadyRunning)
            {
                MessageBox.Show("Извините, но программа уже запущена. Попробуйте проверить трей, возможно она спряталась там.");
                return;
            }
            
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
        }

        static Mutex _appSingleton;
        private static bool IsSyncMutexExists()
        {
            bool onlyInstance = false;
            _appSingleton = new Mutex(true, "MonkeyJob", out onlyInstance);
            return !onlyInstance;
        }
    }
}
