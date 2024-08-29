using Application.Posts;
using CommonDataContract.Post;
using Discussify.API.Extensions;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuickSearchController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IUserRepository _userRepository;
        private readonly IServer _server;
        public QuickSearchController(PostService postService, IUserRepository userRepository, IServer server)
        {
            _postService = postService;
            _userRepository = userRepository;
            _server = server;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string query)
        {
            List<PostListingResponse> postReponse = new List<PostListingResponse>();
            try
            {
                var latestPost = _postService.GetPostByQuery(query).OrderByDescending(x => x.WhenCreated);

                foreach (var post in latestPost)
                {
                    PostListingResponse p = new PostListingResponse()
                    {
                        PostId = post.Id,
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = $"{_server.GetHostUrl()}/Post/{post.Image}",
                    };

                    postReponse.Add(p);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(postReponse);
        }
    }
}
