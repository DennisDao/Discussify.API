using Api.DTOs.Authors;
using Api.DTOs.Post;
using Application.Posts;
using Discussify.API.Extensions;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IServer _server;
        public AuthorController(IUserRepository userRepository, IServer server)
        {
            _userRepository = userRepository;
            _server = server;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int pageSize = 1, int pageNumber = 1)
        {
            List<AuthorResponse> authorReponse = new List<AuthorResponse>();
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

                    authorReponse.Add(author);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(authorReponse);
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
