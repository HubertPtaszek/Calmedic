using Calmedic.Application;
using Calmedic.Domain;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace Calmedic.Areas.Membership.Controllers
{
    [Area(AreaNames.Membership_Area)]
    public class AccountController : AppController
    {
        #region Dependencies

        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly ILogger<LoginVM> _logger;
        private readonly IAppIdentityUserService _appIdentityUserService;
        #endregion Dependencies

        public AccountController(SignInManager<AppIdentityUser> signInManager,
            ILogger<LoginVM> logger,
            UserManager<AppIdentityUser> userManager,
            IAppIdentityUserService appIdentityUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appIdentityUserService = appIdentityUserService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginVM());
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityUser user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("User", ErrorResource.WrongLoginData);
                    return View(model);
                }
                if (!_appIdentityUserService.IsActive(user.Id))
                {
                    ModelState.AddModelError("Account", ErrorResource.AccountInactive);
                    return View(model);
                }
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("Email", ErrorResource.EmailNotConfirmed);
                    return View(model);
                }
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!signInResult.Succeeded)
                {
                    ModelState.AddModelError("Login", ErrorResource.WrongLoginData);
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
                }
                ModelState.AddModelError("Login", ErrorResource.WrongLoginData);
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
            }
            if (userId == null || code == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(new ConfirmEmailVM()
            {
                Code = code,
                UserId = userId,
            });
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailVM model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityUser user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("Password", "Error confirming your email.");
                    model.Password = null;
                    model.ConfirmedPassword = null;
                    return View(model);
                }
                string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Password", "Error confirming your email.");
                    model.Password = null;
                    model.ConfirmedPassword = null;
                    return View(model);
                }
                var signInResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!signInResult.Succeeded)
                {
                    foreach (var error in signInResult.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    model.Password = null;
                    model.ConfirmedPassword = null;
                    return View(model);
                }
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
            }
            if (TempData["ForgotPassword"] as bool? == true)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
            }
            if (code == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new ResetPasswordVM()
            {
                Code = code
            });
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Password", "Error reseting your password.");
                    return View(model);
                }
                string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
                var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (result.Succeeded)
                {
                    TempData["ResetPassword"] = true;
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }
            model.Password = null;
            model.ConfirmedPassword = null;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
            }
            if (TempData["ResetPassword"] as bool? == true)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}