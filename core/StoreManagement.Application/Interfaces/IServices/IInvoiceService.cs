using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;

namespace StoreManagement.Application.Interfaces.IServices
{
    public interface IInvoiceService
    {
        public Task<InvoiceDTO> CreateAsync(InvoiceDTO invoiceDTO);
        public Task<InvoiceDTO> UpdateAsync(int id, InvoiceDTO invoiceDTO);
        public Task<bool> DeleteAsync(int id);
        public Task<InvoiceResponse> GetByIdAsync(int id);
        public Task<bool> Accept(int id);
        public Task<PaginationResult<List<InvoiceResponse>>> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string sortCol = "", bool ascSort = true, bool filter = false, bool status = false);
        Task<int> GetCountAsync(int idStore);
    }
}
