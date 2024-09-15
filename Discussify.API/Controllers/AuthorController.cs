using Api.DTOs.Authors;
using Api.DTOs.Followers;
using Api.DTOs.Post;
using Application.Followers;
using Application.Posts;
using Discussify.API.Extensions;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowerRepository _followerRepository;
        private readonly IPostRepository _postRepository;
        private readonly IServer _server;
        public AuthorController(IUserRepository userRepository,
            IFollowerRepository followerRepository,
            IPostRepository postRepository,
            IServer server)
        {
            _userRepository = userRepository;
            _followerRepository = followerRepository;
            _postRepository = postRepository;
            _server = server;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int pageSize = 1, int pageNumber = 1)
        {
            List<AuthorResponse> authorReponse = new List<AuthorResponse>();
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var following = _followerRepository.GetFollowing(userId);

            try
            {
                var users = _userRepository.GetUsers(pageSize, pageNumber);

                foreach (var user in users)
                {
                    var followers = _followerRepository.GetFollowers(user.Id);
                    var totalPost = _postRepository.GetPostByUserId(user.Id).Count();

                    AuthorResponse author = new AuthorResponse();
                    author.UserId = user.Id;
                    author.Picture = $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
                    author.FirstName = user.FirstName;
                    author.LastName = user.LastName;
                    author.Email = user.Email;
                    author.Bio = user.Bio;
                    author.IsFollowing = following.Any(x => x.FollowingUserId == user.Id);  
                    author.TotalFollowers = followers.Count();
                    author.TotalPost = totalPost;

                    authorReponse.Add(author);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(authorReponse);
        }

        [HttpGet("Following")]
        public async Task<IActionResult> Following(int userId)
        {
            List<FollowAuthorResponse> response = new List<FollowAuthorResponse>();
            var followings = _followerRepository.GetFollowing(userId);

            try
            {
                foreach (var following in followings)
                {
                    var user = await _userRepository.GetUserByIdAsync(following.FollowingUserId);
                    var followers = _followerRepository.GetFollowers(user.Id);
                    var totalPost = _postRepository.GetPostByUserId(user.Id).Count();

                    FollowAuthorResponse author = new FollowAuthorResponse();
                    author.UserId = user.Id;
                    author.Picture = $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
                    author.FirstName = user.FirstName;
                    author.LastName = user.LastName;
                    author.Email = user.Email;
                    author.Bio = user.Bio;
                    author.IsFollowing = true;
                    author.FollowerId = following.Id;
                    author.TotalFollowers = followers.Count();
                    author.TotalPost = totalPost;

                    response.Add(author);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(response);
        }

        [HttpGet("Followers")]
        public async Task<IActionResult> Followers(int userId)
        {
            List<FollowAuthorResponse> response = new List<FollowAuthorResponse>();
            var followers = _followerRepository.GetFollowers(userId);

            try
            {
                foreach (var follower in followers)
                {
                    var user = await _userRepository.GetUserByIdAsync(follower.FollowerUserId);
                    var totalFollower = _followerRepository.GetFollowers(userId).Count();
                    var totalPost = _postRepository.GetPostByUserId(follower.FollowerUserId).Count();

                    FollowAuthorResponse author = new FollowAuthorResponse();
                    author.UserId = user.Id;
                    author.Picture = $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
                    author.FirstName = user.FirstName;
                    author.LastName = user.LastName;
                    author.Email = user.Email;
                    author.Bio = user.Bio;
                    author.IsFollowing = true;
                    author.FollowerId = follower.Id;
                    author.TotalFollowers = followers.Count();
                    author.TotalPost = totalPost;

                    response.Add(author);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(response);
        }

        [HttpPost("DeleteFollower")]
        public async Task<IActionResult> DeleteFollower([FromBody] DeleteFollowerRequest request)
        {
            try
            {
                var follower = _followerRepository.GetFollowerByid(request.FollowerId);
                _followerRepository.Delete(follower);
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok();
        }

        [HttpPost("FollowAuthor")]
        public async Task<IActionResult> FollowAuthor([FromBody] FollowAuthorRequest request)
        {
            try
            {
                var follower = _followerRepository.AddFollower(request.UserId, request.FollowingUserId);
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok();
        }

        [HttpGet("FindAuthor")]
        public async Task<IActionResult> FindAuthor(string query)
        {
            List<AuthorResponse> authorReponse = new List<AuthorResponse>();
            try
            {
                var foundUsers = _userRepository.FindUser(query);

                foreach (var user in foundUsers)
                {
                    AuthorResponse author = new AuthorResponse();
                    author.UserId = user.Id;
                    author.Picture = $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
                    author.FirstName = user.FirstName;
                    author.LastName = user.LastName;
                    author.Email = user.Email;
                    author.Bio = user.Bio;

                    authorReponse.Add(author);
                }

                return Ok(authorReponse);
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }
        }
    }
}
