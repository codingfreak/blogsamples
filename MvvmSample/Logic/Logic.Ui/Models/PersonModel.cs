namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;

    using BaseTypes;

    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Encapsulates the complete data and ui-logic for one single person.
    /// </summary>
    public class PersonModel : BaseModel
    {
        #region methods

        /// <summary>
        /// Override this method in derived types to initialize command logic.
        /// </summary>
        protected override void InitCommands()
        {
            base.InitCommands();
            OkCommand = new RelayCommand(
                () =>
                {
                    Trace.WriteLine("OK");
                },
                () => IsOk);
        }

        /// <summary>
        /// Can be overridden by derived types to react on the finisihing of error-collections.
        /// </summary>
        protected override void OnErrorsCollected()
        {
            base.OnErrorsCollected();
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
        public RelayCommand OkCommand { get; private set; }

        #endregion
    }
}