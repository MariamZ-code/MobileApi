using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderDataController : ControllerBase
    {
        private readonly IProviderDataRepository providerRepo;

        public ProviderDataController(IProviderDataRepository providerRepo)
        {
            this.providerRepo = providerRepo;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll(string providerName , int startpage =1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var providers =  providerRepo.GetProviders();
                if (providerName is not null)
                { 
                    providers = providers.Where(p => p.Provider_name_en.Contains(providerName));
                }
                var totalProviders = providers.Count();
                providers = providers.Skip((startpage - 1) * pageSize).Take(pageSize).OrderByDescending(e => e.Provider_name_en);
                var result = new
                {
                    TotalProviders = totalProviders,
                    PageNum = startpage,
                    PageSize = pageSize,

                    Providers = providers
                };

                return Ok(providers);
            }
            return BadRequest(ModelState);
        }
    }
}
