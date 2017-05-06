namespace codingfreaks.AspNetIdentity.Logic.Ui.Managers
{
    using System;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    using Models;

    using Services;

    /// <summary>
    /// Custom user manager which will use the <see cref="ServiceUserStore" /> to handle users.
    /// </summary>
    public class CustomUserManager : UserManager<ApplicationUser, long>
    {
        #region constructors and destructors

        private CustomUserManager(IUserStore<ApplicationUser, long> store) : base(store)
        {
        }

        #endregion

        #region methods

        /// <summary>
        /// Factory method for retrieving an instance of this type.
        /// </summary>
        /// <param name="options">The options for creating the store.</param>
        /// <param name="context">The OWIN context to use.</param>
        /// <returns>The ready-to-use user manager.</returns>
        public static CustomUserManager Create(IdentityFactoryOptions<CustomUserManager> options, IOwinContext context)
        {
            var manager = new CustomUserManager(new ServiceUserStore());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider(
                "Phone Code",
                new PhoneNumberTokenProvider<ApplicationUser, long>
                {
                    MessageFormat = "Your security code is {0}"
                });
            manager.RegisterTwoFactorProvider(
                "Email Code",
                new EmailTokenProvider<ApplicationUser, long>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is {0}"
                });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        #endregion
    }
}