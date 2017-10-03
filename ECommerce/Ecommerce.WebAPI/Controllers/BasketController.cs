using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.WebAPI.Model;
using UserActor.Interfaces;
using Microsoft.ServiceFabric.Actors.Client;

namespace Ecommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        [HttpGet("{userId}")]
        public async Task<ApiBasket> Get(string userId)
        {
            IUserActor actor = GetActor(userId);

            Dictionary<Guid, int> products = await actor.GetBasketAsync();

            return new ApiBasket()
            {
                UserId = userId,
                Items = products.Select(x=> new ApiBasketItem {  ProductId = x.Key.ToString(), Quantity = x.Value}).ToArray()
            };
        }

        [HttpPost("{userId}")]
        public async Task Add(string userId, [FromBody] ApiBasketAddRequest request)
        {
            IUserActor actor = GetActor(userId);

            await actor.AddToBasketAsync(request.ProductId, request.Quantity);
        }

        [HttpDelete("{userId}")]
        public async Task Delete(string userId)
        {
            IUserActor actor = GetActor(userId);

            await actor.ClearBasketAsync();
        }

        private IUserActor GetActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(
                new Microsoft.ServiceFabric.Actors.ActorId(userId),
                new Uri("fabric:/ECommerce/UserActorService"));
        }
    }
}