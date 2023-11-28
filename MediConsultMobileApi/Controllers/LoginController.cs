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
        private readonly ApplicationDbContext _context;

        public LoginController(IAuthRepository authRepo, ApplicationDbContext _context)
        {
            this.authRepo = authRepo;
            this._context = _context;
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
