using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.BaseClasses
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Abstract base class for all view models.
    /// </summary>
    public abstract class BaseViewModel : ViewModelBase, IDataErrorInfo
    {
        #region constants

        private static List<PropertyInfo> _propertyInfos;

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
        /// Has to be overridden by childs to react to collection of errors in a specific way.
        /// </summary>
        protected abstract void OnErrorsCollected();

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
            PropertyInfos.ForEach(
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
                    // further attributes
                });
            // we have to this because the Dictionary does not implement INotifyPropertyChanged            
            RaisePropertyChanged(() => HasErrors);
            RaisePropertyChanged(() => IsOk);
            // commands do not recognize property changes automatically
            OnErrorsCollected();
        }

        #endregion

        #region properties

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
        /// Retrieves a list of all properties with attributes required for <see cref="IDataErrorInfo" /> automation.
        /// </summary>
        protected List<PropertyInfo> PropertyInfos
        {
            get
            {
                if (_propertyInfos == null)
                {
                    // TODO filter for other attributes
                    _propertyInfos =
                        GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true) || prop.IsDefined(typeof(MaxLengthAttribute), true))
                            .ToList();
                }
                return _propertyInfos;
            }
        }

        /// <summary>
        /// A dictionary of current errors with the name of the error-field as the key and the error
        /// text as the value.
        /// </summary>
        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        #endregion
    }
}