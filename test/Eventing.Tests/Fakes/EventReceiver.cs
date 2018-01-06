// <copyright file="EventReceiver.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Tests.Fakes
{
    using System;
    using System.Threading;

    using Eventing.Abstractions;

    /// <summary>
    /// A receiver of event to be used in the unit test classes.
    /// </summary>
    public class EventReceiver
    {
        private readonly ManualResetEventSlim _receivedSignal;
        private readonly IEventSubscriber _subscriber;
        private readonly Action _onReceivedAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventReceiver"/> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public EventReceiver(IEventSubscriber subscriber)
            : this(subscriber, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventReceiver" /> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="onReceivedAction">The action to execute when the event has been received.</param>
        public EventReceiver(IEventSubscriber subscriber, Action onReceivedAction)
        {
            _subscriber = subscriber;
            _receivedSignal = new ManualResetEventSlim(false);
            _onReceivedAction = onReceivedAction;
        }

        /// <summary>
        /// Gets a value indicating whether an event has been received.
        /// </summary>
        public bool EventReceived => _receivedSignal.IsSet;

        /// <summary>
        /// Gets the event data.
        /// </summary>
        public EventData Data { get; private set; }

        /// <summary>
        /// Subscribes to the event using the <see cref="IEventSubscriber"/> instance.
        /// </summary>
        /// <returns>The instance to be disposed for unsubscribe.</returns>
        public IDisposable Subscribe()
        {
            return _subscriber.Subscribe<EventArgs>(OnReceive);
        }

        /// <summary>
        /// Waits for the event to be published until the specified timeout is reached.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void Wait(int timeout = 250)
        {
            _receivedSignal.Wait(timeout);
        }

        /// <summary>
        /// Raises the <see cref="E:Receive" /> event.
        /// </summary>
        /// <param name="obj">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnReceive(EventArgs obj)
        {
            _onReceivedAction?.Invoke();

            _receivedSignal.Set();

            Data = obj.Data;
        }
    }
}
