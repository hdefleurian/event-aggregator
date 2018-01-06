// <copyright file="IEventSubscriber.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Abstractions
{
    using System;

    /// <summary>
    /// Defines a capability that allow to subscribe to an event.
    /// </summary>
    public interface IEventSubscriber
    {
        /// <summary>
        /// Subscribes to the event using the specified handler.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The event handler.</param>
        /// <returns>An instance of <see cref="IDisposable"/> to be used to unsubscribe from the event.</returns>
        IDisposable Subscribe<TEvent>(Action<TEvent> handler)
            where TEvent : EventBase;
    }
}
