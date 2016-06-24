using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using CB.IO.Common;
using CB.IO.Compression;
using CB.Model.Prism;
using CB.Prism.Interactivity;
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
        public ExtractViewModel(ObservableCollection<FileSystemInfo> entries,
            ConfirmRequestProvider confirmRequestProvider): base(entries, confirmRequestProvider)
        {
            ExtractCommand = new NamedCommand("Extract",
                new DelegateCommand(Extract, () => CanExtract).ObservesProperty(() => CanExtract));
        }
        #endregion


        #region  Commands
        public NamedCommand ExtractCommand { get; }
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

            ConfirmRequestProvider.Confirm("Extract", "Are you sure you want to extract all files/folders?", DoExtract);
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
        private void DoExtract()
        {
            foreach (var entry in Entries.ToArray())
            {
                var entryPath = entry.FullPath;
                switch (entry.Type)
                {
                    case FileSystemType.Folder:
                        IO.MoveContent(entryPath, Path.GetDirectoryName(entryPath));
                        if (DeleteAfterExtracted)
                        {
                            IO.MoveFolderToRecycleBin(entryPath);
                            Entries.Remove(entry);
                        }
                        break;
                    case FileSystemType.Compression:
                        Archiver.ExtractHere(entryPath);
                        if (DeleteAfterExtracted)
                        {
                            IO.MoveFileToRecycleBin(entryPath);
                            Entries.Remove(entry);
                        }
                        break;
                    case FileSystemType.File:
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        #endregion
    }
}