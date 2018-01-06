// <copyright file="EventArgs.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Tests.Fakes
{
    using Eventing.Abstractions;

    /// <summary>
    /// An event to be used in the unit test classes.
    /// </summary>
    /// <seealso cref="EventBase" />
    public class EventArgs : EventBase
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public EventData Data { get; set; }
    }
}
