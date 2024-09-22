using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Application.Common
{
    public interface IPagination
    {
        int currentPage { get; }
        int pageSize { get; }
        int totalPage { get; }
        int totalRecords { get; }
        bool hasPrevPage { get; }
        bool hasNextPage { get; }
    }

    public class Pagination : IPagination
    {
        public int currentPage { get; private set; } = 1;

        public int pageSize { get; private set; } = 0;

        public int totalPage { get; private set; } = 0;

        public int totalRecords { get; private set; } = 0;

        public bool hasPrevPage { get; private set; } = false;

        public bool hasNextPage { get; private set; } = false;

        public Pagination(int currentPage, int pageSize, int totalRecords)
        {
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            totalPage = (int)Math.Ceiling((double)totalRecords / pageSize);
            this.totalRecords = totalRecords;
            this.hasPrevPage = this.currentPage > 1 && this.currentPage <= totalPage;
            this.hasNextPage = this.currentPage < totalPage;
        }
    }

    public interface IPagenationResult<T>
    {
        T List { get; }

        IPagination pagination { get; }

    }
    public class PaginationResult<T> : IPagenationResult<T>
    {
        public T List { get; private set; }

        public IPagination pagination { get; private set; }

        protected PaginationResult(T data, int currentPage, int pageSize, int totalRecords)
        {
            List = data;
            pagination = new Pagination(currentPage, pageSize, totalRecords);
        }

        public static PaginationResult<T> Create(T data, int currentPage, int pageSize, int totalRecords)
        {
            return new PaginationResult<T> (data, currentPage, pageSize, totalRecords);
        }

    }

    
}
