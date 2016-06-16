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
        private const char DEFAULT_MASK_CHAR = '0';
        private const string DEFAULT_NEW_NAME = "New";
        private string _baseName = $"{DEFAULT_NEW_NAME}{DEFAULT_MASK_CHAR}{DEFAULT_MASK_CHAR}";
        private bool _canExtract;
        private bool _deleteAfterExtracted;
        private IEnumerable<FileSystemInfo> _entries;
        private char _maskChar = DEFAULT_MASK_CHAR;
        private IEnumerable<string> _newNames;
        private int _startAt = 1;
        private bool _useMask = true;
        #endregion


        #region  Constructors & Destructor
        public MainViewModel()
        {
            DropCommand = new DelegateCommand<IDataObject>(Drop);
            ExtractCommand = new DelegateCommand(Extract, () => CanExtract);
            RenameCommand = new DelegateCommand(Rename);
            CommandCollection = new CollectionBase<NamedCommand, List<NamedCommand>>(new List<NamedCommand>
            {
                new NamedCommand("Extract", ExtractCommand),
                new NamedCommand("Rename", RenameCommand)
            });
        }
        #endregion


        #region  Commands
        public ICommand DropCommand { get; }
        public ICommand ExtractCommand { get; }
        public ICommand RenameCommand { get; }
        #endregion


        #region  Properties & Indexers
        public string BaseName
        {
            get { return _baseName; }
            set { SetProperty(ref _baseName, value); }
        }

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

        public CollectionBase<NamedCommand, List<NamedCommand>> CommandCollection { get; }

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
                    UpdateNewNames();
                }
            }
        }

        public char MaskChar
        {
            get { return _maskChar; }
            set { SetProperty(ref _maskChar, value); }
        }

        public IEnumerable<string> NewNames
        {
            get { return _newNames; }
            private set { SetProperty(ref _newNames, value); }
        }

        public int StartAt
        {
            get { return _startAt; }
            set { SetProperty(ref _startAt, value); }
        }

        public bool UseMask
        {
            get { return _useMask; }
            set { SetProperty(ref _useMask, value); }
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

        public void Rename()
        {
            // UNDONE: Rename
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

        private void UpdateNewNames()
        {
            NewNames = Entries?.Select((fsi, i) =>
            {
                var baseName = BaseName ?? "";
                var suffix = (i + StartAt).ToString();

                if (!UseMask) return baseName + suffix;

                var maskCharCount = 0;
                for (var j = baseName.Length - 1; j >= 0 && baseName[j] == MaskChar; --j, ++maskCharCount) { }

                var prefixLength = baseName.Length - Math.Min(maskCharCount, suffix.Length);
                return baseName.Substring(0, prefixLength) + suffix;
            });
        }
        #endregion
    }
}