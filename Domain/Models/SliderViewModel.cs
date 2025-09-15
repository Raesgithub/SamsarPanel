namespace Domain.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = "/images/default.jpg";
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}