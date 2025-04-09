# Adobe Experience Manager PDF and PDL operations for Microsoft Power Automate
Adobe Experience Manager PDF and PDL operations for Microsoft Power Automate
# Adobe Experience Manager
Adobe Experience Manager provides a set of powerful cloud-based APIs to integrate multi-step document workflows into any application. Use Adobe Experience Manager to create PDF, PS, PCL, ZPL, DPL, IPL, or TPCL files from XDP or PDF templates and XML data.
## Pre-requisites
* Access to [Adobe Experience Manager Forms Cloud Service Communication APIs](https://experienceleague.adobe.com/docs/experience-manager-cloud-service/content/forms/using-communications/aem-forms-cloud-service-communications-introduction.html?lang=en)
* Adobe Experience Manager Forms Cloud [Service Credentials](https://experienceleague.adobe.com/docs/experience-manager-learn/getting-started-with-aem-headless/authentication/service-credentials.html?lang=en)
## Supported Operations
### Create PDF documents
You can create a PDF document that is based on a form design (XDP or PDF) and XML form data. The output is a non-interactive PDF document. That is, users cannot enter or modify the form data. A basic workflow is to merge XML form data with a form design to create a PDF document.
### Create PostScript (PS), Printer Command Language (PCL), Zebra Printing Language (ZPL) or other Printer Description Language (PDL) files
You can use document generation APIs to create PostScript (PS), Printer Command Language (PCL),  Zebra Printing Language (ZPL), DPL (Datamax Programming Language), IPL (Intermec Printer Language), TPCL (TEC Printer Command Language) document that are based on an XDP form design or PDF document. These APIs help to merge a form design with form data to generate a document. You can save the document to a file and use Power Automate to send it to a printer.
## Deploying to your environment
You can also customize and [deploy the Adobe Experience Manager connector](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to your Microsoft Power Automate and Power Apps.
## Community support
You can join [Adobe Experience Manager Forms community forums](https://www.adobe.com/go/adobe-experience-manager-forms-community) to view and contribute to related discussions.