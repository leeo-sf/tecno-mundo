using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecnoMundo.OrderAPI.Model;

namespace TecnoMundo.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("{profileId}")]
        public async Task<ActionResult<List<OrderHeader>>> GetAllOrder(Guid profileId)
        {
            var orders = await _orderRepository.GetAllOrder(profileId);

            return Ok(orders);
        }
    }
}
