# Infura Ethereum Connector

Infura provides services to access the Ethereum Blockchain without your own Ethereum Node. This connector allows you to interact with the Ethereum Blockchain JSON-RPC API through Infura. Common use cases include checking your Ethereum account's (Wallet) balance, current gas fees, or information on the current block.

## Publisher: Sebastian Zolg

## Prerequisites

* An Infura [Free Plan](https://infura.io/pricing) (or higher)
* An Infura Ethereum Project
* The Project Id of the Infura Ethereum Project

## Supported Operations

The connector supports the following operations:

|Operation Id  |Name  |Description  |Support  |
|---------|---------|---------|---------|
|eth_getBlockNumber|Block Number|Returns the number of most recent block.|✅|
|eth_getBalance|Balance|Returns the balance of the account of given address.|✅|
|eth_gasPrice|Gas Price|Returns the current price per gas in wei.|✅|

## Obtaining Credentials

The Infura Ethereum Connector uses the Infura Project Id to communicate with the JSON-RPC endpoint. See the **Getting Started** section on how to get a new Project Id.
The **Project Id** is considered a secret and securely stored inside the Power Platform connection. Do not share the Project Id.

## Getting Started

1) Signup for an Infura [Free Plan](https://infura.io/pricing).
1) Go to your Infura [Dashboard](https://infura.io/dashboard) and click **Create New Project**.
1) Select **Ethereum** as Product, and provide a new name, e.g. *Power Platform Ethereum Connector*.
1) Inside your new project, click **Project Settings**.
1) Copy the **Project Id** (not the Project Secret).
1) Create a new Connection to the Infura Ethereum Connector, and provide the **Project Id**.

## Known Issues and Limitations

* Currently, there is no support for `basic` or `JWT token` authentication. Please refer to the [Securing Infura](#securing-infura) section for further details.

## Deployment Instructions

In addition to what is listed in the [Getting Started](#getting-started) section, please implement the connector according to the [Securing Infura](#securing-infura) section for additional security. Please refer to the [Transformation and Routing](#transformation-and-routing) section to better understand how the connector works.

### Securing Infura

As mentioned above, the Infura **Project Id** should be treated as a secret. Although the Power Platform is considered a secure environment, and the **Project Id** is stored as a secure string, it is used as part of the url path, which could result in leakage of your **Project Id**, e.g., through log files. There are more advanced security scenarios, such as JWT token auth, which isn't supported by this version of the connector.

To improve security, it is recommended to do the following:

1) Go to your Infura [Dashboard](https://infura.io/dashboard) and click **Settings** on your project.
1) Under **Security** configure `PER SECOND REQUESTS RATE-LIMITING` and `PER DAY TOTAL REQUESTS` according to your usage pattern.
1) If known in advance, add every account and contract address you interact with to the `CONTRACT ADDRESSES` list.
1) Set the allowed `ORIGINS` to `*.flow.microsoft.com`.
1) Limit `API REQUEST METHOD` to the methods you use, e.g., `eth_getBalance`. See [Supported Operations](#supported-operations).

### Transformation and Routing

#### Custom Code

This connector uses a custom code script (`script.csx`) to transform the API request from REST into JSON-RPC as used by the Infura API. JSON-RPC has a few downsides compared to REST when it comes to UX. To have the best possible UI support inside the Power Platform, we do the necessary transformation inside the connector's custom code.
The most important transformation steps are as follows:

* Flattening objects and their properties into a positional array understood by JSON-RPC.
* Transforming the response (`result`) from HEX encoded byte strings into decimal values for ease of use.

#### Path Templates

Unlike RESTful APIs, JSON-RPC doesn't rely on url path templates. Instead, every request hits the exact same url. The action to be invoked, such as `eth_getBalance` is provided as a `method` property in the post `body`. This pattern can't be modeled as OpenAPI (swagger) natively. Since the Power Platform experience relies on this pattern, we model the connector as a RESTful API and use a `Route request` policy template to redirect every incoming request to a single JSON-RPC endpoint. Furthermore, we use this approach to append the necessary **Project Id** to the path template on the fly. See the [apiProperties.json](apiProperties.json) file for details.
