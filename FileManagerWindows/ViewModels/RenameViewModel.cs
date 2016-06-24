using System.Collections.ObjectModel;
using System.Linq;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using FileManagerWindows.Models;
using Prism.Commands;


namespace FileManagerWindows.ViewModels
{
    public class RenameViewModel: HandleNameViewModelBase
    {
        #region  Constructors & Destructor
        public RenameViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider): base(entries, confirmRequestProvider)
        {
            RenameCommand = new NamedCommand("Rename",
                new DelegateCommand(Rename, () => CanRename).ObservesProperty(() => CanRename));
        }
        #endregion


        #region  Commands
        public NamedCommand RenameCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanRename => NewNames != null && NewNames.Any();
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
}