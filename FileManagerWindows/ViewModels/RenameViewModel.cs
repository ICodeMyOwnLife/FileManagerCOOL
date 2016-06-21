using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CB.Prism.Interactivity;
using FileManagerWindows.Models;
using Prism.Commands;


namespace FileManagerWindows.ViewModels
{
    public class RenameViewModel: FileManagerViewModelBase
    {
        #region Fields
        private FileSystemInfo[] _newNames;
        #endregion


        #region  Constructors & Destructor
        public RenameViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider): base(entries, confirmRequestProvider)
        {
            RenameSetting = new RenameSetting();
            RenameSetting.PropertyChanged += RenameSetting_PropertyChanged;
            RenameCommand = new DelegateCommand(Rename, () => CanRename).ObservesProperty(() => CanRename);
        }
        #endregion


        #region  Commands
        public ICommand RenameCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanRename => NewNames != null && NewNames.Any();

        public FileSystemInfo[] NewNames
        {
            get { return _newNames; }
            private set
            {
                if (SetProperty(ref _newNames, value))
                {
                    NotifyPropertiesChanged(nameof(CanRename));
                }
            }
        }

        public RenameSetting RenameSetting { get; }
        #endregion


        #region Methods
        public void Rename()
        {
            if (!CanRename) return;

            ConfirmRequestProvider.Confirm("Rename", "Are you sure you want to rename all files/folders?", DoRename);
        }
        #endregion


        #region Override
        protected override void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnEntriesChanged(sender, e);
            UpdateNewNames();
        }
        #endregion


        #region Event Handlers
        private void RenameSetting_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => UpdateNewNames();
        #endregion


        #region Implementation
        private void DoRename()
        {
            for (var i = 0; i < Entries.Count; ++i)
            {
                FileSystemInfo.Move(Entries[i], NewNames[i]);
            }
        }

        private void UpdateNewNames()
            => NewNames = Entries?.Select((fsi, i) => fsi.CreateNewName(i, RenameSetting)).ToArray();
        #endregion
    }
}