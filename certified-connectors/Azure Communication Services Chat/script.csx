// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.PowerPlatform.Connectors.CustomCode.CSharp;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {

        string[] operationIds = {
            "SendChat",
            "AddParticipants",
            "RemoveParticipant",
            "CreateChat",
            "ListParticipants",
            "ListMessages",
            "ListChatThreads",
            "GetThreadProperties",
            "UpdateChatThreadProperties",
            "DeleteChatThread"
        };

        if (Array.Exists(operationIds, opId => opId == Context.OperationId))
        {
            return await AddAuthorizationHeader();
        }

        // Handle an invalid operation ID
        var error = $"Unknown operation ID '{Context.OperationId}'";
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent("[{\"error\":\"" + error + "\"}]");
        return response;
    }

    public async Task<HttpResponseMessage> AddAuthorizationHeader()
    {

        var headerValuesConString = Context.Request.Headers.GetValues("Endpoint-Url");
        Context.Request.Headers.Remove("Endpoint-Url");
        var endpointString = headerValuesConString.FirstOrDefault<string>();

        var endpointUri = new Uri(endpointString);
        var builder = new UriBuilder(Context.Request.RequestUri);
        builder.Host = endpointUri.Host;
        Context.Request.RequestUri = builder.Uri;

        var headerValuesToken = Context.Request.Headers.GetValues("Access-Token");
        Context.Request.Headers.Remove("Access-Token");
        var accessToken = headerValuesToken.FirstOrDefault<string>();

        Context.Request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);

        var response = await Context.SendAsync(Context.Request, CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
