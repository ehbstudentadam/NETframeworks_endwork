using ExamenAdam.Data;
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
        private readonly ILogger<AuthController> _logger;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;



        public AuthController(ILogger<AuthController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Login(string redirectUrl)
        {
            return View(new LogInModel()
            {
                RedirectUrl = redirectUrl
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInModel model)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Redirect(model.RedirectUrl ?? "/");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {            
            return View(new RegisterModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var address = model.Address;
            if (address.PostalBus == null)
            {
                address.PostalBus = "/";
            }


            var result = await _userManager.CreateAsync(new User
            {
                Birthday = model.Birthday,
                Sex = model.Sex,
                Address = address,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Approved = false
            }, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(model.UserName), "Pending");
                return RedirectToAction(nameof(Login));
            }

            var x = result.Errors.ToList();
            List<string> errors = new();
            foreach (var item in x)
            {
                errors.Add(item.Description);
            }
            model.Errors = errors;

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
