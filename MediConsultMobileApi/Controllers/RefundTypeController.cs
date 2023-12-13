using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundTypeController : ControllerBase
    {
        private readonly IPolicyTypeRepository refundTypeRepo;

        public RefundTypeController(IPolicyTypeRepository refundTypeRepo)
        {
            this.refundTypeRepo = refundTypeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetRefundType()
        {
            if (ModelState.IsValid)
            {
                var refundType =await refundTypeRepo.GetRefundType();
                return Ok(refundType);
            }
            return BadRequest(ModelState);
        }
    }
}
