using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Repository
{
    public class MedicalNetworkRepository : IMedicalNetworkRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MedicalNetworkRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<MedicalNetwork> GetAll(string? providerName, string[]? categories)
        {
            var medicalResult =  dbContext.medicalNetworks.AsNoTracking().AsQueryable();
            if (providerName is not null)
            {
                medicalResult = medicalResult.Where(x => x.Provider_name.Contains(providerName));
            }
            if (categories != null && categories.Any())
            {
                for (int i = 0; i < categories.Length; i++)
                {
                    var status = categories[i];
                }
                medicalResult = medicalResult.Where(c => categories.Contains(c.Category));
            }
           
           

            return medicalResult;
        }
    }
}
