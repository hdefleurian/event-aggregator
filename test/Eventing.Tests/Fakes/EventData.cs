// <copyright file="EventData.cs" company="Hubert de Fleurian">
// Copyright (c) Hubert de Fleurian. All rights reserved.
// </copyright>

namespace Eventing.Tests.Fakes
{
    using System;
    using System.IO;

    /// <summary>
    /// The event data for the <see cref="EventArgs"/> class.
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// Gets or sets the test integer.
        /// </summary>
        public int TestInteger { get; set; }

        /// <summary>
        /// Gets or sets the test string.
        /// </summary>
        public string TestStr { get; set; }

        /// <summary>
        /// Gets or sets the test unique identifier.
        /// </summary>
        public Guid TestGuid { get; set; }

        /// <summary>
        /// Generates an random instance of <see cref="EventData"/>.
        /// </summary>
        /// <returns>The generated instance.</returns>
        public static EventData Random()
        {
            Random rng = new Random();

            return new EventData { TestInteger = rng.Next(), TestStr = Path.GetRandomFileName(), TestGuid = Guid.NewGuid() };
        }
    }
}
