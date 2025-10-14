# Nosco Connector

Nosco is an easy to use innovation management platform enabling businesses to capture, collaborate on, and bring ideas to life.
The Nosco connector allows you to work with data from the platform and automate custom innovation processes.

## Publisher: Nosco ApS

## Prerequisites

Requires the Nosco integration API feature to be enabled in the Nosco platform you're connecting to. Please
contact support for help with this feature.

## Supported Operations

### Query all channels

Query all channels.

### Get a post

Get a post.

### Query all posts

Query all posts.

### When a post reaches a stage

Notify when a post reaches a stage.

### When a post changes status

Notify when a post changes status.

### When a post is published

Notify when a post is published.

### When a post is edited

Notify when a post is edited.

## Obtaining Credentials

For authentication, you need to generate an API key. You can do this in the platform settings.
Please see the Nosco documentation for help with how to create API keys.

You will also need to provide the hostname of your Nosco platform, for example `acme.nos.co`.

## Known Issues and Limitations

The connector currently doesn't return post template fields.

## Deployment Instructions

Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.

For authentication, you need to generate an API key. You can do this in the platform settings.
Please see the Nosco documentation for help with how to create API keys.

You will also need to provide the hostname of your Nosco platform, for example `acme.nos.co`.
