using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CB.Model.Common;
using CB.Model.Prism;
using FileManagerWindows.Models;


namespace FileManagerWindows.ViewModels
{
    public class FileManagerViewModelBase: PrismViewModelBase
    {
        #region  Constructors & Destructor
        public FileManagerViewModelBase( ObservableCollection<FileSystemInfo> entries)
        {
            Entries = entries;
            entries.CollectionChanged += OnEntriesChanged;
        }
        #endregion


        #region  Properties & Indexers
        public ObservableCollection<FileSystemInfo> Entries { get; }
        #endregion


        #region Event Handlers
        protected virtual void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e) { }
        #endregion
    }
}