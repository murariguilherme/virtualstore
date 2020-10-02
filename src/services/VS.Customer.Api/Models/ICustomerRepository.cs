using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.Data;

namespace VS.Customer.Api.Models
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Task AddCustomer(Customer customer);
        Task<Customer> GetCustomerByEmail(string email);
        Task<IEnumerable<Customer>> GetAllCustomers();
    }
}
