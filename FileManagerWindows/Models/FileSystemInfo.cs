using System;
using System.IO;
using CB.Model.Common;


namespace FileManagerWindows.Models
{
    public class FileSystemInfo: BindableObject
    {
        #region Fields
        private string _fullPath;
        private string _name;
        private FileSystemType _type;
        #endregion


        #region  Constructors & Destructor
        public FileSystemInfo(string path)
        {
            FullPath = path;
        }

        protected FileSystemInfo() { }
        #endregion


        #region  Properties & Indexers
        public string FullPath
        {
            get { return _fullPath; }
            private set
            {
                if (SetProperty(ref _fullPath, value))
                {
                    Name = Path.GetFileName(value);
                    Type = GetFileSystemType(value);
                }
            }
        }

        public string Name
        {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        public FileSystemType Type
        {
            get { return _type; }
            private set { SetProperty(ref _type, value); }
        }
        #endregion


        #region Methods
        public static void Move(FileSystemInfo source, FileSystemInfo destination)
        {
            if (source.Type != destination.Type) throw new InvalidOperationException();
            switch (source.Type)
            {
                case FileSystemType.Folder:
                    Directory.Move(source.FullPath, destination.FullPath);
                    break;
                case FileSystemType.File:
                case FileSystemType.Compression:
                    File.Move(source.FullPath, destination.FullPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            source.FullPath = destination.FullPath;
        }

        public FileSystemInfo CreateNewName(int index, RenameSetting setting)
        {
            var baseName = setting.BaseName ?? "";
            var suffix = (index + setting.StartAt).ToString();
            var extension = Type == FileSystemType.Folder ? "" : Path.GetExtension(FullPath) ?? "";
            string prefix;

            if (!setting.UseMask) prefix = baseName;
            else
            {
                var maskCharCount = 0;
                for (var j = baseName.Length - 1; j >= 0 && baseName[j] == setting.MaskChar; --j, ++maskCharCount) { }

                var prefixLength = baseName.Length - Math.Min(maskCharCount, suffix.Length);
                prefix = baseName.Substring(0, prefixLength);
            }

            var newName = prefix + suffix + extension;

            var folder = Path.GetDirectoryName(FullPath);
            var fullPath = folder == null ? newName : Path.Combine(folder, newName);
            return new FileSystemInfo(fullPath);
        }
        #endregion


        #region Override
        public override string ToString() => Name;
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