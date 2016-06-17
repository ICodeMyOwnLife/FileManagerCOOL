using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CB.IO.Common;
using FileManagerWindows.Models;
using Prism.Commands;
using FileSystemInfo = FileManagerWindows.Models.FileSystemInfo;


namespace FileManagerWindows.ViewModels
{
    public class ExtractViewModel: FileManagerViewModelBase
    {
        #region Fields
        private bool _deleteAfterExtracted;
        #endregion


        #region  Constructors & Destructor
        public ExtractViewModel(ObservableCollection<FileSystemInfo> entries): base(entries)
        {
            ExtractCommand = new DelegateCommand(Extract, () => CanExtract).ObservesProperty(() => CanExtract);
        }
        #endregion


        #region  Commands
        public ICommand ExtractCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanExtract
            => Entries.Any(f => f.Type == FileSystemType.Compression || f.Type == FileSystemType.Folder);

        public bool DeleteAfterExtracted
        {
            get { return _deleteAfterExtracted; }
            set { SetProperty(ref _deleteAfterExtracted, value); }
        }
        #endregion


        #region Methods
        public void Extract()
        {
            if (!CanExtract) return;

            foreach (var entry in Entries)
            {
                var entryPath = entry.FullPath;
                switch (entry.Type)
                {
                    case FileSystemType.Folder:
                        IO.MoveContent(entryPath, Path.GetDirectoryName(entryPath));
                        if (DeleteAfterExtracted) IO.MoveFolderToRecycleBin(entryPath);
                        break;
                    case FileSystemType.Compression:
                        ExtractCompresstion(entry);
                        if (DeleteAfterExtracted) IO.MoveFileToRecycleBin(entryPath);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        #endregion


        #region Override
        protected override void OnEntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnEntriesChanged(sender, e);
            NotifyPropertiesChanged(nameof(CanExtract));
        }
        #endregion


        #region Implementation
        private void ExtractCompresstion(FileSystemInfo compressionEntry)
        {
            // UNDONE: ExtractCompresstion
        }
        #endregion
    }
}