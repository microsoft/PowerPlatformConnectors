# Request Maintenance Oxmaint Connector
The **Request Maintenance Oxmaint Connector** allows users to create and manage **maintenance requests** seamlessly through **Power Automate**, **Power Apps**, or **Azure Logic Apps**. It streamlines the submission of maintenance tasks, ensuring issues are tracked, assigned, and resolved efficiently. This connector integrates with **CMMS**, enabling better **work order** management and improving overall maintenance workflows.

## Publisher: Oxmaint Inc.

## Prerequisites
Before using the **Request Maintenance Oxmaint Connector**, ensure that you have:
- Access to **Power Automate**, **Power Apps**, or **Azure Logic Apps**.
- Credentials (username and password) for Oxmaint Maintenance Management System.​

## Supported Operations
This connector supports the following operations:​
### Operation 1
**Create Maintenance Request**
Allows users to create a new maintenance request within the Oxmaint system. The request is logged, tracked, and assigned based on the asset or equipment's status, ensuring timely response and resolution.

## Obtaining Credentials
To use the Request Maintenance Oxmaint Connector, you will need to authenticate using valid user credentials. Follow these steps to obtain the credentials:

1. Create an Account: Ensure you have an active Oxmaint account with appropriate access to the maintenance module or register here https://oxmaint.com/portal/signup.html
2. Login Credentials: Use the registered email and password for your Oxmaint account to authenticate the connector.​
3. API Key: If the connector requires an API key, obtain it by logging into the Oxmaint dashboard and navigating to your API settings.

## Getting Started
To get started with the Request Maintenance Oxmaint Connector:

1. Add the connector to Power Automate, Power Apps, or Azure Logic Apps.
2. Authenticate using your Oxmaint credentials (as described above).
3. Set up your first flow or app by configuring actions such as Create Maintenance Request.
4. Test your flow to ensure that maintenance requests are successfully submitted and tracked in Oxmaint.


## Known Issues and Limitations
- **Field Requirements**: Ensure all required fields are provided in the request to avoid errors.
- **Limited Error Messages**: Error messages from failed maintenance requests may be generic or vague, requiring additional troubleshooting in the Oxmaint system.

### **Support**
For any issues or questions regarding the **Request Maintenance Oxmaint Connector**, please contact:
- **Email**: [contact@oxmaint.com]


### **Changelog**
- **v1.0.0**: Initial release with the ability to create maintenance requests, including fields for title, description, priority, and location/asset information.


### **Resources**
- [Power Platform Custom Connectors Documentation](https://docs.microsoft.com/en-us/connectors/custom-connectors/)
