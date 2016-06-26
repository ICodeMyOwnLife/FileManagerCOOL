using System.IO;
using System.Windows;
using System.Windows.Controls;
using CB.Application.ContextMenuCommands;
using FileManagerParameters;


namespace FileManagerCommandSetup
{
    public partial class MainWindow
    {
        #region Fields
        private const CommandCategories COMMAND_CATEGORIES =
            CommandCategories.AllDirectories | CommandCategories.AllFiles;

        private const string COMMAND_NAME = "File Manager COOL";
        private const string CONVERT_NAME = "Convert...";
        private const string EXTRACT_NAME = "Extract...";
        private const string RENAME_NAME = "Rename...";
        #endregion


        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Event Handlers
        private void CmdAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var appPath = txtPath.Text;
            ContextMenuCommandManager.AddCommand(COMMAND_CATEGORIES, new CascadingContextMenuCommandItem
            {
                Name = COMMAND_NAME,
                Icon = ContextMenuCommandIcon.FromAppPath(appPath),
                SubCommands = new ContextMenuCommandItem[]
                {
                    ContextMenuCommandItem.FromAppPath(EXTRACT_NAME, appPath, true, true,
                        FileManagerParameter.EXTRACT_CMD),
                    ContextMenuCommandItem.FromAppPath(RENAME_NAME, appPath, true, true,
                        FileManagerParameter.RENAME_FILES_CMD),
                    ContextMenuCommandItem.FromAppPath(CONVERT_NAME, appPath, true, true,
                        FileManagerParameter.CONVERT_CMD),
                }
            });
        }

        private void CmdRemove_OnClick(object sender, RoutedEventArgs e)
            => ContextMenuCommandManager.RemoveCommand(COMMAND_CATEGORIES, COMMAND_NAME);

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