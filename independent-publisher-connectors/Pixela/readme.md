# Pixela
Pixela is the pixelation service. With this service, you can get a GitHub-like graph that expresses the degree of your various activities on a basis with a vivid gradation.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
There are no prerequisites needed for this service.

## Obtaining Credentials
In order to use this connector, you will need to make a [POST call to the Users endpoint](https://docs.pixe.la/entry/post-user) to create your username and token.

## Supported Operations
### Create user
Creates a new Pixela user.
### Update user profile
Updates the profile information for the user.
### Update user token
Updates the authentication token for the specified user.
### Delete user
Deletes the specified registered user.
### Get graph definitions
Get all predefined pixelation graph definitions.
### Get a graph definition
Retrieve a predefined pixelation graph definition.
### Create graph
Creates a new pixelation graph definition.
### Delete graph
Deletes a predefined pixelation graph definition.
### Update graph
Updates a predefined pixelation graph definition.
### Get graph SVG
Retrieve the graph as an SVG file.
### Get graph stats
Retrieve the statistics for a graph.
### Get pixels
Retrieve a list of pixels registered in a graph.
### Get pixel
Retrieves the quantity for a pixel.
### Get retina pixel image
Retrieves a higher resolution SVG file for the pixel.
### Create pixel
Creates a number of pixels for a given day.
### Delete pixel
Deletes a pixel.
### Update pixel
Updates the quantity for a pixel.
### Increment pixel
Increments the quantity for the pixel.
### Decrement pixel
Decrements the quantity for the pixel.
### Add to pixel
Adds a quantity to the pixel.
### Subtract from pixel
Subtracts a quantity from the pixel.

## Known Issues and Limitations
Some endpoints will reject 25% of the actions called unless you support Pixela on Patreon. Please consider logic to resubmit failed actions.
