using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.Cart.Api.Models
{
    public class CustomerCart
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }        
        public decimal Amount { get; set; }
        public List<CustomerCartProduct> CustomerCartProducts { get; set; } = new List<CustomerCartProduct>();

        public CustomerCart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public CustomerCart() { }
    }
}
