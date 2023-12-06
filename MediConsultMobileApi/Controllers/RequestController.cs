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

        #region AddNewRequest
        [HttpPost]

        public async Task<IActionResult> PostRequest([FromForm] RequestDTO requestDto, [FromForm] List<IFormFile> files)
        {

            string[] validExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
            const long maxSizeBytes = 5 * 1024 * 1024;
            if (ModelState.IsValid)
            {
                var request = requestRepo.AddRequest(requestDto);

                var providerExists = await providerRepo.ProviderExistsAsync(requestDto.Provider_id);
                var memberExists =  memberRepo.MemberExists(requestDto.Member_id);
                if (requestDto.Provider_id is null)
                {
                    return BadRequest("Enter Provider Id");
                }
                if (!providerExists)
                {
                    return BadRequest("Provider Id not found");
                }
                if (requestDto.Member_id is null)
                {
                    return BadRequest("Enter member Id");
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

                    return BadRequest(new MessageDto { Message = "Folder Path must end with extension .jpg, .png, or .jpeg" });

                }

                return BadRequest(new MessageDto { Message = "Please uploade File " });
            }
            return BadRequest(ModelState);



        }

        #endregion

        #region RequestByMemberId
        [HttpGet("MemberId")]

        public IActionResult GetbyMemberId([Required] int memberId, string? datefrom, string? dateTo, [FromQuery] string[]? status, [FromQuery] string[]? providers, int startpage = 1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
               
                var requests = requestRepo.GetRequestsByMemberId(memberId);
                var reqDto = new List<RequestDetailsForMemberDTO>();
                var memberExist = memberRepo.MemberExists(memberId);

                if (!memberExist)
                {
                    return NotFound("Member Id not found");

                }
                    if (requests is null)
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
                    var totalProviders = requests.Count();
                requests = requests.Skip((startpage - 1) * pageSize).Take(pageSize).OrderByDescending(e => e.Provider.Provider_name_en);
                foreach (var request in requests)
                    {
                       
                    RequestDetailsForMemberDTO reqDetalisDto = new RequestDetailsForMemberDTO
                    {

                        Id = request.ID,
                        CreatedDate = request.created_date,
                        ProviderName = request.Provider.Provider_name_en,
                        Status = request.Status

                    };
                    reqDto.Add(reqDetalisDto);
                }
               

                return Ok(reqDto);
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region RequestByRequestId
        [HttpGet("RequestId")]

        public async Task<IActionResult> GetResultByID([Required]int id)
        {
            if (ModelState.IsValid)
            {
                var request = await requestRepo.GetById(id);
                if (request is null)
                {
                    return NotFound(new MessageDto { Message = $"Id  not found" });
                }

                var reqDto = new RequestDetailsDTO
                {
                    Id = request.ID,
                    ApprovalId = request.Approval_id,
                    CreatedDate = request.created_date,
                    Status = request.Status,
                    Approval = null,
                    Notes = request.Notes


                };
                return Ok(reqDto);

            }
            return BadRequest(ModelState);
        }
        #endregion

    }
}
