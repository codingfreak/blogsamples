using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.BaseTypes
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Threading;

    /// <summary>
    /// Abstract base class for all view models.
    /// </summary>
    public abstract class BaseViewModel : ViewModelBase
    {
        #region constructors and destructors

        public BaseViewModel()
        {
            if (!IsInDesignModeStatic && !IsInDesignMode)
            {
                DispatcherHelper.Initialize();
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// A property that indicates if the view model state is valid.
        /// </summary>
        public bool ValidationOk { get; set; } = true;

        /// <summary>
        /// The caption of the window.
        /// </summary>
        public string WindowTitle { get; protected set; }

        #endregion
    }
}