# Decentraland
The Decentraland platform is a metaverse where people can create avatars, purchase virtual LAND parcels, buy and sell NFTs, and participate in the Decentraland DAO.  This connector allows a user to pull LAND parcel data into their app, flow, or report.

## Publisher: Roy Paar

## Prerequisites
None.

## Obtaining Credentials
An API Key is not required to use this connector.

## Supported Operations
### Get Districts
Get a list of all districts within Decentraland.  Returns all parcels within each district with x, y coordinates.

### Get Parcel Details
Get the details of a particular parcel by location.  Enter x, y coordinates between -150 and 150.  Returns description, image url, external url, contract id, and Ethereum blockchain Id.

### Get Parcel Map
Get a map of Genesis City centered on a particular parcel by x, y coordinates between -150 and 150.  Enter width and height between 32 and 2048 (pixels). Enter size of each parcel between 5 and 40 i.e. 10 will render each parcel as 10x10 pixels.  Returns a .png file.

### Get Id
Get the Ethereum blockchain Id of a particular parcel by x, y coordinates.  Enter X coordinate for x1 and x2 values, enter Y coordinate for y1 and y2 values.  Enter "tokenId" for include value.

## API Documentation
https://docs.decentraland.org/market/api/ 

## Known Issues and Limitations
None.
