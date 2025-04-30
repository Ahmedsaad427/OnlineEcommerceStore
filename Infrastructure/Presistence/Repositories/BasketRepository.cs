using Domain.Contracts;
using Domain.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence.Repositories
    {
    public class BasketRepository(IConnectionMultiplexer connectionMultiplexer) : IBasketRepository
        {

        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        #region Get
        public async Task<CustomerBasket?> GetBasketAsync(string id)
            {
            var redisValue = await _database.StringGetAsync(id);
            if ( redisValue.IsNullOrEmpty ) return null;

            var basket = JsonSerializer.Deserialize<CustomerBasket>(redisValue);
            if ( basket is null ) return null;
            return basket;
            }
        #endregion

        #region Update
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeSpan = null)
            {
            var redisValue = JsonSerializer.Serialize(basket);
            var flag = await _database.StringSetAsync(basket.Id, redisValue, timeSpan ?? TimeSpan.FromDays(30));


            return flag ? await GetBasketAsync(basket.Id) : null;
            }
        #endregion

        #region Delete
        public async Task<bool> DeleteBasketAsync(string id)
            {
            return await _database.KeyDeleteAsync(id);
            }
        #endregion


        }
    }
