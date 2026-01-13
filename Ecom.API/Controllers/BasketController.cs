using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> GetBasketItem(string id)
        {
            var result = await work.customerBasket.GetBasketAsync(id);
           if (result is null)
            {
                return Ok(new CustomerBasket());

            }
           return  Ok(result);
        }
        [HttpPost("update-basket-item")]
        public async Task<IActionResult> UpdateBasketItem(CustomerBasket basket)
        {
            var result = await work.customerBasket.UpdateBasketAsync(basket);
            return Ok(result);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> DeleteBasketItem(string id)
        {
            var result = await work.customerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200,"item deleted")) : BadRequest(new ResponseAPI(400));
        }
    }
}
