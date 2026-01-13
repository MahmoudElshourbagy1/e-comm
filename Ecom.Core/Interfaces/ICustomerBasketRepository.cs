using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Interfaces
{
    public interface ICustomerBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
