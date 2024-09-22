using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.IRepositories
{
    public interface IPaymentTypeRepository<TPaymentType> where TPaymentType : PaymentType
    {
        Task<TPaymentType> CreateAsync(TPaymentType paymentType);
        Task<TPaymentType> UpdateAsync(int id, TPaymentType paymentType, bool incluDeleted = false);
        Task<TPaymentType> DeleteAsync(int id, bool incluDeleted = false);
        Task<TPaymentType> GetByIdAsync(int id, bool incluDeleted = false);
        Task<List<TPaymentType>> GetByNameAsync(int idStore, string name, bool incluDeleted = false);
        Task<int> GetCountAsync(int idStore, string searchTerm = "", bool incluDeleted = false);
        Task<List<TPaymentType>> GetAllByIdStore(int idStore, int currentPage = 1, int pageSize = 5, string searchTerm = "", string sortCol = "", bool ascSort = true, bool incluDeleted = false);
    }
}
