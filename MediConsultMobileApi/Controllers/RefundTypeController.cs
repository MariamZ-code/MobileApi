using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
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

                var memberExists = memberRepo.MemberExists(memberId);

                if (!memberExists)
                {

                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                var refundTypes = await refundTypeRepo.GetRefundTypeByOnProgram(member.program_id); // onProgram==1
                var refundTypesByPolicy = await refundTypeRepo.GetRefundTypeByOnPolicy(member.policy_id); // onProgram==0
                var refundTypesEn = new List<RefundTypeEnDTO>();
                var refundTypesAr = new List<RefundTypeArDTO>();
                if (lang == "en")
                {
                    foreach (var refundType in refundTypes)
                    {

                                RefundTypeEnDTO refundTypeEnDTO = new RefundTypeEnDTO
                                {
                                    en_name = refundType.reimbursementType.en_name,
                                };

                                refundTypesEn.Add(refundTypeEnDTO);
                       
                     }
                    foreach (var refundType in refundTypesByPolicy)
                    {
                        RefundTypeEnDTO refundTypeEnDTO = new RefundTypeEnDTO
                        {
                            en_name = refundType.reimbursementType.en_name,
                          
                        };

                        refundTypesEn.Add(refundTypeEnDTO);
                    }
                    return Ok(refundTypesEn);

                }
                foreach (var refundType in refundTypes)
                {
                   
                        RefundTypeArDTO refundTypeArDTO = new RefundTypeArDTO
                        {
                            ar_name = refundType.reimbursementType.ar_name,
                         
                        };

                        refundTypesAr.Add(refundTypeArDTO);
                    

                }
                foreach (var refundType in refundTypesByPolicy)
                {
                    RefundTypeArDTO refundTypeArDTO = new RefundTypeArDTO
                    {
                        ar_name = refundType.reimbursementType.ar_name,
                       
                    };

                    refundTypesAr.Add(refundTypeArDTO);
                }
                return Ok(refundTypesAr);

            }

            return BadRequest(ModelState);


        }
    }
}
