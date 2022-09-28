public class Script : ScriptBase
{
  /// <summary>The OpenAPI schema for a column of a layer.</summary>
  public class OpenApiColumnSchema
  {
    public OpenApiColumnSchema(string Type)
    {
      this.Type = Type;
    }

    [JsonProperty("type")]
    public string Type { get; set; }
  }

  /// <summary>The OpenAPI schema for the layer's collection of columns.</summary>
  public class OpenApiColumnsSchema
  {
    [JsonProperty("type")]
    public string Type = "object";

    [JsonProperty("properties")]
    public Dictionary<string, OpenApiColumnSchema> Properties { get; set; }
  }

  /// <summary>Deseralizer for boolean properties returned by GIS Cloud.</summary>
  public class GisCloudBooleanConverter : JsonConverter
  {
    public override bool CanWrite { get { return false; } }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
      if (objectType == typeof(bool))
      {
        return true;
      }

      return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var value = reader.Value;

      if (reader.TokenType == JsonToken.Boolean)
      {
        return (bool)value;
      }
      else if (reader.TokenType == JsonToken.String)
      {
        if ("t".Equals(((string)value).ToLower()))
        {
          return true;
        }
        else if ("f".Equals(((string)value).ToLower()))
        {
          return false;
        }
      }

      return false;
    }
  }

  /// <summary>The JSON schema for the datasource property of a GIS Cloud layer.</summary>
  public class DataSource
  {
    [JsonProperty("resource_id")]
    public int ResourceId { get; set; }

    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("owner_id")]
    public int OwnerId { get; set; }

    [JsonProperty("permissions")]
    public string[] Permissions { get; set; }
  }

  /// <summary>The JSON schema for a column contained in the columns property of a GIS Cloud layer.</summary>
  public class Column
  {
    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("datatype")]
    public string? Datatype { get; set; }

    [JsonProperty("alias")]
    public string? Alias { get; set; }

    [JsonProperty("order")]
    public int? Order { get; set; }

    [JsonProperty("column")]
    public string? ColumnName { get; set; }

    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }
  }

  /// <summary>The JSON schema for a GIS Cloud layer.</summary>
  public class Layer
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("owner")]
    public int Owner { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("onscale")]
    public string OnScale { get; set; }

    [JsonProperty("offscale")]
    public string OffScale { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("x_min")]
    public double X_min { get; set; }

    [JsonProperty("x_max")]
    public double X_max { get; set; }

    [JsonProperty("y_min")]
    public double Y_min { get; set; }

    [JsonProperty("y_max")]
    public double Y_max { get; set; }

    [JsonProperty("textfield")]
    public string Textfield { get; set; }

    [JsonProperty("modified")]
    public int Modified { get; set; }

    [JsonProperty("created")]
    public int Created { get; set; }

    [JsonProperty("styles")]
    public string Styles { get; set; }

    [JsonProperty("alpha")]
    public int Alpha { get; set; }

    [JsonProperty("encoding")]
    public string Encoding { get; set; }

    [JsonProperty("margin")]
    public string Margin { get; set; }

    [JsonProperty("visible"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool Visible { get; set; }

    [JsonProperty("lock"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool Lock { get; set; }

    [JsonProperty("raster"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool Raster { get; set; }

    [JsonProperty("grp")]
    public string Grp { get; set; }

    [JsonProperty("exportable"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool Exportable { get; set; }

    [JsonProperty("parent")]
    public string Parent { get; set; }

    [JsonProperty("tooltip")]
    public string Tooltip { get; set; }

    [JsonProperty("label_placement")]
    public string LabelPlacement { get; set; }

    [JsonProperty("hidegeometry"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool HideGeometry { get; set; }

    [JsonProperty("merged"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool Merged { get; set; }

    [JsonProperty("layerclass")]
    public string LayerClass { get; set; }

    [JsonProperty("layerid")]
    public string LayerId { get; set; }

    [JsonProperty("ds_map_id")]
    public string DsMapId { get; set; }

    [JsonProperty("dynamic_mode")]
    public string DynamicMode { get; set; }

    [JsonProperty("geometry_offset")]
    public string GeometryOffset { get; set; }

    [JsonProperty("geometry_precision")]
    public string GeometryPrecision { get; set; }

    [JsonProperty("use_info_window"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool UseInfoWindow { get; set; }

    [JsonProperty("datasource_id")]
    public int DatasourceId { get; set; }

    [JsonProperty("epsg")]
    public int? Epsg { get; set; }

    [JsonProperty("opened"), JsonConverter(typeof(GisCloudBooleanConverter))]
    public bool Opened { get; set; }

    [JsonProperty("resource_id")]
    public int ResourceId { get; set; }

    [JsonProperty("map_id")]
    public int MapId { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("order")]
    public string Order { get; set; }

    [JsonProperty("form")]
    public int? Form { get; set; }

    [JsonProperty("datasource")]
    public DataSource? DataSource { get; set; }

    [JsonProperty("columns")]
    public Dictionary<string, Column> Columns { get; set; }

    [JsonProperty("x-gc-columns-schema")]
    public OpenApiColumnsSchema OpenApiColumnsSchema
    {
      get
      {
        var columnsSchema = new OpenApiColumnsSchema();
        columnsSchema.Properties = new Dictionary<string, OpenApiColumnSchema>();

        string[] acceptedColumnTypes = { "boolean", "number", "string", "integer" };

        foreach (KeyValuePair<string, Column> entry in this.Columns)
        {
          string columnType;

          if (acceptedColumnTypes.Contains(entry.Value.Datatype?.ToLower()))
          {
            columnType = entry.Value.Datatype.ToLower();
          }
          else if (entry.Value.Datatype?.ToLower() == "real")
          {
            columnType = "number";
          }
          else
          {
            // String is the default column type.
            columnType = "string";
          }

          var columnSchema = new OpenApiColumnSchema(columnType);

          columnsSchema.Properties.Add(entry.Key, columnSchema);
        }

        return columnsSchema;
      }
    }
  }

  /// <summary>A description of a layer's column that is returned by the GIS Cloud columns
  /// endpoint (i.e. the endpoint https://api.giscloud.com/1/layers/{LAYER_ID}/columns.json).
  public class ColumnsEndpointRecord
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
  }

  /// <summary>This is the entrypoint of the custom connector.</summary>
  public override async Task<HttpResponseMessage> ExecuteAsync()
  {
    // Handle the request based on the type of operation.
    if (this.Context.OperationId == "GetLayer")
    {
      return await this.GetLayer().ConfigureAwait(false);
    }

    // Handle an invalid operation ID.
    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
    response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");

    return response;
  }

  /// <summary>Handles a request to get the details of a GISCloud layer.</summary>
  private async Task<HttpResponseMessage> GetLayer()
  {
    HttpResponseMessage getLayerResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

    if (getLayerResponse.IsSuccessStatusCode)
    {
      string getLayerResponseString = await getLayerResponse.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

      // Deserialize the JSON to parse its values into a more useful form for PowerApps. For example,
      // the GISCloud API may return the string "t" for a true boolean value. During deserialization,
      // this string will be converted into an actual boolean with a value of 'true'.
      Layer layer = JsonConvert.DeserializeObject<Layer>(getLayerResponseString, new JsonSerializerSettings
      {
        DefaultValueHandling = DefaultValueHandling.Populate,
      });

      // Override the type property of any column object if it is null. The value of an overridden
      // type property comes from columns endpoint of the request layer (i.e. layers/{LAYER_ID}/columns.json).
      string getColumnsRequestUri = string.Format("https://api.giscloud.com/1/layers/{0}/columns.json", layer.Id);
      HttpRequestMessage getColumnsRequest = new HttpRequestMessage(new HttpMethod("GET"), getColumnsRequestUri);

      try
      {
        // Set the API-Key header.
        string apiKey = this.Context.Request.Headers.GetValues("API-Key").First();
        getColumnsRequest.Headers.Add("API-Key", apiKey);
      }
      catch (InvalidOperationException)
      {
        // No API-Key provided. Do nothing.
      }

      HttpResponseMessage getColumnsResponse = await this.Context.SendAsync(getColumnsRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

      if (getColumnsResponse.IsSuccessStatusCode)
      {
        string getColumnsResponseString = await getColumnsResponse.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        List<ColumnsEndpointRecord> columnEndpointRecords = JsonConvert.DeserializeObject<List<ColumnsEndpointRecord>>(getColumnsResponseString, new JsonSerializerSettings
        {
          DefaultValueHandling = DefaultValueHandling.Populate,
        });

        foreach (ColumnsEndpointRecord columnEndpointRecord in columnEndpointRecords)
        {
          Column column;
          if (layer.Columns.TryGetValue(columnEndpointRecord.Name, out column) && (column?.Datatype == null || column?.Datatype == "string"))
          {
            // Override the column's datatype if it is either 'null' or the catch-all type 'string'.
            column.Datatype = columnEndpointRecord.Type;
          }
        }
      }

      getLayerResponse.Content = CreateJsonContent(JsonConvert.SerializeObject(layer));
    }

    return getLayerResponse;
  }
}