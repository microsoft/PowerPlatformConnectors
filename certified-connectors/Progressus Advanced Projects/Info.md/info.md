---
title: Information about Progressus Advanced Projects connection tool 
description: Information on using Progressus Advanced Projects connection tool to make OData calls to Progressus Advanced Projects data in PowerAutomate or other tools
author: ashenk

ms.service: dynamics365-business-central
ms.topic: article
ms.search.keywords: information, Progressus, Advance, project, projects, connector, connection, oData
ms.date:7/26/2021
ms.author: lcontreras

---
# Introduction

Progressus Advanced Projects is a project accounting and project management solution for Microsoft Dynamics 365 Business Central.  Use Progressus Advanced Projects connection tool to make OData calls to Progressus Advanced Projects data in PowerAutomate or other tools.  These queries make use of OData features such as filtering and selecting of fields in order to retrieve only the records and fields needed.  

## Prerequisites

Business Central with Progressus Advanced Projects extension installed

## How to get credentials

Generate a Web Service access key for a User in Business Central:

[https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/webservices/web-services-authentication#generate-a-web-service-access-key](/dynamics365/business-central/dev-itpro/webservices/web-services-authentication)

## Get started with your connector

The connector’s fields are used in the following manner to form a Business Central web service URL:

`https://api.businesscentral.dynamics.com/<API Version>/<Tenant ID>/<Environment Name>/api/progressus/<API Name>/<API Version 2>/companies(<Company ID>)/<Plural API Name>`

:::image type="content" source="images/Connector-1.png" alt-text="Get Resource":::

## Known issues and limitations

This connector is intended for use with Progressus Advanced Projects.  The dropdown of API names is restricted to tables related to Progressus Advanced Projects.  To attempt to query non-listed tables, you may use the ‘Enter Custom Value’ option.

:::image type="content" source="images/Connector-2.png" alt-text="Get Resource":::

## Common errors and remedies

Most errors are likely to be caused by not having the correct syntax for the $filter, $select, and $orderby fields.  Make sure to check that proper Odata syntax is used for these field.
