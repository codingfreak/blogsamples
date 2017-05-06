namespace codingfreaks.AspNetIdentity.Logic.Ui.Managers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    using Models;

    /// <summary>
    /// Custom signin manager to use in this application.
    /// </summary>
    public class CustomSignInManager : SignInManager<ApplicationUser, long>
    {
        #region constructors and destructors

        private CustomSignInManager(CustomUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        #endregion

        #region methods

        /// <summary>
        /// Factory method for retrieving an instance using the provided <paramref name="options" /> and
        /// <paramref name="context" />.
        /// </summary>
        /// <param name="options">The options to use when instantiating.</param>
        /// <param name="context">The OWIN context to use.</param>
        /// <returns>The ready-to-use signin-manager.</returns>
        public static CustomSignInManager Create(IdentityFactoryOptions<CustomSignInManager> options, IOwinContext context)
        {
            return new CustomSignInManager(context.GetUserManager<CustomUserManager>(), context.Authentication);
        }

        /// <summary>
        /// Is used to convert the given <paramref name="user" /> into a claims identity.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <returns>The claims identity.</returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((CustomUserManager)UserManager);
        }

        #endregion
    }
}