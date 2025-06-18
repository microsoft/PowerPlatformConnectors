# sendSMS Connector
The sendSMS connector integrates with the sendSMS platform, a leading European Communications Platform as a Service (CPaaS) known for its reliability and expansive reach. Established in 2008, sendSMS has specialized in providing seamless no-code and low-code integrations with over 30 eCommerce platforms, CRMs, ERPs, and various other integrators, enhancing communication and operational efficiency across the globe.

## Publisher: sendSMS
sendSMS is a premier provider in the CPaaS market, delivering trusted and effective communication solutions since 2008. The platform supports a wide range of integrations, making it an ideal choice for businesses looking to streamline their communication processes.

## Prerequisites
A contract with sendSMS is required to access the services provided by this connector.

## Supported Operations
The connector supports the following operations, making communication management smoother and more reliable:
### message_send
This method activates the process of sending an SMS message to a pre-determined phone number upon receiving a specific request. It ensures efficient transmission of messages to the intended recipient.
### message_status
The Message Status method retrieves the current status of an SMS that has been sent, providing insights into the message lifecycle, including delivery statuses and any errors that may have occurred during transmission.

## Obtaining Credentials
Authentication for this connector uses OAuth 2.0 with a key refresh mechanism. Here are the steps to obtain initial credentials:
- **Contract Signing**: Engage with our sales or business development team to finalize the terms of service.
- **Account Setup**: Once the contract is signed, we will set up your organizational account.
- **Credential Issuance**: Initial credentials, including the client ID and client secret, will be provided. These are essential for accessing our services and must be stored securely.

## Authentication Method
Our system employs OAuth 2.0 for authentication, ensuring secure interactions through token-based permissions:
- **Key Refresh Mechanism**: To maintain security, our OAuth 2.0 setup includes a key refresh mechanism, allowing periodic refresh of access tokens using provided refresh tokens without re-authentication.

## Usage Notes
- Use the client ID and client secret to request an access token from the OAuth 2.0 token endpoint.
- Include the access token in the HTTP Authorization header to authenticate API requests.
- Use the refresh token to obtain new access tokens when necessary, as per our key refresh mechanism.

## Known Issues and Limitations
No known issues or limitations at this time. Please contact support if you encounter any problems using the connector.

## Deployment Instructions
To deploy this connector as a custom connector in Power Automate or Power Apps:
1. Download the connector definition from the provided GitHub repository.
2. Navigate to Power Automate or Power Apps and select 'Custom Connectors' from the Data section.
3. Click 'Import an OpenAPI file', and upload the connector definition file.
4. Fill out the necessary information and update the connector's security settings accordingly.
5. Create a new connection using the initial credentials provided during the account setup phase.

These instructions should help users effectively integrate the sendSMS connector into their workflows, enhancing their communication capabilities.
