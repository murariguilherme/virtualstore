using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.Cart.Api.Models
{
    public class CustomerCartProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public Guid CustomerCartId { get; set; }
        public CustomerCart CustomerCart { get; set; }
        public CustomerCartProduct()
        {
            Id = Guid.NewGuid();
        }
    }
}
