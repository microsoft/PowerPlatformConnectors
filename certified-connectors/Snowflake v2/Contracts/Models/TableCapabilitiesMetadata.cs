// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;

    /// <summary>
    /// Metadata for a table (capabilities)
    /// </summary>
    [DataContract]
    public class TableCapabilitiesMetadata : ICloneable, ICloneable<TableCapabilitiesMetadata>, IEquatable<TableCapabilitiesMetadata>
    {
        /// <summary>
        /// Sort restrictions
        /// </summary>
        [DataMember(Name = "sortRestrictions", EmitDefaultValue = false)]
        public TableSortRestrictionsMetadata SortRestrictions { get; set; }

        /// <summary>
        /// Filter restrictions
        /// </summary>
        [DataMember(Name = "filterRestrictions", EmitDefaultValue = false)]
        public TableFilterRestrictionsMetadata FilterRestrictions { get; set; }

        /// <summary>
        /// Select restrictions
        /// </summary>
        [DataMember(Name = "selectRestrictions", EmitDefaultValue = false)]
        public TableSelectRestrictionsMetadata SelectRestrictions { get; set; }

        /// <summary>
        /// Count restrictions
        /// </summary>
        [DataMember(Name = "countRestrictions", EmitDefaultValue = false)]
        public TableCountRestrictionsMetadata CountRestrictions { get; set; }

        /// <summary>
        /// Server paging restrictions
        /// </summary>
        [DataMember(Name = "isOnlyServerPagable", EmitDefaultValue = false)]
        public bool IsOnlyServerPagable { get; set; }

        /// <summary>
        /// List of supported filter capabilities
        /// </summary>
        [DataMember(Name = "filterFunctionSupport", EmitDefaultValue = false)]
        public CapabilityFilterFunction[] FilterFunctionSupport { get; set; }

        /// <summary>
        /// List of supported server-driven paging capabilities
        /// </summary>
        [DataMember(Name = "serverPagingOptions", EmitDefaultValue = false)]
        public CapabilityPagingFunction[] PagingFunctionSupport { get; set; }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public TableCapabilitiesMetadata Clone()
        {
            return new TableCapabilitiesMetadata()
            {
                SortRestrictions = SortRestrictions?.Clone(),
                FilterRestrictions = FilterRestrictions?.Clone(),
                SelectRestrictions = SelectRestrictions?.Clone(),
                IsOnlyServerPagable = IsOnlyServerPagable,
                FilterFunctionSupport = (CapabilityFilterFunction[])FilterFunctionSupport?.Clone(),
                PagingFunctionSupport = (CapabilityPagingFunction[])PagingFunctionSupport?.Clone(),
            };
        }

        /// <inheritdoc/>
        public bool Equals(TableCapabilitiesMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            bool isEqual = true;

            isEqual = isEqual && (SortRestrictions?.Equals(other.SortRestrictions) ?? other.SortRestrictions == null);
            isEqual = isEqual && (FilterRestrictions?.Equals(other.FilterRestrictions) ?? other.FilterRestrictions == null);
            isEqual = isEqual && (SelectRestrictions?.Equals(other.SelectRestrictions) ?? other.SelectRestrictions == null);
            isEqual = isEqual && IsOnlyServerPagable == other.IsOnlyServerPagable;
            isEqual = isEqual && FilterFuntionSupportEquals();
            isEqual = isEqual && PagingFunctionSupportEquals();

            return isEqual;

            bool FilterFuntionSupportEquals()
            {
                if (FilterFunctionSupport?.Length == other.FilterFunctionSupport?.Length)
                {
                    for (int i = 0; i < FilterFunctionSupport?.Length; i++)
                    {
                        if (FilterFunctionSupport[i] != other.FilterFunctionSupport[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            bool PagingFunctionSupportEquals()
            {
                if (PagingFunctionSupport?.Length == other.PagingFunctionSupport?.Length)
                {
                    for (int i = 0; i < PagingFunctionSupport?.Length; i++)
                    {
                        if (PagingFunctionSupport[i] != other.PagingFunctionSupport[i])
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
