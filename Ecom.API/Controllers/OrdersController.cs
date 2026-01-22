using Ecom.Core.DTO;
using Ecom.Core.Entites.Order;
using Ecom.Core.Services;
using Ecomm.infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;

        public OrdersController(IOrderService orderService, AppDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> create(OrderDTO orderDTO) {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order =await _orderService.CreateOrdersAsync(orderDTO, email);
            return Ok(order);
        }
        [HttpGet("get-orders-for-user")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> getorders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetAllOrdersForUserAsync(email);
            return Ok(order);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult>getOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrderByIdAsync(id, email);
            return Ok(order);
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> getDelivery() => Ok(await _orderService.GetDeliveryMethodAsync());
        [HttpPatch("update-status/{id}")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO statusDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound("Order not found");

            // محاولة تحويل string للـ enum بأمان
            if (!Enum.TryParse<Status>(statusDto.Status, ignoreCase: true, out var newStatus))
                return BadRequest("Invalid status value");

            order.status = newStatus;
            await _context.SaveChangesAsync();

            return Ok(order);
        }

    }
}
