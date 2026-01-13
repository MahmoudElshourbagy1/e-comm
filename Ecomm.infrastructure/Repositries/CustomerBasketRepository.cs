using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Ecomm.infrastructure.Repositries
{
    public class CustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly IDatabase _database;
        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public Task<bool> DeleteBasketAsync(string id)
        {
            return _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var result= await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result.ToString());
            }
            return null;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var _basket= await _database.StringSetAsync(basket.Id.ToString(),JsonSerializer.Serialize(basket),TimeSpan.FromDays(3));
            if (_basket)
            {
                return await GetBasketAsync(basket.Id.ToString());
            }
            return null;
        }
    }
}
