using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Talabat.Core.Entities.BasketModule;
using Talabat.Core.Repositories;

namespace Talabat.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            database = connection.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var RedisBasket = await database.StringGetAsync(basketId);
            return RedisBasket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(RedisBasket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreatedOrUpdatedBasket = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(5));
            return !CreatedOrUpdatedBasket ? null : await GetBasketAsync(basket.Id);
        }
    }
}
