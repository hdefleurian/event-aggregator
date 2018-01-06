// <copyright file="EventManager.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Internals
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Eventing.Abstractions;

    /// <summary>
    /// Manager for the subscribers to an event type.
    /// </summary>
    internal class EventManager : IDisposable
    {
        private readonly ConcurrentDictionary<Guid, EventSubscriber> _subscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager" /> class.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        public EventManager(string eventType)
        {
            _subscribers = new ConcurrentDictionary<Guid, EventSubscriber>();

            EventType = eventType;
        }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        public string EventType { get; }

        /// <summary>
        /// Gets the subscribers.
        /// </summary>
        protected IEnumerable<EventSubscriber> Subscribers => _subscribers.Values;

        /// <summary>
        /// Adds the specified subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Add(EventSubscriber subscriber)
        {
            _subscribers.TryAdd(subscriber.Token, subscriber);
        }

        /// <summary>
        /// Publishes the specified event to each subscriber.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="e">The event.</param>
        public void Publish<TEvent>(TEvent e)
            where TEvent : EventBase
        {
            foreach (var subscriber in Subscribers)
            {
                Debug.WriteLine($"Publish event type {EventType} to subscriber with token {subscriber.Token}");

                subscriber.Handle(e);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var subscriber in _subscribers.Values)
            {
                subscriber.Dispose();
            }

            _subscribers.Clear();
        }

        /// <summary>
        /// Removes the subscriber associated with the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The remaining number of subscribers.</returns>
        public int Remove(Guid token)
        {
            _subscribers.TryRemove(token, out EventSubscriber deletedSubscriber);
            deletedSubscriber?.Dispose();

            return _subscribers.Count;
        }
    }
}
