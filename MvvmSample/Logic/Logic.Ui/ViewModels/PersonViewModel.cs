namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;

    using BaseClasses;

    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Encapsulates the complete data and ui-logic for one single person.
    /// </summary>
    public class PersonViewModel : BaseViewModel
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PersonViewModel()
        {
            OkCommand = new RelayCommand(
                () =>
                {
                    Trace.WriteLine("OK");
                },
                () => IsOk);
        }

        #endregion

        #region methods

        /// <summary>
        /// Has to be overridden by childs to react to collection of errors in a specific way.
        /// </summary>
        protected override void OnErrorsCollected()
        {
            OkCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region properties

        /// <summary>
        /// The calculated age of the person.
        /// </summary>
        public int? Age => Birthday.HasValue ? (int)DateTime.Now.Subtract(Birthday.Value).TotalDays / 364 : default(int?);

        /// <summary>
        /// The lastname of the person.
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// The firstname of the person.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name must not be empty.")]
        [MaxLength(20, ErrorMessage = "Maximum of 50 characters is allowed.")]
        public string Firstname { get; set; }

        /// <summary>
        /// The lastname of the person.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name must not be empty.")]
        public string Lastname { get; set; }

        /// <summary>
        /// The command which does something with the person.
        /// </summary>
        public RelayCommand OkCommand { get; }

        #endregion
    }
}