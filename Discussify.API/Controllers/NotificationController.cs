using Application.Notifications;
using Application.Posts;
using CommonDataContract.Extension;
using CommonDataContract.Post;
using Discussify.API.DTOs.Notifications;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notifciationService;
        public NotificationController(NotificationService notifciationService)
        {
            _notifciationService = notifciationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int userId)
        {
            var response = new List<NotificationResponse>();
            try
            {
                var notifcations = _notifciationService.GetNotifcations(userId);

                foreach (var notification in notifcations)
                {
                    var notificationResponse = new NotificationResponse()
                    {
                        NotificationId = notification.Id,
                        UserId = notification.UserId,
                        Message = notification.Message,
                        Link = notification.Link,
                        EntityId = notification.EntityId,
                        NotificationType = notification.NotificationEntityType.GetDescription()
                    };

                    response.Add(notificationResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(response);
        }

        [HttpPost("SetViewed")]
        public async Task<IActionResult> SetViewed(int notificationId)
        {
            try
            {
               _notifciationService.SetNotificationToViewed(notificationId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }
        }
    }
}
