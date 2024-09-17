using Api.DTOs.Activities;
using Api.DTOs.Post;
using Application.Activites;
using Application.Posts;
using Discussify.API.Extensions;
using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServer _server;
        public ActivityController(IActivityRepository activityRepository, IUserRepository userRepository, IServer server) 
        {
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _server = server;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            try
            {
                var activities = _activityRepository.GetActivitesByUserId(userId)
                                       .GroupBy(x => x.WhenCreated.Date) // Group by date
                                       .Select(group => new ActivityGroupResponse
                                       {
                                           WhenCreated = group.Key.ToShortDateString(),
                                           Activities =  group.Select(activity => new ActivityResponse
                                           {
                                               ActivityId = activity.Id,
                                               Image = GetImage(activity.CreateByUserId).Result,   
                                               Title = activity.Title,      
                                               Content = activity.Content   
                                           }).ToList()
                                       }).ToList();


                return Ok(activities);

            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }
        }

        private async Task<string> GetImage(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return $"{_server.GetHostUrl()}/Avatars/{user.Avatar}";
        }
    }
}
