Bloomflow is the innovation management platform empowering large enterprises to thrive in today’s rapidly changing business landscape.  Leverage this connector to streamline collaboration and enhance productivity through efficient automated workflows.
​
## Prerequisites
You need to be a Bloomflow eterprise user and your organization needs to have access to the [Blooflow Public API](https://docs.bloomflow.com/public/public-api.html) package. 
To know more about Bloomflow and our services, get in touch with us at [hello@bloomflow.com](hello@bloomflow.com).
​
## How to get credentials
Bloomflow APIs use API key authorization to ensure that client requests access data securely. With API key auth, you send a key-value pair to the API in the request headers for every call.
Request header must have a **x-bflow-api-key** field containing the API key
When you are ready to work with the public API ask your credentials by contacting us at customer@bloomflow.com.
​
​
## Known Issues and Limitations
* Currently the `Workflow property changed` trigger only lets you track changes on Workflow Steps. This scope is likely to be expanded to cover changes in other workflow elements such as State, Groups and Tasks.
* Currently the `New Application created` trigger gives access to limited information on incoming applications. 
​
## Common errors and remedies
Here are some issues you could face while using the connector:
* `Error 400`: Appears when you make a bad request, it means that you wrongly enter the parameters. Check how to configure them correctly with our [documentation](https://docs.bloomflow.com/public/public-api.html).
* `Error 401`: Appears when you are unautorized to access the requested information.
* `Error 403`: Appears when you wrongly enter your API Key.
​
If you have any questions about our APIs, check our [documentation](https://docs.bloomflow.com/public/public-api.html) or feel free to [contact us](hello@bloomflow.com). 