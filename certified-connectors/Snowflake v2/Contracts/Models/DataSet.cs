// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Web.OData.Builder;

    /// <summary>
    /// Dataset
    /// </summary>
    [DataContract]
    public class DataSet
    {
        /// <summary>
        /// Tables present in the dataset
        /// </summary>
        private IList<Table> tables;

        /// <summary>
        /// Initializes a new instance of Table
        /// </summary>
        public DataSet()
        {
            tables = new List<Table>();
        }

        /// <summary>
        /// Dataset name
        /// </summary>
        [Key]
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Dataset display name
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Tables contained in the dataset
        /// </summary>
        [Contained]
        [DataMember(Name = "tables")]
        public IList<Table> Tables
        {
            get
            {
                return tables;
            }
        }
    }
}
