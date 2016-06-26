using System.Collections.ObjectModel;
using System.Linq;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using FileManagerModels;
using Prism.Commands;


namespace FileManagerWindows.ViewModels
{
    public class RenameViewModelBase: HandleNameViewModelBase
    {
        #region  Constructors & Destructor
        public RenameViewModelBase(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider, RenameSettingBase renameSetting)
            : base(entries, confirmRequestProvider, renameSetting)
        {
            RenameCommand = new NamedCommand("Rename",
                new DelegateCommand(Rename, () => CanRename).ObservesProperty(() => CanRename));
        }
        #endregion


        #region  Commands
        public NamedCommand RenameCommand { get; }
        #endregion


        #region  Properties & Indexers
        public virtual bool CanRename => CanHandle;
        #endregion


        #region Methods
        public void Rename()
        {
            if (!CanRename) return;

            ConfirmRequestProvider.Confirm("Rename", "Are you sure you want to rename all files/folders?", DoRename);
        }
        #endregion


        #region Override
        protected override void UpdateNewNames()
        {
            base.UpdateNewNames();
            NotifyPropertiesChanged(nameof(CanRename));
        }
        #endregion


        #region Implementation
        private void DoRename()
        {
            for (var i = 0; i < Entries.Count; ++i)
            {
                FileSystemInfo.Move(Entries[i], NewNames[i]);
            }
        }
        #endregion
    }

    public class RenameFileViewModel: RenameViewModelBase
    {
        #region  Constructors & Destructor
        public RenameFileViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider, FileRenameSetting renameSetting)
            : base(entries, confirmRequestProvider, renameSetting)
        {
            RenameCommand.Name = "Rename files";
        }
        #endregion
    }
}