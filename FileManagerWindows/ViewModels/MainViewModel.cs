using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CB.Model.Prism;
using FileManagerWindows.Models;
using Prism.Commands;


namespace FileManagerWindows.ViewModels
{
    public class MainViewModel: PrismViewModelBase
    {
        #region  Constructors & Destructor
        public MainViewModel()
        {
            EntryCollection.CollectionChanged += EntryCollection_CollectionChanged;
            ExtractViewModel = new ExtractViewModel(EntryCollection.Collection);
            RenameViewModel = new RenameViewModel(EntryCollection.Collection);

            DropCommand = new DelegateCommand<IDataObject>(Drop);
            SortAscendingCommand = new DelegateCommand(SortAscending, () => CanSort).ObservesProperty(() => CanSort);
            SortDescendingCommand = new DelegateCommand(SortDescending, () => CanSort).ObservesProperty(() => CanSort);

            CommandCollection = new CollectionBase<NamedCommand, List<NamedCommand>>(new List<NamedCommand>
            {
                new NamedCommand("Extract", ExtractViewModel.ExtractCommand),
                new NamedCommand("Rename", RenameViewModel.RenameCommand)
            });
        }
        #endregion


        #region  Commands
        public ICommand DropCommand { get; }
        public ICommand SortAscendingCommand { get; }
        public ICommand SortDescendingCommand { get; }
        #endregion


        #region  Properties & Indexers
        public bool CanSort => EntryCollection.Collection.Count > 1;
        public CollectionBase<NamedCommand, List<NamedCommand>> CommandCollection { get; }

        public PrismCollectionBase<FileSystemInfo, ObservableCollection<FileSystemInfo>> EntryCollection { get; } =
            new PrismCollectionBase<FileSystemInfo, ObservableCollection<FileSystemInfo>>();

        public ExtractViewModel ExtractViewModel { get; }
        public RenameViewModel RenameViewModel { get; }
        #endregion


        #region Methods
        public void Drop(IDataObject data)
        {
            var dropPaths = data.GetData(DataFormats.FileDrop, true) as string[];
            if (dropPaths == null) return;

            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                EntryCollection.Clear();
            }
            EntryCollection.Collection.AddRange(dropPaths
                .Where(
                    p => EntryCollection.Collection.All(
                        e => !StringComparer.InvariantCultureIgnoreCase.Equals(e.FullPath, p)))
                .Select(p => new FileSystemInfo(p)));
        }

        public void SortAscending()
            => EntryCollection.Sort(e => e.Name, ListSortDirection.Ascending);

        public void SortDescending()
            => EntryCollection.Sort(e => e.Name, ListSortDirection.Descending);
        #endregion


        #region Event Handlers
        private void EntryCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertiesChanged(nameof(CanSort));
        }
        #endregion
    }
}

// TODO: Add Sort buttons -> Sort commands