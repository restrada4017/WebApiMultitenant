using GPT.Application.Contracts.Infrastructure.Utilities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Utilities.Cache
{
    public class CacheRedis : ICache
    {
        private readonly IDistributedCache _distributedCache;

        public CacheRedis(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<byte[]> Get(string key)
        {
            var obj = await _distributedCache.GetAsync(key);
            return obj;
        }

        public async Task Set(string key, byte[] value)
        {
            var option = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            await _distributedCache.SetAsync(key, value, option);

        }
    }
}
