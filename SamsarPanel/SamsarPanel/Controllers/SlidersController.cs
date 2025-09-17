using Domain.Data;
using Domain.Models.panel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SliderController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/slider/getsliders
        [HttpGet("getsliders")]
        public async Task<ActionResult<List<SliderViewModel>>> GetSliders()
        {
            var sliders = await _db.Sliders
                .Where(s => s.IsPublish) // فقط اسلایدرهای منتشر شده
                .Select(s => new SliderViewModel
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    Title = s.Title,
                    Description = s.Description,
                    LinkButton = s.LinkButton,
                    Alt = s.Alt,
                    IsPublish = s.IsPublish
                })
                .ToListAsync();

            return Ok(sliders);
        }
    }

    // ViewModel برای Client
    public class SliderViewModel
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
