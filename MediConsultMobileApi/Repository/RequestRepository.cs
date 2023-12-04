using Google.Api.Gax.ResourceNames;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MediConsultMobileApi.Repository
{

    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RequestRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region AddNewRequest
        public Request AddRequest(RequestDTO requestDto)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            //Request id not provider Id
            var folder = Path.Combine(serverPath, "MemberPortalApp", requestDto.Member_id.ToString(), "Approvals", requestDto.Provider_id.ToString());

            var request = new Request
            {
                Provider_id = requestDto.Provider_id,
                Notes = requestDto.Notes,
                Member_id = requestDto.Member_id,
                Folder_path = folder,


            };
            dbContext.Add(request);
            dbContext.SaveChanges();

            return request;


        }

        #endregion


        #region RequestByMemberId
        public IQueryable<Request> GetRequestsByMemberId(int memberId)
        {

            var members = dbContext.Requests.Include(p => p.Provider).Where(r => r.Member_id == memberId).AsNoTracking().AsQueryable();



            return members;


        }

        #endregion


        #region RequestBy RequestId
        public async Task<Request> GetById(int RequestId)
        {
            return await dbContext.Requests.FirstOrDefaultAsync(r => r.ID == RequestId);
        }
        #endregion


    }  
    
}
