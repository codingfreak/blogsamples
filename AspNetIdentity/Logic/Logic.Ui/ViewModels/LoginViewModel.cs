using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        #region properties

        [Required]
        [Display(Name = "Benutzername")]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [Display(Name = "Angemeldet bleiben")]
        public bool RememberMe { get; set; }

        #endregion
    }
}