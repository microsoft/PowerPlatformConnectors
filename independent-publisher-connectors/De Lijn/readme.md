# De Lijn
'De Lijn' is the public transport company for busses in Flanders. This is a part of Belgium.
This service allows you to find a stop/line based on specific terms or location (lat/longitude)

The api that is used is published by 'De Lijn' - I am not the owner of this endpoint and a Power Automate enthousiast.
Submission containing materials of a third party: 'De lijn' - https://data.delijn.be/
'This portal is the gateway to the open data sets and webservices of De Lijn. Open Data De Lijn combines freely accessible and usable data on routes, stops, interruptions and timetables.'
More info on the webpage linked.

## Publisher: Publisher's Name
Lenard Schockaert

## Prerequisites
Create an account on https://data.delijn.be/ 



## Supported Operations
### Operation 1 - Zoek haltes(stops)
Search stops by description. Eg Veemarkt or station.![ZoekHaltes](https://user-images.githubusercontent.com/8872614/198578007-85c355fe-9368-492c-97ee-df47308f3519.png)


### Operation 2 - Zoek Lijnrichtingen(lines)
Search lines by number or description. Eg search line 48 or Hamont![ZoekLijnrichtingen](https://user-images.githubusercontent.com/8872614/198578027-c0ec1ea7-1e53-4f07-9ff1-bba4bb5ab042.png)


### Operation 3 - Zoek Locaties(locations)
Search locations (x,y) by user adress input. Typical for input on a routeplanner (eg kerkstraat 14 Hasselt or Achter De Kazerne ) . Limitied Geocoding service that retuns stops, adresses or POIs and its XY coordinates
![ZoekLocaties](https://user-images.githubusercontent.com/8872614/198578039-c5294d40-4ab7-449f-bfe6-e968c29b3bc6.png)

## Obtaining Credentials
On the website mentioned at prerequisities, obtain an api key by going to your account and subscribe to the 'Open Data Free' to receive an api key.

## Getting Started
Get the api key
Import the connnector
Create connection
Get the closest bus stops in Power Platform

## Known Issues and Limitations
None, atm. Using the defintion delivered by 'De Lijn' company

## Flow Example
![image](https://user-images.githubusercontent.com/8872614/198582197-efe246dd-aea0-4359-a9ac-0358f9c2670b.png)

