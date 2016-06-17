using System;
using System.IO;
using CB.Model.Common;


namespace FileManagerWindows.Models
{
    public class RenameSetting: BindableObject
    {
        #region Fields
        private const char DEFAULT_MASK_CHAR = '0';
        private const string DEFAULT_NEW_NAME = "New";
        private string _baseName = $"{DEFAULT_NEW_NAME}{DEFAULT_MASK_CHAR}{DEFAULT_MASK_CHAR}";
        private char _maskChar = DEFAULT_MASK_CHAR;
        private int _startAt = 1;
        private bool _useMask = true;
        #endregion


        #region  Properties & Indexers
        public string BaseName
        {
            get { return _baseName; }
            set { SetProperty(ref _baseName, value); }
        }

        public char MaskChar
        {
            get { return _maskChar; }
            set { SetProperty(ref _maskChar, value); }
        }

        public int StartAt
        {
            get { return _startAt; }
            set { SetProperty(ref _startAt, value); }
        }

        public bool UseMask
        {
            get { return _useMask; }
            set { SetProperty(ref _useMask, value); }
        }
        #endregion
    }

    public class FileSystemInfo
    {
        #region  Constructors & Destructor
        public FileSystemInfo(string path)
        {
            FullPath = path;
            Name = Path.GetFileName(path);
            Type = GetFileSystemType(path);
        }

        protected FileSystemInfo() { }
        #endregion


        #region  Properties & Indexers
        public string FullPath { get; private set; }
        public string Name { get; private set; }
        public FileSystemType Type { get; private set; }
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

            return new FileSystemInfo
            {
                FullPath = Path.Combine(Path.GetDirectoryName(FullPath), newName),
                Name = newName,
                Type = Type
            };
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