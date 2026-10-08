# Whale Alert

Monitor crypto market trends and large transactions with Whale Alert. Set personalized alerts, view live price updates and analyze crypto data.

## Publisher: Fördős András

## Obtaining Credentials

You will need to sign up for a developer account and plan at [Whale Alert API](https://developer.whale-alert.io/api-account/signup). Ocnce done, under your profile, you are able to generate an API Key, that you will need to use with this connector.

## Supported Operations

### Check status
Returns a full overview of the supported blockchains and currencies available per blockchain.

### Check blockchain status
Returns the heights of the newest and oldest available blocks of a specific blockchain. Older blocks are available at request.

### Get transaction
Returns a specific transaction for a blockchain.

### List transactions
Returns all transactions for a blockchain since the specified start height. If the start height is not available due to age the first available height is retrieved.

### List address transactions
Returns the transactions for an address for the last 30 days.

### Get owner
Returns the owners of the specified address. A single address might have multiple owners, especially in cases where it's linked to a white label exchange or when the ownership becomes ambiguous due to obfuscation techniques.

## Known Issues and Limitations
There are no known issues with the connector, but the latest status can be checked in the repository (issues). As limitations, this specific connector only implements the core endpoints of the underlying API. Please reach out and let us collaborate, if you are missing something. 