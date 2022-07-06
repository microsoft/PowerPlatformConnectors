# ConsenSys Ethereum Connector
The ConsenSys Ethereum connector allows customers to connect to Ethereum based chains via RPC, creating and interacting with Smart Contracts. This includes reading and writing data on-chain via a known RPC endpoint.

## Prerequisites
---
* An Infura or Quorum Blockchain Services account

## Supported Operations
---
* `Deploy Smart Contract`: Deploy Smart Contract to a configured chain.
* `Execute Query Function`: Execute a function on a smart contract that. **This does not affect chain state.** There operations are do not consume gas.
* `Get Contract State`: Execute all query functions that are parameterless in one call. **This does not affect chain state.**  There operations do not consume gas.
* `Execute Mutating Function`: Execute a function on a smart contract that **affects chain state**. There operations are expected to consume gas.
* `Listen/Poll For Event Triggers`: Create a trigger that listens for on-chain events. This event trigger will provide automated polling data as expected by PowerApps.

## Obtaining Credentials
---
As a Quorum Blockchain Services or Infura customer, you will be provided with RPC Endpoints and API Keys.

## Known Issues and Limitations
---
* Complex data types such as structs are currently not supported.
* Private GoQuorum/Tessera transactions are currently not supported.

## Frequently Asked Questions
---
* `Are parameterless functions supported?` Yes, these are supported for both querying and writing to the chain.
* `Are structs supported?` Not currently as these are under development.
* `Can I customise the event polling/Retry-After?` Not currently.

## Deployment instructions
---
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.