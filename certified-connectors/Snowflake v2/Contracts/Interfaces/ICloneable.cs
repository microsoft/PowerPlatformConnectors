// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    /// <summary>
    /// Generic cloneable Interface. Classes which implement this can clone and return the copy of the same object type.
    /// </summary>
    /// <typeparam name="T">The type of objects that is clonable.</typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        T Clone();
    }
}
