namespace codingfreaks.AspNetIdentity.Ui.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Logic.Ui.Managers;

    using Microsoft.AspNet.Identity.Owin;

    /// <summary>
    /// Abstract base class for all controllers.
    /// </summary>
    public abstract class BaseController : Controller
    {
        #region member vars

        private CustomSignInManager _signInManager;
        private CustomUserManager _userManager;

        #endregion

        #region constructors and destructors

        public BaseController()
        {
        }

        public BaseController(CustomUserManager userManager)
        {
            UserManager = userManager;
        }

        public BaseController(CustomUserManager userManager, CustomSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        #region methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region properties

        /// <summary>
        /// Manager for all signin-operations based on the configured EF context.
        /// </summary>
        public CustomSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<CustomSignInManager>();
            private set => _signInManager = value;
        }

        /// <summary>
        /// Manager for all user-related operations connected to the configured EF context.
        /// </summary>
        public CustomUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<CustomUserManager>();
            private set => _userManager = value;
        }

        #endregion
    }
}