using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.Models
{
    using System;
    using System;
    using System;
    using System.Linq;
    using System.Linq;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Defines the model for external user-logins needed by ASP.NET identity.
    /// </summary>
    public class ApplicationUserLogin : IdentityUserLogin<long>
    {
        #region properties

        /// <summary>
        /// The database-generated unqiue id of the entity.
        /// </summary>
        public long Id { get; set; }

        #endregion
    }
}