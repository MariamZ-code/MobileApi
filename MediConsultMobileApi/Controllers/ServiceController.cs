using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
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

        public ServiceController(IServiceRepository serviceRepo, IMemberRepository memberRepo)
        {
            this.serviceRepo = serviceRepo;
            this.memberRepo = memberRepo;

        }
        [HttpGet("memberId")]
        public async Task<IActionResult> GetById(int id, string lang)
        {

            if (ModelState.IsValid)
            {
                var member = await memberRepo.GetByID(id); // member
                var memberExists = memberRepo.MemberExists(id);
                var services = await serviceRepo.GetById(member);
                var serviceEn = new List<ServiceEnDTO>();
                var serviceAr = new List<ServiceArDTO>();

                if (!memberExists)
                {

                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (member.program_name is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberArchive(lang) });
                }
                if (member.member_status == "Deactivated")
                {


                    return BadRequest(new MessageDto { Message = Messages.MemberDeactivated(lang) });

                }
                if (member.member_status == "Hold")
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberHold(lang) });

                }
                if (lang == "en")
                {
                    foreach (var service in services)
                    {
                        ServiceEnDTO serviceEnDto = new ServiceEnDTO
                        {

                            program_id = service.program_id,
                            service_nameEn = service.service_nameEn,
                            copayment = service.copayment

                        };

                        serviceEn.Add(serviceEnDto);

                    }
                    return Ok(serviceEn);

                }
                foreach (var service in services)
                {
                    ServiceArDTO serviceArDto = new ServiceArDTO
                    {

                        program_id = service.program_id,
                        service_nameAr = service.service_nameAr,
                        copayment = service.copayment

                    };

                    serviceAr.Add(serviceArDto);

                }
                return Ok(serviceAr);    

        }
            return BadRequest(ModelState);


    }
}
}
