public class Script : ScriptBase

{




    public override async Task<HttpResponseMessage> ExecuteAsync()

    {

        if (this.Context.OperationId == "Convert")

        {

            return await this.ConvertToObjects().ConfigureAwait(false);

        }




        return createErrorResponse("Not implemented", HttpStatusCode.BadRequest);

    }




    private async Task<HttpResponseMessage> ConvertToObjects()

    {

        try

        {

            HttpResponseMessage response;




            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

            var contentAsJson = JObject.Parse(contentAsString);




            // check for parameters

            if (contentAsJson["data"] == null || contentAsJson["resultSetMetadata"]==null)

            {

                return createErrorResponse("resultSetMetadata or data parameter are empty!", HttpStatusCode.BadRequest);

            }




            // get metadata

            var cols = JArray.Parse(contentAsJson["resultSetMetadata"].ToString());

            var rows = JArray.Parse(contentAsJson["data"].ToString());




            JArray newRows = new JArray();

            foreach (var row in rows)

            {

                JObject newRow = new JObject();

                int i = 0;




                foreach (var col in cols)

                {

                    var name = col["name"].ToString();

                    string type = col["type"].ToString();

                    if (newRow.ContainsKey(name)) name = name + "_" + Convert.ToString(i);




                    switch (type)

                    {

                        case "fixed":

                            long scale = 0;

                            long.TryParse(col["scale"].ToString(), out scale);

                            if (scale == 0)

                            {

                                long myLong = long.Parse(row[i].ToString());

                                newRow.Add(new JProperty(name.ToString(), myLong));

                            } else {

                                // we are losing fidelity here

                                double myDouble = double.Parse(row[i].ToString());

                                newRow.Add(new JProperty(name.ToString(), myDouble));

                            }

                            break;

                        case "float":

                            float myFloat = float.Parse(row[i].ToString());

                            newRow.Add(new JProperty(name.ToString(), myFloat));

                            break;

                        case "boolean":

                            bool myBool = bool.Parse(row[i].ToString());

                            newRow.Add(new JProperty(name.ToString(), myBool));

                            break;

                        default:

                            if (type.ToString().IndexOf("time") >= 0)

                            {




                                double unixTimeStamp = Convert.ToDouble(row[i].ToString());




                                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                                dateTime = dateTime.AddSeconds(unixTimeStamp);




                                newRow.Add(new JProperty(name.ToString(), dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")));

                            }

                            else

                            {

                                var val = row[i];

                                newRow.Add(new JProperty(name.ToString(), val));

                            }

                            break;

                    }

                    i++;




                }




                newRows.Add(newRow);




            }




            JObject output = new JObject

            {

                ["data"] = newRows

            };

            response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = CreateJsonContent(output.ToString());

            return response;

        }

        catch (JsonReaderException ex)

        {

            return createErrorResponse("'rowType' or 'data' are in an invalid format: " + ex, HttpStatusCode.BadRequest);

        }

        catch (Exception ex)

        {

            return createErrorResponse(ex.GetType()+":"+ex, HttpStatusCode.InternalServerError);

        }

    }




    private static HttpResponseMessage createErrorResponse(String msg, HttpStatusCode code)

    {

        JObject output = new JObject

        {

            ["Message"] = msg

        };

        var response = new HttpResponseMessage(code);

        response.Content = CreateJsonContent(output.ToString());

        return response;

    }




}
