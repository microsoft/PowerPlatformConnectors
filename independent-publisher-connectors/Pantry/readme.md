# Pantry
Pantry is a free service that provides perishable data storage for small projects. Data is securely stored for as long as you and your users need it and is deleted after a period of inactivity. Simply use the restful API to post JSON objects and we'll take care of the rest.

It was built to provide a simple, re-usable storage solution for smaller sized projects. It was created by developers for developers, to be there when you need it and to help you rapidly prototype your next project.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to sign up for an account on the [Pantry site](https://getpantry.cloud/) to create a pantry for your baskets.

## Obtaining Credentials
This connector will use your PantryID with every action. In order to secure your Power Automates, be sure to use the Secure Inputs setting for each action.

## Supported Operations
### Get pantry details
Given a PantryID, return the details of the pantry, including a list of baskets currently stored inside it.
### Get basket contents
Given a basket name, return the full contents of the basket.
### Delete a basket
Delete the entire basket. Warning, this action cannot be undone.
### Create and/or replace basket
Given a basket name as provided in the url, this will either create a new basket inside your pantry, or replace an existing one.
### Update basket contents
Given a basket name, this will update the existing contents and return the contents of the newly updated basket. This operation performs a deep merge and will overwrite the values of any existing keys, or append values to nested objects or arrays.

## Known Issues and Limitations
There are no known issues at this time.
