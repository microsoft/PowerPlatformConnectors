// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Table metadata
    /// </summary>
    [DataContract]
    public class TableMetadata : ICloneable, ICloneable<TableMetadata>, IEquatable<TableMetadata>
    {
        /// <summary>
        /// Table name
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Table title
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Table permission
        /// </summary>
        [DataMember(Name = "x-ms-permission")]
        public string Permission { get; set; }

        /// <summary>
        /// Table capabilities
        /// </summary>
        [DataMember(Name = "x-ms-capabilities", EmitDefaultValue = false)]
        public TableCapabilitiesMetadata Capabilities { get; set; }

        /// <summary>
        /// Table schema
        /// </summary>
        [DataMember(Name = "schema")]
        public JObject Schema { get; set; } = new JObject();

        /// <summary>
        /// Reference entities
        /// </summary>
        [DataMember(Name = "referencedEntities", EmitDefaultValue = false, IsRequired = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject ReferencedEntities { get; set; }

        /// <summary>
        /// URL link
        /// </summary>
        [DataMember(Name = "webUrl", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WebUrl { get; set; }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public TableMetadata Clone()
        {
            return new TableMetadata()
            {
                Name = Name,
                Title = Title,
                Capabilities = Capabilities?.Clone(),
                Permission = Permission,
                Schema = (JObject)Schema?.DeepClone(),
                ReferencedEntities = (JObject)ReferencedEntities?.DeepClone(),
                WebUrl = WebUrl,
            };
        }

        /// <inheritdoc/>
        public bool Equals(TableMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            bool isEqual = true;
            isEqual = isEqual && CompareAndLog(Name, other.Name, nameof(Name));
            isEqual = isEqual && CompareAndLog(Title, other.Title, nameof(Title));
            isEqual = isEqual && CompareAndLog(Permission, other.Permission, nameof(Permission));
            isEqual = isEqual && CompareAndLog(WebUrl, other.WebUrl, nameof(WebUrl));
            isEqual = isEqual && CompareAndLog(Schema, other.Schema, nameof(Schema));
            isEqual = isEqual && CompareAndLog(ReferencedEntities, other.ReferencedEntities, nameof(ReferencedEntities));
            isEqual = isEqual && CompareAndLog(Capabilities, other.Capabilities, nameof(Capabilities));

            return isEqual;

            static bool CompareAndLog(object thisObject, object otherObject, string property)
            {
                if (thisObject == null)
                {
                    if (otherObject == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (thisObject is string)
                {
                    if (!string.Equals(
                        thisObject as string,
                        otherObject as string,
                        StringComparison.Ordinal))
                    {
                        return false;
                    }
                }
                else if (thisObject is JObject)
                {
                    if (!JToken.DeepEquals(thisObject as JObject, otherObject as JObject))
                    {
                        return false;
                    }
                }
                else if (thisObject is TableCapabilitiesMetadata)
                {
                    if (thisObject == null)
                    {
                        if (otherObject != null)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!(thisObject as TableCapabilitiesMetadata).Equals(otherObject as TableCapabilitiesMetadata))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
        }
    }
}
