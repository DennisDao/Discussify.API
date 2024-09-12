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
        private readonly IServer _server;
        public AuthorController(IUserRepository userRepository, IFollowerRepository followerRepository, IServer server)
        {
            _userRepository = userRepository;
            _followerRepository = followerRepository;
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
                    AuthorResponse author = new AuthorResponse();
                    author.UserId = user.Id;
                    author.Picture = $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
                    author.FirstName = user.FirstName;
                    author.LastName = user.LastName;
                    author.Email = user.Email;
                    author.Bio = user.Bio;
                    author.IsFollowing = following.Any(x => x.FollowingUserId == user.Id);    

                    authorReponse.Add(author);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(authorReponse);
        }

        [HttpGet("FollowingAuthors")]
        public async Task<IActionResult> GetFollowingAuthors()
        {
            List<FollowAuthorResponse> response = new List<FollowAuthorResponse>();
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var followings = _followerRepository.GetFollowing(userId);

            try
            {
                foreach (var following in followings)
                {
                    var user = await _userRepository.GetUserByIdAsync(following.FollowingUserId);

                    FollowAuthorResponse author = new FollowAuthorResponse();
                    author.UserId = user.Id;
                    author.Picture = $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
                    author.FirstName = user.FirstName;
                    author.LastName = user.LastName;
                    author.Email = user.Email;
                    author.Bio = user.Bio;
                    author.IsFollowing = true;
                    author.FollowerId = following.Id;

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
