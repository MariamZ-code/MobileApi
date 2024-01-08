using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
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

        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationMessage notificationMessage, string lang)
        {
            if (notificationMessage == null)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationValid(lang) });

            }

            var userIds = notificationMessage.membersIds.Select(id => id.ToString()).ToList();

            var firebaseTokens = await dbContext.clientBranchMembers
                .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
                .Select(x => x.firebase_token)
                .ToListAsync();

            if (firebaseTokens.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationToken(lang) });

            }
            var customData = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },

            };
            if (notificationMessage.ImageUrl == string.Empty)
            {
                var message = new MulticastMessage
                {
                    Tokens = firebaseTokens,
                    Data = customData,
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,
                    },
                };

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });


            }
          
            var messa = new MulticastMessage
            {
                Tokens = firebaseTokens,
                Data = customData,
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                }
            };

            var respo = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);

            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });



        }
        private bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }




        [Route("SendNotificationToAll")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAll([Required][FromBody] NotificationMessageToAll notificationMessage, string lang)
        {
            var customData = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },

            };
            if (notificationMessage.ImageUrl == string.Empty)
            {

                var message = new Message
                {
                    Data = customData,
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,

                    },
                    Topic = "all"
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });



            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });

            }
            var messa = new Message
            {

                Data = customData,
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                },
                Topic = "all"

            };

            var respo = await FirebaseMessaging.DefaultInstance.SendAsync(messa);

            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


        }
    }



}
