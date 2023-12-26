using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalNetworkController : ControllerBase
    {
        private readonly IMedicalNetworkRepository medicalRepo;

        public MedicalNetworkController(IMedicalNetworkRepository medicalRepo)
        {
            this.medicalRepo = medicalRepo;
        }
        [HttpGet]

        public IActionResult MedicalNetwork([FromQuery] string? providerName, string lang, [FromQuery] string[]? categories, int StartPage = 1, int pageSize = 10)
        {

            if (ModelState.IsValid)
            {
                var medicalNets = medicalRepo.GetAll();
                var medicalNetEn = new List<MedicalNetworkEnDTO>();
                var medicalNetAr = new List<MedicalNetworkArDTO>();

                if (lang == "en")
                {
                    if (providerName is not null)
                    {
                        medicalNets = medicalNets.Where(x => x.provider_name_en.Contains(providerName));
                    }
                    if (categories != null && categories.Any())
                    {
                        for (int i = 0; i < categories.Length; i++)
                        {
                            var status = categories[i];
                        }
                        medicalNets = medicalNets.Where(c => categories.Contains(c.Category_Name_En));
                    }

                    var totalCount = medicalNets.Count();
                    medicalNets = medicalNets.Skip((StartPage - 1) * pageSize).Take(pageSize);
                    if (medicalNets == null) { return BadRequest(new MessageDto { Message = Messages.MedicalNetwork(lang) }); }
                    foreach (var medicalNet in medicalNets)
                    {
                        if (!string.IsNullOrEmpty(medicalNet.Latitude) && !string.IsNullOrEmpty(medicalNet.Longitude) &&
                            !medicalNet.Latitude.Contains(",") && !medicalNet.Longitude.Contains(","))
                        {
                            MedicalNetworkEnDTO medicalNetEnDto = new MedicalNetworkEnDTO
                            {

                                Category = medicalNet.Category_Name_En,
                                providerName = medicalNet.provider_name_en,
                                Latitude = medicalNet.Latitude,
                                Longitude = medicalNet.Longitude,
                                Email = medicalNet.Email,
                                Hotline = medicalNet.Hotline,
                                Mobile = medicalNet.Mobile,
                                Telephone = medicalNet.Telephone,
                                SpecialtyName = medicalNet.General_Specialty_Name_En,
                            };

                            medicalNetEn.Add(medicalNetEnDto);
                        }


                    }
                    var medicalEnDto = new
                    {

                        TotalCount = totalCount,
                        PageNumber = StartPage,
                        PageSize = pageSize,
                        MedicalNetwork = medicalNetEn,


                    };


                    return Ok(medicalEnDto);

                }


                if (providerName is not null)
                {
                    medicalNets = medicalNets.Where(x => x.Provider_name.Contains(providerName));
                }
                if (categories != null && categories.Any())
                {
                    for (int i = 0; i < categories.Length; i++)
                    {
                        var cat = categories[i];
                    }
                    medicalNets = medicalNets.Where(c => categories.Contains(c.Category));
                }
              

                var totalCountt = medicalNets.Count();
                medicalNets = medicalNets.Skip((StartPage - 1) * pageSize).Take(pageSize);
                if (medicalNets == null) { return BadRequest(new MessageDto { Message = Messages.MedicalNetwork(lang) }); }
                foreach (var medicalNet in medicalNets)
                {
                    if (!string.IsNullOrEmpty(medicalNet.Latitude) && !string.IsNullOrEmpty(medicalNet.Longitude) &&
                            !medicalNet.Latitude.Contains(",") && !medicalNet.Longitude.Contains(","))
                    {
                        MedicalNetworkArDTO medicalNetArDto = new MedicalNetworkArDTO
                        {

                            Category = medicalNet.Category,
                            providerName = medicalNet.Provider_name,
                            Latitude = medicalNet.Latitude,
                            Longitude = medicalNet.Longitude,
                            Email = medicalNet.Email,
                            Hotline = medicalNet.Hotline,
                            Mobile = medicalNet.Mobile,
                            Telephone = medicalNet.Telephone,
                            SpecialtyName = medicalNet.Speciality,
                        };
                    medicalNetAr.Add(medicalNetArDto);
                    }


                }
                var medicalArDto = new
                {

                    TotalCount = totalCountt,
                    PageNumber = StartPage,
                    PageSize = pageSize,
                    MedicalNetwork = medicalNetAr,


                };


                return Ok(medicalArDto);
            }
            return BadRequest(ModelState);
        }
    }
}
