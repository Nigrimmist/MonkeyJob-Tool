namespace MonkeyJobTool.Helpers
{
    public class InternetChecker
    {
        public static bool IsInternetEnabled()
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            try
            {
                hrm.Get("http://google.com");
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
