// <copyright file="LocationChangedEvent.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace EventingApp
{
    using Eventing.Abstractions;

    /// <summary>
    /// Event for indicating that the location has changed.
    /// </summary>
    /// <seealso cref="EventBase" />
    public class LocationChangedEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationChangedEvent"/> class.
        /// </summary>
        /// <param name="newLoccation">The new location.</param>
        public LocationChangedEvent(Location newLoccation)
        {
            NewLocation = newLoccation;
        }

        /// <summary>
        /// Gets the new location.
        /// </summary>
        /// <value>
        /// The new location.
        /// </value>
        public Location NewLocation { get; }
    }
}
