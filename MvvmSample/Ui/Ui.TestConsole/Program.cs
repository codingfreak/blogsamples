namespace codingfreaks.blogsamples.MvvmSample.Ui.TestConsole
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    internal class Program
    {
        #region methods

        private static void Main(string[] args)
        {
            TestPropertyChanged();
            Console.ReadKey();
        }

        /// <summary>
        /// Tests the functionallity of <see cref="INotifyPropertyChanged" />.
        /// </summary>
        private static void TestPropertyChanged()
        {
            var test = new TestClass();
            test.PropertyChanged += (s, e) =>
            {
                Console.WriteLine($"Property {e.PropertyName} has changed.");
            };
            test.SomeProperty = "Hello World!"; // expected to fire the event
            test.SomeProperty = "Hello World!"; // do not fire the event
            test.SomeProperty = "Hello World again!"; // should fire   
            test.SomeSecretProperty = true;
            test.SomeSecretProperty = false;
        }

        #endregion
    }
}