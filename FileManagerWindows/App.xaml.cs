using System.Linq;
using System.Windows;
using FileManagerWindows.ViewModels;
using FileManagerWindows.Views;


namespace FileManagerWindows
{
    public partial class App
    {
        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow();
            MainWindow = window;
            window.Show();

            var mainViewModel = FindResource("MainViewModel") as MainViewModel;

            if (mainViewModel != null && e.Args.Length > 1)
            {
                mainViewModel.Process(e.Args.Take(e.Args.Length - 1), e.Args.Last());
            }
        }
        #endregion
    }
}