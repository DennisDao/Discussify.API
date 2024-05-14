using CommonDataContract.Extension;
using CommonDataContract.Post;
using Discussify.API.Extensions;
using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServer _server;
        public PostController(IPostRepository postRepository, IUserRepository userRepository, IServer server)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _server = server;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            List<PostResponse> postReponse = new List<PostResponse>();
            var latestPost = _postRepository.GetLatestPost().ToList();

            foreach (var post in latestPost)
            {
                var author = await _userRepository.GetUserByIdAsync(post.UserId);
                PostResponse p = new PostResponse()
                {
                    Title = post.Title,
                    ImageUrl = $"{_server.GetHostUrl()}{post.ImageUrl}",
                    AuthorImageUrl = $"{_server.GetHostUrl()}{author.Avatar}",
                    AuthorId = post.UserId,
                    Tags = post.Topics.Select(x => x.GetDescription()).ToList(),
                    AuthorName = author.FirstName,
                    AuthorLastName = author.LastName,
                    TotalComments = 0,
                    TotalLikes = 0,
                    TotalViews = 0,
                    WhenCreated = post.WhenCreated.ToElaspedTime()
                };

                postReponse.Add(p);
            }

            return Ok(postReponse);
        }
    }
}
