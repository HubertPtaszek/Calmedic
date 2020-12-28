using Calmedic.Application;
using Calmedic.Domain;
using Calmedic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
                    ModelState.AddModelError("Email", "Invalid login attempt.");
                    return View(model);
                }
                if (!_appIdentityUserService.IsActive(user.Id))
                {
                    ModelState.AddModelError("Email", "Account is inactive");
                    return View(model);
                }
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!signInResult.Succeeded || !user.EmailConfirmed)
                {
                    ModelState.AddModelError("Email", "Invalid login attempt.");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
                }
                ModelState.AddModelError("Email", "Invalid login attempt.");
            }
            return View(model);
        }
    }
}