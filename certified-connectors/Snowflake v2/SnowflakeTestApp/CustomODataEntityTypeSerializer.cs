// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp
{
    using System;
    using System.Web.OData;
    using System.Web.OData.Formatter.Serialization;
    using Microsoft.OData.Core;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    public class CustomODataEntityTypeSerializer : ODataEntityTypeSerializer
    {
        /// <summary>
        /// Initializes a new instance of CustomODataEntityTypeSerializer
        /// </summary>
        /// <param name="serializerProvider">serializer provider</param>
        public CustomODataEntityTypeSerializer(ODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        {
        }

        /// <inheritdoc/>
        public override ODataEntry CreateEntry(SelectExpandNode selectExpandNode, EntityInstanceContext entityInstanceContext)
        {
            if (selectExpandNode == null)
            {
                throw new ArgumentNullException("selectExpandNode");
            }

            if (entityInstanceContext == null)
            {
                throw new ArgumentNullException("entityInstanceContext");
            }

            ODataEntry entry = base.CreateEntry(selectExpandNode, entityInstanceContext);

            Item item = entityInstanceContext.EntityInstance as Item;
            if (item == null)
            {
                // Can happen if entityInstanceContext.EntityInstance is not an item (Maybe a table). So return default entry 
                return entry;
            }

            entry.ETag = item.EntityTag;
            entry.Id = item.IdLink;

            return entry;
        }
    }
}