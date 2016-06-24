using System.Windows;
using System.Windows.Navigation;
using CB.Application.SingleInstanceApplication;
using FileManagerWindows.Views;


namespace FileManagerWindows
{
    public partial class App: IArgsProcessor, IRun, IInitializeComponent
    {
        #region  Constructors & Destructor
        public App() { }
        #endregion


        #region  Properties & Indexers
        public IProcessArgs ArgsProcessor { get; private set; }
        #endregion


        #region Override
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            ArgsProcessor = FindResource("MainViewModel") as IProcessArgs;
            base.OnLoadCompleted(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
        }
        #endregion
    }
}