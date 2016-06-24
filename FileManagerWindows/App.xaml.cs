using System;
using System.Collections.Generic;
using System.Windows;
using CB.Application.SingleInstanceApplication;
using FileManagerWindows.ViewModels;
using FileManagerWindows.Views;


namespace FileManagerWindows
{
    public partial class App: IProcessArgs, IApplication
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
    }
}