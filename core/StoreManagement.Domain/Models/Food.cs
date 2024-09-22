using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Domain.Models
{
    public class Food : DeleteableEntity
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public Category Category { get; set; }
        public virtual Collection<OrderDetail> OrderDetails { get; set; }
        public virtual Collection<ProductSell> ProductSells { get; set; }
    }
}
