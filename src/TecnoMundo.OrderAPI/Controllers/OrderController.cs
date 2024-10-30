using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
        private readonly IConfiguration _configuration;
        private readonly DistributedCacheEntryOptions _options;
        private readonly string _keyCache;

        public OrderController(IOrderService service,
            IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
            _keyCache = _configuration.GetSection("Redis").GetSection("Key_Cache").Value ?? "orders";
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(double.Parse(_configuration.GetSection("Redis").GetSection("Absolute_Expire").Value ?? "3600")),
                SlidingExpiration = TimeSpan.FromSeconds(double.Parse(_configuration.GetSection("Redis").GetSection("Sliding_Expire").Value ?? "600"))
            };
        }

        [HttpGet("{profileId}")]
        public async Task<ActionResult<List<OrderHeader>>> GetAllOrder(Guid profileId)
        {
            var orders = await _service.GetAllOrder(profileId, _keyCache, _options);

            return Ok(orders);
        }
    }
}
