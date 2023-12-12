using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(ModelState.IsValid)
            {
                var category = await categoryRepo.GetAll();
                return Ok(category);
            }
            return BadRequest(ModelState);
        }
    }
}
