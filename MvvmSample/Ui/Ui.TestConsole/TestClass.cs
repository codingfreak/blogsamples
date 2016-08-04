using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Ui.TestConsole
{
    using System.ComponentModel;

    using PropertyChanged;

    /// <summary>
    /// A test showing the usage of <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public class TestClass : INotifyPropertyChanged
    {
        #region events

        /// <summary>
        /// Occurs when the value of one of the property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region properties

        /// <summary>
        /// The lenght of the string in <see cref="SomeProperty" />.
        /// </summary>
        public int LengthOfSomeProperty => SomeProperty.Length;

        /// <summary>
        /// Includes a string value.
        /// </summary>
        public string SomeProperty { get; set; }

        /// <summary>
        /// Contains a boolean value and will not raise <see cref="PropertyChanged" /> when it is changed.
        /// </summary>
        [DoNotNotify]
        public bool SomeSecretProperty { get; set; }

        #endregion
    }
}