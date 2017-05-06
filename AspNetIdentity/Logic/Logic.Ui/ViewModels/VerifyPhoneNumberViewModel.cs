namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class VerifyPhoneNumberViewModel
    {
        #region properties

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        #endregion
    }
}