using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FireBaseTokenController : ControllerBase
    {
        private readonly ITokenRepository tokenRepo;

        public FireBaseTokenController(ITokenRepository tokenRepo)
        {
            this.tokenRepo = tokenRepo;
        }
        [HttpPost("SaveToken")]
        public async Task<IActionResult> SaveToken(FirebaseTokenDTO tokenDto)
        {
            if (ModelState.IsValid)
            {
                var token = await tokenRepo.SaveToken(tokenDto);
                return Ok(new MessageDto { Message = "Token Saved"});
            }

            return BadRequest(new MessageDto { Message = "Wrong" });
        }
    }
}
