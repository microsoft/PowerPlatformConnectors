# Ubiqod by Skiply (v2)
Ubiqod provides a simple and powerful platform to connect your Skiply IoT buttons and Qods to third-party platforms. This connector triggers a flow every time data is sent by any of the trackers.

## Publisher: Skiply
Skiply is responsible for the development and maintenance of the Ubiqod connector.

## Prerequisites
To use this connector, you will need:
- A Microsoft Power Apps or Power Automate plan.
- An active Ubiqod account.
- Sufficient credits in your Ubiqod account.

## Supported Operations
This section outlines the various operations supported by the Ubiqod by Skiply connector, categorized by functionality:

### Badge Operations
- **Create Badge List**: Create a new list of badges.
- **Get Badge List**: Retrieve details of a specific badge list by its ID.
- **Update Badge List**: Update details of an existing badge list.
- **Delete Badge List**: Remove a badge list using its ID.

### Pin Code Operations
- **Create Pin Code List**: Establish a new list of pin codes.
- **Get Pin Code List**: Fetch a specific pin code list using its ID.
- **Update Pin Code List**: Modify an existing pin code list.
- **Delete Pin Code List**: Erase a pin code list by ID.

### Site Operations
- **Create Site**: Add a new site with specific geolocation details.
- **Get Site**: Obtain details of a site by its ID.
- **Update Site**: Update the details of an existing site.
- **Delete Site**: Remove a site using its ID.

### Tracker Operations
- **Create Tracker**: Configure a new tracker for deployment.
- **Get Tracker**: Retrieve details of a tracker using its identifier.
- **Update Tracker**: Modify the settings or details of an existing tracker.
- **Delete Tracker**: Remove a tracker from the system using its identifier.

### Trigger Operation
- **Receive Data From Trackers**: This trigger operation is initiated when a dispatch event is sent by Ubiqod. It is configured to activate upon receiving data from trackers, facilitating real-time data processing and integration with third-party platforms.

Each of these operations requires specific parameters and offers detailed responses, which are outlined in the API documentation provided by Skiply.

## Obtaining Credentials
To obtain your Ubiqod API Key:
1. Log in to your Ubiqod account.
2. Navigate to the "Account" section.
3. Your API Key will be listed there.

## Getting Started
### Set up the connection
1. Go to the Connection section in Power Automate.
2. Add a **New Connection**.
3. Search for **Ubiqod By Skiply v2** application.
4. Enter your Ubiqod API Key and create the connection.

### Set up the flow
1. As a trigger, select the **Ubiqod** application and the **Receive Data From Trackers** event.
2. Enter the name of your dispatch, and Ubiqod will automatically create it for you.
3. Add the action(s) of your choice.

### Test the connection
- Simulate the real IoT trackers using the Ubiqod simulator. Press the "play" button in the trackers list to start the simulator.

## Known Issues and Limitations
No major issues or limitations are currently known for the Ubiqod connector.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps.