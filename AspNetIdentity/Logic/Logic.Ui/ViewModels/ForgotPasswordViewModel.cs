namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ForgotPasswordViewModel
    {
        #region properties

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        #endregion
    }
}