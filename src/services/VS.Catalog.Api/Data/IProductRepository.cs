using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Catalog.Api.Models;
using VS.Core.Data;

namespace VS.Catalog.Api.Data
{
    public interface IProductRepository: IRepository<Product>
    {
        public Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
    }
}
