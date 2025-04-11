## PartnerLinq
### About the underlying service

PartnerLinQ is the flagship digital supply chain connectivity solution from the stables of Visionet Systems Inc. With a host of innovative capabilities, PartnerLinQ seeks to seamlessly connect multi-tier supply chain networks, channels, marketplaces, and core systems worldwide to deliver unified connectivity for the future.

With capabilities for intelligent automation, multi-channel integration, and real-time analytics, PartnerLinQ is the epitome of Visionet’s mission to meet the essentiality of connectivity, visibility, transparency, and resilience in today’s supply chains worldwide.

With a simplistic yet futuristic outlook, PartnerLinQ is primed to deliver unparalleled strength and accuracy to supply chain operations across the global e-commerce ecosystem.

### About the Connector

Exposes HTTP APIs for communication. It allows you to send and receive data to and from PartnerLinq.

## Publisher's Name
Visionet Systems Inc.

## Prerequisites

To use the PartnerLinq Connector you will require the following information:

- Basic authentication username and password
- API access key
- Tenant Id
- Environment
- Partner Id
- Company Id
- Process

## Supported Operations

### PartnerLinq Get Action

Gets the data from PartnerLinq

#### Sample Response  
	
```json
		{   	"data": "{\"SalesOrders\":[{\"SOId\":\"aef65587-c617-4af7-b4e0-81b13700c064\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]},{\"SOId\":\"de4df7af-b787-44a7-ba29-4c3ddad92ec2\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]}]}"
		}
```
**Note:** In order to integrate this action into your flow, a scheduled or manual trigger can be used to trigger the flow.

### PartnerLinq Post Action
Send the data to PartnerLinq

#### Sample Payload
	
```json
		{   	"data": "{\"SalesOrders\":[{\"SOId\":\"aef65587-c617-4af7-b4e0-81b13700c064\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]},{\"SOId\":\"de4df7af-b787-44a7-ba29-4c3ddad92ec2\",\"CustomerNo\":\"44433\",\"Lines\":[{\"LineId\":\"555\"},{\"LineId\":\"555666\"}]}]}"
		}
```


### Triggers

None

## Obtaining Credentials

To get the settings information you will have to get registered with PartnerLinq and then PartnerLinq will set up your environment and provide the following details.

- Basic authentication username and password
- API access key
- Tenant Id
- Environment
- Partner Id
- Company Id
- Process

## Known Issues and Limitations
None

## Further Support

For more information
- Please reach out to us at **PL.Supportteam@visionet.com**.
- Visit our website <https://www.partnerlinq.com/contact-us/>.