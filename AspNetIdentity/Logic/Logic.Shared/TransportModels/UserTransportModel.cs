namespace codingfreaks.AspNetIdentity.Logic.Shared.TransportModels
{
    using System;
    using System.Linq;

    /// <summary>
    /// Is used to transport user informations between all layers.
    /// </summary>
    public class UserTransportModel : BaseTransportModel
    {
        #region properties

        public int AccessFailedCount { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEndDateUtc { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string SecurityStamp { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string UserName { get; set; }

        #endregion
    }
}