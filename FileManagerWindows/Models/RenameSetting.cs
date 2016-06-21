using CB.Model.Common;


namespace FileManagerWindows.Models
{
    public class RenameSetting: BindableObject
    {
        #region Fields
        private const char DEFAULT_MASK_CHAR = '0';
        private const string DEFAULT_NEW_NAME = "New";
        private string _baseName = $"{DEFAULT_NEW_NAME}{DEFAULT_MASK_CHAR}{DEFAULT_MASK_CHAR}";
        private string _extension;
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

        public string Extension
        {
            get { return _extension; }
            set { SetProperty(ref _extension, value); }
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
}