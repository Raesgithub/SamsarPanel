using Application.Repositories.cpanel;
using Application.Repositories.Shop;
using Domain.Data;
using Domain.Models.shop;
using Domain.ViewModels;
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
            return await catalogRepository.GetAllAsync();
        }

        [Route("getpro/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ProductVM?>> GetPro(int id)
        {
            ProductRepository productRepository = new ProductRepository();
            if (id == 0)
            {
                return await productRepository.GetPopular();
            }
            else
            {
                return await productRepository.GetProducts_With_CatalogId(id);
            }
        }

        [Route("setorder/{id}")]
        [HttpPost]
        public async Task<bool> SetOrder(int id, [FromBody] ContactModel contactModel)
        {
            OrderRepository orderRepository = new OrderRepository();
            return await orderRepository.InsertAsync(id, contactModel);
        }

        [Route("getdetail/{id}")]
        [HttpGet]
        public async Task<Domain.Models.shop.Product?> GetDetail(int id)
        {
            ProductRepository productRepository = new ProductRepository();
            var shop = await productRepository.GetById(id);
            if (shop == null)
            {
                return null;
            }
            shop.Seen += 1;
            await productRepository.UpdateSeenAsync(id, shop.Seen);
            return shop;
        }

        [Route("getbycatalogid")]
        [HttpGet]
        public async Task<IEnumerable<Catalog?>> GetProductByCatalogId(int id)
        {
            ProductRepository productRepository = new ProductRepository();
            CatalogRepository catalogRepository = new CatalogRepository();
            return await catalogRepository.GetAllAsync();
        }

        // ✅ متد جدید برای دریافت سفارش‌ها (با صفحه‌بندی)
        [Route("getorders")]
        [HttpGet]
        public async Task<IActionResult> GetOrders(int page = 1, int pageSize = 10)
        {
            try
            {
                OrderRepository orderRepository = new OrderRepository();
                var result = await orderRepository.GetPagedOrdersAsync(page, pageSize);
                var data = result.data;
                var total = result.total;
                return Ok(new { data, total });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در گرفتن سفارش‌ها: {ex.Message}");
                return StatusCode(500, "خطا در دریافت اطلاعات سفارش‌ها");
            }
        }
    }
}
