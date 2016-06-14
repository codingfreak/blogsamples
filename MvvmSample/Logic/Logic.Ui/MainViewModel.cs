namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using System.Threading.Tasks;

    using codingfreaks.blogsamples.MvvmSample.Logic.Ui.BaseClasses;
    using codingfreaks.blogsamples.MvvmSample.Logic.Ui.Samples;

    using GalaSoft.MvvmLight.Threading;

    /// <summary>
    /// Contains logic for the main view of the UI.
    /// </summary>
    public class MainViewModel : BaseViewModel
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
                Progress = 30;
            }
            else
            {
                DispatcherHelper.Initialize();
                WindowTitle = "MvvmSample";
                Task.Run(
                    () =>
                    {
                        Task.Delay(2000).ContinueWith(
                            t =>
                            {
                                while (Progress < 100)
                                {
                                    DispatcherHelper.RunAsync(() => Progress += 5);
                                    Task.Delay(500).Wait();
                                }
                            });
                    });
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Has to be overridden by childs to react to collection of errors in a specific way.
        /// </summary>
        protected override void OnErrorsCollected()
        {
        }

        #endregion

        #region properties

        /// <summary>
        /// A person to edit.
        /// </summary>
        public PersonViewModel PersonModel { get; set; } = new PersonViewModel();

        /// <summary>
        /// Indicates the progress.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// The caption of the window.
        /// </summary>
        public string WindowTitle { get; private set; }

        #endregion
    }
}