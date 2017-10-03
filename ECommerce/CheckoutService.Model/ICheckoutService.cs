using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutService.Model
{
    public interface ICheckoutService : IService
    {
        Task<CheckoutSummary> CheckoutAsync(string userId);

        Task<IEnumerable<CheckoutSummary>> GetOrderSummary(string userId);
    }
}
