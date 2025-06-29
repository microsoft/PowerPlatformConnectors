// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants
{
    /// <summary>
    /// Supported data formats for tabular data
    /// </summary>
    public static class DataFormat
    {
        /// <summary>
        /// The format of the column is date.
        /// </summary>
        public const string Date = "date";

        /// <summary>
        /// The format of the column is time.
        /// </summary>
        public const string Time = "time";

        /// <summary>
        /// The format of the column is date-time.
        /// </summary>
        public const string DateTime = "date-time";

        /// <summary>
        /// The format of the column is date-no-tz.
        /// </summary>
        public const string DateTimeNoZone = "date-no-tz";

        /// <summary>
        /// The format of the column is integer (32 bit).
        /// </summary>
        public const string Int32 = "int32";

        /// <summary>
        /// The format of the column is 64 bit integer.
        /// </summary>
        public const string Int64 = "int64";

        /// <summary>
        /// The format of the column is floating-point single-precision.
        /// </summary>
        public const string Float = "float";

        /// <summary>
        /// The format of the column is a floating-point double-precision.
        /// </summary>
        public const string Double = "double";

        /// <summary>
        /// The format of the column is base64 encoded characters.
        /// </summary>
        public const string Byte = "byte";

        /// <summary>
        /// The format of the column is binary.
        /// </summary>
        public const string Binary = "binary";

        /// <summary>
        /// The format of the column is GUID.
        /// </summary>
        public const string Guid = "guid";

        /// <summary>
        /// The format of the column is URL.
        /// </summary>
        public const string Uri = "uri";

        /// <summary>
        /// The format of the column is phone number.
        /// </summary>
        public const string Phone = "phone";

        /// <summary>
        /// The format of the column is an email address.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// The format of the column is a currency.
        /// </summary>
        public const string Currency = "currency";

        /// <summary>
        /// The format of the column is a password.
        /// </summary>
        public const string Password = "password";

        /// <summary>
        /// The format of the column is a zip code.
        /// </summary>
        public const string ZipCode = "zipcode";

        /// <summary>
        /// The format of the column is a zip code+4.
        /// </summary>
        public const string ZipCode4 = "zipcode4";

        /// <summary>
        /// The format of the column is an social security number.
        /// </summary>
        public const string Ssn = "ssn";

        /// <summary>
        /// The format of the column is a currency code.
        /// </summary>
        public const string CurrencyCode = "currency-code";

        /// <summary>
        /// The format of the column is an IP v4 address.
        /// </summary>
        public const string Ipv4 = "ipv4";

        /// <summary>
        /// The format of the column is an IP v6 address.
        /// </summary>
        public const string Ipv6 = "ipv6";

        /// <summary>
        /// The format of the column is a data URI
        /// </summary>
        public const string DataUri = "datauri";

        /// <summary>
        /// The format for an empty Data Format.
        /// Set as readonly to allow for using string empty constant.
        /// </summary>
        public static readonly string Empty = string.Empty;
    }
}
