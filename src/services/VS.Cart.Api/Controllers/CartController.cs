using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VS.Cart.Api.Data;
using VS.Cart.Api.Models;
using VS.Core.Controllers;
using VS.WebApi.Core.User;

namespace VS.Cart.Api.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartDbContext _context;
        private readonly IUser _user;

        public CartController(CartDbContext context, IUser user = null)
        {
            _context = context;
            _user = user;
        }        

        [HttpGet("cart")]
        public async Task<CustomerCart> GetCart()
        {
            return await GetCustomerCart() ?? new CustomerCart();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> PostCartItem(CustomerCartProduct cartProduct)
        {
            var customerCart = await GetCustomerCart();

            if (customerCart == null)
                HandleNewCart(_user.GetUserId(), cartProduct);
            else
                HandleExistentCart(customerCart, cartProduct);

            var commitResult = await _context.SaveChangesAsync();

            if (commitResult <= 0) AddErrorToList("The data was not persisted on database.");

            return GenerateResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveProductCart(Guid productId)
        {
            var customerCart = await GetCustomerCart();
            var customerCartProduct = await GetCartProductValidated(productId, customerCart);

            customerCart.RemoveProduct(customerCartProduct);

            _context.CustomerCartProducts.Remove(customerCartProduct);
            _context.CustomerCarts.Update(customerCart);

            var commitResult = await _context.SaveChangesAsync();

            if (commitResult <= 0) AddErrorToList("The data was not persisted on database.");

            return GenerateResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateProductCart(Guid productId, CustomerCartProduct customerCartProduct)
        {
            var customerCart = await GetCustomerCart();
            var productCart = await GetCartProductValidated(productId, customerCart, customerCartProduct);
            if (productCart == null) return GenerateResponse();

            customerCart.UpdateQuantity(productCart, customerCartProduct.Quantity);

            _context.CustomerCarts.Update(customerCart);
            _context.CustomerCartProducts.Update(productCart);

            var commitResult = await _context.SaveChangesAsync();
            if (commitResult <= 0) AddErrorToList("The data was not persisted on database.");

            return GenerateResponse();
        }

        private void HandleNewCart(Guid customerId, CustomerCartProduct cartProduct)
        {
            var cart = new CustomerCart(customerId);
            cart.AddProduct(cartProduct);

            _context.CustomerCarts.Add(cart);
        }

        private void HandleExistentCart(CustomerCart cart, CustomerCartProduct cartProduct)
        {
            var productInCart = cart.ProductInCart(cartProduct.Id);
            cart.AddProduct(cartProduct);
            if (productInCart)
                _context.CustomerCartProducts.Update(cart.GetProductById(cartProduct.Id));
            else
                _context.CustomerCartProducts.Add(cart.GetProductById(cartProduct.Id));

            _context.CustomerCarts.Update(cart);
        }

        private async Task<CustomerCart> GetCustomerCart()
        {
            var customerCart = await _context.CustomerCarts
                .Include(i => i.CustomerCartProducts)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
            return customerCart;
        }

        private async Task<CustomerCartProduct> GetCartProductValidated(Guid productId,
                                                                        CustomerCart customerCart,
                                                                        CustomerCartProduct customerCartProduct = null)
        {
            if (customerCartProduct != null && productId != customerCartProduct.Id)
            {
                AddErrorToList("Product id doesn't match.");
                return null;
            }

            if (customerCart == null)
            {
                AddErrorToList("Customer cart not found.");
                return null;
            }

            var itemCart = await _context.CustomerCartProducts
                .FirstOrDefaultAsync(p => p.CustomerCartId == customerCart.Id && p.ProductId == productId);

            if (itemCart == null)
            {
                AddErrorToList("Product isn't in the cart.");
                return null;
            }

            return itemCart;
        }
    }
}
