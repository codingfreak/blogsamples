namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class ConfigureTwoFactorViewModel
    {
        #region properties

        public ICollection<SelectListItem> Providers { get; set; }

        public string SelectedProvider { get; set; }

        #endregion
    }
}