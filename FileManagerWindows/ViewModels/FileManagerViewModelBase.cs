using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using FileManagerWindows.Models;


namespace FileManagerWindows.ViewModels
{
    public class FileManagerViewModelBase: PrismViewModelBase
    {
        #region  Constructors & Destructor
        public FileManagerViewModelBase(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider)
        {
            Entries = entries;
            ConfirmRequestProvider = confirmRequestProvider;
            entries.CollectionChanged += OnEntriesChanged;
        }
        #endregion


        #region  Properties & Indexers
        public ConfirmRequestProvider ConfirmRequestProvider { get; }
        public ObservableCollection<FileSystemInfo> Entries { get; }
        #endregion


        #region Event Handlers
        protected virtual void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e) { }
        #endregion
    }
}