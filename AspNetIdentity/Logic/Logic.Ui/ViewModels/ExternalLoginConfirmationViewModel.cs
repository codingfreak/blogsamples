namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ExternalLoginConfirmationViewModel
    {
        #region properties

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        #endregion
    }
}