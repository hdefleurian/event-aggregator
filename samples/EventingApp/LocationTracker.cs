// <copyright file="LocationTracker.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace EventingApp
{
    using Eventing.Abstractions;

    /// <summary>
    /// Location tracker.
    /// </summary>
    public class LocationTracker
    {
        private readonly IEventPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTracker"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public LocationTracker(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        /// <summary>
        /// Tracks the location.
        /// </summary>
        /// <param name="value">The new location.</param>
        public void TrackLocation(Location value)
        {
            _publisher.Publish(new LocationChangedEvent(value));
        }
    }
}
