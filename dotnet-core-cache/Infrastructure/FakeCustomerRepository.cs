using Bogus;
using dotnet_core_cache.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dotnet_core_cache.Infrastructure
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        private ICollection<Customer> customers;

        public FakeCustomerRepository(Faker<Customer> faker)
        {
            customers = faker.Generate(100);
        }

        public IEnumerable<Customer> Get()
        {
            return customers;
        }

        public Customer Get(int id)
        {
            return customers.SingleOrDefault(c => c.Id == id);
        }
    }

    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            RuleFor(p => p.Id, f => f.IndexFaker);
            RuleFor(p => p.FirstName, f => f.Person.FirstName);
            RuleFor(p => p.LastName, f => f.Person.LastName);
        }
    }
}
