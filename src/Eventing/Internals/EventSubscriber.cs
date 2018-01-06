// <copyright file="EventSubscriber.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Internals
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Eventing.Abstractions;

    /// <summary>
    /// The event subscriber containing a <see cref="Delegate"/> to be executed when the event is published.
    /// </summary>
    internal class EventSubscriber : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventSubscriber"/> class.
        /// </summary>
        /// <param name="handler">The event handler.</param>
        public EventSubscriber(Delegate handler)
        {
            Token = Guid.NewGuid();
            Handler = handler;
        }

        /// <summary>
        /// Gets the token used to identify each subscriber.
        /// </summary>
        internal Guid Token { get; }

        /// <summary>
        /// Gets the event handler.
        /// </summary>
        protected Delegate Handler { get; private set; }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="e">The event.</param>
        public void Handle<TEvent>(TEvent e)
            where TEvent : EventBase
        {
            Task.Run(() =>
            {
                try
                {
                    Handler.DynamicInvoke(e);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error on subscriber during handler invoke for event {nameof(TEvent)} : {ex.Message}, stacktrace : {Environment.NewLine}{ex.StackTrace}");
                }
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Handler = null;
        }
    }
}
