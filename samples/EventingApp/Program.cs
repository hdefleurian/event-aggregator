// <copyright file="Program.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace EventingApp
{
    using System;
    using System.Threading.Tasks;

    using Eventing;

    /// <summary>
    /// The main program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main()
        {
            var aggregator = new EventAggregator();
            var provider = new LocationTracker(aggregator);

            Console.WriteLine("Start reporter FixedGPS");
            var reporter1 = new LocationReporter(aggregator, "FixedGPS");
            reporter1.Subscribe();

            Console.WriteLine("Start reporter MobileGPS");
            var reporter2 = new LocationReporter(aggregator, "MobileGPS");
            reporter2.Subscribe();

            Console.WriteLine("Send location : Latitude=47.6456, Longitude=-122.1312");
            await Task.Run(() =>
            {
                provider.TrackLocation(new Location(47.6456, -122.1312));
            });

            await Task.Delay(250); // Delay to ensure all active reporters have received the location

            Console.WriteLine("Disable reporter FixedGPS");
            reporter1.Unsubscribe();

            Console.WriteLine("Send location : Latitude=47.6677, Longitude=-122.1199");
            await Task.Run(() =>
            {
                provider.TrackLocation(new Location(47.6677, -122.1199));
            });

            await Task.Delay(250); // Delay to ensure all active reporters have received the location

            Console.WriteLine("Send location : null");
            await Task.Run(() =>
            {
                provider.TrackLocation(null);
            });

            await Task.Delay(250); // Delay to ensure all active reporters have received the location

            Console.WriteLine("Disable reporter MobileGPS");
            reporter2.Unsubscribe();

            Console.WriteLine("Send location : Latitude=0, Longitude=0");
            await Task.Run(() =>
            {
                provider.TrackLocation(new Location(0, 0));
            });

            await Task.Delay(250); // Delay to ensure all active reporters have received the location

            Console.WriteLine("Press a key for exit");
            Console.ReadKey();
        }
    }
}
