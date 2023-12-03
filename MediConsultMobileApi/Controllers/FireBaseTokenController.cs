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
        private readonly IMemberRepository memberRepo;

        public FireBaseTokenController(ITokenRepository tokenRepo , IMemberRepository memberRepo)
        {
            this.tokenRepo = tokenRepo;
            this.memberRepo = memberRepo;
        }
        [HttpPost("SaveToken")]
        public async Task<IActionResult> SaveToken(FirebaseTokenDTO tokenDto)
        {
            if (ModelState.IsValid)
            {
                var memberExists = await memberRepo.MemberExistsAsync(tokenDto.MemberId);
                if (!memberExists)
                {
                    return BadRequest( new MessageDto { Message = "Member Id not found" });
                }

                if (tokenDto.Firebase_token is null || tokenDto.Firebase_token.Trim().Length==0 )
                {
                    return BadRequest(new MessageDto { Message = "Enter Token " });


                }
                var token =  tokenRepo.SaveToken(tokenDto);
                return Ok(new MessageDto { Message = "Token Saved"});
            }

            return BadRequest(new MessageDto { Message = "Wrong" });
        }
    }
}
