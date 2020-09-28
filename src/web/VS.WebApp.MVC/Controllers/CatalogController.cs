using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VS.WebApp.MVC.Services;

namespace VS.WebApp.MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("store")]
        public async Task<IActionResult> Index()
        {
            var products = await _catalogService.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        [Route("product-detail/{id}")]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await _catalogService.GetProductById(id);
            return View(product);
        }
    }
}
