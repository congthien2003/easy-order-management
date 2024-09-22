using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Application.DTOs.Request;

namespace StoreManagement.Application.DTOs.Response
{
    public class OrderDetaiResponse
    {
        public int Quantity { get; set; }
        public FoodDTO Food { get; set; }
        public decimal Total { get; set; }
    }
}
