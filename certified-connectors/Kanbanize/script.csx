public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync() {
		var operationId = this.Context.OperationId;
		try {
    	    if (operationId == "CreateCard" || operationId == "UpdateCard")
        	{
            	await this.UpdateNameRequest().ConfigureAwait(false);
	        }

	        if (operationId == "DiscardCard" || operationId == "BlockCard" || operationId == "ArchiveCard_V2" || operationId == "LinkCard" || operationId == "UnlinkCard" || operationId == "MoveCard_V2" || operationId == "UpdateCard_V2" || operationId == "SetCustomField" || operationId == "SetTags" || operationId == "SetStickers") {
    	  	    await this.FixPatchRequests().ConfigureAwait(false);
        	}

			if (operationId == "GetBoards_V2" || operationId == "GetColumns_V2" || operationId == "GetLanes_V2" || operationId == "GetTypes_V2" || operationId == "GetWorkflows_V2" || operationId == "LogTime_V2" || operationId == "GetCardByCustomId") {
				await this.FixRequestUrl().ConfigureAwait(false);
			}

			if (operationId == "LogTime_V2") {
				await this.SetLogTimeBody().ConfigureAwait(false);
			}

			if (operationId == "LinkCard" || operationId == "UnlinkCard") {
				await this.SetLinksBody(operationId).ConfigureAwait(false);
			}

			if (operationId == "SetTags" || operationId == "SetStickers") {
				return await this.SetStickersAndTags().ConfigureAwait(false);
			}

		    if (operationId == "CreateCard_V2" || operationId == "UpdateCard_V2" || operationId == "MoveCard_V2") {
				return await this.SetCardBody(operationId);
      		}

		    if (operationId == "SetCustomField") {
		    	return await this.SetCustomFieldBody().ConfigureAwait(false);
	    	}

		    if (operationId == "DownloadAttachment") {
			    return await this.DownloadAttachment().ConfigureAwait(false);
    		}

    	    if (operationId == "UploadAttachment") {
			    return await this.UploadAttachment().ConfigureAwait(false);
	    	}

	    	if (operationId == "GetCustomFieldValues") {
			    return await this.GetDropdownValuesResponse().ConfigureAwait(false);
	    	}

			if (operationId == "GetUsers") {
				return await this.GetUsers().ConfigureAwait(false);
			}

			if (operationId == "GetObjectName") {
				return await this.GetObjectName().ConfigureAwait(false);
			}

	        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

			if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
				return response;
			}

		    if (operationId == "GetBoards_V2" || operationId == "GetTemplates" || operationId == "GetCustomFields" || operationId == "GetTypes_V2" || operationId == "GetLanes_V2" || operationId == "GetColumns_V2" || operationId == "GetTags" || operationId == "GetStickers" || operationId == "GetBlockReasons"){
        	    response = await this.FilterData(response, operationId).ConfigureAwait(false);
	        }

			if (operationId == "GetCardByCustomId") {
				response = await this.GetCardByCustomIdResponse(response).ConfigureAwait(false);
			}

			if (operationId == "GetCard_V2" || operationId == "GetCardByCustomId") {
				response = await this.SetCustomFieldNames(response).ConfigureAwait(false);
				response = await this.SetStickersAndTagsNames(response).ConfigureAwait(false);
			}

	        return response;

		} catch (Exception ex) {
			var exResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
			if ((ex.Message.StartsWith("{") && ex.Message.EndsWith("}")) || (ex.Message.StartsWith("[") && ex.Message.EndsWith("]"))) {
	      		exResponse.Content = CreateJsonContent(new JObject(new JProperty("error,", JContainer.Parse(ex.Message))).ToString());
			} else {
	      		exResponse.Content = CreateJsonContent(new JObject(new JProperty("error", ex.Message)).ToString());
			}

		    return exResponse;
		}
    }

    private async Task UpdateNameRequest() {
		var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
		var contentAsJson = JObject.Parse(contentAsString);
	
		if(contentAsJson.ContainsKey("cfields"))
		{
			var customFieldsValue = contentAsJson["cfields"];
	
			if( customFieldsValue != null && !string.IsNullOrEmpty(customFieldsValue.ToString()))
			{
				try
				{
					var cfieldsAsJson = JObject.Parse(customFieldsValue.ToString());

					contentAsJson.Remove("cfields");
					contentAsJson.Add("cfields", JObject.FromObject(cfieldsAsJson));                        
				}
				catch (JsonReaderException)
				{
					//Invalid json format
					contentAsJson.Remove("cfields");
				}
				catch (Exception) //some other exception
				{
					contentAsJson.Remove("cfields");
				}
			} else {
				contentAsJson.Remove("cfields");
			}
		}
			
		this.Context.Request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
		this.Context.Request.Content = CreateJsonContent(contentAsJson.ToString());
    }

	private async Task FixRequestUrl() {
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
		uriBuilder.Path = uriBuilder.Path.Replace("/"+this.Context.OperationId.Replace("_V2",""), "");
		this.Context.Request.RequestUri = uriBuilder.Uri;
	}

	private async Task FixPatchRequests() {
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
		uriBuilder.Path = uriBuilder.Path.Replace("/"+this.Context.OperationId.Replace("_V2",""), "");
		this.Context.Request.RequestUri = uriBuilder.Uri;
		this.Context.Request.Method = HttpMethod.Post;
		this.Context.Request.Headers.Add("x-http-method-override","PATCH");
	}

	private async Task SetLinksBody(string operationId) {
		var requestBody = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false));

		var linksArray = new JArray();

		linksArray.Add(new JObject{
			["linked_card_id"] = requestBody["linked_card_id"],
			["link_type"] = requestBody["link_type"]
		});

		var parameterName = (operationId == "LinkCard") ? "links_to_existing_cards_to_add_or_update" : "links_to_existing_cards_to_remove";

		this.Context.Request.Content = CreateJsonContent(new JObject(new JProperty(parameterName, linksArray)).ToString());		
	}

	private async Task SetLogTimeBody() {
		var requestBody = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false));

		if(requestBody.ContainsKey("time_unit") && requestBody.ContainsKey("time")) {
			var timeUnit = requestBody["time_unit"].ToString();
			var time = (int)requestBody["time"];
			switch(timeUnit){
				case "Seconds":
					requestBody["time"] = time * 1;
					break;
				case "Minutes":
					requestBody["time"] = time * 60;
					break;
				case "Hours":
					requestBody["time"] = time * 3600;
					break;
				case "Days":
					requestBody["time"] = time * 28800;
					break;
				default:
					break;
			}
			requestBody.Remove("time_unit");

			this.Context.Request.Content = CreateJsonContent(requestBody.ToString());
		}
	}

	private async Task CreateComment(string cardId, string comment) {
		var commentRequestBody = new JObject{
			["text"] = comment
		};

		var response = await this.PerformInternalRequest("/api/v2/cards/" + cardId + "/comments",HttpMethod.Post, commentRequestBody.ToString());
	}

	private async Task<HttpResponseMessage> SetCardBody(string operationId){
		var response = new HttpResponseMessage();
		var requestBody = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false));
		var boardStructureContent = new JObject();

		var boardId = requestBody.ContainsKey("board_id") ? requestBody["board_id"].ToString() : "";
		var workflowId = requestBody.ContainsKey("workflow_id") ? requestBody["workflow_id"].ToString() : "";
		var selectedColumn = "";
		var selectedLane = "";
		var addColumnComment = false;
		var addLaneComment = false;
		var columnIsInt = false;
		var laneIsInt = false;

		if (requestBody.ContainsKey("custom_fields_to_add_or_update")) {
			var customFields = requestBody["custom_fields_to_add_or_update"];
			var customFieldsArray = new JArray();
			var customFieldsList = new List<int>();
	
			foreach (var customField in customFields) {
				customFieldsList.Add((int)customField["field_id"]);
			}

			var customFieldsBody = new JObject(new JProperty("field_ids", String.Join(",", customFieldsList.ToArray()))).ToString();
			response = await this.PerformInternalRequest("/api/v2/customFields?expand=allowed_values",HttpMethod.Post, customFieldsBody, "GET");
			var customFieldsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var selectedCustomField in customFields) {
				var fieldId = (int)selectedCustomField["field_id"];
				var value = selectedCustomField["value"];

				foreach (var availableCustomField in customFieldsContent["data"]) {
					if (fieldId == (int)availableCustomField["field_id"]) {
						var type = availableCustomField["type"].ToString();
						if (type == "dropdown") {
							foreach (var allowedValue in availableCustomField["allowed_values"]) {
								if (allowedValue["value"].ToString() == value.ToString()) {
									var valueId = (int)allowedValue["value_id"];
									customFieldsArray.Add(new JObject{
										["field_id"] = fieldId,
										["selected_values_to_add_or_update"] = new JArray(new JObject{
											["value_id"] = valueId
										})
									});
									break;
								}
							}
						} else {
							customFieldsArray.Add(new JObject{
								["field_id"] = fieldId,
								["value"] = value
							});
						}
					}
				}
			}
			requestBody["custom_fields_to_add_or_update"] = customFieldsArray;
		}

		if (requestBody.ContainsKey("column_id") || requestBody.ContainsKey("lane_id")) {
			response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/currentStructure", HttpMethod.Get);
			boardStructureContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
		}

		if (requestBody.ContainsKey("column_id")) {
			var columnData = boardStructureContent["data"]["columns"];
			selectedColumn = requestBody["column_id"].ToString();
			int columnId;
			var partialMatchColumnId = 0;
			var exactColumnMatch = false;
			var partialColumnMatch = false;
			columnIsInt = Int32.TryParse(selectedColumn, out columnId);

			foreach (JProperty column in columnData) {
				if (column.Value["workflow_id"].ToString() == workflowId && (Convert.ToInt32(column.Name) == columnId || column.Value["name"].ToString() == selectedColumn)){
					columnId = Convert.ToInt32(column.Name);
					exactColumnMatch = true;
					break;
				} else if (column.Value["workflow_id"].ToString() == workflowId && column.Value["name"].ToString().ToLower() == selectedColumn.ToLower() && !partialColumnMatch) {
					partialMatchColumnId = Convert.ToInt32(column.Name);
					partialColumnMatch = true;
				}
			}

			if (!exactColumnMatch && partialColumnMatch) {
				columnId = partialMatchColumnId;
			} else if (operationId == "CreateCard_V2" && !exactColumnMatch && !partialColumnMatch ) {
				columnId = Convert.ToInt32(boardStructureContent["data"]["workflows"][workflowId]["section_columns"]["1"][0]);
				addColumnComment = true;
			}

			requestBody["column_id"] = columnId;
		}

		if (requestBody.ContainsKey("lane_id")) {
			var laneData = boardStructureContent["data"]["lanes"];
			selectedLane = requestBody["lane_id"].ToString();
			int laneId;
			var partialMatchLaneId = 0;
			var exactLaneMatch = false;
			var partialLaneMatch = false;
			laneIsInt = Int32.TryParse(selectedLane, out laneId);

			foreach (JProperty lane in laneData) {
				if (lane.Value["workflow_id"].ToString() == workflowId && (Convert.ToInt32(lane.Name) == laneId || lane.Value["name"].ToString() == selectedLane)){
					laneId = Convert.ToInt32(lane.Name);
					exactLaneMatch = true;
					break;
				} else if (lane.Value["workflow_id"].ToString() == workflowId && lane.Value["name"].ToString().ToLower() == selectedLane.ToLower() && !partialLaneMatch) {
					partialMatchLaneId = Convert.ToInt32(lane.Name);
					partialLaneMatch = true;
				}
			}

			if (!exactLaneMatch && partialLaneMatch) {
				laneId = partialMatchLaneId;
			} else if (operationId == "CreateCard_V2" && !exactLaneMatch && !partialLaneMatch ) {
				laneId = Convert.ToInt32(boardStructureContent["data"]["workflows"][workflowId]["top_lanes"][0]);
				addLaneComment = true;
			}

			requestBody["lane_id"] = laneId;
		}

		if (requestBody.ContainsKey("stickers")) {
			var missingStickers = (requestBody.ContainsKey("missing_stickers")) ? (int)requestBody["missing_stickers"] : 3;
			var stickers = requestBody["stickers"].ToString().Split(',');
			var stickersToAdd = new JArray();

			response = await this.PerformInternalRequest("/api/v2/stickers?is_enabled=1&expand=board_ids",HttpMethod.Get);
			var stickerDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));


			foreach (var selectedSticker in stickers) {
				var stickerFound = false;
				var stickerAssigned = false;
				var stickerId = 0;
				foreach (var sticker in stickerDetailsContent["data"]) {
					if (sticker["label"].ToString().Trim() == selectedSticker.Trim()) {
						stickerFound = true;
						stickerId = (int)sticker["sticker_id"];
						foreach (var board in sticker["board_ids"]) {
							if ((int)board["board_id"] == Convert.ToInt32(boardId)){
								stickerAssigned = true;
								break;
							}
						}
						break;
					}
				}

				if (!stickerFound && missingStickers == 1) {
					var createStickerBody = new JObject{
						["label"] = selectedSticker.Trim()
					};

					response = await this.PerformInternalRequest("/api/v2/stickers", HttpMethod.Post, createStickerBody.ToString());
					var createStickerContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
					stickerId = (int)createStickerContent["data"]["sticker_id"];
				}

				if (!stickerAssigned && missingStickers == 1) {
					response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/stickers/" + stickerId, HttpMethod.Put);
				}
					
				if ((!stickerFound || !stickerAssigned) && missingStickers == 2) {
					continue;
				} else if ((!stickerFound || !stickerAssigned) && missingStickers == 3) {
					var exResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
		      		exResponse.Content = CreateJsonContent(new JObject{
						["message"] = "This sticker `" + selectedSticker.Trim() + "` cannot be added to cards on board with id " + boardId + " because the sticker has not been created or added to it."
					}.ToString());
				    return exResponse;
				}

				stickersToAdd.Add(new JObject{
					["sticker_id"] = stickerId
				});
				
			}

			requestBody.Add(new JProperty("stickers_to_add", stickersToAdd));
			requestBody.Remove("stickers");
		}

		if (requestBody.ContainsKey("tags")) {
			var missingTags = (requestBody.ContainsKey("missing_tags")) ? (int)requestBody["missing_tags"] : 3;
			var tags = requestBody["tags"].ToString().Split(',');
			var tagsToAdd = new JArray();

			response = await this.PerformInternalRequest("/api/v2/tags?is_enabled=1&expand=board_ids", HttpMethod.Get);
			var tagDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var selectedTag in tags) {
				var tagFound = false;
				var tagAssigned = false;
				var tagId = 0;
				foreach (var tag in tagDetailsContent["data"]) {
					if (tag["label"].ToString().Trim() == selectedTag.Trim()) {
						tagFound = true;
						tagId = (int)tag["tag_id"];
						foreach (var board in tag["board_ids"]) {
							if ((int)board["board_id"] == Convert.ToInt32(boardId)){
								tagAssigned = true;
								break;
							}
						}
						break;
					}
				}

				if (!tagFound && missingTags == 1) {
					var createTagBody = new JObject{
						["label"] = selectedTag.Trim()
					};
					response = await this.PerformInternalRequest("/api/v2/tags", HttpMethod.Post, createTagBody.ToString());
					var createTagContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

					if (createTagContent.ContainsKey("data")) {
						tagId = (int)createTagContent["data"]["tag_id"];
					} else {
						continue;
					}
				}

				if (!tagAssigned && missingTags == 1) {
					response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/tags/" + tagId, HttpMethod.Put);
				}
				
				if ((!tagFound || !tagAssigned) && missingTags == 2) {
					continue;
				} else if ((!tagFound || !tagAssigned) && missingTags == 3) {
					var exResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
		      		exResponse.Content = CreateJsonContent(new JObject{
						["message"] = "This tag `" + selectedTag.Trim() + "` cannot be added to cards on board with id " + boardId + " because the tag has not been created or added to it."
					}.ToString());
				    return exResponse;
				}

				tagsToAdd.Add(tagId);
			}

			requestBody.Add(new JProperty("tag_ids_to_add", tagsToAdd));			
			requestBody.Remove("tags");
		}

		if (requestBody.ContainsKey("reporter_user_id") && requestBody.ContainsKey("reporter_email")) {
			requestBody.Remove("reporter_email");
		}

		if (requestBody.ContainsKey("deadline")) {
			requestBody["deadline"] = requestBody["deadline"].ToString() + "T00:00:00Z";
		}

		this.Context.Request.Content = CreateJsonContent(requestBody.ToString());
		response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

		var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			throw new Exception(content["error"].ToString());;
		}

		if (operationId == "CreateCard_V2" && ((JArray)content["data"]).Count > 0) {
			var cardId = content["data"][0]["card_id"].ToString();
			if (addColumnComment) {
				var comment = columnIsInt ? "Unable to create card in column with id " + selectedColumn + ". Please make sure the column exists on the selected board." : "Unable to create card in \"" + selectedColumn + "\" column. Please check the column title.";
				await this.CreateComment(cardId, comment);
			}

			if (addLaneComment) {
				var comment = laneIsInt ? "Unable to create card in lane with id " + selectedLane + ". Please make sure the lane exists on the selected board." : "Unable to create card in \"" + selectedLane + "\" lane. Please check the lane title.";
				await this.CreateComment(cardId, comment);
			}
		}

		response = await this.SetCardResponse(response).ConfigureAwait(false);
		response = await this.SetCustomFieldNames(response).ConfigureAwait(false);
		response = await this.SetStickersAndTagsNames(response).ConfigureAwait(false);
		
		return response;
	}

	private async Task<HttpResponseMessage> SetCardResponse(HttpResponseMessage response) {
		var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		if (!content.ContainsKey("data")) {
			return response;
		}

		if (((JArray)content["data"]).Count == 0) {
			return response;
		}

		content["data"] = content["data"][0];
		response.Content = CreateJsonContent(content.ToString());
		
		return response;
	}

	private async Task<HttpResponseMessage> SetCustomFieldNames(HttpResponseMessage response) {
		var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		if (content["data"]["custom_fields"] != null && ((JArray)content["data"]["custom_fields"]).Count > 0) {
			var fields = content["data"]["custom_fields"];
			var fieldsList = new List<int>();
			var customFields = new JArray();

			foreach (var field in fields) {
				fieldsList.Add((int)field["field_id"]);
			}

			var fieldIds = String.Join(",", fieldsList.ToArray());
			var customFieldsBody = new JObject(new JProperty("field_ids", fieldIds)).ToString();
			var customFieldsResponse = await this.PerformInternalRequest("/api/v2/customFields?expand=allowed_values", HttpMethod.Post, customFieldsBody, "GET");
			var customFieldsContent = JObject.Parse(await customFieldsResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

			if (customFieldsContent["data"] != null) {
				foreach (var selectedField in fields) {
					foreach (var availableField in customFieldsContent["data"]) {
						if ((int)selectedField["field_id"] == (int)availableField["field_id"]) {
							var updatedField = (JObject)selectedField;
							updatedField.Add(new JProperty("name", availableField["name"]));
							if(availableField["type"].ToString() == "dropdown" && updatedField["values"] != null) {
								var customFieldValues = new JArray();
								foreach (var selectedValue in selectedField["values"]) {
									foreach (var availableValue in availableField["allowed_values"]) {
										if ((int)selectedValue["value_id"] == (int)availableValue["value_id"]) {
											customFieldValues.Add(new JObject{
												["value_id"] = availableValue["value_id"],
												["value"] = availableValue["value"]
											});
										}
									}
								}
								updatedField["values"] = customFieldValues;
							}
							customFields.Add(updatedField);
						}
					}
				}
			}
			content["data"]["custom_fields"] = customFields;
		}

		response.Content = CreateJsonContent(content.ToString());
		return response;
	}

	private async Task<HttpResponseMessage> SetStickersAndTagsNames(HttpResponseMessage response) {
		var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);

		if (content["data"]["stickers"] != null && ((JArray)content["data"]["stickers"]).Count > 0) {
			var stickers = content["data"]["stickers"];
			var stickersList = new List<int>();
			var stickersArray = new JArray();

			foreach (var sticker in stickers) {
				stickersList.Add((int)sticker["sticker_id"]);
			}

			var stickerIds = String.Join(",", stickersList.ToArray());
			var stickersRequestBody = new JObject(new JProperty("sticker_ids", stickerIds)).ToString();
			var stickersResponse = await this.PerformInternalRequest("/api/v2/stickers", HttpMethod.Post, stickersRequestBody, "GET");
			var stickersContent = JObject.Parse(await stickersResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var selectedSticker in stickers) {
				foreach (var availableSticker in stickersContent["data"]) {
					if ((int)selectedSticker["sticker_id"] == (int)availableSticker["sticker_id"]) {
						var updateSticker = (JObject)selectedSticker;
						updateSticker.Add(new JProperty("label", availableSticker["label"]));
						stickersArray.Add(updateSticker);
					}
				}
			}
			content["data"]["stickers"] = stickersArray;
		}

		if (content["data"]["tag_ids"] != null && ((JArray)content["data"]["tag_ids"]).Count > 0) {
			var tags = (JArray)content["data"]["tag_ids"];
			var tagsArray = new JArray();
			var tagIds = String.Join(",", tags.ToObject<int[]>());
			var tagsRequestBody = new JObject(new JProperty("tag_ids", tagIds)).ToString();
			var tagsResponse = await this.PerformInternalRequest("/api/v2/tags", HttpMethod.Post, tagsRequestBody, "GET");
			var tagsContent = JObject.Parse(await tagsResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var selectedTag in tags) {
				foreach (var availableTag in tagsContent["data"]) {
					if ((int)selectedTag == (int)availableTag["tag_id"]) {
						var updatedTag = new JObject();
						updatedTag.Add(new JProperty("tag_id", availableTag["tag_id"]));
						updatedTag.Add(new JProperty("label", availableTag["label"]));
						tagsArray.Add(updatedTag);
					}
				}
			}
			content["data"]["tag_ids"] = tagsArray;
		}

		response.Content = CreateJsonContent(content.ToString());
		return response;
	}

	private async Task<HttpResponseMessage> SetStickersAndTags() {
		var response = new HttpResponseMessage();
		var requestBody = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false));
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
		var boardId = requestBody.ContainsKey("board_id") ? requestBody["board_id"].ToString() : "";
		var action = requestBody["action"].ToString();

		if (requestBody.ContainsKey("stickers")) {
			var stickers = requestBody["stickers"].ToString().Split(',');
			var stickersToAdd = new JArray();
			var stickersToRemove = new JArray();
			var cardStickersToRemove = new JArray();

			var missingStickers = (requestBody.ContainsKey("missing_stickers")) ? (int)requestBody["missing_stickers"] : 1;
			if (action == "unset") {
				missingStickers = 2;
			}

			response = await this.PerformInternalRequest("/api/v2/stickers?is_enabled=1&expand=board_ids", HttpMethod.Get);
			var stickerDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var selectedSticker in stickers) {
				var stickerFound = false;
				var stickerAssigned = false;
				var stickerId = 0;
				foreach (var sticker in stickerDetailsContent["data"]) {
					if (sticker["label"].ToString().ToLower().Trim() == selectedSticker.ToLower().Trim()) {
						stickerFound = true;
						stickerId = (int)sticker["sticker_id"];
						foreach (var board in sticker["board_ids"]) {
							if ((int)board["board_id"] == Convert.ToInt32(boardId)){
								stickerAssigned = true;
								break;
							}
						}
						break;
					}
				}

				if (!stickerFound && missingStickers == 1) {
					var createStickerBody = new JObject{
						["label"] = selectedSticker.Trim()
					};
					response = await this.PerformInternalRequest("/api/v2/stickers", HttpMethod.Post, createStickerBody.ToString());
					var createStickerContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

					stickerId = (int)createStickerContent["data"]["sticker_id"];
				}

				if (!stickerAssigned && missingStickers == 1) {
					response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/stickers/" + stickerId, HttpMethod.Put);
				}
					
				if ((!stickerFound || !stickerAssigned) && missingStickers == 2) {
					continue;
				} else if ((!stickerFound || !stickerAssigned) && missingStickers == 3) {
					var exResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
		      		exResponse.Content = CreateJsonContent(new JObject{
						["message"] = "This sticker `" + selectedSticker.Trim() + "` cannot be added to cards on board with id " + boardId + " because the sticker has not been created or added to it."
					}.ToString());
				    return exResponse;
				}

				if (action == "set") {
					stickersToAdd.Add(new JObject{
						["sticker_id"] = stickerId
					});
				} else {
					stickersToRemove.Add(stickerId);
				}
			}

			if (action == "unset") {
				var getCardRequestUri = uriBuilder.Uri;
				var getCardRequest = new HttpRequestMessage(HttpMethod.Get, getCardRequestUri);
				getCardRequest.Headers.Add("apikey", this.Context.Request.Headers.GetValues("apikey"));
				var getCardResponse = await this.Context.SendAsync(getCardRequest, this.CancellationToken).ConfigureAwait(false);
				var getCardContent = JObject.Parse(await getCardResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

				if ((int)getCardResponse.StatusCode != 200 && (int)getCardResponse.StatusCode != 204) {
					throw new Exception(getCardContent["error"].ToString());;
				}

				if (getCardContent["data"]["stickers"] != null) {
					foreach (var selectedSticker in stickersToRemove) {
						foreach (var sticker in getCardContent["data"]["stickers"]) {
							if ((int)selectedSticker == (int)sticker["sticker_id"]) {
								cardStickersToRemove.Add(sticker["id"]);
							}
						}
					}
				}
			}

			requestBody.Add(action == "set" ? new JProperty("stickers_to_add", stickersToAdd) : new JProperty("sticker_ids_to_remove", cardStickersToRemove));
			requestBody.Remove("action");
			requestBody.Remove("stickers");
			requestBody.Remove("missing_stickers");

		}

		if (requestBody.ContainsKey("tags")) {
			var tags = requestBody["tags"].ToString().Split(',');
			var tagIds = new JArray();

			var missingTags = (requestBody.ContainsKey("missing_tags")) ? (int)requestBody["missing_tags"] : 1;
			if (action == "unset") {
				missingTags = 2;
			}

			response = await this.PerformInternalRequest("/api/v2/tags?is_enabled=1&expand=board_ids", HttpMethod.Get);
			var tagDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var selectedTag in tags) {
				var tagFound = false;
				var tagAssigned = false;
				var tagId = 0;
				foreach (var tag in tagDetailsContent["data"]) {
					if (tag["label"].ToString().ToLower().Trim() == selectedTag.ToLower().Trim()) {
						tagFound = true;
						tagId = (int)tag["tag_id"];
						foreach (var board in tag["board_ids"]) {
							if ((int)board["board_id"] == Convert.ToInt32(boardId)){
								tagAssigned = true;
								break;
							}
						}
						break;
					}
				}

				if (!tagFound && missingTags == 1) {
					var createTagBody = new JObject{
						["label"] = selectedTag.Trim()
					};
					response = await this.PerformInternalRequest("/api/v2/tags", HttpMethod.Post, createTagBody.ToString());
					var createTagContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
					
					tagId = (int)createTagContent["data"]["tag_id"];
				}

				if (!tagAssigned && missingTags == 1) {
					response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/tags/" + tagId, HttpMethod.Put);
				}
				
				if ((!tagFound || !tagAssigned) && missingTags == 2) {
					continue;
				} else if ((!tagFound || !tagAssigned) && missingTags == 3) {
					var exResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
		      		exResponse.Content = CreateJsonContent(new JObject{
						["message"] = "The tag `" + selectedTag.Trim() + "` cannot be added to cards on board with id " + boardId + " because the tag has not been created or added to it."
					}.ToString());
				    return exResponse;
				}

				tagIds.Add(new JValue(tagId));
			}

			requestBody.Add(action == "set" ? new JProperty("tag_ids_to_add", tagIds) : new JProperty("tag_ids_to_remove", tagIds));
			requestBody.Remove("action");
			requestBody.Remove("tags");
			requestBody.Remove("missing_tags");

		}

		this.Context.Request.Content = CreateJsonContent(requestBody.ToString());
		response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
		
		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			throw new Exception(content["error"].ToString());
		}

		return response;
	}

	private async Task<HttpResponseMessage> SetCustomFieldBody() {
		var response = new HttpResponseMessage();
		var requestBody = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false));
		var fieldId = requestBody["field_id"].ToString();
		var fieldDetailsRequestBody = new JObject(new JProperty("field_ids", fieldId)).ToString();
		response = await this.PerformInternalRequest("/api/v2/customFields?expand=allowed_values", HttpMethod.Post, fieldDetailsRequestBody, "GET");
		var fieldDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		var type = fieldDetailsContent["data"][0]["type"].ToString();

		var fieldsArray = new JArray();

		if (type == "dropdown") {
			var valuesToAddArray = new JArray();
			var valuesToRemoveArray = new JArray();
			var valuesList = new List<int>();
			var values = requestBody["values"].ToString().Split(',');

			foreach (var selectedValue in values) {
				var valueFound = false;
				foreach (var availableValue in fieldDetailsContent["data"][0]["allowed_values"]) {
					if (selectedValue.Trim() == availableValue["value"].ToString().Trim()) {
						valuesToAddArray.Add(new JObject{
							["value_id"] = (int)availableValue["value_id"]
						});
						valuesList.Add((int)availableValue["value_id"]);
						valueFound = true;
						break;
					}
				}

				if (!valueFound) {
					var exResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
		      		exResponse.Content = CreateJsonContent(new JObject{
						["message"] = "Value \"" + selectedValue.ToString() + "\" does not exist for the custom field"
					}.ToString());
				    return exResponse;
				}
			}

			foreach (var valueOption in fieldDetailsContent["data"][0]["allowed_values"]) {
				if(!valuesList.Contains((int)valueOption["value_id"])){
					valuesToRemoveArray.Add(valueOption["value_id"]);
				}
			}

			fieldsArray.Add(new JObject{
				["field_id"] = fieldId,
				["selected_values_to_add_or_update"] = valuesToAddArray,
				["selected_value_ids_to_remove"] = valuesToRemoveArray
			});				
		} else {
			var valueString = requestBody["values"];
			
			fieldsArray.Add(new JObject{
				["field_id"] = fieldId,
				["value"] = valueString
			});
		}
			
		this.Context.Request.Content = CreateJsonContent(new JObject(new JProperty("custom_fields_to_add_or_update", fieldsArray)).ToString());
		response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			throw new Exception(content["error"].ToString());
		}

		return response;
	}

	private async Task<HttpResponseMessage> GetObjectName() {
		var response = new HttpResponseMessage();
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
		var requestPath = uriBuilder.Path;

		if (requestPath.Contains("workflows") || requestPath.Contains("lanes") || requestPath.Contains("columns")) {
			var requestHeaders = this.Context.Request.Headers;
			var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
			uriBuilder.Path = requestPath.Replace("/api/v2", "/api/v2/boards/"+boardId);
		}

		this.Context.Request.RequestUri = uriBuilder.Uri;
		
		response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		var responseContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			throw new Exception(responseContent["error"].ToString());
		}

		var responseObject = new JObject();

		if (requestPath.Contains("stickers") || requestPath.Contains("tags") || requestPath.Contains("blockReasons")) {
			responseObject = new JObject {
				["name"] = responseContent["data"]["label"],
			};
		} else if (requestPath.Contains("users")) {
			responseObject = new JObject {
				["name"] = responseContent["data"]["username"],
			};
		} else {
			responseObject = new JObject {
				["name"] = responseContent["data"]["name"],
			};
		}
		
		response.Content = CreateJsonContent(new JObject(new JProperty("data", responseObject)).ToString());
	
		return response;
	}

	private async Task<HttpResponseMessage> GetDropdownValuesResponse() {
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
		var fieldDetailsEndpoint = uriBuilder.Path.Replace("/allowedValues", "");
		var response = await this.PerformInternalRequest(fieldDetailsEndpoint, HttpMethod.Get);
		var fieldDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
		var type = fieldDetailsContent["data"]["type"].ToString();

		if (type == "dropdown"){
			response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

			if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
				var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
				throw new Exception(content["error"].ToString());
			}
		} else {
			response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = CreateJsonContent(new JObject{}.ToString());
		}

		return response;
	}

	private async Task<HttpResponseMessage> GetUsers() {
		var response = new HttpResponseMessage();
		var requestHeaders = this.Context.Request.Headers;
		var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
		var usersList = new List<int>();
		var teamsList = new List<int>();

		response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/userRoles", HttpMethod.Get);
		var getBoardUsersContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		foreach (var user in getBoardUsersContent["data"]) {
			usersList.Add((int)user["user_id"]);
		}

		response = await this.PerformInternalRequest("/api/v2/boards/" + boardId + "/teams", HttpMethod.Get);
		var getBoardTeamsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		foreach (var team in getBoardTeamsContent["data"]) {
			teamsList.Add((int)team["team_id"]);
		}

		if (teamsList.Count > 0) {
			var teamIds = String.Join(",", teamsList.ToArray());
			var getTeamsUsersRequestBody = new JObject(new JProperty("team_ids", teamIds)).ToString();
			response = await this.PerformInternalRequest("/api/v2/teams?expand=user_ids", HttpMethod.Post, getTeamsUsersRequestBody, "GET");
			var getTeamsUsersContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

			foreach (var team in getTeamsUsersContent["data"]) {
				foreach (var user in team["user_ids"]) {
					usersList.Add((int)user["user_id"]);
				}
			}
			usersList = usersList.Distinct().ToList();
		}

		if (usersList.Count > 0) {
			var userIds = String.Join(",", usersList.ToArray());
			this.Context.Request.Headers.Add("x-http-method-override","GET");
			this.Context.Request.Content = CreateJsonContent(new JObject(new JProperty("user_ids", userIds)).ToString());
			response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

			if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
				var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
				throw new Exception(content["error"].ToString());
			}
		} else {
			response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = CreateJsonContent(new JObject{}.ToString());
		}

		return response;
	}

	private async Task<HttpResponseMessage> DownloadAttachment() {
		var requestHeaders = this.Context.Request.Headers;
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
	
		var response = new HttpResponseMessage();
	
		var cardId = requestHeaders.GetValues("card_id").ToList()[0].ToString();
		var attachmentId = requestHeaders.GetValues("attachment_id").ToList()[0].ToString();

		response = await this.PerformInternalRequest("/api/v2/cards/" + cardId + "/attachments/" + attachmentId, HttpMethod.Get);
		var attachDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
	
		var attachmentLink = attachDetailsContent["data"]["link"].ToString();
		var attachmentFileName = attachDetailsContent["data"]["file_name"].ToString();

		uriBuilder.Query = "?link=" + attachmentLink + "&encoding=base64";
		this.Context.Request.RequestUri = uriBuilder.Uri;
		response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		var attachmentContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			throw new Exception(attachmentContent);
		}

		var contentObject = new JObject {
			["$content-type"] = "application/octet-stream",
			["$content"] = attachmentContent
		};

		var dataObject = new JObject{
			["file_name"] = attachmentFileName,
			["content"] = contentObject
		};
	
		response.Content = CreateJsonContent(new JObject(new JProperty("data", dataObject)).ToString());
		return response;
	}

	private async Task<HttpResponseMessage> UploadAttachment(){
		var requestHeaders = this.Context.Request.Headers;
		var response = new HttpResponseMessage();
		var requestBody = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false));
		var cardId = requestHeaders.GetValues("card_id").ToList()[0].ToString();

		if (requestBody.ContainsKey("content")) {
			var content = JObject.Parse(requestBody["content"].ToString());
			if (content.ContainsKey("$content")) {
				requestBody["content"] = content["$content"];
			}
			this.Context.Request.Content = CreateJsonContent(requestBody.ToString());
		}

		response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		var attachDetailsContent = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			throw new Exception(attachDetailsContent["error"].ToString());
		}


		var attachmentLink = attachDetailsContent["data"]["link"].ToString();
		var attachmentFileName = attachDetailsContent["data"]["file_name"].ToString();
	
		var dataArray = new JArray();
	
		dataArray.Add(new JObject{
			["file_name"] = attachmentFileName,
			["link"] = attachmentLink
		});
	
		var linkAttachContent = new JObject(new JProperty("attachments_to_add",dataArray));
		response = await this.PerformInternalRequest("/api/v2/cards/" + cardId, HttpMethod.Post, linkAttachContent.ToString(), "PATCH");
	
		return response;
	}

	private async Task<HttpResponseMessage> GetCardByCustomIdResponse(HttpResponseMessage response) {
		var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
		var items = (JArray)content["data"]["data"];

		if (items.Count > 0) {
			response.Content = CreateJsonContent(new JObject(new JProperty("data", content["data"]["data"][0])).ToString());  
		} else {
			response.Content = CreateJsonContent(new JObject(new JProperty("data", new JObject{})).ToString());
		}

		return response;
	}

    private async Task<HttpResponseMessage> FilterData(HttpResponseMessage response, string operationId) {
		var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
		var requestHeaders = this.Context.Request.Headers;
		var dataArray = new JArray();
		var items = (JArray)content["data"];
			
		if (items.Count > 0) {
			switch(operationId){
				case "GetTemplates":
					if(requestHeaders.Contains("board_id")){
						var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
						foreach (var template in content["data"]){
							foreach (var board in template["board_ids"]){
								if ((int)board["board_id"] == Convert.ToInt32(boardId)){
									dataArray.Add(new JObject{
										["template_id"] = template["template_id"],
										["name"] = template["name"]
									});
								}
							} 
						}
					}
					break;
				case "GetCustomFields":
					if(requestHeaders.Contains("board_id")){
						var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
						foreach (var customField in content["data"]){
							foreach (var board in customField["boards"]){
								if ((int)board["board_id"] == Convert.ToInt32(boardId)){
									dataArray.Add(new JObject{
										["field_id"] = customField["field_id"],
										["name"] = customField["name"]
									});
								}
							} 
						}
					}
					break;
				case "GetTypes_V2":
					if(requestHeaders.Contains("board_id")){
						var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
						foreach (var type in content["data"]){
							foreach (var board in type["boards"]){
								if ((int)board["board_id"] == Convert.ToInt32(boardId)){
									dataArray.Add(new JObject{
										["type_id"] = type["type_id"],
										["name"] = type["name"]
									});
								}
							} 
						}
					}
					break;
				case "GetLanes_V2":
					if(requestHeaders.Contains("workflow_id")){
						var workflowID = requestHeaders.GetValues("workflow_id").ToList()[0].ToString();
						foreach (var lane in content["data"]){
							if ((int)lane["workflow_id"] == Convert.ToInt32(workflowID)){
								dataArray.Add(new JObject{
									["lane_id"] = lane["lane_id"],
									["name"] = lane["name"]
								});
							}
						}
					}
					break;
				case "GetColumns_V2":
					if(requestHeaders.Contains("workflow_id")){
						var workflowID = requestHeaders.GetValues("workflow_id").ToList()[0].ToString();
						foreach (var column in content["data"]){
							if ((int)column["workflow_id"] == Convert.ToInt32(workflowID)){
								dataArray.Add(new JObject{
									["column_id"] = column["column_id"],
									["name"] = column["name"]
								});
							}
						}
					}
					break;
				case "GetBoards_V2":
					foreach(var board in content["data"]) {
						dataArray.Add(new JObject{
							["board_id"] = board["board_id"],
							["workspace_id"] = board["workspace_id"],
							["is_archived"] = board["is_archived"],
							["name"] = board["name"]+" (" + board["board_id"] + ")",
							["description"] = board["description"]
						});
					}
					break;
				case "GetStickers":
					if(requestHeaders.Contains("board_id")){
						var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
						foreach (var sticker in content["data"]){
							foreach (var board in sticker["board_ids"]){
								if ((int)board["board_id"] == Convert.ToInt32(boardId)){
									dataArray.Add(new JObject{
										["sticker_id"] = sticker["sticker_id"],
										["name"] = sticker["label"]
									});
								}
							} 
						}
					}
					break;
				case "GetTags":
					if(requestHeaders.Contains("board_id")){
						var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
						foreach (var tag in content["data"]){
							foreach (var board in tag["board_ids"]){
								if ((int)board["board_id"] == Convert.ToInt32(boardId)){
									dataArray.Add(new JObject{
										["tag_id"] = tag["tag_id"],
										["name"] = tag["label"]
									});
								}
							} 
						}
					}
					break;
				case "GetBlockReasons":
					if(requestHeaders.Contains("board_id")){
						var boardId = requestHeaders.GetValues("board_id").ToList()[0].ToString();
						foreach (var reason in content["data"]){
							foreach (var board in reason["board_ids"]){
								if ((int)board["board_id"] == Convert.ToInt32(boardId)){
									dataArray.Add(new JObject{
										["reason_id"] = reason["reason_id"],
										["name"] = reason["label"]
									});
								}
							} 
						}
					}
					break;
 			}
			response.Content = CreateJsonContent(new JObject(new JProperty("data", dataArray)).ToString());
		}

		return response;
    }

	private async Task<HttpResponseMessage> PerformInternalRequest(string endpoint, HttpMethod method, string body="", string overrideMethod="") {
		var response = new HttpResponseMessage();
		var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
		var requestUri = "https://" + uriBuilder.Host + endpoint;
		var request = new HttpRequestMessage(method, requestUri);
		
		request.Headers.Add("apikey", this.Context.Request.Headers.GetValues("apikey"));
		if (overrideMethod != "") {
			request.Headers.Add("x-http-method-override",overrideMethod);
		}
		
		if (body != "") {
			request.Content = CreateJsonContent(body);
		}

		response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		if ((int)response.StatusCode != 200 && (int)response.StatusCode != 204) {
			var content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			throw new Exception(content["error"].ToString());
		}

		return response;
	}
}
