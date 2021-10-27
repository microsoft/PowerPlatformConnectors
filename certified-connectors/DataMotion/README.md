# Secure Message Delivery Connector
DataMotion, Inc. specializes in secure data exchange APIs, providing companies the ability to integrate secure message and file exchange solutions into their existing workflows and automated processes. DataMotion’s secure message delivery connector enables seamless, one-way encrypted messaging, helping users protect their critical data and meet compliance regulations.

# Prerequisites
You will need the following to proceed:
* A DataMotion trial or subscription account with secure message delivery API access. 

# Building the connector
In order to utilize the secure message delivery API functionality, we first need to set up a few things in order to properly authenticate. After that is completed, you can create and test the DataMotion connector.

# Set up a DataMotion account and application 
We first need to create a DataMotion account within the DataMotion self-service portal. From here we will need to create an API secret and key in order to authenticate and properly use the API in your automated workflows. You can read more about this here and follow the steps below:
1. Create a DataMotion trial or subscription account. This can be done using the DataMotion self-service portal (https://datamotion.com/portal), by following the steps here. 
2. An API key and secret must be generated in order to authenticate each API request. The key and secret will be tied to an application for organizational purposes. (Note: You can create multiple applications to organize your users and environments.)
	* Within the DataMotion site, sign in to your DataMotion account and navigate to the secure message delivery documentation, then select the Applications tab. 
	* Within the Applications tab, select the ‘Create Application’ button and give the application a name and description. 
	* Select ‘Other’ for the Application Type and provide a description. 
	* Select the ‘plus’ sign (+) in the center of the ‘API Keys’ section to the left of the ‘Application Details’ and copy the API secret that is provided in the pop up. 
		* Store this string somewhere safe as we will need it later on and you will not be able to access it again through the DataMotion site. 

At this point, we now have a valid secure message delivery application that can be used to authenticate your DataMotion account. 

# Supported Operations
The connector supports the following operations:
* Send a Secure Message: Sends a message in a secure manner ensuring the data is not corrupted.
* Retract a Secure Message: Retract a message that was previously sent.
* Track a Secure Message: Track a secure message to ensure the message is delivered and opened.
