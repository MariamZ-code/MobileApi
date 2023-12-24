using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class RefundRepository : IRefundRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RefundRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Refund AddRefund(RefundDTO refunddto)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var refund = new Refund
            {

                refund_id= refunddto.refund_id,
                notes = refunddto.notes, 
                member_id = refunddto.member_id,
                refund_date = refunddto.refund_date,
                total_amount = refunddto.amount
                
                
            };
            dbContext.Add(refund);

            dbContext.SaveChanges();

            var folder = Path.Combine(serverPath, "MemberPortalApp", refunddto.member_id.ToString(), "Refund", refund.id.ToString());


            refund.folder_path = folder;

            dbContext.SaveChanges();

            return refund;


        }

        #region RefundByMemberId
        public IQueryable<Refund> GetRefundByMemberId(int memberId)
        {

            var refund = dbContext.Refunds.Where(r => r.member_id == memberId).AsNoTracking().AsQueryable();

            return refund;


        }

        #endregion


        #region RefundByRefundId
        public async Task<Refund> GetById(int RefundId)
        {
            return await dbContext.Refunds.FirstOrDefaultAsync(r => r.id == RefundId);
        }
        #endregion

        #region RefundExists
        public async Task<bool> RefundExists(int? refundId)
        {
            return await dbContext.ClientPriceLists.AnyAsync(p => p.id == refundId);
        }

        #endregion
    }
}
