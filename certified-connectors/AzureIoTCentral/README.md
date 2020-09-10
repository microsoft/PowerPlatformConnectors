# Azure IoT Central

Azure IoT Central is an app platform that makes it easy to connect, monitor, and manage your IoT devices at scale. With the IoT Central
connector, you can trigger workflows when a rule has fired, and take actions by sending commands, updating properties, getting telemetry
on devices, and more.

## Pre-requisites

You will need the following to use this connector:

- An Azure subscription
- An Azure IoT Central application

## Supported Operations

| Operation Name                                | Description                                                                                   |
| --------------------------------------------- | --------------------------------------------------------------------------------------------- |
| **Create or update a device (preview)**       | Create a new device or update an existing one by device ID.                                   |
| **Delete a device (preview)**                 | Delete an existing device by device ID.                                                       |
| **Execute a device command (preview)**        | Execute a command on a device.                                                                |
| **Get a device by ID (preview)**              | Get details about an existing device by device ID.                                            |
| **Get device cloud properties (preview)**     | Get all cloud property values of a device by device ID.                                       |
| **Get device properties (preview)**           | Get all property values of a device by device ID.                                             |
| **Get device telemetry value (preview)**      | Get the telemetry value for a device.                                                         |
| **Update device cloud properties (preview)**  | Update all cloud property values of a device by device ID.                                    |
| **Update device properties (preview)**        | Update all property values of a device by device ID.                                          |