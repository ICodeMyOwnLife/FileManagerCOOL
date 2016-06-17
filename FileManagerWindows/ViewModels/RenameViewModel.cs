using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
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
        public RenameViewModel(ObservableCollection<FileSystemInfo> entries): base(entries)
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
            /*for (var i = 0; i < Entries.Count; ++i)
            {
                var entry = Entries[i];
                var folder = Path.GetDirectoryName(entry.FullPath);
                var destination = Path.Combine(folder, NewNames[i]);

                switch (entry.Type)
                {
                    case FileSystemType.Folder:
                        Directory.Move(entry.FullPath, destination);
                        break;
                    case FileSystemType.File:
                    case FileSystemType.Compression:
                        File.Move(entry.FullPath, destination);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }*/
            for (var i = 0; i < Entries.Count; ++i)
            {
                FileSystemInfo.Move(Entries[i], NewNames[i]);
            }
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
        {
            UpdateNewNames();
        }
        #endregion


        #region Implementation
        private void UpdateNewNames()
        {
            /*NewNames = Entries?.Select((fsi, i) =>
            {
                var baseName = BaseName ?? "";
                var suffix = (i + StartAt).ToString();
                var extension = Path.GetExtension(fsi.FullPath) ?? "";
                string prefix;

                if (!UseMask) prefix = baseName;
                else
                {
                    var maskCharCount = 0;
                    for (var j = baseName.Length - 1; j >= 0 && baseName[j] == MaskChar; --j, ++maskCharCount) { }

                    var prefixLength = baseName.Length - Math.Min(maskCharCount, suffix.Length);
                    prefix = baseName.Substring(0, prefixLength);
                }

                return prefix + suffix + extension;
            }).ToArray();*/

            NewNames = Entries?.Select((fsi, i) => fsi.CreateNewName(i, RenameSetting)).ToArray();
        }
        #endregion
    }
}