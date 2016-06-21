using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using CB.Subtitles;
using FileManagerWindows.Models;


namespace FileManagerWindows.ViewModels
{
    public class HandleViewModelBase: FileManagerViewModelBase
    {
        #region Fields
        private FileSystemInfo[] _newNames;
        #endregion


        #region  Constructors & Destructor
        public HandleViewModelBase(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider): base(entries, confirmRequestProvider)
        {
            RenameSetting = new RenameSetting();
            RenameSetting.PropertyChanged += RenameSetting_PropertyChanged;
        }
        #endregion


        #region  Properties & Indexers
        public CollectionBase<string, ObservableCollection<string>> ExtensionsCollection { get; } =
            new CollectionBase<string, ObservableCollection<string>>(new ObservableCollection<string>(new[] { "" }.Concat(Subtitle.Extensions)));

        public FileSystemInfo[] NewNames
        {
            get { return _newNames; }
            private set { SetProperty(ref _newNames, value); }
        }

        public RenameSetting RenameSetting { get; }
        #endregion


        #region Override
        protected override void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnEntriesChanged(sender, e);
            UpdateNewNames();
        }
        #endregion


        #region Event Handlers
        protected void RenameSetting_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => UpdateNewNames();
        #endregion


        #region Implementation
        protected virtual void UpdateNewNames()
            => NewNames = Entries?.Select((fsi, i) => fsi.CreateNewName(i, RenameSetting)).ToArray();
        #endregion
    }
}