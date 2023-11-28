using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
       
        private readonly ApplicationDbContext dbContext;

        public FirebaseController(ApplicationDbContext dbContext)
        {
            
            this.dbContext = dbContext;
        }
        [Route("SendNotification")]
        [HttpPost]
        public async Task<IActionResult> SendNotification([Required][FromBody] NotificationMessage notificationMessage)
        {
            var firebase_token = await dbContext.logins.Where(x => notificationMessage.membersIds.Contains(x.member_id.ToString())).ToListAsync();
            if (firebase_token is null)
            {
                return NotFound(new MessageDto { Message = "member not found" });
            }
            if (firebase_token.Count == 0)
            {
                return NotFound(new MessageDto { Message = "notification token not found" });
            }
            List<string> tokens = new List<string>();
            for (int i = 0; i < firebase_token.Count; i++)
            {
                if (firebase_token[i].firebase_token is null)
                    continue;
                tokens.Add(firebase_token[i].firebase_token);
            }
            var message = new MulticastMessage()
            {
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                },
                Tokens = tokens
            };
            //var message1 = new MulticastMessage()
            //{

            //};
            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return Ok(new MessageDto { Message = "notification sent successfully" });
        }

        [Route("SendNotificationToAll")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAll([Required][FromBody] NotificationMessageToAll notificationMessage)
        {
            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                },
                Topic = "all"
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return Ok(new MessageDto { Message = "notification sent successfully" });
        }

    }
}
