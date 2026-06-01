## WordLift GraphQL Connector

Query and extract data from a Knowledge Graph hosted on WordLift by using the well-known GraphQL language.

## Publisher: WordLift

We have built the first semantic platform that combines natural language processing, knowledge graph publishing and machine learning to increase the organic traffic of a website and to make content accessible to voice search and virtual assistants. More on [worldift.io](https://wordlift.io)

## Prerequisites

You will need the following to proceed:

* A WordLift subscription
* A Knowledge Graph an its related access key
* Basic GraphQL knowledge

## Supported Operations

The connector supports the following operations:

* `Execute GraphQL`: Executes a GraphQL query

### Execute GraphQL

Executes a GraphQL query against the WordLift GraphQL API, extracting and returning data from the target Knowledge Graph. The `Authorization: Key __KEY__` header is required to select the target Knowledge Graph.

A query can be as simple as:

```graphql
query {
  entities(rows: 100) {
    iri
    headline: string(name:"schema:headline")
    description: string(name:"schema:description")
    url: string(name:"schema:url")
    source: string(name:"seovoc:source")
  }
}
```

## Obtaining Credentials

The `Authorization` header is required with a value of `Key __KEY__` where `__KEY__` is a key bound to a Knowledge Graph. To create a Knowledge Graph and obtain a key connect to [wordlift.io](https://wordlift.io) and start a new subscription. It takes only a few minutes.


## Known Issues and Limitations

When querying a Knowledge Graph with large amounts of data it is advised to use the `rows` and `page` parameter in the query to limit the results to a specifc subset:

```graphql
query {
  entities(page: 0, rows: 100) {
    ...
  }
}
```

## Deployment Instructions

To deploy follow these steps:

1. Obtain a key by creating a subscripton at [wordlift.io](https://wordlift.io)
1. Add the step in to your Power Automate Flow
1. Enter the key
1. Enter the GraphQL query
