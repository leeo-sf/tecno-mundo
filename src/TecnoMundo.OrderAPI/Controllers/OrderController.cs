using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("{profileId}")]
        public async Task<ActionResult<List<OrderHeader>>> GetAllOrder(Guid profileId)
        {
            var orders = await _service.GetAllOrder(profileId);

            return Ok(orders);
        }
    }
}
