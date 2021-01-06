SAS Intelligent Decisioning on SAS Viya combines business rules management, decision processing, real-time event detection, decision governance, and powerful SAS advanced analytics to automate and manage decisions across the enterprise. SAS Intelligent Decisioning helps expedite the operationalization of analytical models, including both SAS models and those developed with open source frameworks. It supports the decisioning process in credit services, fraud prevention, manufacturing, personalized marketing, and many other use cases. 
 
The SAS Decisioning connector enables users to connect to their SAS Viya environment and run selected decision modules and analytical models. Example use cases for the SAS Decisioning connector include:

*	Determine if the email content complies with a company’s corporate policy
*	Score a job applicant based on interview feedback
*	Score a sales opportunity
*	Determine if an invoice is legitimate
*	Automatically approve an expense report

To learn more about SAS Intelligent Decisioning and to request a free trial, please visit https://www.sas.com/en_us/software/intelligent-decisioning.html.

## Prerequisites
1.	The user must have an existing SAS Intelligent Decisioning license.
2.	The user must configure the SAS Intelligent Decisioning environment to allow calls coming from the IP Addresses described in [Azure IP Ranges](https://www.microsoft.com/en-us/download/details.aspx?id=56519).


## How to use the SAS Decisioning connector
1. Register your client applications.
2. Obtain an access token for use in connecting to your SAS Viya environment.
3. Connect to your SAS Viya environment.

### Registering clients

All applications and scripts that use SAS Viya REST APIs must be registered with the SAS environment. Your SAS administrator must use the OAuth service in SAS Logon Manager to request an access token and to register a client. SAS Logon Manager issues OAuth access tokens in response to requests that contain a valid token from the SAS Configuration Server. SAS Decisioning Connector uses an OAuth Bearer token as an API Key.

To register a client:

1.  Locate a valid Consul token. A SAS administrator can find a token in the **client.token** file at  `/opt/sas/viya/config/etc/SASSecurityCertificateFramework/tokens/consul/default` inside the `sas-consul-server-0` pod. To retrieve the Consul token string on a Linux system, run the following command:  
    ```bash
    kubectl exec sas-consul-server-0 -- \
        cat /opt/sas/viya/config/etc/SASSecurityCertificateFramework/tokens/consul/default/client.token
    ```
    
2.  Request an OAuth token by posting a request to `/SASLogon/oauth/clients/consul`. Specify the Consul token from step 1 in the `X-Consul-Token` field. For example, to request a token for a client named `app`, submit the following command:  

    ```bash
    curl -X POST "https://server.example.com/SASLogon/oauth/clients/consul?callback=false&serviceId=app" \
          -H "X-Consul-Token: <consul-token-from-step-1>"
    ```

    | Query parameter       | Description |
    |-----------------------|------------------|
    | callback              | Specify `false` in order to receive an access token in the response. Otherwise, the token is sent to the service registered in SAS Configuration Server.|
    | serviceId             | Specify the name of the client that you want to register.|

    In response to the request, SAS Logon Manager returns a JSON response that includes an access token in the **access_token** field.

3.  Register the client application by posting a request to `/SASLogon/oauth/clients`. Specify the access token that was returned in step 2 as the value of the `Authorization` header:  

    ```bash
    curl -X POST "https://server.example.com/SASLogon/oauth/clients" \
        -H "Content-Type: application/json" \
        -H "Authorization: Bearer <access-token-from-step-2>" \
        -d '{
          "client_id": "<client-id-goes-here>",
          "client_secret": "<client-secret-goes-here>",
          "scope": ["openid"],
          "authorized_grant_types": ["client_credentials"],
          "access_token_validity": 43199
         }'
    ```

    **Tip:** By default, a token is valid for 12 hours (or 43200 seconds). To set a shorter or longer duration, set the `access_token_validity` field as needed, using an integer value for time in seconds.
    
    **Note:** There are other "authorized_grant_types" supported, but the SAS Decisioning connector currently expects the `client_credentials` grant type. 

If the request is successful, the client is registered. A successful JSON response looks similar to the following example:  
     
    {"scope":["openid"],"client_id":"<requested-client-id>","resource_ids":["none"],"authorized_grant_types":["client_credentials"],
    "access_token_validity":43199,"authorities":["uaa.none"],"lastModified":1521124986406}

### Obtaining access tokens

Registered clients can request an access token using the SAS Logon OAuth API. To request an access token, post a request to the `/SASLogon/oauth/token` endpoint and specify a form of authorization. The authorization is expressed in the form of an authorization grant. Currently, SAS Viya REST APIs support the `client_credentials` grant type.

For example, given a client identifier of "myclient" with a secret "mysecret," you can request a token as follows:

```bash
    curl -X POST "https://server.example.com/SASLogon/oauth/token" \
      -H "Content-Type: application/x-www-form-urlencoded" \
      -d "grant_type=client_credentials&client_id=myclient&client_secret=mysecret"
```

The JSON response contains a field named `access_token` that contains the value of the token that is used by the SAS Decisioning connector to create Power Automate connections. When the token expires, post a new request to the `/SASLogon/oauth/token` endpoint.

### Connecting to your SAS Viya environment
To connect to your SAS Viya environment, you will need the following information:

| Parameter name            | Description |
|---------------------------|------------------|
| SAS Viya Environment URL  | URL of the SAS Intelligent Decisioning deployment. For example: https://server.example.com|
| Authorization Token       | The access token returned from the command in [Obtaining access tokens](#obtaining-access-tokens) above. Prefix the token with `Bearer `.|

The connection can be shared between users.