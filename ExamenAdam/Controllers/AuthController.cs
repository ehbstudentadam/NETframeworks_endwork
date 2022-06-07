using ExamenAdam.Identity.Entities;
using ExamenAdam.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAdam.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public SignInManager<User> SignInManager { get; }
        public UserManager<User> UserManager { get; }

        public AuthController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            SignInManager = signInManager;
            UserManager = userManager;
        }


        [HttpGet, AllowAnonymous]
        public IActionResult Login(string redirectUrl)
        {
            return View(new SignInModel()
            {
                RedirectUrl = redirectUrl
            });
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(SignInModel model)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                // TODO: login failure message.
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Redirect(model.RedirectUrl ?? "/");
            }

            // TODO: sign in failure.
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(SignUpModel model)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var result = await UserManager.CreateAsync(new User
            {
                Birthday = model.Birthday,
                Sex = model.Sex,
                Address = model.Address,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Approved = false
            }, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return Redirect("/");
        }




        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
