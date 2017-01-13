using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.Messages
{
    /// <summary>
    /// If sent through the Messenger this message tells that a view model wants to
    /// open the child window.
    /// </summary>
    public class OpenChildWindowMessage
    {
        #region constructors and destructors

        public OpenChildWindowMessage(string someText)
        {
            SomeText = someText;
        }

        #endregion

        #region properties

        /// <summary>
        /// Just some text that comes from the sender.
        /// </summary>
        public string SomeText { get; private set; }

        #endregion
    }
}