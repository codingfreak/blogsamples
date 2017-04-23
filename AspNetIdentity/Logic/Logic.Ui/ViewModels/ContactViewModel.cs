using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// View model for the contact form.
    /// </summary>
    public class ContactViewModel
    {
        #region properties

        [Required(ErrorMessage = "Die E-Mail-Adresse muss angegeben werden.")]
        [EmailAddress(ErrorMessage = "Bitte geben Sie eine gültige E-Mail-Adresse ein.")]
        public string Email { get; set; }

        public string Message { get; set; }

        [Required(ErrorMessage = "Die Angabe Ihres Namens ist erforderlich.")]
        public string Name { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Ein Betreff muss eingegeben werden.")]
        public string Subject { get; set; }

        #endregion
    }
}