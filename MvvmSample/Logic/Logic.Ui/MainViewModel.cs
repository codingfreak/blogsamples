namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Data;

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
                                    Task.Delay(100).Wait();
                                }
                            });
                    });
                var personList = new List<PersonModel>();
                for (var i = 0; i < 10; i++)
                {
                    personList.Add(
                        new PersonModel
                        {
                            Firstname = Guid.NewGuid().ToString("N").Substring(0, 10),
                            Lastname = Guid.NewGuid().ToString("N").Substring(0, 10)
                        });
                }
                Persons = new ObservableCollection<PersonModel>(personList);
                PersonsView = CollectionViewSource.GetDefaultView(Persons) as ListCollectionView;
                PersonsView.CurrentChanged += (s, e) =>
                {
                    RaisePropertyChanged(() => PersonModel);
                };
                PersonsView.SortDescriptions.Clear();
                PersonsView.SortDescriptions.Add(new SortDescription(nameof(PersonModel.Firstname), ListSortDirection.Ascending));
                foreach (var item in Persons)
                {
                    item.PropertyChanged += PersonsOnPropertyChanged;
                }
                Persons.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                    {
                        foreach (INotifyPropertyChanged added in e.NewItems)
                        {
                            added.PropertyChanged += PersonsOnPropertyChanged;
                        }
                    }
                    if (e.OldItems != null)
                    {
                        foreach (INotifyPropertyChanged removed in e.OldItems)
                        {
                            removed.PropertyChanged -= PersonsOnPropertyChanged;
                        }
                    }
                };
                OpenChildCommand = new RelayCommand(() => MessengerInstance.Send(new OpenChildWindowMessage("Hello Child!")));
                SetSomeDateCommand = new RelayCommand<PersonModel>(person => person.Birthday = DateTime.Now.AddYears(-20));
                AddPersonCommand = new RelayCommand(
                    () =>
                    {
                        var newPerson = new PersonModel
                        {
                            Firstname = "Z(Firstname)"
                        };
                        Persons.Add(newPerson);
                        PersonModel = newPerson;
                    });
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Event handler for property changes on elements of <see cref="Persons"/>.
        /// </summary>
        /// <param name="sender">The person model.</param>
        /// <param name="e">The event arguments.</param>
        private void PersonsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PersonModel.HasErrors) || e.PropertyName == nameof(PersonModel.IsOk))
            {
                return;
            }
            if (PersonsView.IsEditingItem || PersonsView.IsAddingNew)
            {
                return;
            }
            PersonsView.Refresh();
        }

        #endregion

        #region properties

        /// <summary>
        /// Adds a new person to the <see cref="Persons"/>.
        /// </summary>
        public RelayCommand AddPersonCommand { get; }

        /// <summary>
        /// Opens a new child window.
        /// </summary>
        public RelayCommand OpenChildCommand { get; }

        /// <summary>
        /// A person to edit.
        /// </summary>
        public PersonModel PersonModel
        {
            get => PersonsView.CurrentItem as PersonModel;
            set
            {
                PersonsView.MoveCurrentTo(value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The view for binding the UI against the <see cref="Persons"/>.
        /// </summary>
        public ListCollectionView PersonsView { get; }

        /// <summary>
        /// Indicates the progress.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Adds a birthday to a person.
        /// </summary>
        public RelayCommand<PersonModel> SetSomeDateCommand { get; }

        /// <summary>
        /// The list of persons.
        /// </summary>
        private ObservableCollection<PersonModel> Persons { get; }

        #endregion
    }
}