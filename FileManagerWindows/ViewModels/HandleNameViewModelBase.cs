using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using CB.Subtitles;
using FileManagerModels;


namespace FileManagerWindows.ViewModels
{
    public class HandleNameViewModelBase: FileManagerViewModelBase
    {
        #region Fields
        private FileSystemInfo[] _newNames;
        #endregion


        #region  Constructors & Destructor
        public HandleNameViewModelBase(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider)
            : this(entries, confirmRequestProvider, new FileRenameSetting()) { }

        public HandleNameViewModelBase(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider, RenameSettingBase renameSetting)
            : base(entries, confirmRequestProvider)
        {
            RenameSetting = renameSetting;
            RenameSetting.PropertyChanged += RenameSetting_PropertyChanged;
        }
        #endregion


        #region  Properties & Indexers
        public bool CanHandle => NewNames != null && NewNames.Any();
        public CollectionBase<string, ObservableCollection<string>> ExtensionsCollection { get; } =
            new CollectionBase<string, ObservableCollection<string>>(
                new ObservableCollection<string>(new[] { "" }.Concat(Subtitle.Extensions)));

        public FileSystemInfo[] NewNames
        {
            get { return _newNames; }
            private set { SetProperty(ref _newNames, value); }
        }

        public RenameSettingBase RenameSetting { get; protected set; }
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
        protected virtual FileSystemInfo[] CreateNewNames()
            => Entries?.Select((fsi, i) => fsi.CreateNewFileName(i, (FileRenameSetting)RenameSetting)).ToArray();

        protected virtual void UpdateNewNames()
            => NewNames = CreateNewNames();
        #endregion
    }
}