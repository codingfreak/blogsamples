using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotViewModel
    {
        #region properties

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        #endregion
    }
}