# AfterShip
AfterShip is a shipment tracking solution. It partners with over 900 carriers across the globe including popular services such as FedEx, DHL etc. This connector allows a user to track and receive notifications on shipments and integrate with other services such as Dynamics 365 and third party services to strengthen your ecommerce or streamline your internal business processes.

## Publisher: Taiki Yoshida

## Prerequisites
Sign-up free from https://www.aftership.com/ to create an API key. For more information on creating the API keys are available from https://developers.aftership.com/reference/quick-start

## Supported Operations
### Detect courier
Get a list of matched couriers based on tracking number format and selected couriers or a list of couriers.

### Get all couriers
Get a list of all couriers supported by AfterShip

### Get user activated couriers
Get a list of couriers activated at your AfterShip account.

### Create a tracking
Create a tracking on a shipment that you would like to track.

### Update a tracking
Update a shipment tracking with additional information such as titles, memos etc.

### Get a tracking
Get tracking results of multiple shipment trackings which you created.

### Delete a tracking
Delete an existing shipment tracking.

### Mark tracking as completed
Mark a tracking as completed. The tracking won't auto update until retrack it.
You can mark it as either delivered, lost or returned to sender.

### Retrack an expired tracking
Retrack an expired tracking. Max 3 times per tracking.

### Get last checkpoint
Return the tracking information of the last checkpoint of a single tracking.

### Add a notification
Add notification receivers to a tracking number.

### Remove a notification
Remove notification receivers from a tracking number.

### Get tracking notification
Get contact information for the users to notify when the tracking changes. Note that only customer receivers will be returned. Any email, sms or webhook that belongs to the Store will not be returned.

## API Documentation
https://developers.aftership.com/reference/

## Known Issues and Limitations
None.