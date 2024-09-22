using AutoMapper;
using StoreManagement.Application.Common;
using StoreManagement.Application.DTOs.Request;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Domain.IRepositories;
using StoreManagement.Domain.Models;

namespace StoreManagement.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository<Table> _tableRepository;
        private readonly IMapper _mapper;

        public TableService(IMapper mapper, ITableRepository<Table> tableRepository) 
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }
        public async Task<TableDTO> CreateAsync(TableDTO tableDTO)
        {
            var table = _mapper.Map<Table>(tableDTO);
            var tableCreated = await _tableRepository.CreateAsync(table);
            return _mapper.Map<TableDTO>(tableCreated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _tableRepository.DeleteAsync(id);
            return true;
        }
        public async Task<TableDTO> GetByIdAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            return _mapper.Map<TableDTO>(table);
        }

        public async Task<PaginationResult<List<TableDTO>>> GetAllByIdStore(int id, string currentPage = "1", string pageSize = "9", bool filter = false, bool status = false)
        {
            int _currentPage = int.Parse(currentPage);
            int _pageSize = int.Parse(pageSize);

            var list = await _tableRepository.GetAllByIdStore(id);

            if (filter)
            {
                list = list.Where(x => x.Status == status).ToList();
            }
            var count = list.Count();
            list = list.Skip(_currentPage * _pageSize - _pageSize).Take(_pageSize).ToList();

            var listTable = _mapper.Map<List<TableDTO>>(list);
            return PaginationResult<List<TableDTO>>.Create(listTable, _currentPage, _pageSize, count);
        }
        public async Task<TableDTO> UpdateAsync(int id, TableDTO tableDTO)
        {
            var tableUpdate = _mapper.Map<Table>(tableDTO);
            var update = await _tableRepository.UpdateAsync(id, tableUpdate);
            return _mapper.Map<TableDTO>(update);
        }
        public async Task<int> GetCountAsync(int id)
        {
            var count = await _tableRepository.GetCountAsync(id);
            return count;
        }
    }
}
