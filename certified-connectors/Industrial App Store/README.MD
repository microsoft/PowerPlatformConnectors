# Industrial App Store Connector

The [Industrial App Store](https://appstore.intelligentplant.com) from Intelligent Plant is built on an architecture that enables cloud-based applications and services to interact with on-site Process Data Historians and Alarm & Event Logs. The Industrial App Store Connector for Power Automate allows users to pull Industrial Plant data into their flows.

### Prerequisites

* **An [Industrial App Store](https://appstore.intelligentplant.com) Account**
*Registering is simple and only involves logging in using one of a number of authentication providers.*

* **An Industrial App Store Connector Licence**
*The "Industrial App Store Connector" is a commercial product. If interested in developing the product, contact support@intelligentplant.com for a developer licence.*

### Obtaining Credentials

During an action confgiuration you will be asked to sign in into Industrial App Store. More information for new Industrial App Store users can be found [here](https://appstore.intelligentplant.com/wiki/doku.php?id=general:app_store_users "Intelligent Plant Industrial App Store wiki - Welcome to the Industrial App Store").

### Supported Actions (operations)

Additional information on API calls made and its parameters can be found [here](https://appstore.intelligentplant.com/apihelp "Intelligent Plant Industrial App Store wiki - API help").

* **Get Data Sources**
  
  Returns available data sources for the user.

* **Get Tags**

  Performs a tag search on a data source for the specified filter.

* **Get Snapshot**

  Performs a snapshot (i.e. "what is the value now?") data query on a data source for a specified tag.

* **Get Processed Data**

  Performs a processed (or aggregated) data query (e.g. Max, Min, Avg) on a data source for a specified tag and date range.

* **Get Plot Data**

  Performs a plot data query on a single data source for a specified tags and date range. Plot is a special type of function that returns the minimum number of data points (for a given set of parameters) required to graphically plot an accurate time-series trend.

* **Get Raw Data**

  Performs a raw data query (i.e. requests unprocessed data) on a data source for a specified tag and date range. Raw queries are generally not advised as they are likely to return a bulky result-set that may well be cropped by size constraints. If connecting to a Process Data Historian, it is far more efficient to request "Get Processed Data" with an "INTERP" function instead.

## Supported Triggers

Coming soon. A workaround at the moment is to schedule a fequent task.

## Known Limitations

* Users can only query for a single tag per action. This means if you wish to query two or more tags you'll have to define multiple actions of the same type. This is due to Microsoft not supporting 'explosive arrays' in swagger definition. We are currently working on new API endpoints to support this.

## Examples

* Microsoft Power Automate Connector blog post - [Connecting industrial historians to Microsoft Power Automate. One connector to get them all…](https://powerusers.microsoft.com/t5/Power-Automate-Community-Blog/Connecting-industrial-historians-to-Microsoft-Power-Automate-One/ba-p/525552 "Connecting industrial historians to Microsoft Power Automate. One connector to get them all…").
* Intelligent Plant Industrial App Store Microsoft Power Automate Connector (YouTube video) - [Act on Industrial data with Microsoft Power Automate (Flow)](https://www.youtube.com/watch?v=T5Aks4A60uI "Act on Industrial data with Microsoft Power Automate - A YouTube video")

## Useful Links

* [Intelligent Plant](https://www.intelligentplant.com "Intelligent Plant")
* [Intelligent Plant Git Hub](https://github.com/intelligentplant "Intelligent Plant Git Hub")
* [Industrial App Store](https://appstore.intelligentplant.com "Industrial App Store")
* [Industrial App Store Wiki](https://appstore.intelligentplant.com/wiki "Industrial App Store wiki")
* [Intelligent Plant YouTube channel](https://www.youtube.com/channel/UCGWOUFOjAEk_QW9w1wWWPDw "Intelligent Plant YouTube channel")

## Support

For any questions please contact Intelligent Plant Ltd [here](https://www.intelligentplant.com/contactus "Intelligent Plant - Contact Us") or fire an email to support@intelligentplant.com.

