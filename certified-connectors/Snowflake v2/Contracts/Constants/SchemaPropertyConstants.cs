// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants
{
    /// <summary>
    /// Defines the schema property constants
    /// </summary>
    public static class SchemaPropertyConstants
    {
        /// <summary>
        /// Property description.
        /// </summary>
        public const string Description = "description";

        /// <summary>
        /// Property type.
        /// </summary>
        public const string Type = "type";

        /// <summary>
        /// Property format.
        /// </summary>
        public const string Format = "format";

        /// <summary>
        /// Property title.
        /// </summary>
        public const string Title = "title";

        /// <summary>
        /// Required properties.
        /// </summary>
        public const string Required = "required";

        /// <summary>
        /// Max length.
        /// </summary>
        public const string MaxLength = "maxLength";

        /// <summary>
        /// Minimum value.
        /// </summary>
        public const string Minimum = "minimum";

        /// <summary>
        /// Maximum value.
        /// </summary>
        public const string Maximum = "maximum";

        /// <summary>
        /// Object properties.
        /// </summary>
        public const string Properties = "properties";

        /// <summary>
        /// Object properties.
        /// </summary>
        public const string AdditionalProperties = "additionalProperties";

        /// <summary>
        /// Array items.
        /// </summary>
        public const string Items = "items";

        /// <summary>
        /// Specifies the list of entries a field can take.
        /// </summary>
        public const string Enum = "enum";

        /// <summary>
        /// Column visibility.
        /// </summary>
        public const string Visibility = "x-ms-visibility";

        /// <summary>
        /// Key type.
        /// </summary>
        public const string KeyType = "x-ms-keyType";

        /// <summary>
        /// Key order.
        /// </summary>
        public const string KeyOrder = "x-ms-keyOrder";

        /// <summary>
        /// Permission - Read-only or Read-write.
        /// </summary>
        public const string Permission = "x-ms-permission";

        /// <summary>
        /// Sort by.
        /// </summary>
        public const string Sort = "x-ms-sort";

        /// <summary>
        /// Display format. This is used by Salesforce to format numbers as a percentage.
        /// </summary>
        public const string DisplayFormat = "x-ms-display-format";

        /// <summary>
        /// Currency Code Field format.
        /// </summary>
        public const string CurrencyCodeField = "x-ms-currency-code-field";

        /// <summary>
        /// Default Value.
        /// </summary>
        public const string Default = "default";

        /// <summary>
        /// Value that corresponds to internal visibility
        /// </summary>
        public const string VisibilityInternal = "internal";

        /// <summary>
        /// The capabilities
        /// </summary>
        public const string Capabilities = "x-ms-capabilities";

        /// <summary>
        /// The media kind
        /// </summary>
        public const string MediaKind = "x-ms-media-kind";

        /// <summary>
        /// The summary
        /// </summary>
        public const string Summary = "x-ms-summary";

        /// <summary>
        /// The property title tag for PowerApps.
        /// </summary>
        public const string PropertyTitle = "titleProperty";

        /// <summary>
        /// The property compact display order tag for PowerApps.
        /// </summary>
        public const string PropertyCompactDisplayOrder = "propertiesCompactDisplayOrder";

        /// <summary>
        /// The property display order tag for PowerApps.
        /// </summary>
        public const string PropertyDisplayOrder = "propertiesDisplayOrder";

        /// <summary>
        /// The property tabular display order tag for PowerApps.
        /// </summary>
        public const string PropertyTabularDisplayOrder = "propertiesTabularDisplayOrder";

        /// <summary>
        /// Display order for PowerApps. This is used to specify ordered lists of field names in the metadata.
        /// </summary>
        public const string PowerAppsDisplayOrder = "x-ms-displayFormat";

        /// <summary>
        /// Url encoding type.
        /// </summary>
        public const string UrlEncoding = "x-ms-url-encoding";
    }
}
