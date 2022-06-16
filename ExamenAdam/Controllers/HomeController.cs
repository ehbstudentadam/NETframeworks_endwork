using ExamenAdam.Data;
using ExamenAdam.Entities;
using ExamenAdam.Identity;
using ExamenAdam.Identity.Entities;
using ExamenAdam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExamenAdam.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;
        private UserManager<User> UserManager { get; }

        public HomeController(ILogger<HomeController> logger, PostRepository postRepository, CommentRepository commentRepository, UserManager<User> userManager)
        {
            _logger = logger;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            UserManager = userManager;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AboutUs()
        {
            return View();
        }


        [HttpGet, AllowAnonymous]
        public IActionResult BlogIndex()
        {
            var latestBlogPosts = _postRepository.GetLastXAmount(5);

            if (latestBlogPosts == null)
            {
                return NotFound();
            }

            Dictionary<Post, string> postWithDescription = new();

            foreach ( Post post in latestBlogPosts)
            {                
                string description = "" ;
                var postBody = post.Body;
                if (postBody.Length > 420)
                {
                    description = postBody.Substring(0, 400);
                    description += " ...";
                }
                postWithDescription.Add(post, description);                
            }

            return View(new BlogIndexModel { PostWithDescription = postWithDescription });
        }


        [HttpGet]
        public IActionResult BlogPost(long id)
        {
            var post = _postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }
            BlogPostModel model = new()
            {
                Post = post,
                NewComment = "..."
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlogPost(BlogPostModel model, long id)
        {
            if (ModelState.IsValid is false)
            {
                return RedirectToAction(nameof(BlogPost), new { id });
            }

            var user = await UserManager.GetUserAsync(User);
            var post = _postRepository.FindById(id);

            if (post == null)
            {
                return NotFound();
            }

            string newCommentString = model.NewComment;

            Comment newComment = new() { Commentary = newCommentString, Post = post, User = user };

            _commentRepository.AddEntity(newComment);

            return RedirectToAction(nameof(BlogPost), new { id });
        }


        
        public IActionResult CreateBlogPost()
        {
            return View();
        }


        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostModel model)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);

            Post post = new()
            {
                Title = model.Title,
                Body = model.Body,
                User = user
            };

            _postRepository.AddEntity(post);

            return RedirectToAction(nameof(BlogIndex));
        }


        public ActionResult DeleteComment(long idcomment, long idpost)
        {
            var comment = _commentRepository.FindById(idcomment);
            if (comment == null)
            {
                return NotFound();
            }

            _commentRepository.DeleteComment(comment);

            return RedirectToAction(nameof(BlogPost), new {id = idpost});
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}