# cioplenu connector

The cioplenu connector allows you to easily create tasks automatically on your cioplenu instance.

## Prerequisites

To use the connector you need to have access to a cioplenu instance and you need an API key for your
instance. To get an API token create an app user by checking the "App user" checkbox at the bottom
of the user creation form. Make sure you assign the appropriate API permissions like `WRITE_TASK` to
the user's role. Finally, generate a token by using the key icon in the user table. For a detailed
explanation you can reference [the guide for the CLI tool](https://support.cioplenu.com/en/articles/4472413-using-the-cli-tool).

## Supported operations

The connector supports the following operations:

- `Create task`: Create a new task

