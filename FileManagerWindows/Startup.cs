using System;
using CB.Application.SingleInstanceApplication;


namespace FileManagerWindows
{
    public class Startup: SingleInstanceApplicationController<App>
    {
        #region Implementation
        [STAThread]
        static void Main(string[] args)
        {
            var startup = new Startup();
            startup.Run(args);
        }
        #endregion
    }
}