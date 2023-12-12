using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRefundRepository
    {
        Refund AddRefund(RefundDTO refundDto);
        Task<Refund> GetById(int RefundId);

        IQueryable<Refund> GetRefundByMemberId(int memberId);
    }
}
