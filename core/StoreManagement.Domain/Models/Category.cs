using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using StoreManagement.Domain.Models;

namespace StoreManagement.Domain.Models
{
    public class Category : DeleteableEntity
    {
        public string Name { get; set; }
        public int IdStore { get; set; }
        [ForeignKey("IdStore")]
        public Store Store { get; set; }
        public virtual Collection<Food> Foods { get; set; }
    }
}
