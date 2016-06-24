namespace FileManagerWindows.Models
{
    public class FileRenameSetting: RenameSettingBase
    {
        #region Fields
        private const char DEFAULT_MASK_CHAR = '0';
        private char _maskChar = DEFAULT_MASK_CHAR;
        private int _startAt = 1;
        private bool _useMask = true;
        #endregion


        #region  Constructors & Destructor
        public FileRenameSetting()
        {
            _baseName = $"{DEFAULT_NEW_NAME}{DEFAULT_MASK_CHAR}{DEFAULT_MASK_CHAR}";
        }
        #endregion


        #region  Properties & Indexers
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
}