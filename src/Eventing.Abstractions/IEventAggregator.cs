// <copyright file="IEventAggregator.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Abstractions
{
    /// <summary>
    /// Defines an event aggregator with the necessary capabilities allowing to use the publish-subscribe pattern for events.
    /// </summary>
    /// <seealso cref="IEventPublisher" />
    /// <seealso cref="IEventSubscriber" />
    public interface IEventAggregator : IEventPublisher, IEventSubscriber
    {
    }
}
