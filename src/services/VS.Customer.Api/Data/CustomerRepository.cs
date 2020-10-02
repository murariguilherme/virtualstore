using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VS.Core.Data;
using VS.Customer.Api.Models;

namespace VS.Customer.Api.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddCustomer(Models.Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<IEnumerable<Models.Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Models.Customer> GetCustomerByEmail(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email.EmailAddress == email);
        }
    }
}
