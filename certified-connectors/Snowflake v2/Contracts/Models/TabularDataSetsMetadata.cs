// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Tabular dataset metadata
    /// </summary>
    [DataContract]
    public class TabularDataSetsMetadata
    {
        /// <summary>
        /// Creates a new instance of tabular dataset metadata
        /// </summary>
        public TabularDataSetsMetadata()
        {
            UrlEncoding = DataSetsMetadataUrlEncoding.Single;
            DisplayName = "dataset";
            TableDisplayName = ContractResource.TableDefaultDisplayName;
            TablePluralName = ContractResource.TableDefaultPluralName;
        }

        /// <summary>
        /// Dataset source
        /// </summary>
        [DataMember(Name = "source")]
        public string Source { get; set; }

        /// <summary>
        /// Dataset display name
        /// </summary>
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Dataset url encoding
        /// </summary>
        [DataMember(Name = "urlEncoding")]
        public string UrlEncoding { get; set; }

        /// <summary>
        /// Table display name
        /// </summary>
        [DataMember(Name = "tableDisplayName")]
        public string TableDisplayName { get; set; }

        /// <summary>
        /// Table plural display name
        /// </summary>
        [DataMember(Name = "tablePluralName")]
        public string TablePluralName { get; set; }
    }
}
