using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;

namespace LMS.Service.implementation
{
    public class CouponsService : ICouponsService
    {
        private readonly ICouponsRepository couponsRepository;

        public CouponsService(ICouponsRepository couponsRepository)
        {
            this.couponsRepository = couponsRepository;
        }
        public async Task<string> AddCoupons(List<Coupons> coupons)
        {

            await couponsRepository.AddRangeAsync(coupons);
            return "Created";
        }

        public async Task<bool> Delete(int Id)
        {
            using var transaction = couponsRepository.BeginTransaction();
            try
            {
                var coupon = await couponsRepository.GetByIdAsync(Id);

                if (coupon == null)
                    return false;
                await couponsRepository.DeleteAsync(coupon);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAllExpiredCoupons()
        {
            var List = await couponsRepository.GetAllExpiredAsync();
            using var transaction = couponsRepository.BeginTransaction();
            try
            {
                await couponsRepository.DeleteRangeAsync(List);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<string> DeleteCoupons(List<Coupons> coupons)
        {
            using var transaction = couponsRepository.BeginTransaction();
            try
            {
                await couponsRepository.DeleteRangeAsync(coupons);
                transaction.Commit();
                return "Deleted";
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Coupons>> GetAllCoupons(bool IsActive)
        {
            return await couponsRepository.GetAllAsync(IsActive);
        }

        public IQueryable<Coupons> GetAllCouponsQueryable()
        {
            return couponsRepository.GetTableNoTracking().AsQueryable();
        }

        public async Task<List<Coupons>> GetAllExpiredAsync()
        {
            return await couponsRepository.GetAllExpiredAsync();
        }

        public async Task<Coupons> GetByCode(string code)
        {
            return await couponsRepository.GetByCode(code);
        }

        public async Task<Coupons> GetByID(int id)
        {
            return await couponsRepository.GetByIdAsync(id);
        }

        public async Task<int> GetCouponsCount()
        {
            return await couponsRepository.CountAsync();
        }

        public async Task<string> Update(Coupons coupon)
        {
            await couponsRepository.UpdateAsync(coupon);
            return "Updated";
        }

        public async Task<string> UpdateCoupons(List<Coupons> coupons)
        {
            await couponsRepository.UpdateRangeAsync(coupons);
            return "Updated";

        }
    }
}
