using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using CB.Application.SingleInstanceApplication;
using FileManagerWindows.ViewModels;


namespace FileManagerWindows
{
    /*public partial class App: IProcessArgs, IApplication
    {
        #region Fields
        private readonly SortedSet<string> _files = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);
        private DateTime _lastProcess = DateTime.Now;
        private MainViewModel _mainViewModel;
        private readonly TimeSpan _timeOut = TimeSpan.FromMilliseconds(500);
        private MainWindow _window;
        #endregion


        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            _mainViewModel = FindResource("MainViewModel") as MainViewModel;
            _window = new MainWindow();
            _window.Show();
            ((IProcessArgs)this).ProcessArgs(e.Args);
            base.OnStartup(e);
        }
        #endregion


        #region Implementation
        void IProcessArgs.ProcessArgs(string[] args)
        {
            if (args.Length < 2) return;

            var now = DateTime.Now;
            if (now - _lastProcess > _timeOut) _files.Clear();

            _files.Add(args[0]);
            _mainViewModel?.Process(_files, args[1]);
            _window?.Activate();
            _lastProcess = now;
        }
        #endregion
    }*/

    public partial class App: IProcessArgs, IApplication
    {
        #region Fields
        private string[] _args;
        #endregion


        #region Methods
        public void ProcessArgs(string[] args)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Override
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
            var mainViewModel = FindResource("MainViewModel") as MainViewModel;
            if (mainViewModel != null && _args != null && _args.Length > 1)
            {
                mainViewModel.Process(_args.Take(_args.Length - 1), _args.Last());
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _args = e.Args;
        }
        #endregion
    }
}