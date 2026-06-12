// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;

    /// <summary>
    /// Metadata for a table (sort restrictions)
    /// </summary>
    [DataContract]
    public class TableSortRestrictionsMetadata : ICloneable, ICloneable<TableSortRestrictionsMetadata>, IEquatable<TableSortRestrictionsMetadata>
    {
        /// <summary>
        /// Indicates whether this table has sortable columns
        /// </summary>
        /// <value>
        ///   <c>true</c> if sortable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "sortable")]
        public bool Sortable { get; set; }

        /// <summary>
        /// List of unsortable properties
        /// </summary>
        [DataMember(Name = "unsortableProperties", EmitDefaultValue = false)]
        public string[] UnsortableProperties { get; set; }

        /// <summary>
        /// List of properties which support ascending order only
        /// </summary>
        [DataMember(Name = "ascendingOnlyProperties", EmitDefaultValue = false)]
        public string[] AscendingOnlyProperties { get; set; }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public TableSortRestrictionsMetadata Clone()
        {
            return new TableSortRestrictionsMetadata
            {
                Sortable = Sortable,
                UnsortableProperties = (string[])UnsortableProperties?.Clone(),
                AscendingOnlyProperties = (string[])AscendingOnlyProperties?.Clone(),
            };
        }

        /// <inheritdoc/>
        public bool Equals(TableSortRestrictionsMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            bool isEqual = true;

            isEqual = isEqual && Sortable == other.Sortable;
            isEqual = isEqual && StringArrayEquals(UnsortableProperties, other.UnsortableProperties);
            isEqual = isEqual && StringArrayEquals(AscendingOnlyProperties, other.AscendingOnlyProperties);

            return isEqual;

            static bool StringArrayEquals(string[] thisArray, string[] otherArray)
            {
                if (thisArray?.Length == otherArray?.Length)
                {
                    for (int i = 0; i < thisArray?.Length; i++)
                    {
                        if (thisArray[i] != otherArray[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }
        }
    }
}
