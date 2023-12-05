using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
    

        public LoginController(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
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


    }

}
