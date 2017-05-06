namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNet.Identity;

    public class IndexViewModel
    {
        #region properties

        public bool BrowserRemembered { get; set; }

        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        #endregion
    }
}