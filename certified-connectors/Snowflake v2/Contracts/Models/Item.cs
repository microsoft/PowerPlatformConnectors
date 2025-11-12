// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;

    /// <summary>
    /// Table item entity
    /// </summary>
    [DataContract]
    public class Item : ICloneable, ICloneable<Item>
    {
        /// <summary>
        /// dynamic properties dictionary
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "dynamicProperties")]
        private IDictionary<string, object> dynamicProperties;

        /// <summary>
        /// Initializes a new instance of item
        /// </summary>
        public Item()
        {
            ItemInternalId = Guid.NewGuid().ToString();
            EntityTag = string.Empty;
            IdLink = null;
            dynamicProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Item constructor with properties
        /// </summary>
        /// <param name="properties">properties</param>
        public Item(IDictionary<string, object> properties)
        {
            ItemInternalId = Guid.NewGuid().ToString();
            EntityTag = string.Empty;
            IdLink = null;
            dynamicProperties = properties;
        }

        /// <summary>
        /// Table item internal id
        /// </summary>
        /// <remarks>
        /// Used as key for entity data model
        /// </remarks>
        [Key]
        [DataMember]
        public string ItemInternalId { get; set; }

        /// <summary>
        /// Item entity tag
        /// </summary>
        /// <remarks>
        /// It is sent in the response as @OData.etag.
        /// </remarks>
        [IgnoreDataMember]
        public string EntityTag { get; set; }

        /// <summary>
        /// Item id link
        /// </summary>
        [IgnoreDataMember]
        public Uri IdLink { get; set; }

        /// <summary>
        /// Open entity type
        /// </summary>
        public IDictionary<string, object> DynamicProperties
        {
            get
            {
                return dynamicProperties;
            }
        }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public Item Clone()
        {
            return new Item()
            {
                ItemInternalId = ItemInternalId,
                EntityTag = EntityTag,
                IdLink = IdLink,
                dynamicProperties = DictionaryCloneHelper(),
            };

            Dictionary<string, object> DictionaryCloneHelper()
            {
                var cloned = new Dictionary<string, object>();

                foreach (var value in dynamicProperties)
                {
                    if (value.Value is ICloneable clonableType)
                    {
                        cloned.Add(value.Key, clonableType.Clone());
                    }
                    else
                    {
                        cloned.Add(value.Key, value.Value);
                    }
                }

                return cloned;
            }
        }
    }
}
