namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using System.Threading.Tasks;

    using BaseTypes;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Threading;

    using Messages;

    using Models;

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
                OpenChildCommand = new RelayCommand(() => MessengerInstance.Send(new OpenChildWindowMessage("Hello Child!")));
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// A person to edit.
        /// </summary>
        public PersonModel PersonModel { get; set; } = new PersonModel();

        /// <summary>
        /// Indicates the progress.
        /// </summary>
        public int Progress { get; set; }
       
        /// <summary>
        /// Opens a new child window.
        /// </summary>
        public RelayCommand OpenChildCommand { get; private set; }

        #endregion
    }
}