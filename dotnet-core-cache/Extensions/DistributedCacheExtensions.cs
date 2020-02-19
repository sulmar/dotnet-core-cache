using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnet_core_cache.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static T Get<T>(this IDistributedCache cache, string key)
        {
            var json = cache.GetString(key);

            return json == null ? default(T) : JsonSerializer.Deserialize<T>(json);
        }

        public static void Set<T>(this IDistributedCache cache, string key, T item)
        {
            var json = JsonSerializer.Serialize<T>(item);

            cache.SetString(key, json);
        }



    }
}
