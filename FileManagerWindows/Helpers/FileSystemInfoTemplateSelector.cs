using System;
using System.Windows;
using System.Windows.Controls;
using FileManagerWindows.Models;


namespace FileManagerWindows.Helpers
{
    public class FileSystemInfoTemplateSelector: DataTemplateSelector
    {
        #region Override
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var info = (FileSystemInfo)item;
            if (info == null) return null;

            var element = (FrameworkElement)container;

            switch (info.Type)
            {
                case FileSystemType.Folder:
                    return element.FindResource("FolderTemplate") as DataTemplate;
                case FileSystemType.File:
                    return element.FindResource("FileTemplate") as DataTemplate;
                case FileSystemType.Compression:
                    return element.FindResource("CompressionTemplate") as DataTemplate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}