using MailKit.Net.Smtp;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using MediConsultMobileApi.Services;
using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository authRepo;

        private readonly ISMSService sMS ;

        private readonly IMemberRepository memberRepo;
        private readonly IValidation validation;

        public LoginController(IAuthRepository authRepo, ISMSService sMS , IMemberRepository memberRepo , IValidation validation)
        {
            this.authRepo = authRepo;
            this.sMS = sMS;
            this.memberRepo = memberRepo;
            this.validation = validation;
        }
        #region Login

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto )
        {

            if (userDto.Id == string.Empty || userDto.Password == string.Empty)
                    {
                        return BadRequest(new MessageDto { Message = "Id and Password is required " });
                    }

                if (!int.TryParse(userDto.Id, out _))
                {
                    return BadRequest(new MessageDto { Message = "Invalid Id" });
                }
                var user = await authRepo.Login(userDto);

                if (user.Message is "Id or Password is incorrect" || user.Message is "Account Disabled")
                {
                    return BadRequest(user);
                }
                return Ok(user);                   
                
        }
        #endregion

        #region GenerateOtp
        private string GenerateOtp()
        {
            // Implement your OTP generation logic (e.g., generate a random 6-digit OTP)
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        #endregion



        #region ResetPasswordSendOTPSMS

        [HttpGet("ResetPasswordSendOTPSMS")]
        public async Task<IActionResult> ResetPasswordSendOTPSMS(int memberId)
        {
            if (ModelState.IsValid)
            {
                var member = authRepo.ResetPassword(memberId);
                var memberMobile = memberRepo.GetByID(memberId).Result.mobile;
                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = "Not found Member" });

                } 
                if (memberMobile is null || memberMobile == string.Empty)
                {
                        return BadRequest("Mobile number not found");

                }
                if (!memberMobile.StartsWith("01"))
                {
                    return BadRequest(new MessageDto { Message = "Mobile Number must start with 01" });
                }
                if (memberMobile.Length != 11)
                {
                    return BadRequest(new MessageDto { Message = "Mobile Number must be 11 number" });
                }

                string otp = GenerateOtp();
                string url = $"http://52.28.71.183/sms_api/Message/SendSMS?text={otp}&mobile={memberMobile}"; 
                using (var client = new HttpClient())
                {
                    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                   client.DefaultRequestHeaders.Accept.Clear();
                   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(url,content);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(new MessageDto
                        {
                            Message = "OTP Message delivered"
                        });
                    }
                    else
                    {
                        return BadRequest(response.StatusCode);
                    }
                }

            }
            return BadRequest(ModelState);
        }
        #endregion


        #region ResetPasswordSendOTPEmail
        [HttpGet("ResetPasswordSendOTPEmail")]

        public IActionResult SendEmail(int memberId)
        {
            var member = authRepo.ResetPassword(memberId);
            var memberEmail = memberRepo.GetByID(memberId).Result.email;

            if (memberEmail is not null)
            {
             
                if (!validation.IsValidEmail(memberEmail))
                {
                    return BadRequest(new MessageDto { Message = "Email is not valid." });
                }

            }
            if (memberEmail is null )
            {
                    return BadRequest(new MessageDto { Message = "Email is not Found." });

            }

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Mediconsult", "no-reply@mediconsulteg.com"));

            message.To.Add(new MailboxAddress("", memberEmail));

            message.Subject = "Password Reset";

            //message.Headers.Add("X-Priority", "2");
            //message.Headers.Add("X-MSMail-Priority", "Normal");
            string otp = GenerateOtp();

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = otp;
          
            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587, false);
                client.Authenticate("no-reply@mediconsulteg.com", "Medi@2025");
                client.Send(message);
                client.Disconnect(true);
            }
            return Ok("Send");
        }
        #endregion

    }

}
