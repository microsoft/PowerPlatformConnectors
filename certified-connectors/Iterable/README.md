
# Iterable

This connector allows you to integrate specific workflows with Iterable. You can use this connector to extract data from a data source and upload specific information to your Iterable account.

## Publisher: Microsoft

## Prerequisites

* An Iterable [account](https://iterable.com/) and corresponding administrator credentials.
* An Iterable [API Key](https://support.iterable.com/hc/en-us/articles/360043464871).

## Obtaining Credentials

Please follow [the instructions](https://support.iterable.com/hc/en-us/articles/360043464871) from Iterable's documentation on how to find your Api Key.

## Supported Operations

### Get lists

Allows you to fetch all lists within a project. Rate limit: 100 requests/second, per project.

### Create a static list

Allows you to create a new static list.

### Add subscribers to list

Allows you to add specific subscribers to a list.
Learn about [identifying users by userId and email](https://support.iterable.com/hc/en-us/articles/204780579#identifying-users).

### Remove users from a list

Allows you to Remove specific users from a list.

Learn about [identifying users by userId and email](https://support.iterable.com/hc/en-us/articles/204780579#identifying-users).

### Get users in a list

Allows you to get all users within a list. Rate limit: 5 requests/minute, per project.

## Known issues and limitations

* Get lists operation has a rate limit: 100 requests/second, per project.
* Up to to 1 million customer profiles to Iterable, which can take up to 30 minutes to complete. The number of customer profiles that you can export to Iterable depends on your contract with Iterable.
