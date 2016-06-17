using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Entries = new ObservableCollection<FileSystemInfo>();
            ExtractViewModel = new ExtractViewModel(Entries);
            RenameViewModel = new RenameViewModel(Entries);

            DropCommand = new DelegateCommand<IDataObject>(Drop);

            CommandCollection = new CollectionBase<NamedCommand, List<NamedCommand>>(new List<NamedCommand>
            {
                new NamedCommand("Extract", ExtractViewModel.ExtractCommand),
                new NamedCommand("Rename", RenameViewModel.RenameCommand)
            });
        }
        #endregion


        #region  Commands
        public ICommand DropCommand { get; }
        #endregion


        #region  Properties & Indexers
        public CollectionBase<NamedCommand, List<NamedCommand>> CommandCollection { get; }
        public ObservableCollection<FileSystemInfo> Entries { get; }
        public ExtractViewModel ExtractViewModel { get; }
        public RenameViewModel RenameViewModel { get; }
        #endregion


        #region Implementation
        private void Drop(IDataObject data)
        {
            var dropPaths = data.GetData(DataFormats.FileDrop, true) as string[];
            if (dropPaths != null)
            {
                Entries.Clear();
                Entries.AddRange(dropPaths.Select(p => new FileSystemInfo(p)));
            }
        }
        #endregion
    }
}