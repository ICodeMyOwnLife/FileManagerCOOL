using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using CB.Subtitles;
using FileManagerModels;
using Prism.Commands;


namespace FileManagerWindows.ViewModels
{
    public class ConvertViewModel: HandleNameViewModelBase
    {
        #region  Constructors & Destructor
        public ConvertViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider)
            : this(entries, confirmRequestProvider, new FileRenameSetting()) { }

        public ConvertViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider, FileRenameSetting renameSetting)
            : base(entries, confirmRequestProvider, renameSetting)
        {
            ConvertCommand = new NamedCommand("Convert",
                new DelegateCommand(Convert, () => CanConvert).ObservesProperty(() => CanConvert));
        }
        #endregion


        #region  Commands
        public NamedCommand ConvertCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanConvert => CanHandle;
        #endregion


        #region Methods
        public void Convert()
        {
            if (!CanConvert) return;

            ConfirmRequestProvider.Confirm("Convert", "Are you sure you want to convert all files?", DoConvert);
        }
        #endregion


        #region Override
        protected override FileSystemInfo[] CreateNewNames()
            => Entries.All(e => e.Type == FileSystemType.Subtitle) ? base.CreateNewNames() : null;

        protected override void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnEntriesChanged(sender, e);
            NotifyPropertiesChanged(nameof(CanConvert));
        }
        #endregion


        #region Implementation
        private void DoConvert()
        {
            for (var i = 0; i < Entries.Count; i++)
            {
                Subtitle.Convert(Entries[i].FullPath, NewNames[i].FullPath);
            }
        }
        #endregion
    }
}