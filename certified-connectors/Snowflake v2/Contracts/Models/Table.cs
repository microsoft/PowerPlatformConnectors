// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Web.OData.Builder;

    /// <summary>
    /// Represents a table.
    /// </summary>
    [DataContract]
    public class Table
    {
        /// <summary>
        /// Table items
        /// </summary>
        private IList<Item> items;

        /// <summary>
        /// Additional metadata provided by the connector to the clients.
        /// </summary>
        private IDictionary<string, object> dynamicProperties;

        /// <summary>
        /// Initializes a new instance of Table
        /// </summary>
        public Table()
        {
            items = new List<Item>();
        }

        /// <summary>
        /// The name of the table. The name is used at runtime.
        /// </summary>
        [Key]
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The display name of the table.
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Additional table properties provided by the connector to the clients.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        [Newtonsoft.Json.JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IDictionary<string, object> DynamicProperties
        {
            get
            {
                return dynamicProperties;
            }
        }

        /// <summary>
        /// Table items
        /// </summary>
        [Contained]
        [DataMember(Name = "items")]
        public IList<Item> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Table items
        /// </summary>
        protected IList<Item> ItemsInternal
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        /// <summary>
        /// Adds a new property to the table. Useful if the connector wants to add
        /// additional properties about each table to the clients.
        /// </summary>
        /// <param name="name">The name for the new table property.</param>
        /// <param name="value">The value of the new table property.</param>
        public void AddProperty(string name, object value)
        {
            if (dynamicProperties == null)
            {
                dynamicProperties = new Dictionary<string, object>();
            }

            dynamicProperties.Add(name, value);
        }
    }
}