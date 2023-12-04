﻿using MediConsultMobileApi.DTO;
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
               
                var famDto= new MemberFamilyDTO();
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = "Member ID Not Found " });
                }
                for (int i = 0; i < families.Count; i++) 
                {
                    famDto.MemberId = families[i].member_id;
                    famDto.MemberGender = families[i].member_gender;
                    famDto.MemberName = families[i].member_name;
                    famDto.MemberBirthday = families[i].member_birthday;
                    famDto.MemberLevel = families[i].member_level;
                }
                    return Ok(famDto);




            }
            return BadRequest(ModelState);
        }
        #endregion

    }
}
