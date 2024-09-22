using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Domain.Models
{
    public class OrderDetail
    {
        public int Quantity { get; set; }
        public int IdOrder { get; set; }
        [ForeignKey("IdOrder")]
        public Order Order { get; set; }
        public int IdFood { get; set; }
        [ForeignKey("IdFood")]
        public Food Food { get; set; }
    }
}
