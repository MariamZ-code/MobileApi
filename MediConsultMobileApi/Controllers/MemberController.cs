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
           
            var member = await memberRepo.GetByID(id); // member 
            var valid = await memberRepo.validation(member); // msg 
            if (valid.Message is null)
            {
                return Ok(member);

            }
            return BadRequest(valid.Message);
        }


    }
}
