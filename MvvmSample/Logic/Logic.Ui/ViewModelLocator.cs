namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            SimpleIoc.Default.Register<MainViewModel>();
        }

        #endregion

        #region methods

        /// <summary>
        /// Cleans up resources.
        /// </summary>
        public static void Cleanup()
        {
        }

        #endregion

        #region properties

        /// <summary>
        /// Retrieves the view model for the main view.
        /// </summary>
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        #endregion
    }
}