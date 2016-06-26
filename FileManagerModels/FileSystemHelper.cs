using System.IO;
using CB.Subtitles;


namespace FileManagerModels
{
    public class FileSystemHelper
    {
        #region Methods
        public static FileSystemType GetFileSystemType(string path)
        {
            if (Directory.Exists(path)) return FileSystemType.Folder;

            var extension = Path.GetExtension(path);
            if (Subtitle.IsSubtitleExtension(extension)) return FileSystemType.Subtitle;

            switch (extension)
            {
                case ".zip":
                case ".rar":
                case ".7z":
                case ".tar":
                    return FileSystemType.Compression;
                case ".jpg":
                case ".bmp":
                case ".png":
                case ".gif":
                case ".jpeg":
                    return FileSystemType.Image;
                default:
                    return FileSystemType.File;
            }
        }
        #endregion
    }
}