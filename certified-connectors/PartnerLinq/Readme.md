## PartnerLinq Connector

Exposes HTTP APIs for communication. It allows you to send and receive data to and from PartnerLinq.

## Prerequisites

To use the PartnerLinq Connector you will require following information:

- Basic authentication username and password
- API access key
- Tenant Id
- Environment
- Partner Id
- Company Id
- Process

**How to get the settings:**

To get the settings information you will have to get registered with PartnerLinq and then PartnerLinq will setup your environment and provide the following details.

- Basic authentication username and password
- API access key
- Tenant Id
- Environment
- Partner Id
- Company Id
- Process


## Supported Actions

The connector supports the following operations:
- **PartnerLinq Get:** Gets the data from PartnerLinq

	- Sample Response:  
	
	```json
		{   	"data": "{\"SalesOrders\":[{\"SOId\":\"aef65587-c617-4af7-b4e0-81b13700c064\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]},{\"SOId\":\"de4df7af-b787-44a7-ba29-4c3ddad92ec2\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]}]}"
		}
	```
	**Note:** In order to integrate this action in your flow, scheduled or manual trigger can be used to trigger the flow.


- **PartnerLinq Post:** Send the data to PartnerLinq

	- Sample Payload: 
	
	```json
		{   	"data": "{\"SalesOrders\":[{\"SOId\":\"aef65587-c617-4af7-b4e0-81b13700c064\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]},{\"SOId\":\"de4df7af-b787-44a7-ba29-4c3ddad92ec2\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]}]}"
		}
	```


## Supported Triggers
	None


## Further Support

For more information
- Please reach out to us on **PL.Supportteam@visionet.com**.
- Visit our website <https://www.partnerlinq.com/contact-us/>.