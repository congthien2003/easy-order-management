using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Application.DTOs.Request
{
    public class ProductSellDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int FoodId { get; set; }
    }
}
