using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
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
        public async Task<IActionResult> GetAll(string lang)
        {
            if(ModelState.IsValid)
            {
                var categories = await categoryRepo.GetAll();
                if (lang == "en")
                {
                    var categoryEn = new List<CategoryEnDTO>();
                    foreach (var category in categories)
                    {
                        CategoryEnDTO categoryEnDto = new CategoryEnDTO
                        {

                          Category_Id = category.Category_Id,
                          Category_Name_En = category.Category_Name_En,
                        };

                        categoryEn.Add(categoryEnDto);
                    }

                     return Ok(categoryEn);

                }
                var categoryAr = new List<CategoryArDTO>();
                foreach (var category in categories)
                {
                    CategoryArDTO categoryArDto = new CategoryArDTO
                    {

                        Category_Id = category.Category_Id,
                        Category_Name_Ar = category.Category_Name_Ar,
                    };

                    categoryAr.Add(categoryArDto);
                }

                return Ok(categoryAr);
            }
            return BadRequest(ModelState);
        }
    }
}
