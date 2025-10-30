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
    public class UserController : ControllerBase
    {
        readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("gettelegramUsers")]
        [HttpGet]
        public async Task<List<string>> GettelegramUsers()
        {
            UserRepository repository = new UserRepository();
            return await repository.GetTelegramUsers();
        }

    }
}
