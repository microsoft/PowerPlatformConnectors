ArcGIS Survey123 lets you design location-aware smart forms for your organization and for the public, allowing people to submit data on the web or with the Survey123 field app. Survey123 is part of ArcGIS, bringing the power of location intelligence into your survey data. For more information, see the [ArcGIS Survey123 Overview](https://www.esri.com/en-us/arcgis/products/arcgis-survey123/overview).

## Prerequisites

- An ArcGIS organizational account is required. For more information, see [Create account](https://doc.arcgis.com/en/arcgis-online/get-started/create-account.htm).
  - The Survey123 connector is designed to help you work with Survey123 surveys hosted in ArcGIS Online. If you want to use this connector with surveys hosted in ArcGIS Enterprise, please contact your Esri representative to discuss how Esri can help you configure your own custom Survey123 connector.
- You must be the owner of an [ArcGIS Survey123](https://survey123.arcgis.com) survey.

## Get started

Once you have published your survey, you can familiarize yourself with this connector using templates from the gallery. Any data submitted through your form will be available in Microsoft Power Automate as dynamic content. You can even work with photos and signatures!

## Considerations

You can only access surveys that you own.

By default, a flow is triggered when a survey response is _submitted_. To trigger a flow when a survey response is _edited_, go to the webhook settings for your survey on the [Survey123 website](http://survey123.arcgis.com). In the **Trigger events** section, enable the **Existing record edited** option. For more information on webhook settings, see [Configure a webhook in the Survey123 website](https://doc.arcgis.com/en/survey123/browser/create-surveys/webhooks.htm#GUID-38D2604E-A428-4754-B0E7-A02B8DF9ECD0).
