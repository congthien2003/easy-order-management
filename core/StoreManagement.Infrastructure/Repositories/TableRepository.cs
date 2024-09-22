using StoreManagement.Infrastructure.Data;
using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StoreManagement.Domain.IRepositories;

namespace StoreManagement.Infrastructure.Repositories
{
    public class TableRepository : ITableRepository<Table>
    {
        private readonly DataContext _dataContext;

        public TableRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        public async Task<Table> CreateAsync(Table table)
        {
            var store = await _dataContext.Stores.FirstOrDefaultAsync(x => x.Id == table.IdStore && x.IsDeleted == false);
            if (store == null)
            {
                throw new NullReferenceException("Cửa hàng không tồn tại");
            }
            var count = _dataContext.Tables.Where(x => x.IdStore == table.IdStore).Count();
            table.Name = "Bàn " + (count + 1);
            var newTable = await _dataContext.Tables.AddAsync(table);
            
            await _dataContext.SaveChangesAsync();
            return newTable.Entity;
        }
        public async Task<Table> UpdateAsync(int id, Table table, bool incluDeleted = false)
        {
            var tableUpdate = await _dataContext.Tables.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (tableUpdate == null)
            {
                throw new KeyNotFoundException("Bàn không tồn tại");
            }
            tableUpdate.Status = table.Status;
            _dataContext.Tables.Update(tableUpdate);
            await _dataContext.SaveChangesAsync();
            return tableUpdate;
        }
        public async Task<Table> DeleteAsync(int id, bool incluDeleted = false)
        {
            var table = await _dataContext.Tables.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (table == null)
            {
                throw new KeyNotFoundException("Bàn không tồn tại");
            }
            _dataContext.Remove(table);
            await _dataContext.SaveChangesAsync();
            return table;
        }
        public async Task<Table> GetByIdAsync(int id, bool incluDeleted = false)
        {
            var table = await _dataContext.Tables.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == incluDeleted);
            if (table == null)
            {
                throw new KeyNotFoundException("Bàn  không tồn tại");
            }
            return table;
        }

        public Task<List<Table>> GetAllByIdStore(int id, bool incluDeleted = false)
        {
            var listTable = _dataContext.Tables.Where(x =>x.IdStore == id).AsQueryable();
            if (!incluDeleted)
            {
                listTable = listTable.Where(t => t.IsDeleted == incluDeleted);
            }
            var list = listTable.ToListAsync();
            return list;
        }
        public Expression<Func<Table, object>> GetSortColumnExpression(string sortColumn)
        {
            switch (sortColumn)
            {
                default:
                    return x => x.Id;
            }
        }
        public async Task<int> GetCountAsync(int id,bool incluDeleted = false)
        {
            var table = _dataContext.Tables.Where(x => x.IdStore == id).AsQueryable();
            if (!incluDeleted)
            {
                table = table.Where(t => t.IsDeleted == incluDeleted);
            }
            var searchTable = await table.ToListAsync();
            return searchTable.Count();
        }
    }
}
