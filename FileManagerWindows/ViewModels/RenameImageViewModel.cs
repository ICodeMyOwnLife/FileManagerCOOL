using System.Collections.ObjectModel;
using System.Linq;
using CB.Prism.Interactivity;
using FileManagerModels;


namespace FileManagerWindows.ViewModels
{
    public class RenameImageViewModel: RenameViewModelBase
    {
        #region  Constructors & Destructor
        public RenameImageViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider, ImageRenameSetting imageRenameSetting)
            : base(entries, confirmRequestProvider, imageRenameSetting)
        {
            RenameCommand.Name = "Rename images";
        }
        #endregion


        #region Override
        protected override FileSystemInfo[] CreateNewNames()
            => Entries == null || Entries.Any(e => e.Type != FileSystemType.Image) ? null :
                   Entries?.Select(fsi => fsi.CreateNewImageName((ImageRenameSetting)RenameSetting)).ToArray();
        #endregion
    }
}