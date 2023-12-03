using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepo;
        private readonly ApplicationDbContext dbcontext;

        public MemberController(IMemberRepository memberRepo, ApplicationDbContext dbcontext)
        {
            this.memberRepo = memberRepo;
            this.dbcontext = dbcontext;
        }



        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int id)
        {

            if (ModelState.IsValid)
            {
                var member = await memberRepo.GetByID(id); // member 
                var msg = new MessageDto();

                if (member is null )
                {
                return BadRequest(new MessageDto { Message ="User Not found "});

                }
                if (member.program_name is null)
                {
                    msg.Message = "User in Archive";
                    return BadRequest(msg);
                }
                if (member.member_status == "Deactivated")
                {
                    msg.Message = "User is Deactivated";

                    return BadRequest(msg);

                }
                if (member.member_status == "Hold")
                {
                    msg.Message = "User is Hold";
                    return BadRequest(msg);


                }
                return Ok(member);
            }
            return BadRequest(ModelState);
        }


    }
}
