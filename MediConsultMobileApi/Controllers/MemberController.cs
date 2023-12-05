using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepo;
        


        public MemberController(IMemberRepository memberRepo)
        {
            this.memberRepo = memberRepo;
           
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
        public async Task<IActionResult> MemberFamily([Required]int memberId)
        {
            if (ModelState.IsValid)
            {
                var families = await memberRepo.MemberFamily(memberId);
               
                var famDto= new List<MemberFamilyDTO>();
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = "Member ID Not Found " });
                }
                for (int i = 0; i < families.Count; i++)
                {
                    MemberFamilyDTO member = new MemberFamilyDTO { 
                    
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
        public async Task<IActionResult> MemberDetails([Required]int memberId)
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
    }
}
