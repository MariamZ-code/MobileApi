using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundTypeController : ControllerBase
    {
        private readonly IRefundTypeRepository refundTypeRepo;
        private readonly IMemberRepository memberRepo;

        public RefundTypeController(IRefundTypeRepository refundTypeRepo, IMemberRepository memberRepo)
        {
            this.refundTypeRepo = refundTypeRepo;
            this.memberRepo = memberRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetRefundType(string lang, int memberId)
        {
            if (ModelState.IsValid)
            {

                var member = await memberRepo.GetByID(memberId); // member

                var refundTypes = await refundTypeRepo.GetRefundTypeByOnProgram(member.program_id); // onProgram==1
                var refundTypesByPolicy = await refundTypeRepo.GetRefundTypeByOnPolicy(member.policy_id); // onProgram==0
                var refundTypesEn = new List<RefundTypeDTO>();
                var refundTypesAr = new List<RefundTypeDTO>();
                if (lang == "en")
                {
                    foreach (var refundType in refundTypes)
                    {

                                RefundTypeDTO refundTypeEnDTO = new RefundTypeDTO
                                {
                                    //ar_name = refundType.reimbursementType.ar_name,
                                    en_name = refundType.reimbursementType.en_name,
                                    program_id = refundType.program_id,
                                    is_on_program = refundType.is_on_program,
                                };

                                refundTypesEn.Add(refundTypeEnDTO);
                       
                     }
                    foreach (var refundType in refundTypesByPolicy)
                    {
                        RefundTypeDTO refundTypeEnDTO = new RefundTypeDTO
                        {
                            //ar_name = refundType.reimbursementType.ar_name,
                            en_name = refundType.reimbursementType.en_name,
                            program_id = refundType.program_id,
                            is_on_program = refundType.is_on_program,
                        };

                        refundTypesEn.Add(refundTypeEnDTO);
                    }
                    return Ok(refundTypesEn);

                }
                foreach (var refundType in refundTypes)
                {
                   
                        RefundTypeDTO refundTypeArDTO = new RefundTypeDTO
                        {
                            ar_name = refundType.reimbursementType.ar_name,
                            //en_name = refundType.reimbursementType.en_name,
                            program_id = refundType.program_id,
                            is_on_program = refundType.is_on_program,
                        };

                        refundTypesAr.Add(refundTypeArDTO);
                    

                }
                foreach (var refundType in refundTypesByPolicy)
                {
                    RefundTypeDTO refundTypeEnDTO = new RefundTypeDTO
                    {
                        //ar_name = refundType.reimbursementType.ar,
                        ar_name = refundType.reimbursementType.ar_name,
                        program_id = refundType.program_id,
                        is_on_program = refundType.is_on_program,
                    };

                    refundTypesEn.Add(refundTypeEnDTO);
                }
                return Ok(refundTypesAr);

            }

            return BadRequest(ModelState);


        }
    }
}
