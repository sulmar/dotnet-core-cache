using dotnet_core_cache.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnet_core_cache.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IDistributedCache _distributedCache;

        public CustomersController(ICustomerRepository customerRepository, IDistributedCache distributedCache)
        {
            this.customerRepository = customerRepository;
            this._distributedCache = distributedCache;
        }

        public IActionResult Get()
        {
            var cacheKey = "TheTime";
            var existingTime = _distributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return Ok("Fetched from cache : " + existingTime);
            }
            else
            {
                existingTime = DateTime.UtcNow.ToString();
                _distributedCache.SetString(cacheKey, existingTime);
                return Ok("Added to cache : " + existingTime);
            }

            //var customers = customerRepository.Get();

            //return Ok(customers);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {

            var cached = _distributedCache.GetString($"customers-{id}");

            if (cached!=null)
            {
                var cachedCustomer = JsonSerializer.Deserialize<Customer>(cached);

                return Ok(cachedCustomer);
            }

            var customer = customerRepository.Get(id);

            string json = JsonSerializer.Serialize<Customer>(customer);

            _distributedCache.SetString($"customers-{id}", json);

            return Ok(customer);
        }
    }
}
