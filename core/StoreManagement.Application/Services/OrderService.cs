using AutoMapper;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.DTOs.Response;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Services
{
    public class OrderService : IOrderSerivce
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository<Order> _orderRepository;
        private readonly ITableRepository<Table> _tableRepository;

        public OrderService(IMapper mapper, IOrderRepository<Order> orderRepository, ITableRepository<Table> tableRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _tableRepository = tableRepository;
        }

        public async Task<OrderDTO> AcceptOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            var table = await _tableRepository.GetByIdAsync(order.IdTable);
            order.Status = true;
            table.Status = false;
            order = await _orderRepository.UpdateAsync(id, order);
            await _tableRepository.UpdateAsync(table.Id, table);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            var orderCreated = await _orderRepository.CreateAsync(order);
            return _mapper.Map<OrderDTO>(orderCreated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PaginationResult<List<OrderResponse>>> GetAllByIdStoreAsync(int idStore, string currentPage = "1", string pageSize = "5", string sortCol = "", bool ascSort = true, bool filter = false, bool status = false)
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);

            var list = await _orderRepository.GetAllByIdStoreAsync(idStore, sortCol, ascSort);
            if (filter)
            {
                list = list.Where(x => x.Status == status).ToList();
            }
            var count = list.Count();
            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();

            var listOrders = _mapper.Map<List<OrderResponse>>(list);
            return PaginationResult<List<OrderResponse>>.Create(listOrders, _currentPage, _pageSize, count);
        }

        public async Task<OrderResponse> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<int> GetCountAsync(int idStore, string searchTerm = "")
        {
            var count = await _orderRepository.GetCountAsync(idStore, searchTerm);
            return count;
        }

        public async Task<OrderDTO> UpdateAsync(int id, OrderDTO orderDTO)
        {
            var orderUpdate = _mapper.Map<Order>(orderDTO);
            var update = await _orderRepository.UpdateAsync(id, orderUpdate);
            return _mapper.Map<OrderDTO>(update);
        }
    }
}
