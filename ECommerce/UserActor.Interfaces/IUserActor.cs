﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace UserActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IUserActor : IActor
    {
        Task AddToBasketAsync(Guid productId, int quantity);

        Task<Dictionary<Guid, int>> GetBasketAsync();

        Task ClearBasketAsync();
    }
}
