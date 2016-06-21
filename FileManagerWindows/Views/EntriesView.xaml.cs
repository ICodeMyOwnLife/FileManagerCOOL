using System.Windows;


namespace FileManagerWindows.Views
{
    public partial class EntriesView
    {
        #region  Constructors & Destructor
        public EntriesView()
        {
            InitializeComponent();
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            nameof(SelectedIndex), typeof(int), typeof(EntriesView), new PropertyMetadata(-1));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        #endregion
    }
}