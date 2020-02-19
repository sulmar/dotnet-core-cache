using dotnet_core_cache.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using dotnet_core_cache.Extensions;

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
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var cachedCustomer = _distributedCache.Get<Customer>($"customers-{id}");

            if (cachedCustomer != null)
            {
                return Ok(cachedCustomer);
            }

            var customer = customerRepository.Get(id);

            _distributedCache.Set($"customers-{id}", customer);

            return Ok(customer);
        }
    }
}
