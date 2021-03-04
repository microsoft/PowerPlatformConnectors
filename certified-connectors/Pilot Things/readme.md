# Pilot Things

[Pilot Things](https://www.pilot-things.com/) is a platform to manage, visualize, automate and integrate your IoT device fleet.

This connector grants access to the Pilot Things API from the Microsoft Power Platform.

## Prerequisites

To use this connector, you need a working instance of Pilot Things.

## Supported operations

This connector supports the following operations:
- Alerts:
  - `getAlerts`
  - `updateAlertState`
- Measures:
  - `getMeasures`
  - `getCount`
  - `getMeasure`
- Messages:
  - `getMessages`
  - `getMessagesAndMeasurements`
  - `addMessage`
  - `getMessage`
  - `getPreviousMessage`
- Sites:
  - `getSites`
  - `createSite`
  - `getSite`
  - `deleteSite`
  - `updateSite`
- Things:
  - Custom fields:
    - `getCustomField`
    - `createCustomField`
    - `updateCustomField`
    - `deleteCustomField`
    - `getCustomFieldImage`
    - `postCustomFieldImage`
  - Messages:
    - `getLastMessage`
    - `getThingMessages`
    - `deleteThingMessages`
    - `createThingMessages`
  - Measures:
    - `getLastMeasures`
    - `getThingMeasures`
  - Models:
    - `getThingModel`
    - `getThingActiveModel`
  - Operations:
    - `getThingOperations`
    - `executeThingOperation`
  - Product:
    - `getThingProduct`
    - `dissociateThingProduct`
    - `associateThingProduct`
    - `associateThingsWithProduct`
  - Tags:
    - `updateThingTag`
    - `addThingTag`
    - `getThingTag`
    - `getThingTags`
  - `getThings`
  - `addThingsCsv`
  - `getThingList`
  - `getThing`
  - `ignoreThing`
  - `putThing`
  - `getThingImage`
  - `updateThingFixedPosition`
  - `getFlowsRelatedToThing`
  - `getThingStatsLast`
- Statistics:
  - `getStatsAvg`
  - `getStatsCount`
  - `getStatsLast`
  - `getStatsMax`
  - `getStatsMeasurements`
  - `getStatsMin`
  - `getStatsRepartition`
  - `getStatsSum`
- `getTags`

## Support and assistance

To get help, email us at contact@pilot-things.com or use the [contact form](https://www.pilot-things.com/contact).