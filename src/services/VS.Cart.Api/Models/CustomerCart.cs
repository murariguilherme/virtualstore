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

        public void AddProduct(CustomerCartProduct cartProduct)
        {
            cartProduct.AssociateToCustomerCart(Id);

            if (ProductInCart(cartProduct.Id))
            {                
                var existentItem = GetProductById(cartProduct.Id);
                var quantity = existentItem.Quantity;
    
                CustomerCartProducts.Remove(existentItem);
                cartProduct.AddQuantity(quantity);
            }

            CustomerCartProducts.Add(cartProduct);
            CalculateTotalCart();
        }

        public void UpdateProduct(CustomerCartProduct cartProduct)
        {
            cartProduct.AssociateToCustomerCart(Id);
            var existentProduct = GetProductById(cartProduct.Id);
            CustomerCartProducts.Remove(existentProduct);
            CustomerCartProducts.Add(cartProduct);
            CalculateTotalCart();
        }

        public void RemoveProduct(CustomerCartProduct cartProduct)
        {
            CustomerCartProducts.Remove(GetProductById(cartProduct.Id));
        }

        public void UpdateQuantity(CustomerCartProduct cartProduct, int quantity)
        {
            cartProduct.UpdateQuantity(quantity);
            UpdateProduct(cartProduct);            
        }

        public CustomerCartProduct GetProductById(Guid id)
        {
            return CustomerCartProducts.FirstOrDefault(ci => ci.Id == id);
        }

        public bool ProductInCart(Guid productId)
        {
            return CustomerCartProducts.Any(p => p.ProductId == productId);
        }

        private void CalculateTotalCart()
        {
            Amount = CustomerCartProducts.Sum(ci => ci.CalculateTotal());
        }
    }
}
