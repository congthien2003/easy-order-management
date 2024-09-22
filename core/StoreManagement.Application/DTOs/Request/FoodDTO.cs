namespace StoreManagement.Application.DTOs.Request
{
    public class FoodDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int IdCategory { get; set; }
    }
}
