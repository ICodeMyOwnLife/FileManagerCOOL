using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CB.IO.Common;
using CB.Model.Prism;
using FileManagerWindows.Models;
using Microsoft.Practices.Prism.Commands;
using FileSystemInfo = FileManagerWindows.Models.FileSystemInfo;


namespace FileManagerWindows.ViewModels
{
    public class MainViewModel: PrismViewModelBase
    {
        #region Fields
        private bool _canExtract;
        private bool _deleteAfterExtracted;
        private IEnumerable<FileSystemInfo> _entries;
        #endregion


        #region  Constructors & Destructor
        public MainViewModel()
        {
            DropCommand = new DelegateCommand<IDataObject>(Drop);
            ExtractCommand = new DelegateCommand(Extract, () => CanExtract);
        }
        #endregion


        #region  Commands
        public ICommand DropCommand { get; }
        public ICommand ExtractCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanExtract
        {
            get { return _canExtract; }
            private set
            {
                if (SetProperty(ref _canExtract, value))
                {
                    RaiseCommandsCanExecuteChanged(ExtractCommand);
                }
            }
        }

        public bool DeleteAfterExtracted
        {
            get { return _deleteAfterExtracted; }
            set { SetProperty(ref _deleteAfterExtracted, value); }
        }

        public IEnumerable<FileSystemInfo> Entries
        {
            get { return _entries; }
            private set
            {
                if (SetProperty(ref _entries, value))
                {
                    CanExtract = value != null &&
                                 value.Any(f => f.Type == FileSystemType.Compression || f.Type == FileSystemType.Folder);
                }
            }
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


        #region Implementation
        private void Drop(IDataObject data)
        {
            var dropPaths = data.GetData(DataFormats.FileDrop, true) as string[];
            if (dropPaths != null)
            {
                Entries = dropPaths.Select(p => new FileSystemInfo(p));
            }
        }

        private void ExtractCompresstion(FileSystemInfo compressionEntry)
        {
            // UNDONE: ExtractCompresstion
        }
        #endregion
    }
}