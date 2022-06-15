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

        public ManagementController(ILogger<ManagementController> logger, RoleRepository roleRepository, UserRepository userRepository, UserManager<User> userManager)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            UserManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var unApprovedUsers = _userRepository.GetUnapprovedUsersShowResultFromUntill(0, 10);
            Dictionary<User, Role> unapprovedUsersAndRoles = new();

            foreach (var user1 in unApprovedUsers)
            {
                var roles1 = await UserManager.GetRolesAsync(user1);

                Role? role1 = _roleRepository.FindByName(roles1.First());
                if (role1 == null)
                {
                    unapprovedUsersAndRoles.Add(user1, new Role());
                    continue;
                }
                unapprovedUsersAndRoles.Add(user1, role1);
            }

            var allRoles = _roleRepository.GetAllRoles();

            if (allRoles == null)
            {
                return NotFound();
            }
            return View(new ManageUsersModel { AllRoles = allRoles.ToList(), UnapprovedUsersAndRoles = unapprovedUsersAndRoles });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUsers(ManageUsersModel model, string browse)
        {
            if (ModelState.IsValid is false)
            {
                return RedirectToAction(nameof(ManageUsers));
            }

            //PART TO BROWSE NON APPROVED USERS

            var from = Convert.ToInt32(model.SearchFrom);
            var untill = Convert.ToInt32(model.SearchTo);
            var unApprovedUsers = _userRepository.GetUnapprovedUsersShowResultFromUntill(from, untill);

            if (unApprovedUsers == null)
            {
                return View();
            }

            if (browse != null)
            {
                if (browse.Equals("next"))
                {
                    from += 10;
                    untill += 10;
                    unApprovedUsers = _userRepository.GetUnapprovedUsersShowResultFromUntill(from, untill);
                    model.SearchFrom = from;
                    model.SearchTo = untill;
                }
                else if (browse.Equals("previous"))
                {
                    from -= 10;
                    untill -= 10;
                    if (from < 0 || untill < 0)
                    {
                        from = 0;
                        untill = 10;
                    }
                    unApprovedUsers = _userRepository.GetUnapprovedUsersShowResultFromUntill(from, untill);
                    model.SearchFrom = from;
                    model.SearchTo = untill;
                }
            }else
            {
                unApprovedUsers = _userRepository.GetUnapprovedUsersShowResultFromUntill(0, 10);                
            }

            //PART TO SHOW NON APPROVED USERS

            Dictionary<User, Role> unapprovedUsersAndRoles = new();

            foreach (var user1 in unApprovedUsers)
            {
                var roles1 = await UserManager.GetRolesAsync(user1);

                Role? role1 = _roleRepository.FindByName(roles1.First());
                if (role1 == null)
                {
                    unapprovedUsersAndRoles.Add(user1, new Role());
                    continue;
                }                
                unapprovedUsersAndRoles.Add(user1, role1);
            }

            var allRoles = _roleRepository.GetAllRoles();

            if (allRoles == null)
            {
                return NotFound();
            }

            model.AllRoles = allRoles.ToList();
            model.UnapprovedUsersAndRoles = unapprovedUsersAndRoles;

            //PART TO SEARCH A USER

            string? userName = model.UserName;
            string? firstName = model.FirstName;
            string? lastName = model.LastName;
            string? email = model.Email;

            User? user = null;
            Dictionary<User, Role> searchResult = new();

            if (userName != null)
            {
                var result = _userRepository.FindByUsername(userName);
                if (result is not null)
                {
                    user = result;
                }
            }
            if (firstName != null)
            {
                var result = _userRepository.FindByFirstname(firstName);
                if (result is not null)
                {
                    user = result;
                }
            }
            if (lastName != null)
            {
                var result = _userRepository.FindByLastname(lastName);
                if (result is not null)
                {
                    user = result;
                }
            }
            if (email != null)
            {
                var result = _userRepository.FindByLastname(email);
                if (result is not null)
                {
                    user = result;
                }
            }
            if (user == null)
            {
                return View(model);
            }

            var roles = await UserManager.GetRolesAsync(user);
            Role? role = _roleRepository.FindByName(roles.First());
            if (role == null)
            {
                searchResult.Add(user, new Role());
            }
            else
            {
                searchResult.Add(user, role);
            }

            model.UserResult = searchResult;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditUserAndRole(long id)
        {
            if (ModelState.IsValid is false)
            {
                return View();
            }

            var user = _userRepository.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = _roleRepository.GetAllRoles();
            if (allRoles == null)
            {
                return NotFound();
            }

            var roles = await UserManager.GetRolesAsync(user);
            Role? role = _roleRepository.FindByName(roles.First());
            if (role == null)
            {
                return NotFound();
            }

            EditUserAndRoleModel model = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Approved = user.Approved,
                AllRoles = allRoles.ToList(),
                RoleEdit = role
            };
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserAndRole(EditUserAndRoleModel model, long id)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var user = _userRepository.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Approved = model.Approved;
            _userRepository.UpdateUser(user);

            //Remove old roles
            var roles = await UserManager.GetRolesAsync(user);
            await UserManager.RemoveFromRolesAsync(user, roles);
            //Add to new role
            await UserManager.AddToRoleAsync(user, model.RoleEdit.Name);

            return RedirectToAction(nameof(ManageUsers));
        }


    }
}
