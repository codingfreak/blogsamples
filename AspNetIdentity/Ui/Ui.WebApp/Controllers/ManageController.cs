using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Ui.WebApp.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using Logic.Shared.Enumerations;
    using Logic.Ui.ViewModels;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    /// <summary>
    /// Controller for all views inside the /Manage path.
    /// </summary>
    [Authorize]
    public class ManageController : BaseController
    {
        #region methods

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction(
                "VerifyPhoneNumber",
                new
                {
                    PhoneNumber = model.Number
                });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            var result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false, false);
                return RedirectToAction(
                    "Index",
                    new
                    {
                        Message = ManageMessageId.ChangePasswordSuccess
                    });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            await UserManager.SetTwoFactorEnabledAsync(user.Id, false);
            await SignInManager.SignInAsync(user, false, false);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            await UserManager.SetTwoFactorEnabledAsync(user.Id, true);
            await SignInManager.SignInAsync(user, false, false);
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess
                ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess
                    ? "Your password has been set."
                    : message == ManageMessageId.SetTwoFactorSuccess
                        ? "Your two-factor authentication provider has been set."
                        : message == ManageMessageId.Error
                            ? "An error has occurred."
                            : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added." : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed." : "";

            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(user.Id),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(user.Id),
                Logins = await UserManager.GetLoginsAsync(user.Id),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(user.Id.ToString())
            };
            return View(model);
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction(
                    "ManageLogins",
                    new
                    {
                        Message = ManageMessageId.Error
                    });
            }
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
            return result.Succeeded
                ? RedirectToAction("ManageLogins")
                : RedirectToAction(
                    "ManageLogins",
                    new
                    {
                        Message = ManageMessageId.Error
                    });
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed." : message == ManageMessageId.Error ? "An error has occurred." : "";
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(user.Id);
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(
                new ManageLoginsViewModel
                {
                    CurrentLogins = userLogins,
                    OtherLogins = otherLogins
                });
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            var result = await UserManager.RemoveLoginAsync(user.Id, new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false, false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction(
                "ManageLogins",
                new
                {
                    Message = message
                });
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            var result = await UserManager.SetPhoneNumberAsync(user.Id, null);
            if (!result.Succeeded)
            {
                return RedirectToAction(
                    "Index",
                    new
                    {
                        Message = ManageMessageId.Error
                    });
            }
            await SignInManager.SignInAsync(user, false, false);
            return RedirectToAction(
                "Index",
                new
                {
                    Message = ManageMessageId.RemovePhoneSuccess
                });
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return View("Error");
                }
                var result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);
                    return RedirectToAction(
                        "Index",
                        new
                        {
                            Message = ManageMessageId.SetPasswordSuccess
                        });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null
                ? View("Error")
                : View(
                    new VerifyPhoneNumberViewModel
                    {
                        PhoneNumber = phoneNumber
                    });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var result = await UserManager.ChangePhoneNumberAsync(user.Id, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);
                    return RedirectToAction(
                        "Index",
                        new
                        {
                            Message = ManageMessageId.AddPhoneSuccess
                        });
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        #endregion

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindByName(User.Identity.Name);
            return user?.PasswordHash != null;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindByName(User.Identity.Name);
            return user?.PhoneNumber != null;
        }

        #endregion
    }
}