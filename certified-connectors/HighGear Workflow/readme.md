# HighGear Workflow Connector

HighGear makes it easy to rapidly deploy and securely manage work, at scale. As a highly configurable no-code workflow platform, connecting with HighGear allows you to readily integrate business applications. Get your entire team connected across the enterprise and start building seamless workflows and processes with more visibility, auditability, and real-time measurement of all work activity.

With the HighGear Workflow connector, it's never been easier to rapidly integrate business applications and mission critical work. HighGear's enterprise-grade security meets the rigors of the most highly regulated industries while allowing everyday business users to rapidly build simple to complex workflows. Assign tasks, manage work, track progress, and report the status of activity in real time using HighGear's no-code workflow and process management platform. Easily and securely.

If you're interested in learning more about the HighGear platform, [schedule a demo](https://www.highgear.com/demo/) or [contact us](https://www.highgear.com/company/contact/).

## Publisher: HighGear Software, Inc.​

HighGear Software, Inc. continuously strives to provide unparalleled value to our customers through the HighGear product as well as our [training offerings](https://www.highgear.com/services/conduct-training-sessions/), [professional services](https://www.highgear.com/services/), and [customer support](https://www.highgear.com/support/).

Our total commitment to supporting our customers and making them highly successful is a reflection of the high caliber of people that we attract and retain as well as our core values of passion for our customers' success, humility, teamwork, and integrity.

## Prerequisites

To use this connector, you will need the HighGear application, version 9.1.0 or above.

You will also need the following:
- **The base URL of your HighGear system:**
This is the part of the URL you see in your web browser address bar without the specific page you're on.
The two most common patterns of base URL are `https://SUBDOMAIN.example.com` where SUBDOMAIN is "highgear" or something similar or `https://example.com/highgear`.
- **An integration account and API key:**
This is necessary so that the connector can communicate with the HighGear REST API.
To learn how to create an integration account, go to the help in your HighGear system and search for the article *Creating an Integration Account*.

## Supported Operations

### Actions​

### Create contact
Use this action to create a new contact (individual, organizational unit, permission group, asset, or queue) in your HighGear system. This action returns the ID of the new contact, which you can use in subsequent actions, for example, to store the contact ID in a Contact Lookup field on a task.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Contact Class | contact_class | True | string | Select the contact classification (individual, organizational unit, permission group, asset, or queue) of the contact to be created |
| Contact Type | contact_type | True | integer | Select the contact type for the new contact |
| Other field values | model | True | multiple | Other field values for the new contact |

### Create task
Use this action to create a new task in your HighGear system. This action returns the ID of the new task.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Task Form | job_type | True | integer | Select the task form for the new task |
| Other field values | model | True | multiple | Other field values for the new task |

### Delete contact file
Use this action to delete a file attached to a contact in your HighGear system.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| File Id | fileId | True | integer | The ID of the contact file to be deleted  |


### Delete task file
Use this action to delete a file attached to a task in your HighGear system.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| File Id | fileId | True | integer | The ID of the task file to be deleted  |

### Get contact
Use this action to get the field values for a contact in your HighGear system.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Contact Id | contactId | True | integer | The ID of the task to be retrieved |
| Contact Class | contact_class | True | string | Select the contact classification (individual, organizational unit, permission group, asset, or queue) of the contact to be retrieved |
| Contact Type | contact_type | True | integer | Select the contact type with the data fields to be retrieved |

### Get contact file content
Use this action to get a file attached to a contact in your HighGear system.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| File Id | fileId | True | integer | The ID of the contact file to be retrieved  |

### Get task
Use this action to get the field values for a task in your HighGear system.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Task Id | taskId | True | integer | The ID of the task to be retrieved |
| Task Form | job_type | True | integer | Select the task form with the data fields to be retrieved |

### Get task file content
Use this action to get a file attached to a task in your HighGear system.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| File Id | fileId | True | integer | The ID of the task file to be retrieved  |

### Search contacts
Use this action to search for contacts in your HighGear system. 
The search is based on the field criteria of a specified contact type. 
This action returns the ID, name, and email address of the contacts that match the search criteria. 

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Contact Class | contact_class | True | string | Select the contact classification (individual, organizational unit, permission group, asset, or queue) of the contact type |
| Contact Type | contact_type | True | integer | Select the contact type with the data fields to be used to define the search criteria |

### Search tasks
Use this action to search for tasks in your HighGear system. 
The search is based on the field criteria of a specified task form. 
This action returns the ID, and brief description of the tasks that match the search criteria. 

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Task Form | job_type | True | integer | Select the task form with the data fields to be used to define the search criteria |

### Update contact
Use this action to update the contact type fields for a contact in your HighGear system.
This action returns the ID of the contact that was modified.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Contact Id | contactId | True | integer | The ID of the task to be updated |
| Contact Class | contact_class | True | string | Select the contact classification (individual, organizational unit, permission group, asset, or queue) of the contact to be updated |
| Contact Type | contact_type | True | integer | Select the contact type with the data fields to be updated |

### Update task
Use this action to update the task form fields of a task in your HighGear system.
This action returns the ID of the task and a boolean stating the task was modified (true or false). 

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Task Id | taskId | True | integer | The ID of the task to be retrieved |
| Task Form | job_type | True | integer | Select the task form with the data fields to be retrieved |

### Trigger

### When a task enters a web request node
Triggers a new flow when a task enters a Make Web Request node within a workflow defined in your HighGear System.

| **Name** | **Key** | **Required** | **Type** | **Description** |
| - | - | - | - | - |
| Workflow | workflow_id | True | integer | Select the workflow that contains the Make Web Request node that will trigger the flow. |
| Node | node_id | True | integer | Select the Make Web Request node that will trigger the flow |
| Name | label | True | string | The name of the subscription created for the Make Web Request node. This name will be displayed in the list of Active Subscriptions for the node. |

## Obtaining Credentials

To set up and use the connector, you will need an integration account in your HighGear system with an API key.

Please go to the help in your HighGear system and refer to the following articles to learn how to set up an integration account and create an API key for it.
- **Creating an Integration Account**
- **Creating an Integration API Key for an Account**

## Getting Started

To get started with the HighGear Workflow connector and to trigger a flow when a task enters a workflow web request node in your HighGear system, follow these steps:
1. Create a HighGear Workflow connection if not created. For a new connection, enter the base URL and subdomain for your HighGear system in addition to the API Key for your integration account.
2. Add a **When a task enters a web request node** trigger and select a HighGear workflow and node. This will trigger your flow when a task enters the selected workflow node.
3. Add a **Get task** action to get data from the task. Use the Task Id from the **When a task enters a web request node** trigger and select the task form with the data fields you want to load.
4. Add another connector to send task data to. Some common use cases are to create an Outlook event from the start and end date of a HighGear task, send an email about a task to the assignee's email address, or update the status of a corresponding record in another system.

## Known Issues and Limitations

- Additional data (i.e. Task Fields to Send) included with outbound web requests from a HighGear workflow is not currently supported with the connector. For now, get the data you need via the **Get task** action after receiving a trigger from your HighGear system.

For issues with the HighGear Workflow connector, please contact [HighGear support](https://www.highgear.com/support/).

## Frequently Asked Questions

- **How do I look up a help article?**
  1. To look up a help article in your HighGear system, click the **Help** button in the upper right corner.  
  *A new tab will open with the HighGear user documentation.*
  2. Click the **Search** button, represented by a magnifying glass icon, in the upper right corner.
  3. In the Search textbox, type the name of the article you want to open.  
  *A dropdown list will show the search results.**
  4. Click the name of the article.

## Deployment Instructions

The HighGear Workflow connector can be easily added from Microsoft's catalog of connectors to your Microsoft Power Apps, Microsoft Power Automate, and Azure Logic Apps integrations.

However, if you want to use the HighGear Workflow connector as a custom connector, please refer to [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy it as custom connector in Microsoft Power Automate and Power Apps.
