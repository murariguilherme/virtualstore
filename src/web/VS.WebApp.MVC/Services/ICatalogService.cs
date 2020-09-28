using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProducts();
        Task<ProductViewModel> GetProductById(Guid id);
    }
}
