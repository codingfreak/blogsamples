namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using BaseTypes;

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
                var personList = new List<PersonModel>();
                for (var i = 0; i < 100; i++)
                {
                    personList.Add(
                        new PersonModel
                        {
                            Firstname = Guid.NewGuid().ToString("N").Substring(0, 10),
                            Lastname = Guid.NewGuid().ToString("N").Substring(0, 10)
                        });
                }
                Persons = new ObservableCollection<PersonModel>(personList);

                OpenChildCommand = new RelayCommand(() => MessengerInstance.Send(new OpenChildWindowMessage("Hello Child!")));
                SetSomeDateCommand = new RelayCommand<PersonModel>(person => person.Birthday = DateTime.Now.AddYears(-20));
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Opens a new child window.
        /// </summary>
        public RelayCommand OpenChildCommand { get; }

        /// <summary>
        /// Takes a single person model and sets it's date to a certain value.
        /// </summary>
        public RelayCommand<PersonModel> SetSomeDateCommand { get; }

        /// <summary>
        /// A person to edit.
        /// </summary>
        public PersonModel PersonModel { get; set; } = new PersonModel();

        /// <summary>
        /// The list of person.
        /// </summary>
        public ObservableCollection<PersonModel> Persons { get; }

        /// <summary>
        /// Indicates the progress.
        /// </summary>
        public int Progress { get; set; }

        #endregion
    }
}