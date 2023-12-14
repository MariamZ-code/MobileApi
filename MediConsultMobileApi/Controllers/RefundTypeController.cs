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

        public RefundTypeController(IRefundTypeRepository refundTypeRepo)
        {
            this.refundTypeRepo = refundTypeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetRefundType()
        {
            if (ModelState.IsValid)
            {
                var refundTypes = await refundTypeRepo.GetRefundTypeByOnProgram();


                return Ok(refundTypes);

            }
            return BadRequest(ModelState);
        }
    }
}
