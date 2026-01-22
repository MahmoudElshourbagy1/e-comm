using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Entites
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }
        public CustomerBasket(string id)
        {
            Id=id;
        }
        public string Id { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public List<BasketItem> basketItems { get; set; } = new List<BasketItem>();

    }
}
