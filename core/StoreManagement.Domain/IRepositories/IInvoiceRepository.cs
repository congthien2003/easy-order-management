using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.IRepositories
{
    public interface IInvoiceRepository<TInvoice> where TInvoice : Invoice
    {
        public Task<TInvoice> CreateAsync(TInvoice invoice);
        public Task<TInvoice> UpdateAsync(int id, TInvoice invoice, bool incluDeleted = false);
        public Task<TInvoice> DeleteAsync(int id, bool incluDeleted = false);
        public Task<TInvoice> GetByIdAsync(int id, bool incluDeleted = false);
        public Task<List<TInvoice>> GetAllByIdStoreAsync(int idStore, string sortCol = "", bool ascSort = true, bool incluDeleted = false);
        Task<int> GetCountAsync(int idStore, bool incluDeleted = false);
    }
}
