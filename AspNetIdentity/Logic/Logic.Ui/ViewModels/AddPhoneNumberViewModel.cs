namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class AddPhoneNumberViewModel
    {
        #region properties

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        #endregion
    }
}