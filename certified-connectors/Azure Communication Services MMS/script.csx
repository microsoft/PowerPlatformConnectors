// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.PowerPlatform.Connectors.CustomCode.CSharp;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (Context.OperationId == "SendMMS")
        {
            await PrepareRequestWithEndpointAndHMAC();
            return await Context.SendAsync(Context.Request, CancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        
        var error = $"Unknown operation ID '{Context.OperationId}'";
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent("{\"error\": \"" + error + "\"}");
        return response;
    }

    /**
     * PrepareRequestWithEndpointAndHMAC is a common function between connectors. We don't have a good way to share code
     * right now so it is simply copy-pasted to other connectors. If you make changes to this function please update the
     * other connectors and unit tests.
     *
     * Uses Connection-String header to extract endpoint and generate HMAC. Applies endpoint to the Request in Context
     * and adds the HMAC as an Authorization header.
     */
    public async Task PrepareRequestWithEndpointAndHMAC()
    {
        var headerValues = Context.Request.Headers.GetValues("Connection-String");
        Context.Request.Headers.Remove("Connection-String");
        var connectionString = headerValues.FirstOrDefault<string>();

        var endpoint = ExtractEndpointFromConnectionString(connectionString);
        var accessKey = ExtractAccessKeyFromConnectionString(connectionString);

        var body = await Context.Request.Content.ReadAsStringAsync();
        var sha256 = SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(body);
        sha256.ComputeHash(bytes);

        var contentHash = Convert.ToBase64String(sha256.Hash);
        var date = DateTimeOffset.UtcNow.ToString("r");

        var queryString = Context.Request.RequestUri.PathAndQuery;

        var authority = new Uri(endpoint).Authority;
        var payload = System.Text.Encoding.UTF8.GetBytes(Context.Request.Method.ToString() + "\n" + queryString + "\n" + date + ";" + authority + ";" + contentHash);
        var hmac = new HMACSHA256(Convert.FromBase64String(accessKey)).ComputeHash(payload);
        var authorizationHeader = "HMAC-SHA256 SignedHeaders=date;host;x-ms-content-sha256&Signature=" + Convert.ToBase64String(hmac);
        Context.Request.Headers.TryAddWithoutValidation("Authorization", authorizationHeader);
        Context.Request.Headers.TryAddWithoutValidation("Date", date);
        Context.Request.Headers.TryAddWithoutValidation("x-ms-content-sha256", contentHash);

        var endpointUri = new Uri(endpoint);

        var builder = new UriBuilder(Context.Request.RequestUri);
        builder.Host = endpointUri.Host;
        Context.Request.RequestUri = builder.Uri;
    }

    /**
     * ExtractEndpointFromConnectionString is a common function between connectors. We don't have a good way to share
     * code right now so it is simply copy-pasted to other connectors. If you make changes to this function please
     * update the other connectors and unit tests.
     *
     * Extracts the endpoint as a string from the given connectionString.
     */
    public static string ExtractEndpointFromConnectionString(string connectionString)
    {
        //Get endpoint from connection string
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("EXPECTED400: connectionString cannot be empty.");
        }

        const string endpointPrefix = "endpoint=";

        var splitConnStr = connectionString.Split(';');
        var endpoint = "";
        foreach (var v in splitConnStr)
        {
            if (v.StartsWith(endpointPrefix, StringComparison.OrdinalIgnoreCase))
            {
                endpoint = v.Substring(endpointPrefix.Length);
            }
        }

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new Exception("EXPECTED400: endpoint in connectionString is empty.");
        }

        return endpoint;
    }

    /**
     * ExtractAccessKeyFromConnectionString is a common function between connectors. We don't have a good way to share
     * code right now so it is simply copy-pasted to other connectors. If you make changes to this function please
     * update the other connectors and unit tests.
     *
     * Extracts the access key as a string from the given connectionString.
     */
    public static string ExtractAccessKeyFromConnectionString(string connectionString)
    {
        //Get AccessKey from connection string
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("EXPECTED400: connectionString cannot be empty.");
        }

        const string accessKeyPrefix = "accesskey=";

        var splitConnStr = connectionString.Split(';');
        var accessKey = "";
        foreach (var v in splitConnStr)
        {
            if (v.StartsWith(accessKeyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                accessKey = v.Substring(accessKeyPrefix.Length);
            }
        }

        if (string.IsNullOrWhiteSpace(accessKey))
        {
            throw new Exception("EXPECTED400: accessKey in connectionString is empty.");
        }

        return accessKey;
    }
}
