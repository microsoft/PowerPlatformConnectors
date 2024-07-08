
## Jedox OData Hub Connector
The Jedox OData Hub exposes functionality from different Jedox components into a powerful REST Api using the OData V4 standard.

## Publisher: Jedox GmbH

## Prerequisites
You will need the following to proceed:
* An active Jedox Instance
* A license including usage of the Jedox OData Hub

## Supported Operations
The connector supports the following operations:
* `Get databases`: Get a list of databases found in the server. System and config databases are excluded from the list, but can be requested by providing the ID.
* `Get database by id`: Get the database with the given ID.
* `Get cubes`: Get a list of cubes in the given database. To prevent issues with the URL encodings, Attribute cubes will be renamed, e.g. \#_Years to ATT_Years.
* `Get cube by ID`: Get the cube with the given ID in the given database. To prevent issues with the URL encodings, Attribute cubes will be renamed, e.g. \#_Years to ATT_Years.
* `Get cube cells`: Get the cells from a cube. This returns the cells' values and element names. If the cell has a string value, the value is instead stored in the stringValue field. Element names are stored in dynamic properties.
* `Get dimensions`: Get a list of dimensions in the given database.
* `Get dimension by ID`: Get the dimension with the given ID in the given database.
* `Get elements`: Get a list of elements in the given dimension.
* `Get element by ID`: Returns the element with the given ID in the given dimension.
* `Get stored views`: Get a list of stored views in the given database.
* `Get stored view by ID`: Get the view with the given ID in the given database.
* `Get stored view cells`: Get all cells from a view. This returns the cells' values and element names. If the cell has a string value, the value is instead stored in the stringValue field. Element names are stored in dynamic properties.
* `Get Integrator project groups`: Get a list of integrator project groups found in the server.
* `Get Integrator project group by identifier`: Get the project group with the given ID.
* `Get Integrator projects`: Get a list of integrator projects found in the server.
* `Get Integrator project by name`: Get the integrator project with the given name.
* `Get extracts`: Get a list of extracts in the given integrator project.
* `Get extract by Name`: Get the extract with the given name in the given integrator project.
* `Get extract rows`: Stream the rows of the extract with the given name in the given integrator project.
* `Get jobs`: Get a list of jobs in the given integrator project.
* `Get job by name`: Get the jobs with the given name in the given integrator project.
* `Run job`: Run the job with the given name in the given integrator project. The execution will be added to the queue.
* `Run job with variables`: Run the job with the given name in the given integrator project. The execution will be added to the queue.
* `Get loads`: Get a list of loads in the given integrator project.
* `Get load by name`: Get the transform with the given name in the given integrator project.
* `Run load`: Run the load with the given name in the given integrator project. The execution will be added to the queue.
* `Run load with variables`: Run the load with the given name in the given integrator project. The execution will be added to the queue.
* `Get transforms`: Get a list of transforms in the given integrator project.
* `Get transform by name`: Get the transform with the given name in the given integrator project.
* `Get transform Rows`: Stream the rows of the transform with the given name in the given integrator project.

## Obtaining Credentials
This connector uses basic authentication. To sign in use your credentials of the corresponding Jedox instance. SAML users are currently not supported.

## Known Issues and Limitations
- SAML users are currently not supported.

## More Information
You can find more information in [our Knowledgebase](https://knowledgebase.jedox.com/integration/odata-hub/odata-powerplatform.htm).