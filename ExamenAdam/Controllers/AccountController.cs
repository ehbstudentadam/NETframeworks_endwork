using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAdam.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Settings()
        {
            return Settings();
        }


    }
}
