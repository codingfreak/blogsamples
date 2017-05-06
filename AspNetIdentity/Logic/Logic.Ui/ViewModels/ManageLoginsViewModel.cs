namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    public class ManageLoginsViewModel
    {
        #region properties

        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationDescription> OtherLogins { get; set; }

        #endregion
    }
}