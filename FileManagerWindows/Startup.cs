using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using CB.Application.SingleInstanceApplication;


namespace FileManagerWindows
{
    public class Startup: SingleInstanceApplicationController<App>
    {
        #region Implementation
        [STAThread]
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        static void Main(string[] args)
        {
            var startup = new Startup();
            startup.Run(args);
        }
        #endregion
    }
}