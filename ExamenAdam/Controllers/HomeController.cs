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
    [Authorize (Policy = Policies.Approved)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;

        public UserManager<User> UserManager { get; }

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
                return View();
            }

            Dictionary<Post, string> postWithDescription = new Dictionary<Post, string>();

            foreach ( Post post in latestBlogPosts)
            {
                
                string description = "" ;
                var postBody = post.Body;
                if (postBody.Length > 220)
                {
                    description = postBody.Substring(0, 200);
                    description += " ...";
                }
                postWithDescription.Add(post, description);                
            }

            return View(new BlogIndexModel { PostWithDescription = postWithDescription });
        }


        [HttpGet]
        public IActionResult BlogPost(BlogPostModel model, long id)
        {
            /*if (ModelState.IsValid is false)
            {
                return View(model);
            }*/

            var post = _postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }

            model.Post = post;
/*            model.PostId = id;
            model.PostTitle = post.Title;
            model.PostBody = post.Body;
            model.Comments = post.Comments;*/


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CommentOnPostAsync(BlogPostModel model, long id)
        {
            /*if (ModelState.IsValid is false)
            {
                return View(model);
            }*/
            var user = await UserManager.GetUserAsync(User);
            var post = _postRepository.FindById(id);

            if (post == null)
            {
                return NotFound();
            }

            string newCommentString = model.NewComment + $"<br><i>{post.User.UserName}</i>";

            Comment newComment = new() { Commentary = newCommentString, Post = post, User = user };

            _commentRepository.AddEntity(newComment);

            //_commentRepository.AddNewComment(post, newCommentString);

            return RedirectToAction(nameof(BlogPost), new { id, model });
        }


        
        public IActionResult CreateBlogPost()
        {
            return View();
        }


        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostModel model)
        {
            /*if (ModelState.IsValid is false)
            {
                return View(model);
            }*/

            var user = await UserManager.GetUserAsync(User);

            Post post = model.Post;
            post.User = user;

            _postRepository.AddEntity(post);

            return RedirectToAction(nameof(BlogIndex));
        }

        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}