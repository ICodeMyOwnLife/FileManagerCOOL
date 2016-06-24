namespace FileManagerWindows.Models
{
    public class ImageRenameSetting: RenameSettingBase
    {
        #region Fields
        private string _heightMask = "{h}";
        private string _widthMask = "{w}";
        #endregion


        #region  Properties & Indexers
        public string HeightMask
        {
            get { return _heightMask; }
            set { SetProperty(ref _heightMask, value); }
        }

        public string WidthMask
        {
            get { return _widthMask; }
            set { SetProperty(ref _widthMask, value); }
        }
        #endregion
    }
}