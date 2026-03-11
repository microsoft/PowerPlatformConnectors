// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Filter functions that could be made available via OData
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CapabilityFilterFunction
    {
        /// <summary>
        /// The equal filter predicate
        /// </summary>
        [EnumMember(Value = "eq")]
        Equal,

        /// <summary>
        /// The not equal filter predicate
        /// </summary>
        [EnumMember(Value = "ne")]
        NotEqual,

        /// <summary>
        /// The greater than filter predicate
        /// </summary>
        [EnumMember(Value = "gt")]
        GreaterThan,

        /// <summary>
        /// The greater than or equal filter predicate
        /// </summary>
        [EnumMember(Value = "ge")]
        GreaterThanOrEqual,

        /// <summary>
        /// The less than filter predicate
        /// </summary>
        [EnumMember(Value = "lt")]
        LessThan,

        /// <summary>
        /// The less than or equal filter predicate
        /// </summary>
        [EnumMember(Value = "le")]
        LessThanOrEqual,

        /// <summary>
        /// The and filter operand
        /// </summary>
        [EnumMember(Value = "and")]
        And,

        /// <summary>
        /// The or filter operand
        /// </summary>
        [EnumMember(Value = "or")]
        Or,

        /// <summary>
        /// The contains filter predicate
        /// </summary>
        [EnumMember(Value = "contains")]
        Contains,

        /// <summary>
        /// The starts with predicate
        /// </summary>
        [EnumMember(Value = "startswith")]
        StartsWith,

        /// <summary>
        /// The ends with predicate
        /// </summary>
        [EnumMember(Value = "endswith")]
        EndsWith,

        /// <summary>
        /// The length filter function
        /// </summary>
        [EnumMember(Value = "length")]
        Length,

        /// <summary>
        /// The index of filter predicate
        /// </summary>
        [EnumMember(Value = "indexof")]
        IndexOf,

        /// <summary>
        /// The replace filter function
        /// </summary>
        [EnumMember(Value = "replace")]
        Replace,

        /// <summary>
        /// The substring filter function (OData v4)
        /// </summary>
        [EnumMember(Value = "substring")]
        Substring,

        /// <summary>
        /// The substring of filter function (OData v2)
        /// </summary>
        [EnumMember(Value = "substringof")]
        SubstringOf,

        /// <summary>
        /// The to lower filter function
        /// </summary>
        [EnumMember(Value = "tolower")]
        ToLower,

        /// <summary>
        /// The to upper filter function
        /// </summary>
        [EnumMember(Value = "toupper")]
        ToUpper,

        /// <summary>
        /// The trim filter function
        /// </summary>
        [EnumMember(Value = "trim")]
        Trim,

        /// <summary>
        /// The concatenate filter function
        /// </summary>
        [EnumMember(Value = "concat")]
        Concat,

        /// <summary>
        /// The year filter function
        /// </summary>
        [EnumMember(Value = "year")]
        Year,

        /// <summary>
        /// The month filter function
        /// </summary>
        [EnumMember(Value = "month")]
        Month,

        /// <summary>
        /// The day filter function
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// The hour filter function
        /// </summary>
        [EnumMember(Value = "hour")]
        Hour,

        /// <summary>
        /// The minute filter function
        /// </summary>
        [EnumMember(Value = "minute")]
        Minute,

        /// <summary>
        /// The second filter function
        /// </summary>
        [EnumMember(Value = "second")]
        Second,

        /// <summary>
        /// The date filter function
        /// </summary>
        [EnumMember(Value = "date")]
        Date,

        /// <summary>
        /// The time filter function
        /// </summary>
        [EnumMember(Value = "time")]
        Time,

        /// <summary>
        /// The now filter function
        /// </summary>
        [EnumMember(Value = "now")]
        Now,

        /// <summary>
        /// The total offset minutes filter function
        /// </summary>
        [EnumMember(Value = "totaloffsetminutes")]
        TotalOffsetMinutes,

        /// <summary>
        /// The total seconds filter function
        /// </summary>
        [EnumMember(Value = "totalseconds")]
        TotalSeconds,

        /// <summary>
        /// The floor filter function
        /// </summary>
        [EnumMember(Value = "floor")]
        Floor,

        /// <summary>
        /// The ceiling filter function
        /// </summary>
        [EnumMember(Value = "ceiling")]
        Ceiling,

        /// <summary>
        /// The round filter function
        /// </summary>
        [EnumMember(Value = "round")]
        Round,

        /// <summary>
        /// The not unary filter function
        /// </summary>
        [EnumMember(Value = "not")]
        Not,

        /// <summary>
        /// The negate unary operator (-)
        /// </summary>
        [EnumMember(Value = "negate")]
        Negate,

        /// <summary>
        /// The addition binary operator (+)
        /// </summary>
        [EnumMember(Value = "add")]
        Addition,

        /// <summary>
        /// The subtraction binary operator (-)
        /// </summary>
        [EnumMember(Value = "sub")]
        Subtraction,

        /// <summary>
        /// The multiplication binary operator (*)
        /// </summary>
        [EnumMember(Value = "mul")]
        Multiplication,

        /// <summary>
        /// The division binary operator (/)
        /// </summary>
        [EnumMember(Value = "div")]
        Division,

        /// <summary>
        /// The modulus filter function (%)
        /// </summary>
        [EnumMember(Value = "mod")]
        Modulo,

        /// <summary>
        /// The summation aggregation
        /// </summary>
        [EnumMember(Value = "sum")]
        Sum,

        /// <summary>
        /// The min aggregation
        /// </summary>
        [EnumMember(Value = "min")]
        Min,

        /// <summary>
        /// The max aggregation
        /// </summary>
        [EnumMember(Value = "max")]
        Max,

        /// <summary>
        /// The average aggregation
        /// </summary>
        [EnumMember(Value = "average")]
        Average,

        /// <summary>
        /// Distinct count aggregation
        /// </summary>
        [EnumMember(Value = "countdistinct")]
        CountDistinct,

        /// <summary>
        /// The null operator
        /// </summary>
        [EnumMember(Value = "null")]
        Null,
    }
}
