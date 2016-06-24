using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FileManagerParameters;
using FileManagerShellContextMenu.Properties;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;


namespace FileManagerShellContextMenu
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class FileManagerContextMenu: SharpContextMenu
    {
        #region Fields
        private const string EXE_FILE = "";
        #endregion


        #region Override
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var extractMenuItem = new ToolStripMenuItem
            {
                Text = Resources.EXTRACT_S,
                Image = Resources.extract,
            };
            extractMenuItem.Click += delegate { DoCommand(FileManagerParameter.EXTRACT_ARGS); };

            var renameMenuItem = new ToolStripMenuItem
            {
                Text = Resources.RENAME_S,
                Image = Resources.rename,
            };
            renameMenuItem.Click += delegate { DoCommand(FileManagerParameter.RENAME_ARGS); };

            var convertMenuItem = new ToolStripMenuItem
            {
                Text = Resources.CONVERT_S,
                Image = Resources.convert,
            };
            convertMenuItem.Click += delegate { DoCommand(FileManagerParameter.CONVERT_ARGS); };

            var fileManagerMenuItem = new ToolStripMenuItem
            {
                Text = Resources.FILE_MANAGER_S
            };

            fileManagerMenuItem.DropDownItems.Add(extractMenuItem);
            fileManagerMenuItem.DropDownItems.Add(renameMenuItem);
            fileManagerMenuItem.DropDownItems.Add(convertMenuItem);

            var menu = new ContextMenuStrip();
            menu.Items.Add(fileManagerMenuItem);
            return menu;
        }
        #endregion


        #region Implementation
        private void DoCommand(string command)
        {
            Process.Start(EXE_FILE, $"{string.Join(" ", SelectedItemPaths)} {command}");
        }
        #endregion
    }
}