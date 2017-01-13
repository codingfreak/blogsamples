using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Ui.Desktop
{
    using GalaSoft.MvvmLight.Messaging;

    using Logic.Ui;
    using Logic.Ui.Messages;

    /// <summary>
    /// Central listenere for all messages of the app.
    /// </summary>
    public class MessageListener
    {
        #region constructors and destructors

        public MessageListener()
        {
            InitMessenger();
        }

        #endregion

        #region methods

        /// <summary>
        /// Is called by the constructor to define the messages we are interested in.
        /// </summary>
        private void InitMessenger()
        {
            // Hook to the message that states that some caller wants to open a ChildWindow.
            Messenger.Default.Register<OpenChildWindowMessage>(
                this,
                msg =>
                {
                    var window = new ChildWindow();
                    var model = window.DataContext as ChildViewModel;
                    if (model != null)
                    {
                        model.MessageFromParent = msg.SomeText;
                    }
                    window.ShowDialog();
                });
        }

        #endregion

        #region properties

        /// <summary>
        /// We need this property so that this type can be put into the ressources.
        /// </summary>
        public bool BindableProperty => true;

        #endregion
    }
}