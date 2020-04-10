# Industrial App Store Microsoft Power Automate Connector

![IAS Connector](https://www.intelligentplant.com/datasheets/powerplatform/resources/AppStore-Automate-Connector-100px.png "Industrial App Store - Microsoft Power Automate (Flow) Connector")

Intelligent Plant’s [Industrial App Store](https://appstore.intelligentplant.com) Microsoft Automate (Flow) Connector enables Microsoft’s powerful analytics and visualizations to be applied to real-time and historical process data. Seamlessly integrate plant and corporate data and share with any colleague on any device, enabling faster, better, real-time decision making. The Industrial App Store Connector connects to Intelligent Plant’s Industrial App Store, while all data **remains** secure and safe on-premises. The plant data may be centralized in a corporate data lake or globally dispersed across many sites and historians. Either way, Industrial App Store Power Automate Connector brings it all together and delivers to your fingertips.

## Industrial App Store - What is it

Intelligent Plant developed the Industrial App Store which enables interaction with all the different historian data through a unified API. At start of May 2020 Intelligent Plant released an official, Microsoft certified Industrial App Store connector for Power Automate. No more hefty SQL queries or Excel document interrogations with no easy real-time update option or complicated architecture solutions, simply install Industrial App Store connect and out of the box you will be able to connect to Aspentech IP.21, OSIsoft PI, Honeywell Dynamo, OPC DA & HDA, Siemens and many more. This allows our customers to bring data from various sources into Microsoft Automate flows to deliver insights that weren’t possible before. Real-time updates prompt money-saving decisions by engineers that also increase plant safety and efficiency.

## Getting started with Microsoft Power Automate Industrial App Store Connector

To start with Microsoft Power Automate (Flow) check out this article [here](https://docs.microsoft.com/en-us/power-automate/getting-started). To get started with Industrial App Store Connector configure a step and choose an action from the connector (see below).

![IAS Connector](https://www.intelligentplant.com/datasheets/powerplatform/resources/msflow-ias-getstarted.gif "Industrial App Store Connector - Getting started")

### Prerequisites

* You will need App Store Connect to connect to you own data but its not essential if you wish to play around with demo data provided by Intelligent Plant. More info on how to connect your data to Industrial App Store can be found [here](https://community.powerbi.com/t5/Community-Blog/Connecting-industrial-historians-to-Microsoft-Power-BI-One/ba-p/942200 "Connecting industrial historians to Microsoft Power BI. One connector to get them all… ") and [here](https://appstore.intelligentplant.com/wiki/doku.php?id=app_store_connect:app_store_connect "Industrial App Store Wiki").

* Any kind of Microsoft Power Automate (Flow) subscription is needed in order to create Flows. More info on subscription types can be found [here](http://download.microsoft.com/download/9/5/6/9568EFD0-403D-4AE4-95F0-7FACA2CCB2E4/PowerApps%20and%20Flow%20Licensing%20Guide%20-%20August.pdf).

### Obtaining credentials

During an action confgiuration you will be asked to sign in into Industrial App Store. More information for new Industrial App Store users can be found [here](https://appstore.intelligentplant.com/wiki/doku.php?id=general:app_store_users "Intelligent Plant Industrial App Store wiki - Welcome to the Industrial App Store").

### Supported actions (operations)

Additional information on API calls made and its parameters can be found [here](https://appstore.intelligentplant.com/apihelp "Intelligent Plant Industrial App Store wiki - API help").

* **Get data sources**
  
  Returns available data sources for the user.

* **Get tags**

  Performs a tag search on a single data source for the specified filter.

* **Get snapshot**

  Performs a snapshot data query on a single data source for specified tags.

* **Get historical data**

  Performs a historical data query on a single data source for specified tags, date range and aggregation function.

* **Get Processed data**

  Performs a processed or aggregated data query on a single data source for specified tags, date range.

* **Get plot data**

  Performs a processed or aggregated data query on a single data source for specified tags, date range.

* **Get raw data**

  Performs a RAW data query on a single data source for specified tags, date range.

## Supported triggers

Coming soon. A workaround at the moment would be to schedule a fequent task.

## Known limitations

* Users can only query for a single tag per action. This means if you wish to query two or more tags you'll have to define multiple actions of the same type. This is due to Microsoft not supporting 'explosive arrays' in swagger definition. We are currently working on a new API endpoint to support this.

## Examples

* [Intelligent Plant Industrial App Store Microsoft Power Automate Connector - Act on Industrial data with Microsoft Power Automate (Flow)](https://www.youtube.com/watch?v=T5Aks4A60uI "Act on Industrial data with Microsoft Power Automate - A YouTube video")

* Microsoft Power Automate Blog post - Coming soon.

## Built With

Industrial App Store Connector was built using

* [Industrial App Store API](https://appstore.intelligentplant.com/wiki/doku.php?id=dev:app_store_developers "Intelligent Plant Industrial App Store Developers")
* [Swagger 2.0](https://swagger.io/docs/specification/2-0/basic-structure/)

## Contributing

Please read [Microsoft Power Platform Connectors - Contributing](https://github.com/Microsoft/PowerPlatformConnectors#contributing) for details on our code of conduct, and the process for submitting pull requests.

## Useful links

* [Intelligent Plant](https://www.intelligentplant.com "Intelligent Plant")
* [Industrial App Store](https://appstore.intelligentplant.com "Industrial App Store")
* [Industrial App Store Wiki](https://appstore.intelligentplant.com/wiki "Industrial App Store wiki")
* [Intelligent Plant YouTube channel](https://www.youtube.com/channel/UCGWOUFOjAEk_QW9w1wWWPDw "Intelligent Plant YouTube channel")

## Support

For any questions please contact Intelligent Plant Ltd [here](https://www.intelligentplant.com/contactus "Intelligent Plant - Contact Us") or fire an email to support@intelligentplant.com.

## Authors

* Intelligent Plant team.

## Acknowledgments

* Hat tip to anyone who contributed.
* Inspiration

![IAS Connector](https://www.intelligentplant.com/datasheets/powerplatform/resources/Welcome-Industrial-AppStore.png "Intelligent Plant = Industrial App Store")
