namespace BizzyQCU.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string PreparationTime { get; set; }
        public string Delivery { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}