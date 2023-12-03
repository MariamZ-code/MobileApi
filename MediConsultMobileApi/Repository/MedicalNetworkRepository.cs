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
        public IQueryable<MedicalNetwork> GetAll(string? providerName,  string? category)
        {
            var medicalResult =  dbContext.medicalNetworks.AsNoTracking().AsQueryable();
            if (providerName is not null)
            {
                medicalResult = medicalResult.Where(x => x.Provider_name.Contains(providerName));
            }
            if (category is not null)
            {
                medicalResult = medicalResult.Where(x => x.Category.Contains(category));
            }
           

            return medicalResult;
        }
    }
}
