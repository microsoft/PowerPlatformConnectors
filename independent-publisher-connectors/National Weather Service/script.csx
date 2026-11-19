using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    private static readonly string SERVER_NAME = "ConnectorMcpServer";
    private static readonly string SERVER_VERSION = "1.0.0";
    
    // Tool definitions - add your custom tool definitions here
    // To add a new tool:
    // 1. Add a JObject to AVAILABLE_TOOLS array with these required properties:
    //    - "name": string - unique identifier for the tool
    //    - "description": string - what the tool does
    //    - "inputSchema": JObject - JSON Schema defining the tool's parameters
    //      Standard schema properties: "type", "properties", "required", "additionalProperties"
    //      Each property should have: "type", "description", and optionally "default", "enum", etc.
    // 2. Create an implementation method following the naming convention below
    private static readonly JArray AVAILABLE_TOOLS = new JArray
    {
        new JObject
        {
            ["name"] = "get_active_alerts",
            ["description"] = "Get all currently active weather alerts from the National Weather Service. Returns alerts with filtering options for area, zone, severity, etc.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["area"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "State/territory code (e.g., 'TX', 'CA', 'FL') to filter alerts by area"
                    },
                    ["zone"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "NWS zone ID (e.g., 'TXZ001') to filter alerts by specific zone"
                    },
                    ["event"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Alert event name to filter by (e.g., 'Tornado Warning', 'Flash Flood Watch')"
                    },
                    ["severity"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Alert severity level: 'Minor', 'Moderate', 'Severe', 'Extreme'"
                    },
                    ["urgency"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Alert urgency level: 'Past', 'Future', 'Expected', 'Immediate'"
                    }
                }
            }
        },
        new JObject
        {
            ["name"] = "get_alerts_for_area",
            ["description"] = "Get active weather alerts for a specific state or territory. Provide a 2-letter state code.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["area"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "2-letter state/territory code (e.g., 'TX', 'CA', 'FL', 'PR')",
                        ["pattern"] = "^[A-Z]{2}$"
                    }
                },
                ["required"] = new JArray { "area" }
            }
        },
        new JObject
        {
            ["name"] = "get_alerts_for_zone",
            ["description"] = "Get active weather alerts for a specific NWS zone (county or forecast zone). Provide a zone ID like 'TXZ001' or 'CAC037'.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["zone_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "NWS zone identifier (e.g., 'TXZ001', 'CAC037'). Format: 2-letter state + C/Z + 3 digits",
                        ["pattern"] = "^[A-Z]{2}[CZ]\\d{3}$"
                    }
                },
                ["required"] = new JArray { "zone_id" }
            }
        },
        new JObject
        {
            ["name"] = "get_latest_observation",
            ["description"] = "Get the latest weather observation from a specific weather station. Provide a station ID (e.g., airport codes like 'KDFW', 'KLAX').",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["station_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Weather station identifier (e.g., 'KDFW' for Dallas Fort Worth, 'KLAX' for Los Angeles)"
                    },
                    ["require_qc"] = new JObject
                    {
                        ["type"] = "boolean",
                        ["description"] = "Whether to require quality control on the observation data",
                        ["default"] = false
                    }
                },
                ["required"] = new JArray { "station_id" }
            }
        },
        new JObject
        {
            ["name"] = "get_point_metadata",
            ["description"] = "Get NWS metadata for a specific latitude/longitude point. This returns the associated NWS office, grid coordinates, and zone information for the location.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["latitude"] = new JObject
                    {
                        ["type"] = "number",
                        ["description"] = "Latitude coordinate (decimal degrees, e.g., 32.7767)",
                        ["minimum"] = -90,
                        ["maximum"] = 90
                    },
                    ["longitude"] = new JObject
                    {
                        ["type"] = "number",
                        ["description"] = "Longitude coordinate (decimal degrees, e.g., -96.7970)",
                        ["minimum"] = -180,
                        ["maximum"] = 180
                    }
                },
                ["required"] = new JArray { "latitude", "longitude" }
            }
        },
        new JObject
        {
            ["name"] = "get_gridpoint_forecast",
            ["description"] = "Get a detailed textual forecast for a specific 2.5km grid area. Requires NWS office code and grid coordinates (get these from get_point_metadata first).",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["wfo"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "NWS Weather Forecast Office identifier (e.g., 'FWD', 'LOX')",
                        ["pattern"] = "^[A-Z]{3}$"
                    },
                    ["x"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Grid X coordinate",
                        ["minimum"] = 0
                    },
                    ["y"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Grid Y coordinate",
                        ["minimum"] = 0
                    },
                    ["units"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Unit system for the forecast",
                        ["enum"] = new JArray { "us", "si" },
                        ["default"] = "us"
                    }
                },
                ["required"] = new JArray { "wfo", "x", "y" }
            }
        },
        new JObject
        {
            ["name"] = "get_zone_forecast",
            ["description"] = "Get the official NWS forecast for a specific zone (county or forecast area). Provide zone type and zone ID.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["zone_type"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Type of zone",
                        ["enum"] = new JArray { "land", "marine", "forecast", "public", "coastal", "offshore", "fire", "county" },
                        ["default"] = "forecast"
                    },
                    ["zone_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "NWS zone identifier (e.g., 'TXZ001', 'CAC037')",
                        ["pattern"] = "^[A-Z]{2}[CZ]\\d{3}$"
                    }
                },
                ["required"] = new JArray { "zone_type", "zone_id" }
            }
        },
        new JObject
        {
            ["name"] = "get_observation_stations",
            ["description"] = "Find weather observation stations. Filter by state, station ID, or get all available stations in an area.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["state"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "2-letter state/territory code to filter stations (e.g., 'TX', 'CA', 'FL')",
                        ["pattern"] = "^[A-Z]{2}$"
                    },
                    ["station_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Specific station identifier to search for (e.g., 'KDFW', 'KLAX')"
                    },
                    ["limit"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Maximum number of stations to return",
                        ["minimum"] = 1,
                        ["maximum"] = 500,
                        ["default"] = 20
                    }
                }
            }
        },
        new JObject
        {
            ["name"] = "get_zones",
            ["description"] = "Get NWS zones (forecast areas, counties, marine zones). Filter by area, type, or point location.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["area"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "2-letter state/territory code to filter zones (e.g., 'TX', 'CA', 'FL')",
                        ["pattern"] = "^[A-Z]{2}$"
                    },
                    ["zone_type"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Type of zones to retrieve",
                        ["enum"] = new JArray { "land", "marine", "forecast", "public", "coastal", "offshore", "fire", "county" }
                    },
                    ["point"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Point coordinates as 'latitude,longitude' to find zones containing this point",
                        ["pattern"] = "^(-?\\d+(?:\\.\\d+)?),(-?\\d+(?:\\.\\d+)?)$"
                    },
                    ["limit"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Maximum number of zones to return",
                        ["minimum"] = 1,
                        ["default"] = 20
                    }
                }
            }
        },
        new JObject
        {
            ["name"] = "get_hourly_forecast",
            ["description"] = "Get detailed hourly weather forecast for a specific 2.5km grid area. Requires NWS office code and grid coordinates.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["wfo"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "NWS Weather Forecast Office identifier (e.g., 'FWD', 'LOX')",
                        ["pattern"] = "^[A-Z]{3}$"
                    },
                    ["x"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Grid X coordinate",
                        ["minimum"] = 0
                    },
                    ["y"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Grid Y coordinate",
                        ["minimum"] = 0
                    },
                    ["units"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Unit system for the forecast",
                        ["enum"] = new JArray { "us", "si" },
                        ["default"] = "us"
                    }
                },
                ["required"] = new JArray { "wfo", "x", "y" }
            }
        },
        new JObject
        {
            ["name"] = "get_station_observations",
            ["description"] = "Get historical weather observations from a specific station over a time period. Useful for trends and historical data.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["station_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Weather station identifier (e.g., 'KDFW', 'KLAX')"
                    },
                    ["start_time"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Start time for observations (ISO 8601 format, e.g., '2024-01-01T00:00:00Z')",
                        ["format"] = "date-time"
                    },
                    ["end_time"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "End time for observations (ISO 8601 format, e.g., '2024-01-02T00:00:00Z')",
                        ["format"] = "date-time"
                    },
                    ["limit"] = new JObject
                    {
                        ["type"] = "integer",
                        ["description"] = "Maximum number of observations to return",
                        ["minimum"] = 1,
                        ["maximum"] = 500,
                        ["default"] = 48
                    }
                },
                ["required"] = new JArray { "station_id" }
            }
        },
        new JObject
        {
            ["name"] = "get_terminal_forecast",
            ["description"] = "Get Terminal Aerodrome Forecast (TAF) for aviation planning at airports. Provides detailed aviation weather conditions.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["station_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Airport station identifier (e.g., 'KDFW', 'KLAX', 'KORD')"
                    }
                },
                ["required"] = new JArray { "station_id" }
            }
        },
        new JObject
        {
            ["name"] = "get_marine_alerts",
            ["description"] = "Get active weather alerts for marine regions (coastal and offshore areas). Important for maritime activities.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["region"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Marine region identifier",
                        ["enum"] = new JArray { "AL", "AT", "GL", "GM", "PA", "PI" },
                        ["default"] = "AL"
                    }
                },
                ["required"] = new JArray { "region" }
            }
        },
        new JObject
        {
            ["name"] = "get_office_headlines",
            ["description"] = "Get official news headlines and statements from a specific NWS forecast office. Provides important weather communications.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["office_id"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "NWS forecast office identifier (e.g., 'FWD', 'LOX', 'BOX')",
                        ["pattern"] = "^[A-Z]{3}$"
                    }
                },
                ["required"] = new JArray { "office_id" }
            }
        },
        new JObject
        {
            ["name"] = "get_alert_types",
            ["description"] = "Get a list of all available weather alert types and event names recognized by the National Weather Service.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    // No parameters needed - returns all alert types
                }
            }
        },
        new JObject
        {
            ["name"] = "get_weather_glossary",
            ["description"] = "Get weather terminology definitions from the NWS glossary. Search for specific terms or get all definitions.",
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["search_term"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Optional search term to filter glossary entries (e.g., 'heat index', 'tornado', 'humidity')"
                    }
                }
            }
        }
    };
    
    // Tool implementations - add your custom tool logic here
    // For each tool in AVAILABLE_TOOLS, add a corresponding implementation method
    // Method name MUST be: Execute{ToolName}Tool where {ToolName} is PascalCase version of tool name
    // Example: "hello_world" tool -> ExecuteHelloWorldTool method
    // Method signature: private async Task<JObject> Execute{ToolName}Tool(JObject arguments)
    // Return: JObject with content array (use MCP format: { "content": [{ "type": "text", "text": "..." }] })
    
    private async Task<JObject> ExecuteGetActiveAlertsTool(JObject arguments)
    {
        var queryParams = new List<string>();
        
        // Build query parameters from arguments
        var area = arguments.GetValue("area")?.ToString();
        if (!string.IsNullOrEmpty(area))
            queryParams.Add($"area={Uri.EscapeDataString(area)}");
        
        var zone = arguments.GetValue("zone")?.ToString();
        if (!string.IsNullOrEmpty(zone))
            queryParams.Add($"zone={Uri.EscapeDataString(zone)}");
        
        var eventName = arguments.GetValue("event")?.ToString();
        if (!string.IsNullOrEmpty(eventName))
            queryParams.Add($"event={Uri.EscapeDataString(eventName)}");
        
        var severity = arguments.GetValue("severity")?.ToString();
        if (!string.IsNullOrEmpty(severity))
            queryParams.Add($"severity={Uri.EscapeDataString(severity)}");
        
        var urgency = arguments.GetValue("urgency")?.ToString();
        if (!string.IsNullOrEmpty(urgency))
            queryParams.Add($"urgency={Uri.EscapeDataString(urgency)}");
        
        var endpoint = "/alerts/active";
        if (queryParams.Count > 0)
        {
            endpoint += "?" + string.Join("&", queryParams);
        }
        
        var response = await MakeNWSApiCall(endpoint);
        var features = response["features"] as JArray;

        if (features != null && features.Count > 0)
        {
            var alertList = features.Take(10).Select(alert => 
            {
                var props = alert["properties"];
                return $"• {props["event"]}: {props["headline"]}\n" +
                       $"  Areas: {string.Join(", ", props["areaDesc"].ToString().Split(';').Take(3))}\n" +
                       $"  Severity: {props["severity"]} | Urgency: {props["urgency"]}";
            });

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Active Weather Alerts ({features.Count} total):\n\n" +
                                  string.Join("\n\n", alertList)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = "No active weather alerts found"
                }
            }
        };
    }
    
    private async Task<JObject> ExecuteGetAlertsForAreaTool(JObject arguments)
    {
        var area = arguments.GetValue("area")?.ToString();
        if (string.IsNullOrEmpty(area))
        {
            throw new ArgumentException("area parameter is required (e.g., 'TX', 'CA', 'FL')");
        }
        
        var endpoint = $"/alerts/active/area/{area.ToUpper()}";
        var response = await MakeNWSApiCall(endpoint);
        var features = response["features"] as JArray;

        if (features != null && features.Count > 0)
        {
            var alertList = features.Take(10).Select(alert => 
            {
                var props = alert["properties"];
                return $"• {props["event"]}: {props["headline"]}\n" +
                       $"  Areas: {string.Join(", ", props["areaDesc"].ToString().Split(';').Take(3))}\n" +
                       $"  Severity: {props["severity"]} | Urgency: {props["urgency"]}";
            });

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Active Weather Alerts for {area.ToUpper()} ({features.Count} total):\n\n" +
                                  string.Join("\n\n", alertList)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No active weather alerts found for {area.ToUpper()}"
                }
            }
        };
    }
    
    private async Task<JObject> ExecuteGetAlertsForZoneTool(JObject arguments)
    {
        var zoneId = arguments.GetValue("zone_id")?.ToString();
        if (string.IsNullOrEmpty(zoneId))
        {
            throw new ArgumentException("zone_id parameter is required (e.g., 'TXZ001', 'CAC037')");
        }
        
        var endpoint = $"/alerts/active/zone/{zoneId.ToUpper()}";
        var response = await MakeNWSApiCall(endpoint);
        var features = response["features"] as JArray;

        if (features != null && features.Count > 0)
        {
            var alertList = features.Take(10).Select(alert => 
            {
                var props = alert["properties"];
                return $"• {props["event"]}: {props["headline"]}\n" +
                       $"  Areas: {string.Join(", ", props["areaDesc"].ToString().Split(';').Take(3))}\n" +
                       $"  Severity: {props["severity"]} | Urgency: {props["urgency"]}";
            });

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Active Weather Alerts for Zone {zoneId.ToUpper()} ({features.Count} total):\n\n" +
                                  string.Join("\n\n", alertList)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No active weather alerts found for zone {zoneId.ToUpper()}"
                }
            }
        };
    }
    
    private async Task<JObject> ExecuteGetLatestObservationTool(JObject arguments)
    {
        var stationId = arguments.GetValue("station_id")?.ToString();
        var requireQc = arguments.GetValue("require_qc")?.ToObject<bool?>() ?? false;

        if (string.IsNullOrEmpty(stationId))
        {
            throw new ArgumentException("station_id parameter is required");
        }

        var endpoint = $"/stations/{stationId.ToUpper()}/observations/latest";
        if (requireQc)
        {
            endpoint += "?require_qc=true";
        }

        var response = await MakeNWSApiCall(endpoint);
        var properties = response["properties"];

        if (properties != null)
        {
            var timestamp = DateTime.Parse(properties["timestamp"].ToString()).ToString("MMM dd, yyyy h:mm tt");
            var temp = properties["temperature"]?["value"]?.ToString();
            var tempUnit = properties["temperature"]?["unitCode"]?.ToString()?.Replace("wmoUnit:", "");
            var condition = properties["textDescription"]?.ToString() ?? "N/A";
            var humidity = properties["relativeHumidity"]?["value"]?.ToString();
            var windSpeed = properties["windSpeed"]?["value"]?.ToString();
            var windDirection = properties["windDirection"]?["value"]?.ToString();
            var pressure = properties["barometricPressure"]?["value"]?.ToString();
            var visibility = properties["visibility"]?["value"]?.ToString();

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Latest Weather Observation for {stationId.ToUpper()}:\n\n" +
                                  $"Time: {timestamp}\n" +
                                  $"Temperature: {temp}°{tempUnit}\n" +
                                  $"Condition: {condition}\n" +
                                  $"Humidity: {humidity}%\n" +
                                  $"Wind: {windSpeed} from {windDirection}°\n" +
                                  $"Pressure: {pressure}\n" +
                                  $"Visibility: {visibility}"
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No observation data found for station {stationId.ToUpper()}"
                }
            }
        };
    }
    
    private async Task<JObject> ExecuteGetPointMetadataTool(JObject arguments)
    {
        var latitude = arguments.GetValue("latitude")?.ToObject<double?>();
        var longitude = arguments.GetValue("longitude")?.ToObject<double?>();

        if (!latitude.HasValue || !longitude.HasValue)
        {
            throw new ArgumentException("Both latitude and longitude parameters are required");
        }

        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentException("Latitude must be between -90 and 90 degrees");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentException("Longitude must be between -180 and 180 degrees");
        }

        var endpoint = $"/points/{latitude},{longitude}";
        var response = await MakeNWSApiCall(endpoint);
        var properties = response["properties"];

        if (properties != null)
        {
            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"NWS Point Metadata for {latitude}, {longitude}:\n\n" +
                                  $"Forecast Office: {properties["cwa"]}\n" +
                                  $"Grid ID: {properties["gridId"]}\n" +
                                  $"Grid Coordinates: {properties["gridX"]}, {properties["gridY"]}\n" +
                                  $"County: {properties["county"]}\n" +
                                  $"Fire Weather Zone: {properties["fireWeatherZone"]}\n" +
                                  $"Forecast Zone: {properties["forecastZone"]}\n" +
                                  $"Time Zone: {properties["timeZone"]}\n" +
                                  $"Radar Station: {properties["radarStation"]}"
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No point metadata found for coordinates {latitude}, {longitude}"
                }
            }
        };
    }
    
    private async Task<JObject> ExecuteGetGridpointForecastTool(JObject arguments)
    {
        var wfo = arguments.GetValue("wfo")?.ToString();
        var x = arguments.GetValue("x")?.ToObject<int?>();
        var y = arguments.GetValue("y")?.ToObject<int?>();
        var units = arguments.GetValue("units")?.ToString() ?? "us";

        if (string.IsNullOrEmpty(wfo) || !x.HasValue || !y.HasValue)
        {
            throw new ArgumentException("wfo, x, and y parameters are required");
        }

        var endpoint = $"/gridpoints/{wfo.ToUpper()}/{x},{y}/forecast";
        if (units == "si")
        {
            endpoint += "?units=si";
        }

        var response = await MakeNWSApiCall(endpoint);
        var periods = response["properties"]["periods"] as JArray;

        if (periods != null && periods.Count > 0)
        {
            var forecastData = periods.Take(7).Select(period => new JObject
            {
                ["name"] = period["name"]?.ToString(),
                ["temperature"] = $"{period["temperature"]} {period["temperatureUnit"]}",
                ["condition"] = period["shortForecast"]?.ToString(),
                ["wind"] = $"{period["windSpeed"]} {period["windDirection"]}",
                ["detailed"] = period["detailedForecast"]?.ToString()
            }).ToArray();

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Gridpoint Forecast for {wfo.ToUpper()} {x},{y}:\n\n" +
                                  string.Join("\n\n", forecastData.Select(f => 
                                      $"{f["name"]}: {f["temperature"]}, {f["condition"]}, Wind: {f["wind"]}\n{f["detailed"]}"))
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No forecast data found for grid {wfo}/{x},{y}"
                }
            }
        };
    }
    
    private async Task<JObject> ExecuteGetZoneForecastTool(JObject arguments)
    {
        var zoneType = arguments.GetValue("zone_type")?.ToString() ?? "forecast";
        var zoneId = arguments.GetValue("zone_id")?.ToString();

        if (string.IsNullOrEmpty(zoneId))
        {
            throw new ArgumentException("zone_id parameter is required (e.g., 'TXZ001', 'CAC037')");
        }

        var endpoint = $"/zones/{zoneType}/{zoneId.ToUpper()}/forecast";
        var response = await MakeNWSApiCall(endpoint);

        if (response["properties"] != null)
        {
            var periods = response["properties"]["periods"] as JArray;
            if (periods != null && periods.Count > 0)
            {
                var result = new JObject
                {
                    ["zone"] = $"{zoneType}/{zoneId}",
                    ["zone_name"] = response["properties"]["zone"]?.ToString(),
                    ["forecast_periods"] = new JArray(periods.Take(5).Select(period => new JObject
                    {
                        ["name"] = period["name"]?.ToString(),
                        ["detailed_forecast"] = period["detailedForecast"]?.ToString(),
                        ["temperature"] = period["temperature"]?.ToString(),
                        ["temperature_unit"] = period["temperatureUnit"]?.ToString(),
                        ["wind_speed"] = period["windSpeed"]?.ToString(),
                        ["wind_direction"] = period["windDirection"]?.ToString()
                    }).ToArray())
                };

                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"Zone Forecast for {zoneType.ToUpper()}/{zoneId.ToUpper()}:\n\n" +
                                      string.Join("\n\n", result["forecast_periods"].Select(p => 
                                          $"{p["name"]}: {p["detailed_forecast"]}"))
                        }
                    }
                };
            }
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No forecast data found for zone {zoneType}/{zoneId}"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetObservationStationsTool(JObject arguments)
    {
        var state = arguments.GetValue("state")?.ToString();
        var stationId = arguments.GetValue("station_id")?.ToString();
        var limit = arguments.GetValue("limit")?.ToObject<int?>() ?? 20;

        string endpoint;
        if (!string.IsNullOrEmpty(stationId))
        {
            endpoint = $"/stations/{stationId}";
        }
        else
        {
            endpoint = "/stations";
            var queryParams = new List<string>();
            
            if (!string.IsNullOrEmpty(state))
            {
                queryParams.Add($"id={state}");
            }
            queryParams.Add($"limit={limit}");
            
            if (queryParams.Count > 0)
            {
                endpoint += "?" + string.Join("&", queryParams);
            }
        }

        var response = await MakeNWSApiCall(endpoint);

        if (!string.IsNullOrEmpty(stationId))
        {
            // Single station response
            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Station: {response["properties"]["stationIdentifier"]}\n" +
                                  $"Name: {response["properties"]["name"]}\n" +
                                  $"Location: {response["geometry"]["coordinates"][1]}, {response["geometry"]["coordinates"][0]}\n" +
                                  $"Elevation: {response["properties"]["elevation"]["value"]} {response["properties"]["elevation"]["unitCode"]}"
                    }
                }
            };
        }
        else
        {
            // Station list response
            var features = response["features"] as JArray;
            if (features != null && features.Count > 0)
            {
                var stationList = features.Take(limit).Select(station => 
                {
                    var props = station["properties"];
                    var coords = station["geometry"]["coordinates"];
                    return $"• {props["stationIdentifier"]} - {props["name"]} (Lat: {coords[1]}, Lon: {coords[0]})";
                });

                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"Weather Observation Stations{(string.IsNullOrEmpty(state) ? "" : $" in {state.ToUpper()}")}:\n\n" +
                                      string.Join("\n", stationList)
                        }
                    }
                };
            }
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = "No observation stations found for the specified criteria"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetZonesTool(JObject arguments)
    {
        var area = arguments.GetValue("area")?.ToString();
        var zoneType = arguments.GetValue("zone_type")?.ToString();
        var point = arguments.GetValue("point")?.ToString();
        var limit = arguments.GetValue("limit")?.ToObject<int?>() ?? 20;

        var endpoint = "/zones";
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(area))
        {
            queryParams.Add($"area={area}");
        }
        if (!string.IsNullOrEmpty(zoneType))
        {
            queryParams.Add($"type={zoneType}");
        }
        if (!string.IsNullOrEmpty(point))
        {
            queryParams.Add($"point={point}");
        }
        queryParams.Add($"limit={limit}");

        if (queryParams.Count > 0)
        {
            endpoint += "?" + string.Join("&", queryParams);
        }

        var response = await MakeNWSApiCall(endpoint);
        var features = response["features"] as JArray;

        if (features != null && features.Count > 0)
        {
            var zoneList = features.Take(limit).Select(zone => 
            {
                var props = zone["properties"];
                return $"• {props["id"]} - {props["name"]} ({props["type"]})";
            });

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"NWS Zones{(string.IsNullOrEmpty(area) ? "" : $" in {area.ToUpper()}")}:\n\n" +
                                  string.Join("\n", zoneList)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = "No zones found for the specified criteria"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetHourlyForecastTool(JObject arguments)
    {
        var wfo = arguments.GetValue("wfo")?.ToString();
        var x = arguments.GetValue("x")?.ToObject<int?>();
        var y = arguments.GetValue("y")?.ToObject<int?>();
        var units = arguments.GetValue("units")?.ToString() ?? "us";

        if (string.IsNullOrEmpty(wfo) || !x.HasValue || !y.HasValue)
        {
            throw new ArgumentException("wfo, x, and y parameters are required");
        }

        var endpoint = $"/gridpoints/{wfo.ToUpper()}/{x},{y}/forecast/hourly";
        if (units == "si")
        {
            endpoint += "?units=si";
        }

        var response = await MakeNWSApiCall(endpoint);
        var periods = response["properties"]["periods"] as JArray;

        if (periods != null && periods.Count > 0)
        {
            var hourlyData = periods.Take(12).Select(period => new JObject
            {
                ["time"] = DateTime.Parse(period["startTime"].ToString()).ToString("MMM dd, h:mm tt"),
                ["temperature"] = $"{period["temperature"]} {period["temperatureUnit"]}",
                ["condition"] = period["shortForecast"]?.ToString(),
                ["wind"] = $"{period["windSpeed"]} {period["windDirection"]}",
                ["humidity"] = period["relativeHumidity"]?["value"]?.ToString() + "%"
            }).ToArray();

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Hourly Forecast for Grid {wfo.ToUpper()} {x},{y}:\n\n" +
                                  string.Join("\n", hourlyData.Select(h => 
                                      $"{h["time"]}: {h["temperature"]}, {h["condition"]}, Wind: {h["wind"]}, Humidity: {h["humidity"]}"))
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No hourly forecast data found for grid {wfo}/{x},{y}"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetStationObservationsTool(JObject arguments)
    {
        var stationId = arguments.GetValue("station_id")?.ToString();
        var startTime = arguments.GetValue("start_time")?.ToString();
        var endTime = arguments.GetValue("end_time")?.ToString();
        var limit = arguments.GetValue("limit")?.ToObject<int?>() ?? 48;

        if (string.IsNullOrEmpty(stationId))
        {
            throw new ArgumentException("station_id parameter is required");
        }

        var endpoint = $"/stations/{stationId.ToUpper()}/observations";
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(startTime))
        {
            queryParams.Add($"start={startTime}");
        }
        if (!string.IsNullOrEmpty(endTime))
        {
            queryParams.Add($"end={endTime}");
        }
        queryParams.Add($"limit={limit}");

        if (queryParams.Count > 0)
        {
            endpoint += "?" + string.Join("&", queryParams);
        }

        var response = await MakeNWSApiCall(endpoint);
        var features = response["features"] as JArray;

        if (features != null && features.Count > 0)
        {
            var observations = features.Take(limit).Select(obs => 
            {
                var props = obs["properties"];
                var timestamp = DateTime.Parse(props["timestamp"].ToString()).ToString("MMM dd, h:mm tt");
                var temp = props["temperature"]?["value"]?.ToString();
                var tempUnit = props["temperature"]?["unitCode"]?.ToString()?.Replace("wmoUnit:", "");
                var condition = props["textDescription"]?.ToString() ?? "N/A";
                var windSpeed = props["windSpeed"]?["value"]?.ToString();
                var windDir = props["windDirection"]?["value"]?.ToString();
                
                return $"{timestamp}: {temp}°{tempUnit}, {condition}" + 
                       (windSpeed != null ? $", Wind: {windSpeed} from {windDir}°" : "");
            });

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Weather Observations for {stationId.ToUpper()}:\n\n" +
                                  string.Join("\n", observations)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No observations found for station {stationId.ToUpper()}"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetTerminalForecastTool(JObject arguments)
    {
        var stationId = arguments.GetValue("station_id")?.ToString();

        if (string.IsNullOrEmpty(stationId))
        {
            throw new ArgumentException("station_id parameter is required");
        }

        var endpoint = $"/products/types/TAF/locations/{stationId.ToUpper()}";
        var response = await MakeNWSApiCall(endpoint);

        var products = response["@graph"] as JArray;
        if (products != null && products.Count > 0)
        {
            var latestTaf = products.OrderByDescending(p => p["issuanceTime"]).FirstOrDefault();
            if (latestTaf != null)
            {
                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"Terminal Aerodrome Forecast (TAF) for {stationId.ToUpper()}:\n\n" +
                                      $"Issued: {DateTime.Parse(latestTaf["issuanceTime"].ToString()).ToString("MMM dd, yyyy h:mm tt")} UTC\n" +
                                      $"Product: {latestTaf["productText"]?.ToString()?.Replace("\n", "\n")}"
                        }
                    }
                };
            }
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No Terminal Aerodrome Forecast (TAF) found for {stationId.ToUpper()}"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetMarineAlertsTool(JObject arguments)
    {
        var region = arguments.GetValue("region")?.ToString() ?? "AL";

        var endpoint = $"/alerts/active/region/{region}";
        var response = await MakeNWSApiCall(endpoint);

        var features = response["features"] as JArray;
        if (features != null && features.Count > 0)
        {
            var marineAlerts = features.Where(alert => 
            {
                var categories = alert["properties"]["category"] as JArray;
                return categories?.Any(c => c.ToString().ToLower().Contains("marine")) == true;
            }).Take(10);

            if (marineAlerts.Any())
            {
                var alertList = marineAlerts.Select(alert => 
                {
                    var props = alert["properties"];
                    return $"• {props["event"]}: {props["headline"]}\n" +
                           $"  Areas: {string.Join(", ", props["areaDesc"].ToString().Split(';').Take(3))}\n" +
                           $"  Severity: {props["severity"]} | Urgency: {props["urgency"]}";
                });

                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"Active Marine Weather Alerts - Region {region}:\n\n" +
                                      string.Join("\n\n", alertList)
                        }
                    }
                };
            }
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No active marine weather alerts found for region {region}"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetOfficeHeadlinesTool(JObject arguments)
    {
        var officeId = arguments.GetValue("office_id")?.ToString();

        if (string.IsNullOrEmpty(officeId))
        {
            throw new ArgumentException("office_id parameter is required");
        }

        var endpoint = $"/offices/{officeId.ToUpper()}/headlines";
        var response = await MakeNWSApiCall(endpoint);

        var headlines = response["@graph"] as JArray;
        if (headlines != null && headlines.Count > 0)
        {
            var headlineList = headlines.Take(5).Select(headline => 
            {
                var issuanceTime = DateTime.Parse(headline["issuanceTime"].ToString()).ToString("MMM dd, h:mm tt");
                return $"• {headline["name"]}\n" +
                       $"  Issued: {issuanceTime}\n" +
                       $"  Content: {headline["content"]?.ToString()?.Substring(0, Math.Min(200, headline["content"].ToString().Length))}...";
            });

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Latest Headlines from NWS Office {officeId.ToUpper()}:\n\n" +
                                  string.Join("\n\n", headlineList)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = $"No headlines found for NWS office {officeId.ToUpper()}"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetAlertTypesTool(JObject arguments)
    {
        var endpoint = "/alerts/types";
        var response = await MakeNWSApiCall(endpoint);

        var eventTypes = response["eventTypes"] as JArray;
        if (eventTypes != null && eventTypes.Count > 0)
        {
            var typeList = eventTypes.Select(eventType => $"• {eventType}").ToArray();

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Available Weather Alert Types ({typeList.Length} total):\n\n" +
                                  string.Join("\n", typeList)
                    }
                }
            };
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = "No alert types available"
                }
            }
        };
    }

    private async Task<JObject> ExecuteGetWeatherGlossaryTool(JObject arguments)
    {
        var searchTerm = arguments.GetValue("search_term")?.ToString();

        var endpoint = "/glossary";
        var response = await MakeNWSApiCall(endpoint);

        var glossary = response["glossary"] as JArray;
        if (glossary != null && glossary.Count > 0)
        {
            IEnumerable<JToken> filteredTerms = glossary;
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredTerms = glossary.Where(term => 
                    term["term"]?.ToString().ToLower().Contains(searchTerm.ToLower()) == true ||
                    term["definition"]?.ToString().ToLower().Contains(searchTerm.ToLower()) == true);
            }

            var termList = filteredTerms.Take(10).Select(term => 
                $"**{term["term"]}**: {term["definition"]}");

            if (termList.Any())
            {
                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"Weather Glossary{(string.IsNullOrEmpty(searchTerm) ? " (showing first 10 terms)" : $" - Results for '{searchTerm}'")}:\n\n" +
                                      string.Join("\n\n", termList)
                        }
                    }
                };
            }
            else
            {
                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"No glossary terms found matching '{searchTerm}'"
                        }
                    }
                };
            }
        }

        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = "Weather glossary not available"
                }
            }
        };
    }

    // Helper method to make calls to the NWS API using connector's HTTP client
    private async Task<JObject> MakeNWSApiCall(string endpoint)
    {
        try
        {
            var url = $"https://api.weather.gov{endpoint}";
            
            // Create HTTP request message
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            // Set required User-Agent header for NWS API
            request.Headers.Add("User-Agent", $"{SERVER_NAME}/{SERVER_VERSION} (Power Platform MCP Server)");
            request.Headers.Add("Accept", "application/json");
            
            // Use the connector's HTTP client
            var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            if (response.IsSuccessStatusCode)
            {
                return JObject.Parse(content);
            }
            else
            {
                throw new HttpRequestException($"NWS API Error ({response.StatusCode}): {content}");
            }
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse NWS API response: {ex.Message}");
        }
        catch (HttpRequestException)
        {
            throw; // Re-throw HTTP exceptions as-is
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"NWS API call failed: {ex.Message}");
        }
    }


    // ****** DO NOT MODIFY BELOW THIS LINE ******
    // Server capabilities - MCP protocol configuration
    // The code below this point implements the MCP protocol specification
    // and should not be changed unless you understand the JSON-RPC 2.0 and MCP standards.
    // Only modify AVAILABLE_TOOLS above and add tool logic to HandleToolsCallAsync() method above.
    private static readonly string PROTOCOL_VERSION = "2025-06-18";
    private static bool _isInitialized = false;
    private static readonly JObject SERVER_CAPABILITIES = new JObject
    {
        ["tools"] = new JObject
        {
            ["listChanged"] = true
        }
    };
    
    // Dynamically get tool names from AVAILABLE_TOOLS
    private static string[] GetToolNames()
    {
        return AVAILABLE_TOOLS.Select(tool => tool["name"]?.ToString()).Where(name => !string.IsNullOrEmpty(name)).ToArray();
    }
    
    // Convert tool name to method name (e.g., "hello_world" -> "HelloWorld")
    private static string ConvertToMethodName(string toolName)
    {
        if (string.IsNullOrEmpty(toolName)) return "";
        
        var parts = toolName.Split('_');
        var result = new StringBuilder();
        
        foreach (var part in parts)
        {
            if (!string.IsNullOrEmpty(part))
            {
                result.Append(char.ToUpper(part[0]));
                if (part.Length > 1)
                {
                    result.Append(part.Substring(1).ToLower());
                }
            }
        }
        
        return result.ToString();
    }
    
    // Tool response helpers - create properly formatted MCP tool results
    private static JObject CreateTextToolResponse(string text)
    {
        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "text",
                    ["text"] = text
                }
            }
        };
    }
    
    private static JObject CreateImageToolResponse(string data, string mimeType)
    {
        return new JObject
        {
            ["content"] = new JArray
            {
                new JObject
                {
                    ["type"] = "image",
                    ["data"] = data,
                    ["mimeType"] = mimeType
                }
            }
        };
    }
    
    private static JObject CreateResourceToolResponse(string uri, string name = null, string mimeType = null)
    {
        var resourceObj = new JObject
        {
            ["type"] = "resource",
            ["resource"] = new JObject
            {
                ["uri"] = uri
            }
        };
        
        if (!string.IsNullOrEmpty(name))
            resourceObj["resource"]["name"] = name;
        if (!string.IsNullOrEmpty(mimeType))
            resourceObj["resource"]["mimeType"] = mimeType;
            
        return new JObject
        {
            ["content"] = new JArray { resourceObj }
        };
    }
    
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            var operationId = GetOperationId();
            
            if (operationId == "InvokeServer")
            {
                return await HandleMcpRequestAsync().ConfigureAwait(false);
            }
            else if (operationId == "GetInvokeServer")
            {
                return await HandleGetProtocolSchemaAsync().ConfigureAwait(false);
            }
            else
            {
                return CreateJsonRpcErrorResponse(null, -32601, "Method not found", $"Unknown operation ID '{operationId}'");
            }
        }
        catch (JsonException ex)
        {
            return CreateJsonRpcErrorResponse(null, -32700, "Parse error", ex.Message);
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(null, -32603, "Internal error", ex.Message);
        }
    }
    
    private async Task<HttpResponseMessage> HandleMcpRequestAsync()
    {
        var requestBody = await ParseRequestBodyAsync().ConfigureAwait(false);
        
        // Handle the case where the body is just {} (for notifications/initialized)
        // This happens when the client sends an empty object as the initialized notification
        if (requestBody.Count == 0 || string.IsNullOrEmpty(GetStringProperty(requestBody, "method", "")))
        {
            // This is the notifications/initialized message
            return await HandleInitializedAsync().ConfigureAwait(false);
        }
        
        var method = GetStringProperty(requestBody, "method", "");
        var requestId = GetRequestId(requestBody);
        
        switch (method)
        {
            case "initialize":
                return await HandleInitializeAsync(requestBody, requestId).ConfigureAwait(false);
            case "notifications/initialized":
                return await HandleInitializedAsync().ConfigureAwait(false);
            case "tools/list":
                return await HandleToolsListAsync(requestId).ConfigureAwait(false);
            case "tools/call":
                return await HandleToolsCallAsync(requestBody, requestId).ConfigureAwait(false);
            default:
                return CreateJsonRpcErrorResponse(requestId, -32601, "Method not found", $"Unknown method '{method}'");
        }
    }
    
    private async Task<HttpResponseMessage> HandleInitializeAsync(JObject requestBody, object requestId)
    {
        try
        {
            var paramsObj = requestBody["params"] as JObject;
            var clientVersion = GetStringProperty(paramsObj, "protocolVersion", "");
            
            if (string.IsNullOrEmpty(clientVersion))
            {
                return CreateJsonRpcErrorResponse(requestId, -32602, "Invalid params", "protocolVersion is required");
            }
            
            var initializeResult = new JObject
            {
                ["protocolVersion"] = PROTOCOL_VERSION,
                ["capabilities"] = SERVER_CAPABILITIES,
                ["serverInfo"] = new JObject
                {
                    ["name"] = SERVER_NAME,
                    ["version"] = SERVER_VERSION
                },
                ["instructions"] = "This is a Model Context Protocol server implemented in Power Platform custom connector. It provides basic Hello World functionality for demonstration purposes."
            };
            
            return CreateJsonRpcSuccessResponse(requestId, initializeResult);
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(requestId, -32603, "Internal error", ex.Message);
        }
    }
    
    private async Task<HttpResponseMessage> HandleInitializedAsync()
    {
        _isInitialized = true;
        
        // Return a proper confirmation response for the initialization notification
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var confirmationResponse = new JObject
        {
            ["status"] = "initialized",
            ["message"] = "MCP server initialization complete - ready to handle tool requests",
            ["serverName"] = SERVER_NAME,
            ["serverVersion"] = SERVER_VERSION,
            ["protocolVersion"] = PROTOCOL_VERSION,
            ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            ["capabilities"] = new JObject
            {
                ["tools"] = new JArray(GetToolNames())
            }
        };
        
        response.Content = CreateJsonContent(confirmationResponse.ToString());
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        return response;
    }
    
    private async Task<HttpResponseMessage> HandleToolsListAsync(object requestId)
    {
        if (!_isInitialized)
        {
            return CreateJsonRpcErrorResponse(requestId, -32002, "Server not initialized", "Must call initialize first");
        }
        
        try
        {
            var result = new JObject
            {
                ["tools"] = AVAILABLE_TOOLS
            };
            
            return CreateJsonRpcSuccessResponse(requestId, result);
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(requestId, -32603, "Internal error", ex.Message);
        }
    }
    
    private async Task<HttpResponseMessage> HandleToolsCallAsync(JObject requestBody, object requestId)
    {
        if (!_isInitialized)
        {
            return CreateJsonRpcErrorResponse(requestId, -32002, "Server not initialized", "Must call initialize first");
        }
        
        try
        {
            var paramsObj = requestBody["params"] as JObject;
            if (paramsObj == null)
            {
                return CreateJsonRpcErrorResponse(requestId, -32602, "Invalid params", "params object is required");
            }
            
            var toolName = GetStringProperty(paramsObj, "name", "");
            if (string.IsNullOrEmpty(toolName))
            {
                return CreateJsonRpcErrorResponse(requestId, -32602, "Invalid params", "tool name is required");
            }
            
            // Dynamically route tool calls to their implementations
            // Converts tool name to method name: "get_weather_alerts" -> "ExecuteGetWeatherAlertsTool"
            var methodName = "Execute" + ConvertToMethodName(toolName) + "Tool";
            var method = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (method == null)
            {
                return CreateJsonRpcErrorResponse(requestId, -32602, "Invalid params", $"Unknown tool: {toolName}");
            }
            
            var arguments = paramsObj["arguments"] as JObject ?? new JObject();
            
            // Check if method is async
            if (method.ReturnType == typeof(Task<JObject>))
            {
                var task = method.Invoke(this, new object[] { arguments }) as Task<JObject>;
                var result = await task;
                return CreateJsonRpcSuccessResponse(requestId, result);
            }
            else
            {
                var result = method.Invoke(this, new object[] { arguments }) as JObject;
                return CreateJsonRpcSuccessResponse(requestId, result);
            }
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(requestId, -32603, "Internal error", ex.Message);
        }
    }
    
    private async Task<HttpResponseMessage> HandleGetProtocolSchemaAsync()
    {
        try
        {
            // Return the MCP protocol schema information
            var schema = new JObject
            {
                ["protocol"] = "mcp",
                ["version"] = PROTOCOL_VERSION,
                ["serverInfo"] = new JObject
                {
                    ["name"] = SERVER_NAME,
                    ["version"] = SERVER_VERSION,
                    ["description"] = "Model Context Protocol server implemented in Power Platform custom connector"
                },
                ["capabilities"] = SERVER_CAPABILITIES,
                ["methods"] = new JArray
                {
                    "initialize",
                    "notifications/initialized", 
                    "tools/list",
                    "tools/call"
                },
                ["tools"] = AVAILABLE_TOOLS
            };
            
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(schema.ToString());
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return response;
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(null, -32603, "Internal error", ex.Message);
        }
    }
    
    private string GetOperationId()
    {
        string operationId = this.Context.OperationId;
        
        // For MCP agentic protocol, the operation ID should be directly available
        if (string.IsNullOrEmpty(operationId))
        {
            return "InvokeServer"; // Default for MCP protocol
        }
        
        // Only try Base64 decoding if it looks like Base64 and isn't already "InvokeServer"
        if (operationId != "InvokeServer" && IsBase64String(operationId))
        {
            try 
            {
                byte[] data = Convert.FromBase64String(operationId);
                operationId = System.Text.Encoding.UTF8.GetString(data);
            }
            catch (FormatException) 
            {
                // If Base64 decoding fails, use the original value
            }
        }
        
        return operationId;
    }
    
    private bool IsBase64String(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        
        // Basic check for Base64 format
        return s.Length % 4 == 0 && 
               System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", System.Text.RegularExpressions.RegexOptions.None);
    }
    
    private async Task<JObject> ParseRequestBodyAsync()
    {
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JObject.Parse(contentAsString);
    }
    
    private object GetRequestId(JObject requestBody)
    {
        var id = requestBody["id"];
        if (id == null) return null;
        
        if (id.Type == JTokenType.String)
            return id.ToString();
        if (id.Type == JTokenType.Integer)
            return id.ToObject<int>();
        if (id.Type == JTokenType.Float)
            return id.ToObject<double>();
            
        return id.ToString();
    }
    
    private HttpResponseMessage CreateJsonRpcSuccessResponse(object id, JObject result)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var jsonRpcResponse = new JObject
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id != null ? JToken.FromObject(id) : null,
            ["result"] = result
        };
        
        response.Content = CreateJsonContent(jsonRpcResponse.ToString());
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        return response;
    }
    
    private HttpResponseMessage CreateJsonRpcErrorResponse(object id, int code, string message, string data = null)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var errorObject = new JObject
        {
            ["code"] = code,
            ["message"] = message
        };
        
        if (!string.IsNullOrEmpty(data))
        {
            errorObject["data"] = data;
        }
        
        var jsonRpcResponse = new JObject
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id != null ? JToken.FromObject(id) : null,
            ["error"] = errorObject
        };
        
        response.Content = CreateJsonContent(jsonRpcResponse.ToString());
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        return response;
    }
    
    private string GetStringProperty(JObject json, string propertyName, string defaultValue = "")
    {
        if (json == null) return defaultValue;
        return json[propertyName]?.ToString() ?? defaultValue;
    }
    
    private int GetIntProperty(JObject json, string propertyName, int defaultValue = 0)
    {
        if (json == null) return defaultValue;
        return json[propertyName]?.ToObject<int>() ?? defaultValue;
    }
    
    private bool GetBoolProperty(JObject json, string propertyName, bool defaultValue = false)
    {
        if (json == null) return defaultValue;
        return json[propertyName]?.ToObject<bool>() ?? defaultValue;
    }
}