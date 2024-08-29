using Application.Posts;
using CommonDataContract.Extension;
using CommonDataContract.Post;
using Discussify.API.DTOs.Post;
using Discussify.API.Extensions;
using Domain.AggegratesModel.PostAggegrate;
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
            List<PostListingResponse> postReponse = new List<PostListingResponse>();
            try
            {
                var latestPost = _postService.GetLatestPost().OrderByDescending(x => x.WhenCreated);

                foreach (var post in latestPost)
                {
                    var author = await _userRepository.GetUserByIdAsync(post.UserId);
                    PostListingResponse p = new PostListingResponse()
                    {
                        PostId = post.Id,
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = $"{_server.GetHostUrl()}/Post/{post.Image}",
                        AuthorImageUrl = $"{_server.GetHostUrl()}/Avatars/{author.Avatar}",
                        AuthorId = post.UserId,
                        Tags = post.Tags.Select(x => x.Name).ToList(),
                        AuthorName = author.FirstName,
                        AuthorLastName = author.LastName,
                        TotalComments = post.Comments.Count(),
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

        [HttpGet("{postId}")]
        public async Task<IActionResult> Get(int postId)
        {
            try
            {
                var post = _postService.GetPost(postId);

                PostResponse postResponse = new PostResponse();
                postResponse.UserId = post.UserId;
                postResponse.PostId = post.Id;
                postResponse.Title = post.Title;
                postResponse.Description = post.Description;
                postResponse.ImageUrl = $"{_server.GetHostUrl()}{post.Image}";

                foreach (var c in post.Comments)
                {
                    var author = await _userRepository.GetUserByIdAsync(c.UserId); 
                    var postComment = new PostComment();
                    postComment.CommentId = c.Id;
                    postComment.PostId = post.Id;
                    postComment.UserId = c.UserId;
                    postComment.AuthorName = author.FirstName;
                    postComment.WhenCreated = c.WhenCreated.ToElaspedTime(); ;
                    postComment.AuthorImageUrl = $"{_server.GetHostUrl()}{author.Avatar}";
                    postComment.Content = c.Content;

                    postResponse.Comments.Add(postComment);
                }


                return Ok(postResponse);
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(null);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest request)
        {
            var post = _postService.CreatePost(request.UserId, request.Title, request.Description, request.CategoryId, request.Tags.ToArray());
            return Ok(new { PostId = post.Id, Message = "Post created successfully." });
        }

        [HttpPost("Comment")]
        public async Task<IActionResult> Comment([FromBody] NewCommentRequest request)
        {
            var isCommentAdded = _postService.AddComment(request.PostId, request.UserId, request.Comment);
            if(isCommentAdded)
            {
                return Ok(new { Message = "Comment created successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Unable to add comment" });
            }
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
            post.ChangeImage(uniqueFileName);

            _postService.Save();

            return Ok(new { imageUrl });
        }
    }
}
