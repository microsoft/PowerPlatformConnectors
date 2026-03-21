// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;

    /// <summary>
    /// Metadata for a table (select restrictions)
    /// </summary>
    [DataContract]
    public class TableSelectRestrictionsMetadata : ICloneable, ICloneable<TableSelectRestrictionsMetadata>, IEquatable<TableSelectRestrictionsMetadata>
    {
        /// <summary>
        /// Indicates whether this table has selectable columns
        /// </summary>
        /// <value>
        ///   <c>true</c> if selectable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "selectable")]
        public bool Selectable { get; set; }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public TableSelectRestrictionsMetadata Clone()
        {
            return new TableSelectRestrictionsMetadata
            {
                Selectable = Selectable,
            };
        }

        /// <inheritdoc/>
        public bool Equals(TableSelectRestrictionsMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            return Selectable == other.Selectable;
        }
    }
}
