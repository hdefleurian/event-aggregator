// <copyright file="EventAggregator.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;

    using Eventing.Abstractions;
    using Eventing.Internals;

    /// <summary>
    /// The event aggregator.
    /// </summary>
    /// <seealso cref="IEventAggregator" />
    /// <seealso cref="IDisposable" />
    public sealed class EventAggregator : IEventAggregator, IDisposable
    {
        private readonly ConcurrentDictionary<string, EventManager> _managers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        public EventAggregator()
        {
            _managers = new ConcurrentDictionary<string, EventManager>();
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent e)
            where TEvent : EventBase
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var manager = GetManagerForEvent(typeof(TEvent).Name);
            if (manager == null)
            {
                return;
            }

            manager.Publish(e);
        }

        /// <inheritdoc />
        public IDisposable Subscribe<TEvent>(Action<TEvent> handler)
            where TEvent : EventBase
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var eventType = typeof(TEvent).Name;
            var subscriber = new EventSubscriber(handler);
            _managers.AddOrUpdate(eventType, key => OnAddSubscriberForNewEventType(key, subscriber), (key, value) => OnAddSubscriberForExistEventType(key, value, subscriber as EventSubscriber));
            return new EventUnsubscriber<TEvent>(this, subscriber);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var manager in _managers.Values)
            {
                manager.Dispose();
            }

            _managers.Clear();
        }

        /// <summary>
        /// Removes the subscriber from the list of subscribers for the associated event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="subscriber">The subscriber to remove.</param>
        internal void Remove<TEvent>(EventSubscriber subscriber)
            where TEvent : EventBase
        {
            var eventType = typeof(TEvent).Name;
            Remove(eventType, subscriber);
        }

        private static EventManager OnAddSubscriberForNewEventType(string eventType, EventSubscriber subscriber)
        {
            var manager = new EventManager(eventType);

            return OnAddSubscriberForExistEventType(eventType, manager, subscriber);
        }

        private static EventManager OnAddSubscriberForExistEventType(string eventType, EventManager manager, EventSubscriber subscriber)
        {
            Debug.WriteLine($"Add new subscriber for event {eventType} with token {subscriber.Token}");

            manager.Add(subscriber);
            return manager;
        }

        private void Remove(string eventType, EventSubscriber subscriber)
        {
            Debug.WriteLine($"Remove subscriber for event {eventType} with token {subscriber.Token}");

            var manager = GetManagerForEvent(eventType);
            if (manager == null)
            {
                Debug.WriteLine($"No subscriber to remove for event {eventType}");
                return;
            }

            var remainingSubscribers = manager.Remove(subscriber.Token);
            if (remainingSubscribers == 0)
            {
                Debug.WriteLine($"Remove manager for event {eventType}");

                if (_managers.TryRemove(eventType, out manager))
                {
                    manager.Dispose();
                }
            }
        }

        private EventManager GetManagerForEvent(string eventType)
        {
            if (_managers.TryGetValue(eventType, out EventManager manager))
            {
                return manager;
            }

            Debug.WriteLine($"No manager for event {eventType}");
            return null;
        }
    }
}
