namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class LoginViewModel
    {
        #region properties

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Stay logged in")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        #endregion
    }
}