using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Domain.Models.panel
{
    public class Slider
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = "/images/default.jpg";
        [Required]
        public string Alt { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? LinkButton { get; set; }

        public bool IsPublish { get; set; } = true;



    }
}