using ECommerce.ProductCatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog
{
    interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task AddProductAsync(Product product);

        Task<Product> GetProductAsync(Guid productId);
    }
}
