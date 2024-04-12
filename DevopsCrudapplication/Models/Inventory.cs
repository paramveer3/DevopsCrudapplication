namespace DevopsCrudapplication.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int IsAvailable { get; set; }
    }
}
