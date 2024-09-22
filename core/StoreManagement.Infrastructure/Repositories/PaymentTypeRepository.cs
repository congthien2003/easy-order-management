using StoreManagement.Infrastructure.Data;
using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StoreManagement.Domain.IRepositories;

namespace StoreManagement.Infrastructure.Repositories
{
    public class PaymentTypeRepository : IPaymentTypeRepository<PaymentType>
    {
        private readonly DataContext _dataContext;

        public PaymentTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<PaymentType> CreateAsync(PaymentType paymentType)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == paymentType.IdStore);
            if (store == null)
            {
                throw new InvalidOperationException("Cửa hàng không tồn tại");
            }
            var exitsPayment = await _dataContext.PaymentTypes.FirstOrDefaultAsync(x => x.Name.Contains(paymentType.Name) && x.IsDeleted == false && x.IdStore == store.Id);
            if (exitsPayment != null)
            {
                throw new InvalidOperationException("Thể loại thanh toán này đã tồn tại");
            }
            var newPayment = await _dataContext.PaymentTypes.AddAsync(paymentType);
            await _dataContext.SaveChangesAsync();
            return newPayment.Entity;
        }

        public async Task<PaymentType> DeleteAsync(int id, bool incluDeleted = false)
        {
            var payment = await _dataContext.PaymentTypes.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (payment == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thể loại thanh toán");
            }
            _dataContext.PaymentTypes.Remove(payment);
            await _dataContext.SaveChangesAsync();
            return payment;
        }

        public Task<List<PaymentType>> GetAllByIdStore(int idStore, int currentPage = 1, int pageSize = 5, string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false)
        {
            var payment = _dataContext.PaymentTypes.Where(x => x.IdStore == idStore && x.IsDeleted == incluDeleted).AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                payment = payment.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                payment = payment.Where(t => t.IsDeleted == incluDeleted);
            }
            if (!string.IsNullOrEmpty(sortCol))
            {
                if (ascSort)
                {
                    payment = payment.OrderByDescending(GetSortColumnExpression(sortCol.ToLower()));
                }
                else
                {
                    payment = payment.OrderBy(GetSortColumnExpression(sortCol.ToLower()));

                }
            }
            var list = payment.Skip(currentPage * pageSize - pageSize).Take(pageSize).ToListAsync();
            return list;
        }
        public Expression<Func<PaymentType, object>> GetSortColumnExpression(string sortColumn)
        {
            switch (sortColumn)
            {
                case "name":
                    return x => x.Name;
                default:
                    return x => x.Id;
            }
        }
        public async Task<PaymentType> GetByIdAsync(int id, bool incluDeleted = false)
        {
            var payment = await _dataContext.PaymentTypes.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (payment == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thể loại thanh toán");
            }
            return payment;
        }

        public async Task<List<PaymentType>> GetByNameAsync(int idStore, string name, bool incluDeleted = false)
        {
            var Listpayment = await _dataContext.PaymentTypes.Where(x => x.Name.Contains(name) &&x.IsDeleted == incluDeleted && x.IdStore == idStore).ToListAsync();
            if(Listpayment.Count == 0)
            {
                throw new NullReferenceException("Thể loại thanh toán không tồn tại");
            }
            return Listpayment;
        }

        public async Task<int> GetCountAsync(int idStore, string searchTerm = "", bool incluDeleted = false)
        {
            var payment = _dataContext.PaymentTypes.Where(x=>x.IdStore == idStore && x.IsDeleted == incluDeleted).AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                payment = payment.Where(t => t.Name.Contains(searchTerm));
            }
            if (!incluDeleted)
            {
                payment = payment.Where(t => t.IsDeleted == incluDeleted);
            }
            var searchFood = await payment.ToListAsync();
            return searchFood.Count();
        }

        public async Task<PaymentType> UpdateAsync(int id, PaymentType paymentType, bool incluDeleted = false)
        {
            var paymentUpdate = await _dataContext.PaymentTypes.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if(paymentUpdate == null)
            {
                throw new KeyNotFoundException("Thể loại thanh toán không tồn tại");
            }
            paymentUpdate.Name = paymentType.Name;
            _dataContext.PaymentTypes.Update(paymentType);
            await _dataContext.SaveChangesAsync();
            return paymentUpdate;
        }
    }
}
