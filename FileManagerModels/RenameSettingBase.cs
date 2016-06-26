using CB.Model.Common;


namespace FileManagerModels
{
    public class RenameSettingBase: BindableObject
    {
        #region Fields
        protected const string DEFAULT_NEW_NAME = "New";
        protected string _baseName = DEFAULT_NEW_NAME;
        private string _extension;
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
        #endregion
    }
}