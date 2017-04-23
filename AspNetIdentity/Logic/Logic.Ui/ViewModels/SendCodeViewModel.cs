using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SendCodeViewModel
    {
        #region properties

        public ICollection<SelectListItem> Providers { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public string SelectedProvider { get; set; }

        #endregion
    }
}