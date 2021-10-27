## Nextcom CRM Connector
Nextcom’s cloud-based CRM solution is a safe and efficient way to streamline your workflow, create collaboration internally and increase sales. Tailored for you.

## Prerequisites
You will need the following to proceed:

* [Sign Up](https://nextcom.no/en/try-the-system-for-free/) for a Nextcom trial account.
* Once signed up, a Nextcom representative will supply you with an URL and login info, and walk you through the steps of setting up your Nextcom domain.
* Our support staff will help you set up a dedicated API user in your Nextcom web interface, and this API user will be used to connect your services to Nextcom through Power Automate.
* As Nextcom's CRM system relies heavily on automated steps in the CRM process, very few Power Automate actions are needed to establish a new sales opportunity. Two are enough to initiate the CRM loop.

## Connecting Power Automate to Nextcom
* When your Nextcom domain has been set up, and you have created an API user, please connect to Nextcom using the API users Username and Password, along with your dedicated domain name.

## Supported Operations
The connector supports the following operations:
* ```Add Contact```: Adds a new contact to a given list. Automatically handles duplicates, and returns the Contact ID of either the new contact or the duplicate.
* ```Add Pipeline```: Adds a new pipeline opportunity on a given contact ID.

## Our public API documentation
API documentation can be found [here](https://dev.nextcom.no/rest-api/swagger/ui/index).

## Further Support
For any questions please feel free to [contact-us](https://nextcom.no/en/services/support/).
