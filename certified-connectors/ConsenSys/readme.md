# ConsenSys Ethereum Connector
The ConsenSys Ethereum connector allows customers to connect to Ethereum based blockchains via RPC, creating and interacting with Smart Contracts. This includes reading and writing data on-chain via a known RPC endpoint.

## Publisher: ConsenSys

## Prerequisites
---
* An Infura or Quorum Blockchain Services account

## Supported Operations
---
### Get Next Nonce
Gets the next transaction nonce for the account being used.

### Get Transaction Receipt
Retrieves the transaction receipt for a known transaction hash (from deployments or contract execution). This will return if the transaction is processed, failed, the transaction hash, and if applicable, the new contract address.

### Deploy Smart Contract
Deploy Smart Contract to a configured chain receiving the transaction hash in response. This returns transaction hash.

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
* Private GoQuorum/Tessera transactions are currently not supported.

## Frequently Asked Questions
---
* `Are parameterless functions supported?` Yes, these are supported for both querying and writing to the chain.
* `Are structs supported?` Not currently as these are under development.
* `Can I customise the event polling/Retry-After?` Not currently.
* `How do I send or receive complex data types?` These are supported as JSON objects and should be passed and interpreted as such unless they are arrays of base types, or just base types.
* `What are the new optional gas fields?` These fields are to allow more refined gas customisation for networks that support the EIP-1559 gas estimation/calculation. If unspecified and the chain supports it, we now default to EIP-1559 estimation.
* `Why do I need to get the next nonce?` Transactions need to be sequential and have a unique reference. In order to correct/cancel a failed transaction or rework it before it is processed (or fails to process), you will need to pass the nonce in sequence. 
* `Why do I need to send the nonce?` Using the nonce as an input will allow customisation and manual setting to correct chain blockages.
* `Why do I need to poll for the transaction receipt?` This is needed as transactions may take a longer time to be processed, depending on gas market cost fluctuations or previous pending transactions. Be sure to use a delay between polling requests.

## Deployment instructions
---
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.