using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository requestRepo;
        private readonly IProviderDataRepository providerRepo;
        private readonly IMemberRepository memberRepo;

        public RequestController(IRequestRepository requestRepo , IProviderDataRepository providerRepo ,IMemberRepository memberRepo)
        {
            this.requestRepo = requestRepo;
            this.providerRepo = providerRepo;
            this.memberRepo = memberRepo;
        }


        [HttpPost]
        public async Task<IActionResult> PostRequest([FromForm]RequestDTO requestDto, [FromForm] List<IFormFile> files)
        {
            
                string[] validExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
                const long maxSizeBytes = 5 * 1024 * 1024;
                if (ModelState.IsValid)
                {
                    var request = requestRepo.AddRequest(requestDto);

                var providerExists = await providerRepo.ProviderExistsAsync(requestDto.Provider_id);
                var memberExists = await memberRepo.MemberExistsAsync(requestDto.Member_id);
                if (requestDto.Provider_id is null)
                {
                    return BadRequest("Enter Provider Id");
                }

                if (requestDto.Member_id is null)
                {
                    return BadRequest("Enter member Id");
                }
                if (!providerExists )
                {
                    return BadRequest("Provider Id not found");
                }
                if (!memberExists)
                {
                    return BadRequest("Member Id not found");

                }

                for (int i = 0; i < files.Count; i++)
                    {

                        if (files[i] is null || files[i].Length == 0)
                        {
                            return BadRequest("No file uploaded.");
                        }
                        if (files[i].Length > maxSizeBytes)
                        {
                            return BadRequest($"File size must be less than 5 MB.");
                        }




                    var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                    var folder = Path.Combine(serverPath, "MemberPortalApp", requestDto.Member_id.ToString(), "Approvals", request.ID.ToString());
                     

                    foreach (var extension in validExtensions)
                        {
                            if (files[i].FileName.EndsWith(extension))
                            {
                                if (!Directory.Exists(folder))
                                {
                                    Directory.CreateDirectory(folder);
                                }


                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + files[i].FileName;

                                string filePath = Path.Combine(folder, uniqueFileName);


                                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await files[i].CopyToAsync(stream);
                                }
                              
                                return Ok(request);
                            }
                        }
             
                       return BadRequest("Folder Path must end with extension .pdf, .jpg, .png, or .jpeg");
                    }
                        
                    return BadRequest("Please uploade File ");
                 }
                return BadRequest(ModelState);



         }


        [HttpGet("MemberId")]
        
        public IActionResult GetbyMemberId([Required] int memberId, string? datefrom, string? dateTo ,[FromQuery] string[]? status, [FromQuery] string[]? providers)
        {
            if (ModelState.IsValid)
            {
                var requests = requestRepo.GetRequestsByMemberId(memberId);

                foreach (var request in requests)
                {
                    if (request is null)
                    {
                        return NotFound("Request not found");

                    }
                    if (datefrom is not null)
                    {
                        requests = requests.Where(x => x.created_date.Contains(datefrom));
                    }
                    if (dateTo is not null)
                    {
                        requests = requests.Where(x => x.created_date.Contains(dateTo));
                    }
                    if (status != null && status.Any())
                    {
                        for (int i = 0; i < status.Length; i++)
                        {
                            var sta = status[i];
                        }
                        requests = requests.Where(c => status.Contains(c.Status));
                    }
                    if (providers != null && providers.Any())
                    {
                        for (int i = 0; i < providers.Length; i++)
                        {
                            var provider = providers[i];
                        }
                        requests = requests.Where(p => providers.Contains(p.Provider.Provider_name_en));
                    }
                }
                return Ok(requests);
            }
            return BadRequest(ModelState);

        }
    }
}
