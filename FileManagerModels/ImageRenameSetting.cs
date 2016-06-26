namespace FileManagerModels
{
    public class ImageRenameSetting: RenameSettingBase
    {
        #region Fields
        private string _heightMask = "{h}";
        private string _widthMask = "{w}";
        #endregion


        #region  Constructors & Destructor
        public ImageRenameSetting()
        {
            BaseName = $"{DEFAULT_NEW_NAME} ({WidthMask}x{HeightMask})";
        }
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