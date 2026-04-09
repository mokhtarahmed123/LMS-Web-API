using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface ICouponsRepository : IGenericRepositoryAsync<Coupons>
    {
        public Task<int> CountAsync();
        public Task<List<Coupons>> GetAllAsync(bool IsActive);
        public Task<List<Coupons>> GetAllExpiredAsync();
        public Task<Coupons> GetByCode(string code);



    }
}
