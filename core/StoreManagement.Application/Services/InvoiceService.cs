using AutoMapper;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository<Invoice> _InvoiceRepository;
        private readonly ITableRepository<Table> _TableRepository;
        private readonly IPaymentTypeRepository<PaymentType> _PaymentTypeRepository;

        public InvoiceService(IInvoiceRepository<Invoice> invoiceRepository,
                              IMapper mapper,
                              ITableRepository<Table> tableRepository,
                              IPaymentTypeRepository<PaymentType> paymentTypeRepository)
        {
            _mapper = mapper;
            _InvoiceRepository = invoiceRepository;
            _TableRepository = tableRepository;
            _PaymentTypeRepository = paymentTypeRepository;
        }

        public async Task<bool> Accept(int id)
        {
            var result = await _InvoiceRepository.GetByIdAsync(id);
            var table = await _TableRepository.GetByIdAsync(result.Order.IdTable);
            result.Status = true;
            table.Status = false;
            result = await _InvoiceRepository.UpdateAsync(id, result);
            await _TableRepository.UpdateAsync(table.Id, table);
            return true;
        }

        public async Task<InvoiceDTO> CreateAsync(InvoiceDTO invoiceDTO)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDTO);
            var created = await _InvoiceRepository.CreateAsync(invoice);
            return _mapper.Map<InvoiceDTO>(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _InvoiceRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PaginationResult<List<InvoiceResponse>>> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string sortCol = "", bool ascSort = true, bool filter = false, bool status = false)
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);

            
            var list = await _InvoiceRepository.GetAllByIdStoreAsync(idStore, sortCol, ascSort);
            if (filter)
            {
                list = list.Where(x => x.Status == status).ToList();
            }
            var count = list.Count();
            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();
            var listInvoices = _mapper.Map<List<InvoiceResponse>>(list);
            foreach(var item in listInvoices) {
                Table table = await _TableRepository.GetByIdAsync(item.Order.IdTable);
                item.TableName = table.Name;
            }
            return PaginationResult<List<InvoiceResponse>>.Create(listInvoices, _currentPage, _pageSize, count);
        }

        public async Task<InvoiceResponse> GetByIdAsync(int id)
        {
            var result = await _InvoiceRepository.GetByIdAsync(id);
            return _mapper.Map<InvoiceResponse>(result);
        }

        public async Task<int> GetCountAsync(int idStore)
        {
            var count = await _InvoiceRepository.GetCountAsync(idStore);
            return count;
        }

        public async Task<InvoiceDTO> UpdateAsync(int id, InvoiceDTO invoiceDTO)
        {
            var invoiceUpdate = _mapper.Map<Invoice>(invoiceDTO);
            var update = await _InvoiceRepository.UpdateAsync(id,invoiceUpdate);
            return _mapper.Map<InvoiceDTO>(update);
        }
    }
}
