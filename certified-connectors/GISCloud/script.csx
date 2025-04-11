//
// using Newtonsoft.Json.Linq namespace for JObject;
// using Newtonsoft.Json namespace for JsonProperty;
//
// Apply to GetLayer only
//
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

  /// <summary>The JSON schema for the 'source' property of a GIS Cloud layer.</summary>
  public class LayerSource
  {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("dbtype")]
    public string DbType { get; set; }

    [JsonProperty("src")]
    public string Src { get; set; }

    public LayerSource(string Type, string DbType, string Src)
    {
      this.Type = Type;
      this.DbType = DbType;
      this.Src = Src;
    }
  }

  /// <summary>Deseralizer for JSON properties with schema LayerSource.</summary>
  public class LayerSourceConverter : JsonConverter
  {
    public override bool CanWrite { get { return false; } }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
      if (objectType == typeof(LayerSource))
      {
        return true;
      }

      return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      string Type;
      string DbType;
      string Src;

      if (reader.TokenType == JsonToken.Null)
      {
        Type = "";
        DbType = "";
        Src = "";
      }
      else if (reader.TokenType == JsonToken.String)
      {
        JObject sourceJson = JObject.Parse((string)reader.Value);

        Type = (string)sourceJson.SelectToken("type");
        DbType = (string)sourceJson.SelectToken("dbtype");
        Src = (string)sourceJson.SelectToken("src");
      }
      else
      {
        // Assume the value is an object.
        var value = (JObject)reader.Value;

        Type = (string)value.SelectToken("type");
        DbType = (string)value.SelectToken("dbtype");
        Src = (string)value.SelectToken("src");
      }

      LayerSource source = new LayerSource(Type, DbType, Src);

      return source;
    }
  }

  /// <summary>The JSON schema for the 'datasource' property of a GIS Cloud layer.</summary>
  public class LayerDataSource
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

  /// <summary> The JSON schema for the 'form' property of a GIS Cloud layer.</summary>
  public class LayerForm
  {

    [JsonProperty("Id")]
    public string Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("definition")]
    public string Definition { get; set; }

    public LayerForm(string Id, string Name, string Definition)
    {
      this.Id = Id;
      this.Name = Name;
      this.Definition = Definition;
    }
  }

  /// <summary>Deseralizer for JSON properties with schema LayerForm.</summary>
  public class LayerFormConverter : JsonConverter
  {
    public override bool CanWrite { get { return false; } }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
      if (objectType == typeof(LayerForm))
      {
        return true;
      }

      return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      string Id;
      string Name;
      string Definition;

      if (reader.TokenType == JsonToken.Null)
      {
        Id = "";
        Name = "";
        Definition = "";
      }
      else if (reader.TokenType == JsonToken.String)
      {
        Id = (string)reader.Value;
        Name = "";
        Definition = "";
      }
      else
      {
        // Assume the value is an object.
        var value = (JObject)reader.Value;

        Id = "";
        Name = (string)value.SelectToken("name");
        Definition = (string)value.SelectToken("definition");
      }

      LayerForm form = new LayerForm(Id, Name, Definition);

      return form;
    }
  }

  /// <summary> 
  /// The JSON schema for the 'additionalProperties' property of the options of
  /// a GIS Cloud's layer.
  /// </summary>
  public class LayerOptions
  {
    [JsonProperty("additionalProperties")]
    public Dictionary<string, string> AdditionalProperties { get; set; }
  }

  /// <summary>The JSON schema for a column contained in the columns property of a GIS Cloud layer.</summary>
  public class LayerColumn
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
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("owner")]
    public string Owner { get; set; }

    [JsonProperty("source"), JsonConverter(typeof(LayerSourceConverter))]
    public LayerSource? Source { get; set; }

    [JsonProperty("onscale")]
    public string OnScale { get; set; }

    [JsonProperty("offscale")]
    public string OffScale { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("x_min")]
    public string X_min { get; set; }

    [JsonProperty("x_max")]
    public string X_max { get; set; }

    [JsonProperty("y_min")]
    public string Y_min { get; set; }

    [JsonProperty("y_max")]
    public string Y_max { get; set; }

    [JsonProperty("textfield")]
    public string Textfield { get; set; }

    [JsonProperty("modified")]
    public int Modified { get; set; }

    [JsonProperty("created")]
    public int Created { get; set; }

    [JsonProperty("styles")]
    public string Styles { get; set; }

    [JsonProperty("alpha")]
    public string Alpha { get; set; }

    [JsonProperty("encoding")]
    public string Encoding { get; set; }

    [JsonProperty("margin")]
    public string Margin { get; set; }

    [JsonProperty("visible")]
    public string Visible { get; set; }

    [JsonProperty("lock")]
    public string Lock { get; set; }

    [JsonProperty("raster")]
    public string Raster { get; set; }

    [JsonProperty("grp")]
    public string Grp { get; set; }

    [JsonProperty("exportable")]
    public string Exportable { get; set; }

    [JsonProperty("parent")]
    public string Parent { get; set; }

    [JsonProperty("tooltip")]
    public string Tooltip { get; set; }

    [JsonProperty("label_placement")]
    public string LabelPlacement { get; set; }

    [JsonProperty("hidegeometry")]
    public string HideGeometry { get; set; }

    [JsonProperty("merged")]
    public string Merged { get; set; }

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

    [JsonProperty("use_info_window")]
    public string UseInfoWindow { get; set; }

    [JsonProperty("datasource_id")]
    public string DatasourceId { get; set; }

    [JsonProperty("epsg")]
    public string? Epsg { get; set; }

    [JsonProperty("opened")]
    public string Opened { get; set; }

    [JsonProperty("resource_id")]
    public string ResourceId { get; set; }

    [JsonProperty("map_id")]
    public string MapId { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("order")]
    public string Order { get; set; }

    [JsonProperty("form"), JsonConverter(typeof(LayerFormConverter))]
    public LayerForm? Form { get; set; }

    [JsonProperty("options")]
    public LayerOptions? Options { get; set; }

    [JsonProperty("datasource")]
    public LayerDataSource? DataSource { get; set; }

    [JsonProperty("columns")]
    public Dictionary<string, LayerColumn> Columns { get; set; }

    [JsonProperty("x-gc-columns")]
    public OpenApiColumnsSchema OpenApiColumnsSchema
    {
      get
      {
        var columnsSchema = new OpenApiColumnsSchema();
        columnsSchema.Properties = new Dictionary<string, OpenApiColumnSchema>();

        string[] acceptedColumnTypes = { "boolean", "number", "string", "integer" };

        foreach (KeyValuePair<string, LayerColumn> entry in this.Columns)
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

  /// <summary>
  /// A description of a layer's column that is returned by the GIS Cloud columns
  /// endpoint (i.e. the endpoint https://api.giscloud.com/1/layers/{LAYER_ID}/columns.json).
  /// </summary>
  public class LayerColumnsEndpointRecord
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
  }

  /// <summary>
  /// Enforces consistency of the structure of the data returned by 
  /// GIS Cloud returned with the OpenAPI schema for this connector. For example,
  /// the 'datasource' property of a GIS Cloud layer may be either an object
  /// or an integer (the datasource's id) depending on the options a user
  /// passes to the API. To maintain consistency, this function will convert
  /// the integer to object with a property containing that integer. 
  /// </summary>
  public string CleanGiscloudJson(string json)
  {
    // Keep track of the types the GIS Cloud columns should be coerced to.
    // The default coerce will be string types
    string[] columnsToCoerceToNumber = new string[] { "created", "modified", "accessed", "order", "total", "page", "lastlog", "id" };
    string[] columnsToCoerceToObject = new string[] { "columns", "datasource", "options", "bounds", "source", "form" };
    string[] columnsToCoerceToArray = new string[] { "owner", "analysis", "permissions" };

    // for storing the results
    var cleanedJson = new Dictionary<string, object>() { };

    // Retrieve the root to start arbitrary traversal
    JObject rootEl = JObject.Parse(json);

    // Recursively traverse the JSON element tree
    foreach (var childEl in rootEl)
    {
      if (childEl.Value.HasValues)
      {
        if (childEl.Value.Type == JTokenType.Array)  // Array type handling, need to loop over elements
        {
          cleanedJson.Add(
            childEl.Key,
            childEl.Value.Children().Select(el => JObject.Parse(CleanGiscloudJson(el.ToString()))).ToArray()  // Recurse over Array Elements
          );
        }
        else
        {
          // Since the child element has children of its own and is not an array,
          // we will assume it is an object.
          cleanedJson.Add(childEl.Key, JObject.Parse(CleanGiscloudJson(childEl.Value.ToString())));
        }
      }
      else if (
        !childEl.Key.StartsWith("__") &
        !Array.Exists(columnsToCoerceToNumber, columnName => childEl.Key == columnName) &
        (childEl.Value.Type == JTokenType.Integer | childEl.Value.Type == JTokenType.Float)
      )  // Exclude system params starting with "__" and the provided type in the response is numeric and shouldn't be kept that way
      {
        // so we Coerce to String.
        cleanedJson.Add(childEl.Key, childEl.Value.ToString());
      }
      else if (
        !childEl.Key.StartsWith("__") &
        Array.Exists(columnsToCoerceToNumber, columnName => childEl.Key == columnName) &
        !(childEl.Value.Type == JTokenType.Integer | childEl.Value.Type == JTokenType.Float)
      )  // Exclude system params starting with "__" and the provided type in the response is String and should be a number
      {
        // so we Coerce to Numeric.
        int intNumber;
        float floatNumber;
        bool intSuccess = int.TryParse(childEl.Value.ToString(), out intNumber);
        if (intSuccess) {
          // Converted to Int32
          cleanedJson.Add(childEl.Key, intNumber);
        } else {
          bool floatSuccess = float.TryParse(childEl.Value.ToString(), out floatNumber);
          if (floatSuccess) {
            // Converted to Float
            cleanedJson.Add(childEl.Key, floatNumber);
          } else {
            // Conversion failure fallback
            cleanedJson.Add(childEl.Key, childEl.Value);
          }
        }
      }
      else if (
        !childEl.Key.StartsWith("__") &
        Array.Exists(columnsToCoerceToObject, columnName => childEl.Key == columnName) &
        childEl.Value.Type != JTokenType.Object
      ) // Exclude system params starting with "__" and the provided type should be an Object but is not
      {
        // so we Coerce to Object.
        if (childEl.Value.Type == JTokenType.Null)
        {
          // If provided value is NULL, set an empty default
          cleanedJson.Add(childEl.Key, new Dictionary<string, object>() { });
        }
        else
        {
          cleanedJson.Add(childEl.Key, new Dictionary<string, object>()
          {
            ["value"] = new Dictionary<string, object>() { }
            // ["value"] = childEl.Value
          });
        }
      }
      else if (
        !childEl.Key.StartsWith("__") &
        Array.Exists(
          columnsToCoerceToArray, columnName => childEl.Key == columnName) &
          childEl.Value.Type != JTokenType.Array
        )  // Exclude system params starting with "__" and the provided type should be Array but is not
      {
        // so we Coerce to array.
        if (childEl.Value.Type == JTokenType.Null)
        {
          // If provided value is NULL, set an empty default
          cleanedJson.Add(childEl.Key, new object[] { });
        }
        else
        {
          cleanedJson.Add(
            childEl.Key,
            new object[] { childEl.Value }
          );
        }
      }
      else
      {
        // No coercion.
        cleanedJson.Add(childEl.Key, childEl.Value);
      }
    }
    // After we have recursively walked all children return parsed result
    return JsonConvert.SerializeObject(cleanedJson, Newtonsoft.Json.Formatting.Indented);
  }

  /// <summary>
  /// This is the entrypoint of the custom connector.
  /// All OpenAPI operations start here first.
  /// </summary>
  public override async Task<HttpResponseMessage> ExecuteAsync()
  {
    var logger = this.Context.Logger;
    logger.LogInformation("GIS Cloud Connector");
    // Handle the request based on the type of operation.
    if (this.Context.OperationId == "GetLayer")
    {
      // This is a special method that will extend the existing layer definition with the column attributes
      // so that the connector can provide a dynamic interface
      return await this.GetLayer().ConfigureAwait(false);
    }

    if (this.Context.OperationId == "GetFeatureMediaFile")
    {
      this.Context.Request.RequestUri = new Uri(Uri.UnescapeDataString(this.Context.Request.RequestUri.OriginalString), true);
    }

    // Use the context to forward/send an HTTP request for any other operation type
    HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    try {
      if (response.IsSuccessStatusCode)
      {
        if (response.Headers.Contains("Location"))
        {
          char delimiter = '/';
          var locationHeader = response.Headers.GetValues("Location").First();
          if (locationHeader.StartsWith("1/storage/fs/"))
          {
            response.Content = CreateJsonContent(new JObject
            {
              ["location"] = locationHeader.Substring(13),
            }.ToString());

          }
          else
          {
            response.Content = CreateJsonContent(new JObject
            {
              ["location"] = locationHeader.Split(delimiter).Last(),
            }.ToString());
          }
        } 
        // Do the transformation if the response was successful, otherwise return error responses as-is
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

        // Clean GIS Cloud JSON will try to coerce all types into strings to ensure schema consistency
        string result = CleanGiscloudJson(responseString);
        
        // Once we have cleaned up the types, re-encode it into a JSON payload
        response.Content = CreateJsonContent(result);
      }      
    } catch {
      return response;
    }
    return response;
  }

  /// <summary>
  /// Executes a pre-request to get the column details of a GISCloud layer via the API.
  /// The retrieved column details are then appended to provide the required schema output to meet the needs of the dynamic Feature operations
  /// </summary>
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
        List<LayerColumnsEndpointRecord> columnEndpointRecords = JsonConvert.DeserializeObject<List<LayerColumnsEndpointRecord>>(getColumnsResponseString, new JsonSerializerSettings
        {
          DefaultValueHandling = DefaultValueHandling.Populate,
        });

        foreach (LayerColumnsEndpointRecord columnEndpointRecord in columnEndpointRecords)
        {
          LayerColumn column;
          if (layer.Columns.TryGetValue(columnEndpointRecord.Name, out column) && (column?.Datatype == null || column?.Datatype == "string"))
          {
            // Override the column's datatype if it is either 'null' or the catch-all type 'string'.
            column.Datatype = columnEndpointRecord.Type;
          }
        }
      }

      string layerAsJson = CleanGiscloudJson(JsonConvert.SerializeObject(layer));
      getLayerResponse.Content = CreateJsonContent(layerAsJson);
    }

    return getLayerResponse;
  }
}