
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecomm.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, AppDbContext appDbContext)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _appDbContext = appDbContext;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId,int? deliverMethodId)
        {
            var basket = await _unitOfWork.customerBasket.GetBasketAsync(basketId);
            StripeConfiguration.ApiKey = _configuration["StripeSetting:secretkey"];
            var shippingPrice = 0m;
            if (deliverMethodId.HasValue)
            {
                var delivery=await _appDbContext.DeliveryMethods.AsNoTracking()
                    .FirstOrDefaultAsync(m=>m.Id==deliverMethodId.Value);
                shippingPrice = delivery.Price;
            }
            foreach(var item in basket.basketItems)
            {
                var product = await _unitOfWork.productRepositry.GetByIdAsync(item.Id);
                item.Price = product.NewPrice;
            }
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent _intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.basketItems.Sum(m => m.Quantity * (m.Price * 100)) + (long)(shippingPrice * 100),
                    Currency = "USD",
                    PaymentMethodTypes=new List<string> { "card"}
                };
                _intent =await paymentIntentService.CreateAsync(option);
                basket.PaymentIntentId = _intent.Id;
                basket.ClientSecret = _intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.basketItems.Sum(m => m.Quantity * (m.Price * 100) ) +(long) (shippingPrice * 100),
                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId,option);
            }
            await _unitOfWork.customerBasket.UpdateBasketAsync(basket);
            return basket;
        }
    }
}
