# Instagram Basic Display

## Prerequisites

An Instagram developer account is required to generate an access token, and using that token to fetch the feed data from [Instagram's Basic Display API](https://developers.facebook.com/docs/instagram-basic-display-api/overview#instagram-user-access-tokens) environment.

Follow the steps to generate your user access token.

### Step 1

Go to developers.facebook.com and sign in to your Facebook account. Click on the "My Apps" button on the top right.

### Step 2

Click on the "Create App" button.

### Step 3

Select either "Consumer" or "None" as your application type.

### Step 4

Give your application a name, enter your contact email, and create your app.

### Step 5

Re-enter your Facebook account password

### Step 6

Click on the "Set Up" button in the "Instagram Basic Display" box.

### Step 7

Click on "Create New App" and click on "Create App" from the pop up to create a new instagram app id.

### Step 8

Save your changes. In the "User Token Generator" section, click on the "Add or Remove Instagram Testers" button, and follow the instructions.

### Step 9

Click on the link "apps and websites" link to manage instagram tester invitations and click on accept.

### Step 11

Click on "Basic Display", click on "Generate Token" under "User Token Generator" and from the pop up click on "continue as <testername>".

### Step 12

Click on "Allow" from the pop up to authorise the app to retrieve profile and media information about the instagram user.

### Step 13

Copy the user token to be used on the webpart.

You must then enter this access token in the connector to get the instagram feeds from the authenticated user.

## Known Limitations
nstagram Basic API only allows access to basic data such as user profiles and photos. It does not provide access to more advanced features such as direct messaging, insights, and analytics.

## Supported Operations
### Get My Details
Retrieves information about the authenticated user. By default it retrieves the Id and other details like username,media_count and account_type can be returned by requesting them via the fields parameter.

### Get My Media
Retrieves a list of media items posted by the authenticated user and additional fields like media_type, media_url, caption, permalink and timestamp of each item can be specified to be retrieved.
 
### Get Media Details
Retrieves details of a media item posted by the authenticated user and additional fields like media_type, media_url, caption, permalink and timestamp can be specified to be retrieved.
