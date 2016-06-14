using System.IO;


namespace FileManagerWindows.Models
{
    public class FileSystemInfo
    {
        #region  Constructors & Destructor
        public FileSystemInfo(string path)
        {
            FullPath = path;
            Name = Path.GetFileName(path);
            Type = GetFileSystemType(path);
        }
        #endregion


        #region  Properties & Indexers
        public string FullPath { get; private set; }
        public string Name { get; private set; }
        public FileSystemType Type { get; private set; }
        #endregion


        #region Implementation
        private static FileSystemType GetFileSystemType(string path)
        {
            if (Directory.Exists(path)) return FileSystemType.Folder;

            switch (Path.GetExtension(path))
            {
                case ".zip":
                case ".rar":
                case ".7zip":
                case ".tar":
                    return FileSystemType.Compression;
                default:
                    return FileSystemType.File;
            }
        }
        #endregion
    }
}