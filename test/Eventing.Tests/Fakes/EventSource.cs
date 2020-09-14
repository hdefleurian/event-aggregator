// <copyright file="EventSource.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Tests.Fakes
{
    using System;
    using Eventing.Abstractions;

    /// <summary>
    /// A source of event to be used in the unit test classes.
    /// </summary>
    public class EventSource
    {
        private readonly IEventPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSource"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public EventSource(IEventPublisher publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        /// <summary>
        /// Emits the specified data by publishing it with the <see cref="IEventPublisher"/> instance.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Emit(EventData data)
        {
            _publisher.Publish(new EventArgs { Data = data });
        }
    }
}
