using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using BaseTypes;

    /// <summary>
    /// View logic for a child window.
    /// </summary>
    public class ChildViewModel : BaseViewModel
    {
        #region constructors and destructors

        public ChildViewModel()
        {
            if (IsInDesignMode)
            {
                WindowTitle = "ChildWindow (Design)";
            }
            else
            {
                WindowTitle = "ChildWindow";
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Some message that is set by the messenger and is defined in the calling
        /// code from the main view.
        /// </summary>
        public string MessageFromParent { get; set; }

        #endregion
    }
}