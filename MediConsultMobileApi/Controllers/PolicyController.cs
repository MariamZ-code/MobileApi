using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyRepository policyRepo;

        public PolicyController(IPolicyRepository policyRepo)
        {
            this.policyRepo = policyRepo;
        }



        [HttpGet("ProgramId")]

        public async Task<IActionResult> GetByProgramId(int programId)
        {
            if (ModelState.IsValid)
            {
               var policies = await policyRepo.GetByProgramId(programId);
                var policyListDto = new List<PolicyDTO>();
                foreach (var policy in policies)
                {
                    var policyDto = new PolicyDTO
                    {
                        Policy_id = policy.Policy_id,
                        Program_id = policy.Program_id,
                        Service_Class_Id = policy.Service_Class_Id,
                        SL_Copayment = policy.SL_Copayment,
                        SL_Limit = policy.SL_Limit,
                        ServiceNameAr = policy.Service.Service_Class_Name_Ar,
                        ServiceNameEn = policy.Service.Service_Class_Name_En

                    };
                    policyListDto.Add(policyDto);
                }
                return Ok(policyListDto);


            }
            return BadRequest(ModelState);
        }
    }
}
