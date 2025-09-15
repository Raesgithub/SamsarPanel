namespace SamsarPanel.Client.ViewModels
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = "/images/default.jpg";
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? LinkButton { get; set; }
        public string? Alt { get; set; }
        public bool? IsPublish { get; set; } = false;

    }
}