using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// The custom service for mailing.
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        #region explicit interfaces

        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        #endregion
    }
}