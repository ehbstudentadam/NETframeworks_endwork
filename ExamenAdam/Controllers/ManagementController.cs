using Microsoft.AspNetCore.Mvc;

namespace ExamenAdam.Controllers
{
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;

        public ManagementController(ILogger<ManagementController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageUsers()
        {
            return View();
        }
    }
}
