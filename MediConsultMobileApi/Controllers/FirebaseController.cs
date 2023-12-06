using FirebaseAdmin;
using FirebaseAdmin.Auth;
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
        //[Route("SendNotification")]
        //[HttpPost]
        //public async Task<IActionResult> SendNotification([Required][FromBody] NotificationMessage notificationMessage)
        //{
        //    var firebase_token = await dbContext.logins.Where(x => notificationMessage.membersIds.Contains(x.member_id.ToString())).ToListAsync();
        //    if (firebase_token is null)
        //    {
        //        return NotFound(new MessageDto { Message = "member not found" });
        //    }
        //    if (firebase_token.Count == 0)
        //    {
        //        return NotFound(new MessageDto { Message = "notification token not found" });
        //    }

        //    List<string> tokens = new List<string>();
        //    if (firebase_token.Any(x => x == null || x.firebase_token == null))
        //    {
        //        return NotFound(new MessageDto { Message = "Invalid or null firebase tokens found" });
        //    }
        //    for (int i = 0; i < firebase_token.Count; i++)
        //    {
        //        if (firebase_token[i] != null && firebase_token[i].firebase_token != null)
        //        {
        //            tokens.Add(firebase_token[i].firebase_token);
        //        }
        //    }
        //    //for (int i = 0; i < firebase_token.Count; i++)
        //    //{
        //    //    if (firebase_token[i].firebase_token is null)
        //    //        continue;
        //    //    tokens.Add(firebase_token[i].firebase_token);
        //    //}
        //    if (firebase_token == null || firebase_token.Count == 0)
        //    {
        //        return NotFound(new MessageDto { Message = "No matching members found" });
        //    }
        //    if (dbContext.logins == null)
        //    {
        //        return BadRequest(new MessageDto { Message = "Database context is not initialized" });
        //    }
        //    var message = new MulticastMessage()
        //    {
        //        Notification = new Notification
        //        {
        //            Title = notificationMessage.Title,
        //            Body = notificationMessage.Body,
        //            ImageUrl = notificationMessage.ImageUrl
        //        },
        //        Tokens = tokens
        //    };

        //    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
        //    return Ok(new MessageDto { Message = "notification sent successfully" });
        //}




        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationMessage notificationMessage)
        {
            if (notificationMessage == null)
            {
                return BadRequest(new MessageDto { Message = "Invalid notification message" });
            }

            var userIds = notificationMessage.membersIds.Select(id => id.ToString()).ToList();

            var firebaseTokens = await dbContext.logins
                .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
                .Select(x => x.firebase_token)
                .ToListAsync();

            if (firebaseTokens.Count == 0)
            {
                return NotFound(new MessageDto { Message = "No valid tokens found for the specified members" });
            }

            if (notificationMessage.ImageUrl is null)
            {
                var message = new MulticastMessage
                {
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,
                        //ImageUrl = notificationMessage.ImageUrl
                    },
                    Tokens = firebaseTokens
                };

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return Ok(new MessageDto { Message = "Notification sent successfully" });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = "Imges not Url " });

            }
            var messa = new MulticastMessage
            {
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                },
                Tokens = firebaseTokens
            };

            var respo = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);

            return Ok(new MessageDto { Message = "Notification sent successfully" });


        }
        private bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }




        [Route("SendNotificationToAll")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAll([Required][FromBody] NotificationMessageToAll notificationMessage)
        {

            if (notificationMessage.ImageUrl is null)
            {
                var message = new Message
                {
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,
                        //ImageUrl = notificationMessage.ImageUrl
                    },
                    Topic = "all"
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return Ok(new MessageDto { Message = "Notification sent successfully" });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = "Imges not Url " });

            }
            var messa = new Message
            {
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                },
                Topic = "all"

            };

            var respo = await FirebaseMessaging.DefaultInstance.SendAsync(messa);

            return Ok(new MessageDto { Message = "Notification sent successfully" });

        }
    }
    


}
