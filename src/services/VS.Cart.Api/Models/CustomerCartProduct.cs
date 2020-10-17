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

        public decimal CalculateTotal()
        {
            return Quantity * Amount;
        }

        public void AssociateToCustomerCart(Guid id)
        {
            CustomerCartId = id;
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}
