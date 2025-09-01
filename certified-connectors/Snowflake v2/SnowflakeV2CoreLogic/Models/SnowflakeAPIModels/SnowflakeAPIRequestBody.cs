// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.SnowflakeAPIModels
{
    using Newtonsoft.Json.Linq;

    public class SnowflakeRequestPostBody
    {
        public string? statement { get; set; } = null;

        public string? schema { get; set; } = null;

        public string? database { get; set; } = null;

        public string? warehouse { get; set; } = null;

        public string? role { get; set; } = null;

        public JObject? bindings { get; set; } = null;

        public RequestParameters? parameters { get; set; } = null;
    }

    public class SnowflakeRequestBindings
    {
        public JObject? bindings { get; set; } = null;

        public void AddTextBinding(int index, string value)
        {
            // Create a new JObject
            JObject textBinding = new JObject
            {
                ["type"] = "TEXT",
                ["value"] = value,
            };

            AddBinding(index, textBinding);
        }

        public void AddBinding(int index, object value)
        {
            AddTextBinding(index, value.ToString());
        }

        private void AddBinding(int index, JObject value)
        {
            if (bindings == null)
            {
                bindings = new JObject();
            }

            bindings[index.ToString()] = value;
        }
    }

    public class RequestParameters
    {
        public int? MULTI_STATEMENT_COUNT { get; set; }
    }
}