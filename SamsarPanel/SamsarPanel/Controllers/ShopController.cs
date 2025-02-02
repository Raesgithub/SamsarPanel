using Application.Repositories.Shop;
using Domain.Data;
using Domain.Models.shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SamsarPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        readonly ApplicationDbContext _context;
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("getcats")]
        [HttpGet]
        public async Task<IEnumerable<Catalog?>> GetCats()
        {
            CatalogRepository catalogRepository = new CatalogRepository();
            return  await catalogRepository.GetAllAsync();
        }
    }
}
