using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        #region properties

        [DataType(DataType.Password)]
        [Display(Name = "Password confirm")]
        [Compare("Password", ErrorMessage = "Passwords dont match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Mail address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password {0} has to be {2} chars in max.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]        
        [Display(Name = "User name")]
        public string UserName { get; set; }

        #endregion
    }
}