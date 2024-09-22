using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Domain.Models
{
    public class Table : DeleteableEntity
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public int IdStore { get; set; }
        [ForeignKey("IdStore")]
        public Store Store { get; set; }
        public virtual Collection<Order> Orders { get; set; }
    }
}
