# XC-Gate
XC-Gate is a form digitization solution that helps improve operational efficiency in record keeping and management. XC-Gate can digitize paper forms such as daily reports and inspection sheets that exist in any business.

## Prerequisites
For more information on XC-Gate, please refer to this URL (https://product.technotree.com/xc-gate/).

The following are required to use the connector.
* "XC-Gate.V3" or "XC-Gate.ENT" has been purchased and installed.
* For on-premise version, the server has already been published with a domain name or global IP address.
* WebAPI option is purchased and installed.

## How to get credentials
The connector itself does not require an authentication process.
Please use the login action (LoginAuth) of the connector to obtain the XC-Gate user identification number and authentication key, and use the obtained key to access the XC-Gate.

## API Documentation
Please refer to the WebAPI option documentation included with your XC-Gate purchase. Currently, the connector is a preview version and there is no official documentation.

## How to Use
Please use the following sequence to access XC-Gate.
1. Obtain "user identification number" and "authentication key" using "LoginAuth".
2. Use each action with the obtained user identification number and authentication key.

### Supported Operations
|  Action Name  |  Description  |
| ---- | ---- |
|  LoginAuth  |  Login to XC-Gate to obtain credentials.  |
|  ActionFind  |  Searches for and retrieves a list of achievements.  |
|  ActionGet  |  Obtains various types of information on designated performance.<BR>(Restrictions : Only 20 pieces from DATA1~DATA20 can be obtained.)  |

### Precautions
Each action should be appended with the host to be accessed (ex. "technotree.xc-gate.com") as a query parameter.
