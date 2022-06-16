using ExamenAdam.Data;
using ExamenAdam.Entities;
using ExamenAdam.Identity;
using ExamenAdam.Identity.Entities;
using ExamenAdam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAdam.Controllers
{
    [Authorize(Policy = Policies.Approved)]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserRepository _userRepository;
        private readonly PostRepository _postRepository;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;


        public AccountController(ILogger<AccountController> logger, UserRepository userRepository, PostRepository postRepository, UserManager<User> usermanager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _userManager = usermanager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            var postFromUser = _postRepository.GetAllPoststFromUserFromUntill(user, 0, 10);

            if (postFromUser == null)
            {
                return View(new AccountSettingsModel
                {
                    AccountId = user.Id
                });
            }
            else
            {
                return View(new AccountSettingsModel
                {
                    AccountId = user.Id,
                    Posts = (List<Post>)postFromUser
                });
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(AccountSettingsModel model, string browse)
        {
            if (ModelState.IsValid is false)
            {
                return RedirectToAction(nameof(Settings));
            }

            var user = await _userManager.GetUserAsync(User);

            var from = Convert.ToInt32(model.SearchFrom);
            var untill = Convert.ToInt32(model.SearchTo);
            var postFromUser = _postRepository.GetAllPoststFromUserFromUntill(user, from, untill);

            if (postFromUser == null)
            {
                return View();
            }

            if (browse != null)
            {
                if (browse.Equals("next"))
                {
                    from += 10;
                    untill += 10;
                    postFromUser = _postRepository.GetAllPoststFromUserFromUntill(user, from, untill);
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
                    postFromUser = _postRepository.GetAllPoststFromUserFromUntill(user, from, untill);
                    model.SearchFrom = from;
                    model.SearchTo = untill;
                }
            }
            else
            {
                postFromUser = _postRepository.GetAllPoststFromUserFromUntill(user, 0, 10);
            }

            model.Posts = (List<Post>?)postFromUser;
            model.AccountId = user.Id;

            return View(model);
        }



        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(long id)
        {
            var post = _postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }

            _postRepository.DeletePost(post);

            return RedirectToAction(nameof(Settings));
        }



        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(long id)
        {
            var user = _userRepository.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            //Remove roles
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            _userRepository.DeleteUser(user);

            await _signInManager.SignOutAsync();

            return Redirect("/");
        }



    }
}
