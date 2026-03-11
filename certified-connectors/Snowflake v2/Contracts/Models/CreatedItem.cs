// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    /// <summary>
    /// Created Item
    /// </summary>
    /// <typeparam name="TItem">Item entity</typeparam>
    public class CreatedItem<TItem>
    {
        /// <summary>
        /// Item ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Created item
        /// </summary>
        public TItem Item { get; set; }
    }
}
