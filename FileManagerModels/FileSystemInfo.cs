using System;
using System.Drawing;
using System.IO;
using CB.Model.Common;
using CB.Subtitles;


namespace FileManagerModels
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
                    Type = FileSystemHelper.GetFileSystemType(value);
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

            if (source.Type == FileSystemType.Folder)
            {
                Directory.Move(source.FullPath, destination.FullPath);
            }
            else
            {
                File.Move(source.FullPath, destination.FullPath);
            }

            source.FullPath = destination.FullPath;
        }

        public FileSystemInfo CreateNewFileName(int index, FileRenameSetting setting)
        {
            var baseName = setting.BaseName ?? "";
            var suffix = (index + setting.StartAt).ToString();
            var extension = Type == FileSystemType.Folder
                                ? ""
                                : string.IsNullOrEmpty(setting.Extension)
                                      ? Path.GetExtension(FullPath) ?? ""
                                      : setting.Extension;
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
            return CreateNewName(newName);
        }

        public FileSystemInfo CreateNewImageName(ImageRenameSetting setting)
        {
            if (Type != FileSystemType.Image) throw new NotSupportedException();

            using (var image = Image.FromFile(FullPath))
            {
                var newName =
                    setting.BaseName.Replace(setting.WidthMask, image.Width.ToString()).Replace(setting.HeightMask,
                        image.Height.ToString()) + Path.GetExtension(FullPath);
                return CreateNewName(newName);
            }
        }
        #endregion


        #region Override
        public override string ToString() => Name;
        #endregion


        #region Implementation
        private FileSystemInfo CreateNewName(string newName)
        {
            var folder = Path.GetDirectoryName(FullPath);
            var fullPath = folder == null ? newName : Path.Combine(folder, newName);
            return new FileSystemInfo(fullPath);
        }
        #endregion
    }
}