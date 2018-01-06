// <copyright file="LocationReporter.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace EventingApp
{
    using System;

    using Eventing.Abstractions;

    /// <summary>
    /// Report the current location for the specified source name
    /// </summary>
    public class LocationReporter
    {
        private readonly IEventSubscriber _subscriber;

        private IDisposable _eventUnsubscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationReporter" /> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="name">The name.</param>
        public LocationReporter(IEventSubscriber subscriber, string name)
        {
            _subscriber = subscriber;

            Name = name;
        }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Subscribes to the event indicating that the location has changed.
        /// </summary>
        public void Subscribe()
        {
            if (_eventUnsubscriber != null)
            {
                return;
            }

            _eventUnsubscriber = _subscriber.Subscribe<LocationChangedEvent>(e => OnLocationChanged(e.NewLocation));
        }

        /// <summary>
        /// Unsubscribes from the event indicating that the location has changed.
        /// </summary>
        public void Unsubscribe()
        {
            _eventUnsubscriber.Dispose();
        }

        private void OnLocationChanged(Location value)
        {
            if (value == null)
            {
                Console.WriteLine($"{Name}: The location cannot be determined.");
            }
            else
            {
                Console.WriteLine($"{Name}: The current location is {value.Latitude}, {value.Longitude}");
            }
        }
    }
}
