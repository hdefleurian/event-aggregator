// <copyright file="IEventPublisher.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Abstractions
{
    /// <summary>
    /// Defines a capability that allow to publish an event.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="e">The event.</param>
        void Publish<TEvent>(TEvent e)
            where TEvent : EventBase;
    }
}
