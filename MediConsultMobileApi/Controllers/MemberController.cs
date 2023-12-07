using Azure.Core;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepo;
        private readonly IValidation validation;

        public MemberController(IMemberRepository memberRepo, IValidation validation)
        {
            this.memberRepo = memberRepo;
            this.validation = validation;
        }

        #region MemberById
        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int id)
        {

            if (ModelState.IsValid)
            {
                var member = await memberRepo.GetByID(id); // member 
                var msg = new MessageDto();

                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = "User Not found " });

                }
                if (member.program_name is null)
                {
                    msg.Message = "User in Archive";
                    return BadRequest(msg);
                }
                if (member.member_status == "Deactivated")
                {
                    msg.Message = "User is Deactivated";

                    return BadRequest(msg);

                }
                if (member.member_status == "Hold")
                {
                    msg.Message = "User is Hold";
                    return BadRequest(msg);


                }
                return Ok(member);
            }
            return BadRequest(ModelState);
        }

        #endregion

        #region MemberFamily
        [HttpGet("Family")]
        public async Task<IActionResult> MemberFamily([Required] int memberId)
        {
            if (ModelState.IsValid)
            {
                var families = await memberRepo.MemberFamily(memberId);

                var famDto = new List<MemberFamilyDTO>();
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = "Member ID Not Found " });
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
                        MemberStatus = families[i].member_status

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
        public async Task<IActionResult> MemberDetails([Required] int memberId)
        {
            if (ModelState.IsValid)
            {
                var member = await memberRepo.MemberDetails(memberId);
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = "Member ID Not Found " });
                }

                var memberDTo = new MemberDetailsDTO
                {
                    member_id = member.member_id,
                    member_name = member.member_name,
                    member_gender = member.member_gender,
                    email = member.email,
                    member_nid = member.member_nid,
                    member_photo = member.member_photo,
                    mobile = member.mobile
                };
                return Ok(memberDTo);
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region UpdateMember
        [HttpPut]
        public async Task<IActionResult> UpdateMember([FromForm] UpdateMemberDTO memberDTO, [Required] int id)
        {
            if (ModelState.IsValid)
            {
                var result = await memberRepo.MemberDetails(id);
                var memberExists = memberRepo.MemberExists(id);

                string[] validExtensions = { ".jpg", ".jpeg", ".png" };
                const long maxSizeBytes = 5 * 1024 * 1024;

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = "Member ID Not Found " });
                }
                var existingMemberWithSameMobile =  memberRepo.GetMemberByMobile(memberDTO.Mobile);
                var existingMemberWithSameEmail =  memberRepo.GetMemberByEmail(memberDTO.Email);
                var existingMemberWithSameNationalId =  memberRepo.GetMemberByNationalId(memberDTO.SSN);


                if (memberDTO.Email is not null)
                {

                    if (existingMemberWithSameEmail != null && existingMemberWithSameEmail.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = "Email already exists for another member." });
                    }
                    if (!validation.IsValidEmail(memberDTO.Email))
                    {
                        return BadRequest(new MessageDto { Message = "Email is not valid." });
                    }

                }

                if (memberDTO.Mobile is not null)
                {
                    if (!long.TryParse(memberDTO.Mobile, out _))
                    {
                        return BadRequest(new MessageDto { Message = "Mobile must be number" });

                    }
                    if (!memberDTO.Mobile.StartsWith("01"))
                    {
                        return BadRequest(new MessageDto { Message = "Mobile Number must start with 01" });
                    }
                    if (memberDTO.Mobile.Length != 11)
                    {
                        return BadRequest(new MessageDto { Message = "Mobile Number must be 11 number" });
                    }

                    if (existingMemberWithSameMobile != null && existingMemberWithSameMobile.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = "Mobile number already exists for another member." });
                    }
                    //if (memberRepo.PhoneExists(memberDTO.Mobile))
                    //{
                    //    return BadRequest(new MessageDto { Message = "Mobile number already Exists" });

                    //}


                }



                if (memberDTO.SSN is not null)
                {

                    if (!long.TryParse(memberDTO.SSN, out _))
                    {
                        return BadRequest(new MessageDto { Message = "National Id must be number" });

                    }
                    if (memberDTO.SSN.Length != 14)
                    {
                        return BadRequest(new MessageDto { Message = "National Id must be 14 number" });

                    }
                    if (existingMemberWithSameNationalId != null && existingMemberWithSameNationalId.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = "National Id already exists for another member." });
                    }
                }


                if (memberDTO.Photo is not null)
                {

                    if (memberDTO.Photo is null || memberDTO.Photo.Length == 0)
                    {
                        return BadRequest("No file uploaded.");
                    }
                    if (memberDTO.Photo.Length > maxSizeBytes)
                    {
                        return BadRequest($"File size must be less than 5 MB.");
                    }
                    var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                    // serverPath/Member/id
                    var folder = Path.Combine(serverPath, "Members", result.member_id.ToString());


                    foreach (var extension in validExtensions)
                    {
                        if (!memberDTO.Photo.FileName.EndsWith(extension))
                        {
                            return BadRequest(new MessageDto { Message = "Folder Path must end with extension .jpg, .png, or .jpeg" });

                        }
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }


                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + memberDTO.Photo.FileName;

                        string filePath = Path.Combine(folder, uniqueFileName);


                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            await memberDTO.Photo.CopyToAsync(stream);
                        }

                    }
                }
                memberRepo.UpdateMember(memberDTO, id);

                memberRepo.SaveDatabase();
                return Ok(new MessageDto { Message = "Done" });


            }
            return BadRequest(ModelState);
        }
        #endregion

    }
}
