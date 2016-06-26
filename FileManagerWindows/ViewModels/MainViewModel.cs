using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CB.IO.Common;
using CB.Model.Common;
using CB.Model.Prism;
using CB.Prism.Interactivity;
using FileManagerModels;
using FileManagerParameters;
using Prism.Commands;
using FileSystemInfo = FileManagerModels.FileSystemInfo;


namespace FileManagerWindows.ViewModels
{
    public class MainViewModel: PrismViewModelBase
    {
        #region  Constructors & Destructor
        public MainViewModel()
        {
            EntryCollection.CollectionChanged += EntryCollection_CollectionChanged;
            EntryCollection.CurrentChanged += EntryCollection_CurrentChanged;

            var fileRenameSetting = new FileRenameSetting();
            var imageRenameSetting = new ImageRenameSetting();

            ExtractViewModel = new ExtractViewModel(EntryCollection.Collection, ConfirmRequestProvider);
            ConvertViewModel = new ConvertViewModel(EntryCollection.Collection, ConfirmRequestProvider,
                fileRenameSetting);
            RenameFileViewModel = new RenameFileViewModel(EntryCollection.Collection, ConfirmRequestProvider,
                fileRenameSetting);
            RenameImageViewModel = new RenameImageViewModel(EntryCollection.Collection, ConfirmRequestProvider,
                imageRenameSetting);

            AddFilesCommand = new DelegateCommand(AddFiles);
            DropCommand = new DelegateCommand<IDataObject>(Drop);
            OpenCommand = new DelegateCommand(Open, () => CanOpen).ObservesProperty(() => CanOpen);
            OpenLocationCommand =
                new DelegateCommand(OpenLocation, () => CanOpenLocation).ObservesProperty(() => CanOpenLocation);
            SortAscendingCommand = new DelegateCommand(SortAscending, () => CanSort).ObservesProperty(() => CanSort);
            SortDescendingCommand = new DelegateCommand(SortDescending, () => CanSort).ObservesProperty(() => CanSort);

            CommandCollection = new CollectionBase<NamedCommand, List<NamedCommand>>(new List<NamedCommand>
            {
                ExtractViewModel.ExtractCommand,
                RenameFileViewModel.RenameCommand,
                RenameImageViewModel.RenameCommand,
                ConvertViewModel.ConvertCommand
            });
        }
        #endregion


        #region  Commands
        public ICommand AddFilesCommand { get; }
        public ICommand DropCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand OpenLocationCommand { get; }
        public ICommand SortAscendingCommand { get; }
        public ICommand SortDescendingCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanOpen => EntryPathExists();
        public bool CanOpenLocation => EntryPathExists();
        public bool CanSort => EntryCollection.Collection.Count > 1;
        public CollectionBase<NamedCommand, List<NamedCommand>> CommandCollection { get; }

        public ConfirmRequestProvider ConfirmRequestProvider { get; } = new ConfirmRequestProvider();

        public ConvertViewModel ConvertViewModel { get; }

        public PrismCollectionBase<FileSystemInfo, ExtendedObservableCollection<FileSystemInfo>> EntryCollection { get;
        } = new PrismCollectionBase<FileSystemInfo, ExtendedObservableCollection<FileSystemInfo>>();

        public ExtractViewModel ExtractViewModel { get; }

        public CommonInteractionRequest FileSystemInteractionRequest { get; } = new CommonInteractionRequest();
        public RenameFileViewModel RenameFileViewModel { get; }
        public RenameImageViewModel RenameImageViewModel { get; }
        #endregion


        #region Methods
        public void AddFiles()
            => FileSystemInteractionRequest.Raise(new OpenFileDialogInfo
            {
                MultiSelect = true
            }, info =>
            {
                if (info.Confirmed)
                {
                    DropPaths(info.FileNames, false);
                }
            });

        public void Drop(IDataObject data)
        {
            var dropPaths = data.GetData(DataFormats.FileDrop, true) as string[];
            if (dropPaths == null) return;

            DropPaths(dropPaths, Keyboard.Modifiers != ModifierKeys.Control);
        }

        public void Open()
        {
            if (!CanOpen) return;

            System.Diagnostics.Process.Start(EntryCollection.SelectedItem.FullPath);
        }

        public void OpenLocation()
        {
            if (!CanOpenLocation) return;

            IO.OpenExplorerToShow(EntryCollection.SelectedItem.FullPath);
        }

        public void Process(IEnumerable<string> paths, string command)
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            DropPaths(paths, true);
            switch (command)
            {
                case FileManagerParameter.CONVERT_CMD:
                    CommandCollection.Select(ConvertViewModel.ConvertCommand);
                    break;
                case FileManagerParameter.EXTRACT_CMD:
                    CommandCollection.Select(ExtractViewModel.ExtractCommand);
                    break;
                case FileManagerParameter.RENAME_FILES_CMD:
                    CommandCollection.Select(RenameFileViewModel.RenameCommand);
                    break;
                case FileManagerParameter.RENAME_IMAGES_CMD:
                    CommandCollection.Select(RenameImageViewModel.RenameCommand);
                    break;
            }
        }

        public void SortAscending()
            => EntryCollection.Collection.SortBy(e => e.Name);

        public void SortDescending()
            => EntryCollection.Collection.SortByDescending(e => e.Name);
        #endregion


        #region Event Handlers
        private void EntryCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => NotifyPropertiesChanged(nameof(CanSort));

        private void EntryCollection_CurrentChanged(object sender, EventArgs e)
            => NotifyPropertiesChanged(nameof(CanOpen), nameof(CanOpenLocation));
        #endregion


        #region Implementation
        private void DropPaths(IEnumerable<string> paths, bool replace = true)
        {
            var entries = paths.Where(
                p => EntryCollection.Collection.All(
                    e => !StringComparer.InvariantCultureIgnoreCase.Equals(e.FullPath, p)))
                               .Select(p => new FileSystemInfo(p));
            if (replace)
            {
                EntryCollection.Collection.ReplaceRange(entries);
            }
            else
            {
                EntryCollection.Collection.AddRange(entries);
            }
        }

        private bool EntryPathExists()
            => EntryCollection.SelectedItem != null &&
               (File.Exists(EntryCollection.SelectedItem.FullPath) ||
                Directory.Exists(EntryCollection.SelectedItem.FullPath));
        #endregion
    }
}


// TODO: Add Edit subtitle feature
// TODO: Handle IOException, name conflict
// TODO: AddFolderCommand
// TODO: Deploy ShellContextMenu