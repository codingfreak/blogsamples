namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Contains logic for the main view of the UI.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                WindowTitle = "MvvmSample (Design)";
            }
            else
            {
                WindowTitle = "MvvmSample";
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// The caption of the window.
        /// </summary>
        public string WindowTitle { get; private set; }

        #endregion
    }
}