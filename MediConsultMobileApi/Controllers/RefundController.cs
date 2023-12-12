using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundRepository refundRepo;
        private readonly IMemberRepository memberRepo;

        public RefundController(IRefundRepository refundRepo, IMemberRepository memberRepo)
        {
            this.refundRepo = refundRepo;
            this.memberRepo = memberRepo;
        }

        #region AddNewRefund
        [HttpPost("AddNewRefund")]
        public async Task<IActionResult> PostRefund([FromForm] RefundDTO refundDto, [FromForm] List<IFormFile> files)
        {

            const long maxSizeBytes = 5 * 1024 * 1024;
            if (ModelState.IsValid)
            {

                var memberExists = memberRepo.MemberExists(refundDto.member_id);
              
                if (refundDto.member_id is null)
                {
                    return BadRequest(new MessageDto { Message = "Enter member Id" });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = "Member Id not found" });

                }

                var serverPath = AppDomain.CurrentDomain.BaseDirectory;

                string[] validExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
                if (files.Count == 0)
                {
                    return BadRequest(new MessageDto { Message = "Please uploade File " });

                }
                for (int j = 0; j < files.Count; j++)
                {

                    if (files[j].Length == 0)
                    {
                        return BadRequest("No file uploaded.");
                    }
                    if (files[j].Length >= maxSizeBytes)
                    {
                        return BadRequest($"File size must be less than 5 MB.");
                    }
                    // image.png --0
                    switch (Path.GetExtension(files[j].FileName))
                    {
                        case ".pdf":
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            break;
                        default:
                            return BadRequest(new MessageDto { Message = "Folder Path must end with extension .jpg, .png, or .jpeg" });
                    }
                }
                var refund = refundRepo.AddRefund(refundDto);
                var folder = Path.Combine(serverPath, "MemberPortalApp", refundDto.member_id.ToString(), "Approvals", refund.id.ToString());

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



                return Ok(refund);

            }
            return BadRequest(ModelState);


        }
        #endregion

        #region RefundByMemberId
        [HttpGet("HistoryRefund")]

        public IActionResult GetbyMemberId([Required] int memberId, [FromQuery] string? startDate, [FromQuery] string? endDate, [FromQuery] string[]? status, [FromQuery] string[]? providers, [FromQuery] int startpage = 1, [FromQuery] int pageSize = 10)
        {
            if (ModelState.IsValid)
            {

                var refunds = refundRepo.GetRefundByMemberId(memberId);
                var refDto = new List<RefundDetailsForMemberDTO>();
                var memberExist = memberRepo.MemberExists(memberId);

                if (!memberExist)
                {
                    return NotFound(new MessageDto { Message = "Member Id not found" });

                }
                if (refunds is null)
                {
                    return NotFound(new MessageDto { Message = "Request not found" });

                }


                if (status != null && status.Any())
                {
                    for (int i = 0; i < status.Length; i++)
                    {
                        var sta = status[i];
                    }
                    refunds = refunds.Where(c => status.Contains(c.Status));
                }
           
                if (endDate is not null || startDate is not null)
                {

                    DateTime startDat = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime endDat = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    refunds = refunds
                         .AsEnumerable().Where(entity =>
                               DateTime.TryParseExact(entity.created_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var entityDate) &&
                               entityDate >= startDat &&
                               entityDate <= endDat)
                        .AsQueryable();
                }
                var totalProviders = refunds.Count();
                refunds = refunds.Skip((startpage - 1) * pageSize).Take(pageSize);
                foreach (var refund in refunds)
                {


                    RefundDetailsForMemberDTO refDetalisDto = new RefundDetailsForMemberDTO
                    {

                        Id = refund.id,
                        CreatedDate = refund.created_date,
                        //RefundType = refund.Provider.Provider_name_en,
                        Status = refund.Status

                    };

                    refDto.Add(refDetalisDto);
                }

                var medicalDto = new
                {

                    TotalCount = totalProviders,
                    PageNumber = startpage,
                    PageSize = pageSize,
                    Refund = refDto,


                };

                return Ok(medicalDto);
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region RefundByRefundId
        [HttpGet("RefundDetails")]

        public async Task<IActionResult> GetResultByID([Required] int id)
        {
            if (ModelState.IsValid)
            {
                var refund = await refundRepo.GetById(id);
                if (refund is null)
                {
                    return NotFound(new MessageDto { Message = $"Id  not found" });
                }

                var reqDto = new RefundDetailsDTO
                {
                    Id = refund.id,
                    RefundId = refund.refund_id,
                    Refund_date = refund.refund_date,
                    RefundType = null,
                    Notes = refund.notes


                };
                return Ok(reqDto);

            }
            return BadRequest(ModelState);
        }
        #endregion

    }
}
