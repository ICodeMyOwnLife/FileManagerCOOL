using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using CB.Prism.Interactivity;
using CB.Subtitles;
using FileManagerWindows.Models;
using Prism.Commands;


namespace FileManagerWindows.ViewModels
{
    public class ConvertViewModel: HandleViewModelBase
    {
        #region  Constructors & Destructor
        public ConvertViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider): base(entries, confirmRequestProvider)
        {
            ConvertCommand = new DelegateCommand(Convert, () => CanConvert).ObservesProperty(() => CanConvert);
        }
        #endregion


        #region  Commands
        public ICommand ConvertCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanConvert => Entries.All(e => Subtitle.IsSubtitleFile(e.FullPath));
        #endregion


        #region Methods
        public void Convert()
        {
            if (!CanConvert) return;

            ConfirmRequestProvider.Confirm("Convert", "Are you sure you want to convert all files?", DoConvert);
        }
        #endregion


        #region Override
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