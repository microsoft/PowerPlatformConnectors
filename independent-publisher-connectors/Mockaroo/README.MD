# Mockaroo
**Mockaroo** allows users to generate realistic sample data which can be instrumental in testing and developing applications. This connector for Power Automate provides an interface to harness the power of Mockaroo directly within your workflows.

## Publisher: Richard Wilson

## Prerequisites
- An active account on [Mockaroo](https://www.mockaroo.com/).
- Your Mockaroo API key. This can be obtained from the [My Account](https://www.mockaroo.com/my_account) page after logging in.

## Supported Operations
### Generate Data

This operation delivers record data based on a saved schema from the Mockaroo platform or directly from a supplied schema in JSON format.

#### Properties
- **Schema Name**: 
  - Specify the name of a saved schema on Mockaroo to fetch data.
  - For successful data generation, ensure the schema exists on Mockaroo.
  - If this property is in use, refrain from using the Schema JSON property.

- **Schema JSON**: 
  - Directly input the schema's JSON definition.
  - Bypasses the need for a schema saved on Mockaroo.
  - If this property is active, avoid using the Schema Name property.

    **Example:** 
    ```
    [
        {
        "name": "id",
        "null_percentage": 0,
        "type": "Row Number",
        "formula": null
        },
        {
        "name": "first_name",
        "null_percentage": 0,
        "type": "First Name",
        "formula": null
        },
        {
        "name": "last_name",
        "null_percentage": 0,
        "type": "Last Name",
        "formula": null
        }
    ]
    ```
- **Record Count**: 
    - Dictates the number of records to return.
    - If unspecified and using the Schema Name property, the output is based on the record count defined in the saved schema.
    - If unspecified and using the Schema JSON, a single record will be generated.


## API Documentation
For a detailed understanding of how the Mockaroo API works, and for additional endpoint details, refer to the official [Mockaroo API documentation](https://www.mockaroo.com/docs).

## Known Issues and Limitations
Currently, no known issues or limitations exist. Always refer to this section for updated information.
