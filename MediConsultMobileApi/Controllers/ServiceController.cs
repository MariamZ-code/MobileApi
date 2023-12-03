using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository serviceRepo;
        private readonly IMemberRepository memberRepo;
     
        public ServiceController(IServiceRepository serviceRepo , IMemberRepository memberRepo)
        {
            this.serviceRepo = serviceRepo;
            this.memberRepo = memberRepo;
            
        }
        [HttpGet("memberId")]
        public async Task<IActionResult> GetById(int id)
        {

            var member = await memberRepo.GetByID(id); // member 
            var valid = await memberRepo.validation(member); // msg 
            if (valid.Message is null)
            {
                
                var service = await serviceRepo.GetById(member);
                return Ok(service);

            }


            return Ok(valid.Message);

        
        }
    }
}
