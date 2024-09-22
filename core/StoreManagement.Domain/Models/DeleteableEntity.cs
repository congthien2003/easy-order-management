namespace StoreManagement.Domain.Models
{
    public class DeleteableEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
