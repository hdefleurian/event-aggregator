// <copyright file="EventAggregatorTests.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Eventing.Abstractions;
    using Eventing.Tests.Fakes;

    using Xunit;

    /// <summary>
    /// Test class for the <see cref="EventAggregator"/> class.
    /// </summary>
    public class EventAggregatorTests
    {
        /// <summary>
        /// Test that a call to Dispose() method with single subscriber should not raise exception.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_DisposeWithSingleSubscriber_ShouldNotRaiseException")]
        public void CheckDisposeWithSingleSubscriberShouldNotRaiseException()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            eventAggregator.Subscribe<FakeEventArgs>(e => { /* Empty handler */ });

            // Act
            eventAggregator.Dispose();
        }

        /// <summary>
        /// Test that a call to Dispose() method with multiple subscribers should not raise exception.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_DisposeWithMultipleSubscribers_ShouldNotRaiseException")]
        public void CheckDisposeWithMultipleSubscribersShouldNotRaiseException()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            eventAggregator.Subscribe<FakeEventArgs>(e => { /* Empty handler */ });
            eventAggregator.Subscribe<OtherFakeEventArgs>(e => { /* Empty handler */ });

            // Act
            eventAggregator.Dispose();
        }

        /// <summary>
        /// Test that a subscribe then a publish should receive event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_PublishNullEvent_ShouldRaiseException")]
        public void CheckPublishNullEventShouldRaiseException()
        {
            // Arrange
            var eventAggregator = new EventAggregator();

            // Act
            Action action = () => eventAggregator.Publish<FakeEventArgs>(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);

            // Cleanup
            eventAggregator.Dispose();
        }

        /// <summary>
        /// Test that a subscribe then a publish should receive event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscribeNullHandler_ShouldRaiseException")]
        public void CheckSubscribeNullHandlerShouldRaiseException()
        {
            // Arrange
            var eventAggregator = new EventAggregator();

            // Act
            Action action = () => eventAggregator.Subscribe<FakeEventArgs>(null);

            // Assert
            Assert.Throws<ArgumentNullException>(action);

            // Cleanup
            eventAggregator.Dispose();
        }

        /// <summary>
        /// Test that a subscribe then a publish should receive event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscribeWithExceptionInHandler_ShouldNotRaiseException")]
        public void CheckSubscribeWithExceptionInHandlerShouldNotRaiseException()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var receiver = new EventReceiver(eventAggregator, () => throw new Exception("Test"));

            var data = EventData.Random();

            // Act
            var unsubscriber = receiver.Subscribe();
            source.Emit(data);

            // Cleanup
            unsubscriber.Dispose();
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that a subscribe then a publish should receive event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscribeThenPublish_ShouldReceiveEvent")]
        public void CheckSubscribeThenPublishShouldReceiveEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var receiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            var unsubscriber = receiver.Subscribe();
            source.Emit(data);

            receiver.Wait();

            // Assert
            Assert.True(receiver.EventReceived);
            Assert.Equal(data, receiver.Data);
            Assert.Equal(data.TestGuid, receiver.Data.TestGuid);
            Assert.Equal(data.TestInteger, receiver.Data.TestInteger);
            Assert.Equal(data.TestStr, receiver.Data.TestStr);

            // Cleanup
            unsubscriber.Dispose();
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that a publish without any subscriber should not received event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_PublisherWithoutSubscriber_ShouldNotReceivedEvent")]
        public void CheckPublisherWithoutSubscriberShouldNotReceivedEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var receiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            source.Emit(data);

            receiver.Wait();

            // Assert
            Assert.False(receiver.EventReceived);
            Assert.Null(receiver.Data);

            // Cleanup
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that a subscribe without any publisher should not received event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscriberWithoutPublisher_ShouldNotReceivedEvent")]
        public void CheckSubscriberWithoutPublisherShouldNotReceivedEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var receiver = new EventReceiver(eventAggregator);

            // Act
            var unsubscriber = receiver.Subscribe();

            receiver.Wait();

            // Assert
            Assert.False(receiver.EventReceived);
            Assert.Null(receiver.Data);

            // Cleanup
            unsubscriber.Dispose();
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that a subscribe after a publish should not received event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscribeAfterPublish_ShouldNotReceivedEvent")]
        public void CheckSubscribeAfterPublishShouldNotReceivedEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var receiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            source.Emit(data);

            var unsubscriber = receiver.Subscribe();
            receiver.Wait();

            // Assert
            Assert.False(receiver.EventReceived);
            Assert.Null(receiver.Data);

            // Cleanup
            unsubscriber.Dispose();
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that a subscribe then the unsubscribe should not received event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscribeThenUnsubscribe_ShouldNotReceivedEvent")]
        public void CheckSubscribeThenUnsubscribeShouldNotReceivedEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var receiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            var unsubscriber = receiver.Subscribe();
            unsubscriber.Dispose();

            source.Emit(data);

            receiver.Wait();

            // Assert
            Assert.False(receiver.EventReceived);
            Assert.Null(receiver.Data);

            // Cleanup
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that two subscribers should receive same event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_TwoSubscribers_ShouldReceiveSameEvent")]
        public void CheckTwoSubscribersShouldReceiveSameEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var firstReceiver = new EventReceiver(eventAggregator);
            var secondReceiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            var firstUnsubscriber = firstReceiver.Subscribe();
            var secondUnsubscriber = secondReceiver.Subscribe();
            source.Emit(data);

            firstReceiver.Wait();
            secondReceiver.Wait();

            // Assert
            Assert.True(firstReceiver.EventReceived);
            Assert.Equal(data, firstReceiver.Data);
            Assert.Equal(data.TestGuid, firstReceiver.Data.TestGuid);
            Assert.Equal(data.TestInteger, firstReceiver.Data.TestInteger);
            Assert.Equal(data.TestStr, firstReceiver.Data.TestStr);

            Assert.True(secondReceiver.EventReceived);
            Assert.Equal(data, secondReceiver.Data);
            Assert.Equal(data.TestGuid, secondReceiver.Data.TestGuid);
            Assert.Equal(data.TestInteger, secondReceiver.Data.TestInteger);
            Assert.Equal(data.TestStr, secondReceiver.Data.TestStr);

            // Cleanup
            firstUnsubscriber.Dispose();
            secondUnsubscriber.Dispose();
            eventAggregator.Dispose();
            firstReceiver.Dispose();
            secondReceiver.Dispose();
        }

        /// <summary>
        /// Test that a subscribe then a publish should receive event.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_SubscribeThenPublishFromAnotherThread_ShouldReceiveEvent")]
        public void CheckSubscribeThenPublishFromAnotherThreadShouldReceiveEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var receiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            var unsubscriber = receiver.Subscribe();

            Task.Run(() =>
            {
                source.Emit(data);
            });

            receiver.Wait();

            // Assert
            Assert.True(receiver.EventReceived);
            Assert.Equal(data, receiver.Data);
            Assert.Equal(data.TestGuid, receiver.Data.TestGuid);
            Assert.Equal(data.TestInteger, receiver.Data.TestInteger);
            Assert.Equal(data.TestStr, receiver.Data.TestStr);

            // Cleanup
            unsubscriber.Dispose();
            eventAggregator.Dispose();
            receiver.Dispose();
        }

        /// <summary>
        /// Test that a long running handler should not block other handler.
        /// </summary>
        [Fact(DisplayName = "EventAggregator_LongRunningHandler_ShouldNotBlockOtherHandler")]
        public void CheckLongRunningHandlerShouldNotBlockOtherHandler()
        {
            // Arrange
            var timerElapsed = new ManualResetEventSlim(false);
            var timer = new Timer(
                _ =>
                {
                    timerElapsed.Set();
                },
                null,
                1000,
                0);

            var eventAggregator = new EventAggregator();
            var source = new EventSource(eventAggregator);
            var firstReceiver = new EventReceiver(eventAggregator, () =>
            {
                timerElapsed.Wait();
            });
            var secondReceiver = new EventReceiver(eventAggregator);

            var data = EventData.Random();

            // Act
            var firstUnsubscriber = firstReceiver.Subscribe();
            var secondUnsubscriber = secondReceiver.Subscribe();
            source.Emit(data);

            firstReceiver.Wait();
            secondReceiver.Wait();

            // Assert
            Assert.False(firstReceiver.EventReceived);
            Assert.True(secondReceiver.EventReceived);

            Assert.Null(firstReceiver.Data);
            Assert.Equal(data, secondReceiver.Data);
            Assert.Equal(data.TestGuid, secondReceiver.Data.TestGuid);
            Assert.Equal(data.TestInteger, secondReceiver.Data.TestInteger);
            Assert.Equal(data.TestStr, secondReceiver.Data.TestStr);

            firstReceiver.Wait(1000);

            Assert.True(firstReceiver.EventReceived);

            Assert.Equal(data, firstReceiver.Data);
            Assert.Equal(data.TestGuid, firstReceiver.Data.TestGuid);
            Assert.Equal(data.TestInteger, firstReceiver.Data.TestInteger);
            Assert.Equal(data.TestStr, firstReceiver.Data.TestStr);

            // Cleanup
            firstUnsubscriber.Dispose();
            secondUnsubscriber.Dispose();
            eventAggregator.Dispose();
            firstReceiver.Dispose();
            secondReceiver.Dispose();

            timer.Dispose();
            timerElapsed.Dispose();
        }

#pragma warning disable CA1812
        private class FakeEventArgs : EventBase
        {
        }

        private class OtherFakeEventArgs : EventBase
        {
        }
#pragma warning restore CA1812
    }
}