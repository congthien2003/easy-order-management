using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.Models
{
    public class Order : DeleteableEntity
    {
        public decimal Total { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int IdTable { get; set; }
        [ForeignKey("IdTable")]
        public Table Table { get; set; }
        public virtual Collection<OrderDetail> OrderDetails { get; set; }
    }
}
