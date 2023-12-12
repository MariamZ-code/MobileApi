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

            var request = new Request
            {
              
                Provider_id = requestDto.Provider_id,
                Notes = requestDto.Notes,
                Member_id = requestDto.Member_id,
            };

            dbContext.Add(request);

            dbContext.SaveChanges();

            var folder = Path.Combine(serverPath, "MemberPortalApp", request.Member_id.ToString(), "Approvals", request.ID.ToString());

            request.Folder_path = folder;

            dbContext.SaveChanges();

            return request;


        }

        #endregion


        #region RequestByMemberId
        public IQueryable<Request> GetRequestsByMemberId(int memberId)
        {

            var requests = dbContext.Requests.Include(p => p.Provider).Where(r => r.Member_id == memberId).AsNoTracking().AsQueryable();
            
            return requests;
            

        }

        #endregion


        #region RequestByRequestId
        public async Task<Request> GetById(int RequestId)
        {
            return await dbContext.Requests.FirstOrDefaultAsync(r => r.ID == RequestId);
        }
        #endregion




    }  
    
}
