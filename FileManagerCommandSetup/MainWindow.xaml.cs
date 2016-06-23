using System.IO;
using System.Windows;
using System.Windows.Controls;
using CB.Application.ContextMenuCommands;


namespace FileManagerCommandSetup
{
    public partial class MainWindow
    {
        #region Fields
        private const CommandCategories COMMAND_CATEGORIES =
            CommandCategories.AllDirectories | CommandCategories.AllFiles;

        private const string COMMAND_NAME = "File Manager COOL";
        private readonly string[] _args = new string[0];
        #endregion


        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Event Handlers
        private void CmdAdd_OnClick(object sender, RoutedEventArgs e)
            => ContextMenuCommand.AddCommand(COMMAND_CATEGORIES, COMMAND_NAME, txtPath.Text, true, true, _args);

        private void CmdRemove_OnClick(object sender, RoutedEventArgs e)
            => ContextMenuCommand.RemoveCommand(COMMAND_CATEGORIES, COMMAND_NAME);

        private void OnDrop(object sender, DragEventArgs e)
            => txtPath.Text = (e.Data.GetData(DataFormats.FileDrop) as string[])?[0];

        private void TxtPath_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TxtPath_OnTextChanged(object sender, TextChangedEventArgs e)
            => cmdAdd.IsEnabled = cmdRemove.IsEnabled = !string.IsNullOrEmpty(txtPath.Text) && File.Exists(txtPath.Text);
        #endregion
    }
}