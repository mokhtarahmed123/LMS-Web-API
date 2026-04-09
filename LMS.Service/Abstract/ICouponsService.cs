using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface ICouponsService
    {
        public Task<string> AddCoupons(List<Coupons> coupons);
        public Task<string> UpdateCoupons(List<Coupons> coupons);
        public Task<string> DeleteCoupons(List<Coupons> coupons);
        public Task<string> Update(Coupons coupon);
        public Task<bool> Delete(int Id);
        public Task<int> GetCouponsCount();
        public Task<List<Coupons>> GetAllCoupons(bool IsActive);
        public Task<bool> DeleteAllExpiredCoupons();
        public Task<Coupons> GetByID(int id);
        Task<List<Coupons>> GetAllExpiredAsync();
        Task<Coupons> GetByCode(string code);
        IQueryable<Coupons> GetAllCouponsQueryable();


    }
}
