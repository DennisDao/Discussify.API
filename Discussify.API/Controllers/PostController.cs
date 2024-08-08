using Application.Posts;
using CommonDataContract.Extension;
using CommonDataContract.Post;
using Discussify.API.DTOs.Post;
using Discussify.API.Extensions;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IUserRepository _userRepository;
        private readonly IServer _server;
        public PostController(PostService postService, IUserRepository userRepository, IServer server)
        {
            _postService = postService;
            _userRepository = userRepository;
            _server = server;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            List<PostResponse> postReponse = new List<PostResponse>();
            try
            {
                var latestPost = _postService.GetLatestPost();

                foreach (var post in latestPost)
                {
                    var author = await _userRepository.GetUserByIdAsync(post.UserId);
                    PostResponse p = new PostResponse()
                    {
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = $"{_server.GetHostUrl()}{post.ImageUrl}",
                        AuthorImageUrl = $"{_server.GetHostUrl()}{author.Avatar}",
                        AuthorId = post.UserId,
                        Tags = post.Tags.Select(x => x.Name).ToList(),
                        AuthorName = author.FirstName,
                        AuthorLastName = author.LastName,
                        TotalComments = 0,
                        TotalLikes = 0,
                        TotalViews = 0,
                        WhenCreated = post.WhenCreated.ToElaspedTime()
                    };

                    postReponse.Add(p);
                }
            }
            catch(Exception ex) 
            {
                return BadRequest("Oops!");
            }

            return Ok(postReponse);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest request)
        {
            _postService.CreatePost(request.UserId, request.Title, request.Description, request.CategoryId, request.Tags.ToArray());
            return Ok();
        }

        //[HttpPost("SetTags")]
        //public async Task<IActionResult> SetTag([FromBody] PostCreateRequest request)
        //{
        //    _postService.CreatePost(request.UserId, request.Title, request.Description);
        //    return Ok();
        //}

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile image, int postId)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No file uploaded.");

            var post = _postService.GetPost(postId);

            var uploadsFolder = Path.Combine("wwwroot", "Post");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName.Replace(" ", "");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var imageUrl = Url.Content($"~/Post/{uniqueFileName}");
            post.ChangeImage(imageUrl);

            _postService.Save();

            return Ok(new { imageUrl });
        }
    }
}
