using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository requestRepo;
        private readonly IProviderDataRepository providerRepo;
        private readonly IMemberRepository memberRepo;
        private readonly ApplicationDbContext dbContext;

        public RequestController(IRequestRepository requestRepo, IProviderDataRepository providerRepo, IMemberRepository memberRepo , ApplicationDbContext dbContext)
        {
            this.requestRepo = requestRepo;
            this.providerRepo = providerRepo;
            this.memberRepo = memberRepo;
            this.dbContext = dbContext;
        }

        #region AddNewRequest
        [HttpPost]

        
        public async Task<IActionResult> PostRequest([FromForm] RequestDTO requestDto, [FromForm] List<IFormFile> files , string lang)
        {

            const long maxSizeBytes = 5 * 1024 * 1024;
            if (ModelState.IsValid)
            {

                var providerExists = await providerRepo.ProviderExistsAsync(requestDto.Provider_id);
                var memberExists = memberRepo.MemberExists(requestDto.Member_id);
                if (requestDto.Provider_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterProvider(lang)});
                }
                if (!providerExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.ProviderNotFound(lang)});
                }
                if (requestDto.Member_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterMember(lang) });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang)});

                }

                var serverPath = AppDomain.CurrentDomain.BaseDirectory;

                string[] validExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
                if (files.Count == 0)
                {
                    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                }
                for (int j = 0; j < files.Count; j++)
                {

                    if (files[j].Length == 0)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                    }
                    if (files[j].Length >= maxSizeBytes)
                    {
                        return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                    }
                    // image.png --0
                    switch (Path.GetExtension( files[j].FileName))
                    {
                        case ".pdf":
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            break;
                        default:
                            return BadRequest(new MessageDto { Message = Messages.FileExtension(lang) });
                    }
                }
                var request = requestRepo.AddRequest(requestDto);
                var folder = Path.Combine(serverPath, "MemberPortalApp", requestDto.Member_id.ToString(), "Approvals", request.ID.ToString());

                for (int i = 0; i < files.Count; i++)
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
                }



                return Ok(request);

            }
            return BadRequest(ModelState);


        }

        #endregion

        #region RequestByMemberId
        [HttpGet("MemberId")]

        public IActionResult GetbyMemberId(string lang ,[Required] int memberId, [FromQuery] string? startDate, [FromQuery] string? endDate, [FromQuery] string[]? status, [FromQuery] string[]? providers, [FromQuery] int startpage = 1, [FromQuery] int pageSize = 10)
        {
            if (ModelState.IsValid)
            {

                var requests = requestRepo.GetRequestsByMemberId(memberId);
                var reqDto = new List<RequestDetailsForMemberDTO>();
                var memberExist = memberRepo.MemberExists(memberId);

                if (!memberExist)
                {
                    return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (requests is null)
                {
                    return NotFound(new MessageDto { Message =Messages.RequestNotFound(lang) });

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
                    requests = requests.Where(p => p.Provider.Provider_name_en.Contains(provider));
                    }
                   

                }
                if (endDate is not null || startDate is not null)
                {

                    DateTime startDat = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime endDat = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    requests = requests
                         .AsEnumerable().Where(entity =>
                               DateTime.TryParseExact(entity.created_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var entityDate) &&
                               entityDate >= startDat &&
                               entityDate <= endDat)
                        .AsQueryable();
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

                var medicalDto = new
                {

                    TotalCount = totalProviders,
                    PageNumber = startpage,
                    PageSize = pageSize,
                    Requests = reqDto,


                };

                return Ok(medicalDto);
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region RequestByRequestId
        [HttpGet("RequestId")]

        public async Task<IActionResult> GetResultByID([Required] int id , string lang)
        {
            if (ModelState.IsValid)
            {
                var request = await requestRepo.GetById(id);
                if (request is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RequestNotFound(lang)});
                }
              
                string[] fileNames = Directory.GetFiles(request.Folder_path);
                List<string> fileNameList = fileNames.ToList();
                var reqDto = new RequestDetailsDTO
                {
                    Id = request.ID,
                    ApprovalId = request.Approval_id,
                    ProviderName = request.Provider?.Provider_name_en,
                    ProviderId = request.Provider_id,
                    Approval = null,
                    Notes = request.Notes,
                    FolderPath = fileNameList


                };

              

                return Ok(reqDto);

            }
            return BadRequest(ModelState);
        }
        #endregion
     
    }
}
