using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
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
        //string? providerNameEn , string? providerNameAr,
        public async Task<IActionResult> GetAll(string lang, string? providerName, int startpage = 1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var providers = providerRepo.GetProviders();
              
                var providerEn = new List<ProviderDataEnDTO>();
                var providerAr = new List<ProviderDataArDTO>();
                if (lang == "en")
                {
                    if (providerName is not null)
                    {
                        providers = providers.Where(p => p.Provider_name_en.Contains(providerName));
                    }
                    var totalProvidersEn = providers.Count();
                    providers = providers.Skip((startpage - 1) * pageSize).Take(pageSize).OrderByDescending(e => e.Provider_name_en);
                    foreach (var provider in providers)
                    {
                        ProviderDataEnDTO providerEnDto = new ProviderDataEnDTO
                        {

                            Provider_id = provider.Provider_id,
                            Provider_name_en = provider.Provider_name_en,

                        };

                        providerEn.Add(providerEnDto);
                    }
                    var resultEn = new
                    {
                        TotalProviders = totalProvidersEn,
                        PageNum = startpage,
                        PageSize = pageSize,

                        Providers = providerEn
                    };

                    return Ok(resultEn);

                }

                if (providerName is not null)
                {
                    providers = providers.Where(p => p.Provider_name_ar.Contains(providerName));
                }
                foreach (var provider in providers)
                {
                    ProviderDataArDTO providerArDto = new ProviderDataArDTO
                    {

                        Provider_id = provider.Provider_id,
                        Provider_name_ar = provider.Provider_name_ar,

                    };

                    providerAr.Add(providerArDto);
                }

                var totalProvidersAR = providers.Count();
                providers = providers.Skip((startpage - 1) * pageSize).Take(pageSize).OrderByDescending(e => e.Provider_name_en);


                var resultAr = new
                {
                    TotalProviders = totalProvidersAR,
                    PageNum = startpage,
                    PageSize = pageSize,

                    Providers = providerAr
                };

                return Ok(resultAr);
            }
            return BadRequest(ModelState);
        }
    }
}
