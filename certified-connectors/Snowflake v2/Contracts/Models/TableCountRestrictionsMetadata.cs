// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;

    /// <summary>
    /// Metadata for a table (count restrictions)
    /// </summary>
    [DataContract]
    public class TableCountRestrictionsMetadata : ICloneable, ICloneable<TableCountRestrictionsMetadata>, IEquatable<TableCountRestrictionsMetadata>
    {
        /// <summary>
        /// Indicates whether this table has countable columns
        /// </summary>
        /// <value>
        ///   <c>true</c> if countable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "countable")]
        public bool Countable { get; set; }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <inheritdoc/>
        public TableCountRestrictionsMetadata Clone()
        {
            return new TableCountRestrictionsMetadata
            {
                Countable = this.Countable,
            };
        }

        /// <inheritdoc/>
        public bool Equals(TableCountRestrictionsMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            return this.Countable == other.Countable;
        }
    }
}
