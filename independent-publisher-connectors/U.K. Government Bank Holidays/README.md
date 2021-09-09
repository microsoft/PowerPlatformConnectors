# U.K. Government Bank Holidays
This connector presents the UK Goverment's Bank Holidays list in JSON format

## Publisher: Martyn Lesbirel

## Prerequisites: 
There are no prerequisities for this connector

## Supported Operations
There are no individual operations using the connector itself provides the list of Bank Holidays.

## Obtaining Credentials
Credentials are not requied for this conenctor.​

## Getting Started
Just adding the connector to your flow provides access to the holidays. 

**Note**
No compose is used on the output of the connector. For example to get the count of each kingdom's holidays a string varaiable can be initalised. The count is obtained by reffering to the connector output body.

For example to count the Scotish Bank Holiday the instruction is 

`length(outputs('All_holidays_for_all_kingdoms')?['body']['scotland']['events'])`

This can in turn be converted to an integer value if required.

The very highest level of the response refers to all the kingdoms.

```
{
    "england-and-wales": {},
    "scotland": {},
    "northern-ireland": {}
}
```

When expanded each has the same format.

```
"scotland": {
    "division": "scotland",
    "events": []
},
```

A division (kingdom) name and an event array. Each event is a Bank Holiday record and contains the following details.

```
{
    "title": "St Andrew’s Day",
    "date": "2019-12-02",
    "notes": "Substitute day",
    "bunting": true
},
```

Each attribute should be self explanatory.


## Known Issues and Limitations
Beaware the list extends only a few years into the past and future if you are looking for Bank Holidays of say a decade ago this connector will not help.