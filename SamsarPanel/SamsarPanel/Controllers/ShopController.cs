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
            return  await catalogRepository.GetAllAsync();
        }

        [Route("getpro/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ProductVM?>> GetPro(int id)
        {
            ProductRepository productRepository = new ProductRepository();
            if (id==0)
            {
                return await productRepository.GetPopular();
            }
            else
            {
                return await productRepository.GetProducts_With_CatalogId(id);  
            }
            
        }


        [Route("setorder/{id}/{phone}")]
        [HttpGet]
        public async Task<bool> SetOrder(int id,string phone)
        {
            OrderRepository orderRepository = new OrderRepository();
            
                return await orderRepository.InsertAsync(id,phone);
            
        }


        [Route("getdetail/{id}")]
        [HttpGet]
        public async Task<Product?> GetDetail(int id)
        {
            ProductRepository productRepository = new ProductRepository();
                var shop= await productRepository.GetById(id);
            if (shop==null)
            {
                return null;
            }
            shop.Seen+=1;
            await productRepository.UpdateSeenAsync(id, shop.Seen);
            return shop;    
        }





        [Route("getbycatalogid")]
        [HttpGet]
        public async Task<IEnumerable<Catalog?>> GetProductByCatalogId(int id)
        {
            ProductRepository productRepository=new ProductRepository ();
            

            CatalogRepository catalogRepository = new CatalogRepository();
            return await catalogRepository.GetAllAsync();
        }

    }
}
