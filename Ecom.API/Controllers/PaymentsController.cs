using Ecom.Core.Entites;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<CustomerBasket>> create(string basketId,int? deliveryId)
        {
            return await _paymentService.CreateOrUpdatePaymentAsync(basketId, deliveryId);
        }
    }
}
