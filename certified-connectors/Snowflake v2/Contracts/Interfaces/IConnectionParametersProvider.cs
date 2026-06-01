// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    /// <summary>
    /// Classes which implement this interface can provide the parameters and authentication token required to establish connection.
    /// </summary>
    public interface IConnectionParametersProvider
    {
        /// <summary>
        /// Get the connection property value for the given key.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="key">The identifier of the value being requested.</param>
        /// <returns>The connection property if it exists.</returns>
        public T GetProperty<T>(string key);

        /// <summary>
        /// Check if the connection property exists for the given key.
        /// </summary>
        /// <param name="key">The identifier of the value being requested.</param>
        /// <returns>True if the property exists, otherwise false.</returns>
        public bool PropertyExists(string key);

        /// <summary>
        /// Try to get the connection property value for the given key.
        /// </summary>
        /// <typeparam name="T">The expected value type.</typeparam>
        /// <param name="key">The identifier of the value being requested.</param>
        /// <param name="value">The value of the property if it exists.</param>
        /// <returns>True if the property exists, otherwise false.</returns>
        public bool TryGetProperty<T>(string key, out T value);

        /// <summary>
        /// Get the authentication token to establish connection.
        /// </summary>
        /// <returns>the instance of the <see cref="IToken"/> to use for establishing a connection.</returns>
        public IToken GetToken();

        /// <summary>
        /// Get the referrer Uri.
        /// </summary>
        /// <returns>The referrer URI for the connection.</returns>
        public Uri GetReferrerUri();
    }
}
