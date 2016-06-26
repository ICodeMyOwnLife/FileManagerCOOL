using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FileManagerModels;
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
        private const string EXE_FILE =
            @"F:\Projects\Applications\FileManagerCOOL\FileManagerWindows\bin\Release\FileManagerWindows.exe";
        #endregion


        #region Override
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var fileManagerMenuItem = new ToolStripMenuItem
            {
                Text = Resources.FILE_MANAGER_S,
                Image = Resources.filemanager
            };

            var entryTypes = SelectedItemPaths.Select(FileSystemHelper.GetFileSystemType).ToArray();

            if (entryTypes.All(t => t == FileSystemType.Compression || t == FileSystemType.Folder))
                fileManagerMenuItem.DropDownItems.Add(CreateExtractMenuItem());

            fileManagerMenuItem.DropDownItems.Add(CreateRenameFilesMenuItem());

            if (entryTypes.All(t => t == FileSystemType.Image))
                fileManagerMenuItem.DropDownItems.Add(CreateRenameImagesMenuItem());

            if (entryTypes.All(t => t == FileSystemType.Subtitle))
                fileManagerMenuItem.DropDownItems.Add(CreateConvertMenuItem());

            var menu = new ContextMenuStrip();
            menu.Items.Add(fileManagerMenuItem);
            return menu;
        }
        #endregion


        #region Implementation
        private ToolStripMenuItem CreateConvertMenuItem()
        {
            var convertMenuItem = new ToolStripMenuItem
            {
                Text = Resources.CONVERT_S,
                Image = Resources.convert,
            };
            convertMenuItem.Click += delegate { DoCommand(FileManagerParameter.CONVERT_CMD); };
            return convertMenuItem;
        }

        private ToolStripMenuItem CreateExtractMenuItem()
        {
            var extractMenuItem = new ToolStripMenuItem
            {
                Text = Resources.EXTRACT_S,
                Image = Resources.extract,
            };
            extractMenuItem.Click += delegate { DoCommand(FileManagerParameter.EXTRACT_CMD); };
            return extractMenuItem;
        }

        private ToolStripMenuItem CreateRenameFilesMenuItem()
        {
            var renameFilesMenuItem = new ToolStripMenuItem
            {
                Text = Resources.RENAME_FILES_S,
                Image = Resources.rename,
            };
            renameFilesMenuItem.Click += delegate { DoCommand(FileManagerParameter.RENAME_FILES_CMD); };
            return renameFilesMenuItem;
        }

        private ToolStripMenuItem CreateRenameImagesMenuItem()
        {
            var renameImagesMenuItem = new ToolStripMenuItem
            {
                Text = Resources.RENAME_IMAGES_S,
                Image = Resources.image,
            };
            renameImagesMenuItem.Click += delegate { DoCommand(FileManagerParameter.RENAME_IMAGES_CMD); };
            return renameImagesMenuItem;
        }

        private void DoCommand(string command)
        {
            var args = $"{string.Join(" ", SelectedItemPaths.Select(p => $"\"{p}\""))} {command}";
            Process.Start(EXE_FILE, args);
        }
        #endregion
    }
}


// TODO: Copy location context menu