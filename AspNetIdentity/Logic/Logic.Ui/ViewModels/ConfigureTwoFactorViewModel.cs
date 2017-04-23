using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ConfigureTwoFactorViewModel
    {
        #region properties

        public ICollection<SelectListItem> Providers { get; set; }

        public string SelectedProvider { get; set; }

        #endregion
    }
}