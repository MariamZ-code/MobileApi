using Azure.Core;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepo;
        private readonly IValidation validation;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string imageBaseUrl = "https://api.mediconsulteg.com/";
        public MemberController(IMemberRepository memberRepo, IValidation validation , IWebHostEnvironment webHostEnvironment)
        {
            this.memberRepo = memberRepo;
            this.validation = validation;
            this.webHostEnvironment = webHostEnvironment;
        }


        #region MemberById
        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int id, string lang)
        {

            if (ModelState.IsValid)
            {
                var member = await memberRepo.GetByID(id); // member 
                var msg = new MessageDto();

                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (member.program_name is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberArchive(lang) });
                }
                if (member.member_status == "Deactivated")
                {


                    return BadRequest(new MessageDto { Message = Messages.MemberDeactivated(lang) });

                }
                if (member.member_status == "Hold")
                {

                    return BadRequest(new MessageDto { Message = Messages.MemberHold(lang) });


                }


                string imageUrl(string imageName)
                {
                    if (string.IsNullOrEmpty(imageName))
                    {
                        return string.Empty;
                    }

                    if (Path.Exists(imageName))
                    {
                        string[] fileNames = Directory.GetFiles(imageName);
                        return $"{fileNames[0]}";

                    }
                    return string.Empty;
                }



                if (lang == "en")
                {
                    MemberDetailsProfileEnDTO memberEnDTo = new MemberDetailsProfileEnDTO
                    {

                        member_id = member.member_id,
                        member_name = member.member_name,
                        email = member.email,
                        room_class = member.room_class,
                        member_photo = imageUrl(member.member_photo),
                        mobile = member.mobile,
                        program_name = member.Type_Name_En,
                        member_status = member.member_status,
                        job_title = member.job_title,
                        policy_id = member.policy_id,
                        program_id = member.program_id,
                        renew_date = member.renew_date,
                        member_birthday = member.member_birthday,


                    };


                    return Ok(memberEnDTo);
                }

                MemberDetailsProfileArDTO memberArDTo = new MemberDetailsProfileArDTO
                {

                    member_id = member.member_id,
                    member_name = member.member_name,
                    email = member.email,
                    room_class = member.room_class,
                    member_photo = imageUrl(member.member_photo),
                    mobile = member.mobile,
                    program_name = member.Type_Name_Ar,
                    member_status = member.member_status,
                    job_title = member.job_title,
                    policy_id = member.policy_id,
                    program_id = member.program_id,
                    renew_date = member.renew_date,
                    member_birthday = member.member_birthday,

                };
                return Ok(memberArDTo);
            }
            return BadRequest(ModelState);
        }

        #endregion

        #region MemberFamily
        [HttpGet("Family")]
        public async Task<IActionResult> MemberFamily([Required] int memberId, string lang)
        {
            if (ModelState.IsValid)
            {
                var families = await memberRepo.MemberFamily(memberId);

                var famDto = new List<MemberFamilyDTO>();
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                for (int i = 0; i < families.Count; i++)
                {
                    MemberFamilyDTO member = new MemberFamilyDTO
                    {

                        MemberId = families[i].member_id,
                        //male
                        MemberGender = families[i].member_gender,
                        //ahmed
                        MemberName = families[i].member_name,
                        //2022-03-17
                        MemberBirthday = families[i].member_birthday,
                        //Member
                        MemberLevel = families[i].member_level,

                        MemberStatus = families[i].member_status,

                        PhoneNumber = families[i].mobile,

                        NationalId = families[i].member_nid,

                        Photo = families[i].member_photo,


                    };
                    famDto.Add(member);
                }
                return Ok(famDto);




            }
            return BadRequest(ModelState);
        }
        #endregion


        #region MemberDetails
        [HttpGet("MemberDetails")]
        public async Task<IActionResult> MemberDetails([Required] int memberId, string lang)
        {
            if (ModelState.IsValid)
            {
                var member = memberRepo.MemberDetails(memberId);
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                string imageUrl(string imageName)
                {
                    if (string.IsNullOrEmpty(imageName))
                    {
                        return string.Empty;
                    }

                    if (Path.Exists(imageName))
                    {
                        string[] fileNames = Directory.GetFiles(imageName);
                        return $"{fileNames[0]}";

                    }
                    return string.Empty;
                }




                var memberDTo = new MemberDetailsDTO
                {

                    member_id = member.member_id,
                    member_name = member.member_name,
                    member_gender = member.member_gender,
                    email = member.email,
                    member_nid = member.member_nid,
                    member_photo =imageUrl(member.member_photo),
                    mobile = member.mobile,
                    birthDate = member.member_birthday,
                    jobTitle = member.job_title

                };
                return Ok(memberDTo);
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region UpdateMember
        [HttpPost]
        public async Task<IActionResult> UpdateMember([FromForm] UpdateMemberDTO memberDTO, [Required] int id, string lang)
        {
            if (ModelState.IsValid)
            {
                var result = memberRepo.MemberDetails(id);
                var memberExists = memberRepo.MemberExists(id);

                string[] validExtensions = { ".jpg", ".jpeg", ".png" };
                const long maxSizeBytes = 5 * 1024 * 1024;

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
               
                var existingMemberWithSameMobile = memberRepo.GetMemberByMobile(memberDTO.Mobile);
                var existingMemberWithSameEmail = memberRepo.GetMemberByEmail(memberDTO.Email);
                var existingMemberWithSameNationalId = memberRepo.GetMemberByNationalId(memberDTO.SSN);


                if (memberDTO.Email is not null)
                {

                    if (existingMemberWithSameEmail != null && existingMemberWithSameEmail.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = Messages.Emailexist(lang) });

                    }

                    if (!validation.IsValidEmail(memberDTO.Email))
                    {
                        return BadRequest(new MessageDto { Message = Messages.EmailNotValid(lang) });

                    }

                }

                if (memberDTO.Mobile is not null)
                {
                    if (!long.TryParse(memberDTO.Mobile, out _))
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumber(lang) });


                    }
                    if (!memberDTO.Mobile.StartsWith("01"))
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileStartWith(lang) });

                    }
                    if (memberDTO.Mobile.Length != 11)
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumberFormat(lang) });

                    }

                    if (existingMemberWithSameMobile != null && existingMemberWithSameMobile.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumbeExist(lang) });

                    }


                }

                var (date, gender) = memberRepo.CreateDateAndGender(memberDTO.SSN);


                if (memberDTO.SSN is not null)
                {

                    if (!long.TryParse(memberDTO.SSN, out _))
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdNumber(lang) });


                    }
                    if (memberDTO.SSN.Length != 14)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdFormat(lang) });


                    }
                    if (existingMemberWithSameNationalId != null && existingMemberWithSameNationalId.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdExist(lang) });

                    }

                    if (!memberRepo.IsValidDate(date))
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdInvalid(lang) });


                    }
                }


                if (memberDTO.Photo is not null)
                {

                    var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                    var folder = Path.Combine(webHostEnvironment.WebRootPath, "Members", result.member_id.ToString(), memberDTO.Photo.FileName);
                    var folder2 = Path.Combine(serverPath, "Members", result.member_id.ToString());




                    if (memberDTO.Photo.Length == 0)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                    }
                    if (memberDTO.Photo.Length >= maxSizeBytes)
                    {
                        return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });

                    }

                    switch (Path.GetExtension(memberDTO.Photo.FileName))
                    {
                        case ".pdf":
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            break;
                        default:
                            return BadRequest(new MessageDto { Message = Messages.FileExtension(lang) });

                    }
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + memberDTO.Photo.FileName;

                    string filePath = Path.Combine(folder, uniqueFileName);
                    string filePath2 = Path.Combine(folder2, uniqueFileName);

                    //using (FileStream fileStream = System.IO.File.Create(filePath))
                    //{
                    //    await memberDTO.Photo.CopyToAsync(fileStream);
                    //    fileStream.Flush();

                    //}
                    using (FileStream stream = new FileStream(filePath2, FileMode.Create))
                    {
                        await memberDTO.Photo.CopyToAsync(stream);
                    }
                  
                }


                memberRepo.UpdateMember(memberDTO, id);
                memberRepo.SaveDatabase();

                return Ok(new MessageDto { Message = Messages.MemberChange(lang) });
            }
            return BadRequest(ModelState);
        }




        #endregion


    }
}
