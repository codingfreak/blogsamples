using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        #region properties

        [Required]
        [Display(Name = "User name")]        
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Stay logged in")]
        public bool RememberMe { get; set; }

        #endregion
    }
}