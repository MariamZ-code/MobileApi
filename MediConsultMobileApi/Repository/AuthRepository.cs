﻿using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Net;

using MediConsultMobileApi.Language;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace MediConsultMobileApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMemberRepository memberRepo;

        public AuthRepository(ApplicationDbContext dbContext , IMemberRepository memberRepo)
        {
            this.dbContext = dbContext;
            this.memberRepo = memberRepo;
        }


        #region GetClientBranchMemberById
        public ClientBranchMember  GetById(int memberId)
        {
            return dbContext.clientBranchMembers.FirstOrDefault(m => m.member_id == memberId);
        }

        #endregion


        #region Registeration

        public async void Registeration(RegisterUserDto userDto, int memberId)
        {
            var member = GetById(memberId);
            var createDateAndGender = memberRepo.CreateDateAndGender(userDto.NationalId);

            member.mobile = userDto.Mobile;
            member.member_nid = userDto.NationalId;
            member.member_birthday = createDateAndGender.date;
            member.member_gender = createDateAndGender.gender;

            var memberPassword = ResetPassword(memberId);
            memberPassword.the_password = userDto.Password;

            dbContext.clientBranchMembers.Update(member);
            dbContext.logins.Update(memberPassword);

        }
        #endregion

        #region Login
        public async Task<MessageDto> Login(LoginUserDto userDto , string lang)
        {
            var authDto = new MessageDto();
            var user = await dbContext.logins.FirstOrDefaultAsync(u => u.member_id == int.Parse(userDto.Id) && u.the_password == userDto.Password);
         
            if (user is null)
            {
               authDto.Message = Messages.PasswordAndIdIncorrect(lang);
               
                return authDto;
            }
          
            if (user.is_enabled == 0)
            {
                authDto.Message =Messages.AccountDisabled(lang);

                return authDto;
            }
            
            authDto.Message = Messages.LoginSuccessfully(lang);

            return authDto;
        }
        #endregion

        #region MemberExists
        public bool MemberLoginExists(int? memberId)
        {
            return dbContext.logins.Any(m => m.member_id == memberId);

        }


        #endregion

        #region ResetPassword

        public Login ResetPassword(int memberId)
        {
           return dbContext.logins.FirstOrDefault(m => m.member_id == memberId);
        }

        #endregion

        #region SendOtp

        public  void SendOtp(string otp, int memberId)
        {
            Login member = ResetPassword(memberId);
            member.Otp = otp;

            dbContext.logins.Update(member);
            dbContext.SaveChanges();

        }
        #endregion

        #region ChangePassword

        public void ChangePass(string otp,int id , ChangePasswordDTO changeDto)
        {
            var member = ResetPassword(id);

          
                member.the_password = changeDto.Password;

                dbContext.logins.Update(member);

                dbContext.SaveChanges();

        }

        #endregion

        #region SaveChange
            public  void Save()
            { 

                dbContext.SaveChanges();

            }
         #endregion

        }
}
