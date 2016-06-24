using System.Windows;
using CB.Application.SingleInstanceApplication;
using FileManagerWindows.ViewModels;
using FileManagerWindows.Views;


namespace FileManagerWindows
{
    public partial class App: IArgsProcessor, IRun, IInitializeComponent
    {
        public App()
        {
            
        }
        #region  Properties & Indexers
        public IProcessArgs ArgsProcessor => FindResource("MainViewModel") as MainViewModel;
        #endregion


        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
        }
        #endregion
    }
}