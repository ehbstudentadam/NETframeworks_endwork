using ExamenAdam.Data;
using ExamenAdam.Identity.Entities;
using ExamenAdam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAdam.Controllers
{
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private UserManager<User> UserManager { get; }
        private RoleManager<Role> RoleManager { get; }

        public ManagementController(ILogger<ManagementController> logger, RoleRepository roleRepository, UserRepository userRepository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> ManageUsers()
        {
            /*if (ModelState.IsValid is false)
            {
                return View(model);
            }*/

            var unApprovedUsers = _userRepository.GetUnapprovedUsersShowResultFromUntill(0, 10);

            if (unApprovedUsers == null)
            {
                return NotFound();
            }

            Dictionary<User, Role> unapprovedUsersAndRoles = new();

            foreach (var user in unApprovedUsers)
            {
                var roles = await UserManager.GetRolesAsync(user);

                Role? role = _roleRepository.FindByName(roles.First());
                if (role == null)
                {
                    unapprovedUsersAndRoles.Add(user, new Role());
                    continue;
                }                
                unapprovedUsersAndRoles.Add(user, role);
            }

            var allRoles = _roleRepository.GetAllRoles();

            if (allRoles == null)
            {
                return NotFound();
            }


            ManageUsersModel manage = new ManageUsersModel()
            {
                UnapprovedUsersAndRoles = unapprovedUsersAndRoles,
                AllRoles = allRoles.ToList()
            };


            return View(manage);
        }



        public async Task<IActionResult> UpdateUsers(ManageUsersModel model)
        {
            /*if (ModelState.IsValid is false)
            {
                return View(model);
            }*/

            
            foreach(KeyValuePair<User, Role> kvp in model.UnapprovedUsersAndRoles)
            {
                User user = kvp.Key;
                Role role = kvp.Value;

                _userRepository.UpdateUser(user);

                //Remove old roles
                var roles = await UserManager.GetRolesAsync(user);
                await UserManager.RemoveFromRolesAsync(user, roles);
                //Add to new role
                await UserManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction(nameof(ManageUsers));
        }


    }
}
