using System;
using System.Windows;
using CB.Xaml.Common;
using FileManagerModels;


namespace FileManagerWindows.Helpers
{
    public class FileSystemInfoTemplateSelector: ExtendedDataTemplateSelector
    {
        #region Override
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var info = (FileSystemInfo)item;
            if (info == null) return null;

            switch (info.Type)
            {
                case FileSystemType.Folder:
                    return FindDataTemplate("FolderEntryTemplate", container);
                case FileSystemType.File:
                    return FindDataTemplate("FileEntryTemplate", container);
                case FileSystemType.Compression:
                    return FindDataTemplate("CompressionEntryTemplate", container);
                case FileSystemType.Image:
                    return FindDataTemplate("ImageEntryTemplate", container);
                case FileSystemType.Subtitle:
                    return FindDataTemplate("SubtitleEntryTemplate", container);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}