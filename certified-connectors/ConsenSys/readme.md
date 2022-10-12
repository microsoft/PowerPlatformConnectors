# ConsenSys Ethereum Connector
The ConsenSys Ethereum connector allows customers to connect to Ethereum based blockchains via RPC, creating and interacting with Smart Contracts. This includes reading and writing data on-chain via a known RPC endpoint.

## Publisher: ConsenSys

## Prerequisites
---
* An Infura or Quorum Blockchain Services account

## Supported Operations
---
### Deploy Smart Contract
Deploy Smart Contract to a configured chain receiving the contract address and transaction hash in response. This returns the new contract address and transaction hash.

### Execute Query Function
Execute a function on a smart contract that **does NOT affects chain state**. These operations do not consume gas. Returns the function output.

### Get Contract State
Execute all query functions that are parameterless in one call that **does NOT affect chain state**. These operations do not consume 
gas. Returns output for all parameterless query functions.

### Execute Mutating Function
Execute a function on a smart contract that **affects chain state**. These operations are expected to consume gas. Returns the transaction hash.

### Listen/Poll For Event Triggers
Create a trigger that listens for on-chain events. This event trigger will provide automated polling data and a `Retry-After` header as expected by PowerApps. Returns the event values if there is a new one and can be empty if no events found.

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