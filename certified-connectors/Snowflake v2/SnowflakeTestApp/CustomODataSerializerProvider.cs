// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp
{
    using System.Web.OData.Formatter.Serialization;
    using Microsoft.OData.Edm;

    public class CustomODataSerializerProvider : DefaultODataSerializerProvider
    {
        /// <summary>
        /// custom entity type serializer
        /// </summary>
        private CustomODataEntityTypeSerializer customODataEntityTypeSerializer;

        /// <summary>
        /// Initializes a new instance of CustomODataSerializerProvider
        /// </summary>
        public CustomODataSerializerProvider()
        {
            this.customODataEntityTypeSerializer = new CustomODataEntityTypeSerializer(this);
        }

        /// <inheritdoc/>
        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.IsEntity())
            {
                return this.customODataEntityTypeSerializer;
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }
}