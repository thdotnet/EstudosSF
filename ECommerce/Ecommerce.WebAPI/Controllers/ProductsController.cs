using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Ecommerce.WebAPI.Model;

namespace Ecommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService _catalogService;

        public ProductsController()
        {
            _catalogService = ServiceProxy.Create<IProductCatalogService>(new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"), new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> Get()
        {
            var products = await _catalogService.GetAllProductsAsync();

            return products.Select(x => new ApiProduct
            {
                Description = x.Description,
                Id = x.Id,
                IsAvailable = x.Availability > 0,
                Price = x.Price
            }).ToList();
        }

        [HttpPost]
        public async Task Post([FromBody] ApiProduct product)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Availability = 100,
                Name = product.Description,
                Description = product.Description,
                Price = product.Price
            };

            await _catalogService.AddProductAsync(newProduct);
        }
    }
}
