[[_TOC_]]

# 3E Events connector
3E provides law and professional service firm practice management software primarily for back office time, billing and accounting for firm operations as well as new client onboarding, record keeping, conflicts searching, asset reporting and purchasing. With the 3E Events Connector, you can extend 3E’s capabilities by creating event-based integrations, resulting in more interactive, immersive, responsive solutions.

# Prerequisites
You will need the following to proceed:
1. Office 365 account
2. Login access to Microsoft Power Automate.

Note: Please contact your IT administrator for any issues related to Power automate

# How to get credentials
Request access by contacting [Elite Support](https://legal.thomsonreuters.com/en/support)

When requesting access and to establish connection to the 3E Events Connector, the user will need access to the following 3E information.: :

•	3E Cloud environment.

•	3E instance ID.

•	An existing 3E user for the given instance.

Include this information when submitting the access request.


# Supported triggers
3E Events Connector supports the following operations:
1.  Client Created – This operation triggers a flow when a new client is created.
2.  Client Updated – This operation triggers a flow when a client’s Number, Display Name, Status Date, Status, Opening Timekeeper or Closing Timekeeper fields are updated on a client maintenance record in 3E.
3.  Matter Created – This operation triggers a flow when a new matter is created in 3E.
4.  Matter Updated – This operation triggers a flow when a Matter’s Number, Display Name, Client Number, Date Opened, Status, Opening Timekeeper or Closing Timekeeper fields are updated on a Matter maintenance record in 3E.
5.  Timekeeper Updated – This operation triggers a flow when a timekeeper’s status or display name is changed.
6.  Cash Receipt Created – The operation will trigger a flow when a receipt is created in 3E, and the message will contain information regarding Receipt Date, Receipt Amount, Document number and Payor.
7.  Trust Receipts Created – This operation triggers a flow when a trust receipt is created in 3E and the message will include information on Transaction Date, Trust Account, Received from or Drawn by fields.
8.  DataObjectEvent - This operation triggers a flow when an object is created, updated, deleted or read. Please, select the object name and state to subscribe to particular events.
9.  DataAttributeEvent- This operation triggers a flow when an existed attribute state is updated
10. 3E Templates Document Generated - This operation triggers a flow when a new 3E Templates document is generated for a template that has been enabled in 3E’s Distribution Setup for 3rd party distribution.

# Supported Actions
1.  Delete 3E Templates document - Notify the platform to delete all 3E Templates temporary files and complete the operation.
2.  Download 3E Templates document - Download the 3E Templates generated document.
3.  Get 3E Templates document metadata - Obtain the metadata associated with a 3E Templates document.