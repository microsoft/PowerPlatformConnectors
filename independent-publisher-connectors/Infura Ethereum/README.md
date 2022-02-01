# Infura Ethereum Connector

Infura provides services to access the Ethereum Blockchain without your own Ethereum Node. This connector allows you to interact with the Ethereum Blockchain JSON-RPC API through Infura. Common use cases include checking your Ethereum account's (Wallet) balance, current gas fees, or information on the current block.

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
