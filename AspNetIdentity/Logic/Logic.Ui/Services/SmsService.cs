namespace codingfreaks.AspNetIdentity.Logic.Ui.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// The custom service for SMS.
    /// </summary>
    public class SmsService : IIdentityMessageService
    {
        #region explicit interfaces

        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        #endregion
    }
}