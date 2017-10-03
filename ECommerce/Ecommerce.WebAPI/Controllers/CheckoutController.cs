using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckoutService.Model;
using Ecommerce.WebAPI.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Ecommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CheckoutController : Controller
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Second);

        [Route("{userId}")]
        public async Task<ApiCheckoutSummary> Checkout(string userId)
        {
            CheckoutSummary summary = await GetCheckoutService().CheckoutAsync(userId);

            return ToApiCheckoutSummary(summary);
        }

        private ICheckoutService GetCheckoutService()
        {
            long key = LongRandom();

            return ServiceProxy.Create<ICheckoutService>(new Uri("fabric:/ECommerce/CheckoutService"),
                new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(key));
        }

        private long LongRandom()
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return longRand;
        }

        private ApiCheckoutSummary ToApiCheckoutSummary(CheckoutSummary summary)
        {
            return new ApiCheckoutSummary
            {
                Products = summary.Products.Select(x=> new ApiCheckoutProduct{
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    Price = x.Product.Price,
                    Quantity = x.Quantity
                }).ToList(),
                Date = summary.Date,
                TotalPrice = summary.TotalPrice                
            };
        }

        [Route("history/{userId}")]

        public async Task<IEnumerable<ApiCheckoutSummary>> GetHistory(string userId)
        {
            IEnumerable<CheckoutSummary> history = await GetCheckoutService().GetOrderSummary(userId);

            return history.Select(ToApiCheckoutSummary);
        }
    }
}