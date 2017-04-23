using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.ComponentModel.DataAnnotations;

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