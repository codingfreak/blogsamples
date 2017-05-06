namespace codingfreaks.AspNetIdentity.Logic.Ui.Models
{
    using System;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Defines the model for roles needed by ASP.NET identity.
    /// </summary>
    public class ApplicationRole : IdentityRole<long, ApplicationUserRole>
    {
    }
}