using System.Collections.Generic;
using Newtonsoft.Json;

namespace SnowflakeTestApp.Tests.Infrastructure
{
    /// <summary>
    /// Represents the OData response from the Snowflake connector API
    /// </summary>
    public class ODataResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("value")]
        public List<T> Value { get; set; }
    }
} 