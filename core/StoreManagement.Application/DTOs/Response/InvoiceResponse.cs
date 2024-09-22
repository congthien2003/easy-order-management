using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Application.DTOs.Response
{
    public class InvoiceResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public bool Status { get; set; }
        public decimal TotalOrder { get; set; }
        public decimal Charge { get; set; }
        public decimal Total { get; set; }
        public OrderDTO Order { get; set; }
        public string TableName { get; set; }
        public PaymentTypeDTO PaymentType { get; set; }
    }
}
