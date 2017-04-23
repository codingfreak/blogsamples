using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Defines the model for users needed by ASP.NET identity.
    /// </summary>
    public class ApplicationUser : IdentityUser<long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        #region methods

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion

        #region properties

        /// <summary>
        /// Converting to UTC enabled timestamp.
        /// </summary>
        public new DateTimeOffset? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// The ASP.NET Identity expected array if role names.
        /// </summary>
        public List<string> RolesResolved { get; set; }

        public string RolesText => string.Join(",", RolesResolved ?? Enumerable.Empty<string>());

        public string StateText => LockoutEndDateUtc.HasValue ? "Locked" : "OK";

        #endregion
    }
}