using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Domain.Models
{
    public class Store : DeleteableEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; } 
        public Collection<Category> Categories { get; set; }
        public Collection<Table> Tables { get; set; }
        public Collection<PaymentType> PaymentTypes { get; set; }
        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
