// <copyright file="EventUnsubscriber.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Internals
{
    using System;

    using Eventing.Abstractions;

    /// <summary>
    /// The event unsubscriber used for unsubscribing from an event.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    internal class EventUnsubscriber<TEvent> : IDisposable
        where TEvent : EventBase
    {
        private readonly EventAggregator _aggregator;
        private readonly EventSubscriber _subscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventUnsubscriber{TEvent}"/> class.
        /// </summary>
        /// <param name="aggregator">The aggregator.</param>
        /// <param name="subscriber">The subscriber.</param>
        public EventUnsubscriber(EventAggregator aggregator, EventSubscriber subscriber)
        {
            _aggregator = aggregator;
            _subscriber = subscriber;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _aggregator.Remove<TEvent>(_subscriber);
        }
    }
}
