using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VS.Catalog.Api.Data;
using VS.Catalog.Api.Models;
using VS.WebApi.Core.Identity;

namespace VS.Catalog.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _repository;

        public CatalogController(IProductRepository repository)
        {
            _repository = repository;
        }

        [ClaimsAuthorize("Catalog", "Read")]
        [HttpGet]
        [Route("catalog/products/{id}")]
        public async Task<Product> GetProduct(Guid id)
        {
            return await _repository.GetById(id);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("catalog/products")]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _repository.GetAll();
        }
    }
}
