namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class VerifyCodeViewModel
    {
        #region properties

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        public string Provider { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        #endregion
    }
}