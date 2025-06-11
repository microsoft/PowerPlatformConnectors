// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;

    /// <summary>
    /// Metadata for a table (filter restrictions)
    /// </summary>
    [DataContract]
    public class TableFilterRestrictionsMetadata : ICloneable, ICloneable<TableFilterRestrictionsMetadata>, IEquatable<TableFilterRestrictionsMetadata>
    {
        /// <summary>
        /// Indicates whether this table has filterable columns
        /// </summary>
        /// <value>
        ///   <c>true</c> if filterable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "filterable")]
        public bool Filterable { get; set; }

        /// <summary>
        /// List of non filterable properties
        /// </summary>
        [DataMember(Name = "nonFilterableProperties", EmitDefaultValue = false)]
        public string[] NonFilterableProperties { get; set; }

        /// <summary>
        /// List of required properties
        /// </summary>
        [DataMember(Name = "requiredProperties", EmitDefaultValue = false)]
        public string[] RequiredProperties { get; set; }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public TableFilterRestrictionsMetadata Clone()
        {
            return new TableFilterRestrictionsMetadata
            {
                Filterable = Filterable,
                NonFilterableProperties = (string[])NonFilterableProperties?.Clone(),
                RequiredProperties = (string[])RequiredProperties?.Clone(),
            };
        }

        /// <inheritdoc/>
        public bool Equals(TableFilterRestrictionsMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            bool isEqual = true;

            isEqual = isEqual && Filterable == other.Filterable;
            isEqual = isEqual && StringArrayEquals(NonFilterableProperties, other.NonFilterableProperties);
            isEqual = isEqual && StringArrayEquals(RequiredProperties, other.RequiredProperties);

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
