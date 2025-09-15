using System.Data.SqlTypes;

namespace Domain.Models.panel
{
    public class Slider
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = "/images/default.jpg";
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? LinkButton { get; set; }
        public string? Alt { get; set; }
        public bool IsPublish { get; set; } = true;



    }
}