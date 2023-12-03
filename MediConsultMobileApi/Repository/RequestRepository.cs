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
        public  Request AddRequest(RequestDTO requestDto)
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

        public  List<Request> GetRequestsByMemberId(int id)
        {
            var member =  dbContext.Requests.Where(r => r.Member_id == id).AsNoTracking().ToList();
            return member;
            
            
        }


    }  
    
}
