using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, int? deliverId);
    }
}
