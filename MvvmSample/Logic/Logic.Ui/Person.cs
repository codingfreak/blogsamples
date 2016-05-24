using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Annotations;

    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Encapsulates the complete data and ui-logic for one single person.
    /// </summary>
    public class Person : INotifyPropertyChanged, IDataErrorInfo
    {
        #region events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Person()
        {
            OkCommand = new RelayCommand(
                () =>
                {
                    Trace.WriteLine("OK");
                },
                () => IsOk);
        }

        #endregion

        #region explicit interfaces

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error => string.Empty;

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(columnName) ? Errors[columnName] : string.Empty;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property which value has changed.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Is called by the indexer to collect all errors and not only the one for a special field.
        /// </summary>
        /// <remarks>
        /// Because <see cref="HasErrors" /> depends on the <see cref="Errors" /> dictionary this
        /// ensures that controls like buttons can switch their state accordingly.
        /// </remarks>
        private void CollectErrors()
        {
            Errors.Clear();
            var properties =
                this.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true) || prop.IsDefined(typeof(MaxLengthAttribute), true))
                    .ToList();
             properties.ForEach(
                 prop =>
                 {
                     var currentValue = prop.GetValue(this);
                     var requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
                     var maxLenAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                     if (requiredAttr != null)
                     {
                         if (string.IsNullOrEmpty(currentValue?.ToString() ?? string.Empty))
                         {
                             Errors.Add(prop.Name, requiredAttr.ErrorMessage);
                         }
                     }
                     if (maxLenAttr != null)
                     {
                         if ((currentValue?.ToString() ?? string.Empty).Length > maxLenAttr.Length)
                         {
                             Errors.Add(prop.Name, maxLenAttr.ErrorMessage);
                         }
                     }
                 });            
            // we have to this because the Dictionary does not implement INotifyPropertyChanged            
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsOk));
            // commands do not recognize property changes automatically
            OkCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region properties

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
        /// The calculated age of the person.
        /// </summary>        
        public int? Age => Birthday.HasValue ? (int)DateTime.Now.Subtract(Birthday.Value).TotalDays / 364 : default(int?);

        /// <summary>
        /// Indicates whether this instance has any errors.
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// The opposite of <see cref="HasErrors" />.
        /// </summary>
        /// <remarks>
        /// Exists for convenient binding only.
        /// </remarks>
        public bool IsOk => !HasErrors;

        /// <summary>
        /// The lastname of the person.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name must not be empty.")]
        public string Lastname { get; set; }

        /// <summary>
        /// The command which does something with the person.
        /// </summary>
        public RelayCommand OkCommand { get; }

        /// <summary>
        /// A dictionary of current errors with the name of the error-field as the key and the error
        /// text as the value.
        /// </summary>
        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        #endregion
    }
}