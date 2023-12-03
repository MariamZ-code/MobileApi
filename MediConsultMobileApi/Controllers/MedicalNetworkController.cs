using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalNetworkController : ControllerBase
    {
        private readonly IMedicalNetworkRepository medicalRepo;

        public MedicalNetworkController(IMedicalNetworkRepository medicalRepo)
        {
            this.medicalRepo = medicalRepo;
        }
        [HttpGet]

        public IActionResult MedicalNetwork([FromQuery] string? providerName , [FromQuery] string[]? categories , int StartPage = 1, int pageSize = 10)
        {

            if (ModelState.IsValid)
            {
                var medicalNet = medicalRepo.GetAll(providerName , categories);
                var totalCount = medicalNet.Count();
                medicalNet = medicalNet.Skip((StartPage - 1) * pageSize).Take(pageSize);
                if (medicalNet == null) { return BadRequest(new MessageDto { Message = "Not found" }); }
                return Ok(medicalNet);
                
            }
            return BadRequest(ModelState);

        }
    }
}
