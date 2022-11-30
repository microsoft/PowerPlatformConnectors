# De Lijn
'De Lijn' is the public transport company for busses in Flanders. This is a part of Belgium.
This service allows you to find a stop/line based on specific terms or location (lat/longitude)

The api that is used is published by 'De Lijn' - I am not the owner of this endpoint and a Power Automate enthusiast.
Submission containing materials of a third party: 'De lijn' - https://data.delijn.be/
'This portal is the gateway to the open data sets and webservices of De Lijn. Open Data De Lijn combines freely accessible and usable data on routes, stops, interruptions and timetables.'
More info on the webpage linked.

## Publisher: Lenard Schockaert

## Prerequisites
Create an account on https://data.delijn.be/ 



## Supported Operations
### Operation 1 - Search stops
Search stops by description. Eg Veemarkt or station.
![image](https://user-images.githubusercontent.com/8872614/199250884-83367551-7158-47b7-9df0-3311f73e80b3.png)


### Operation 2 - Search lines
Search lines by number or description. Eg search line 48 or Hamont
![image](https://user-images.githubusercontent.com/8872614/199250920-b0afe0a6-f4c2-42e8-9bf7-8724c5c78e5f.png)


### Operation 3 - SearchLocations
Search locations (x,y) by user adress input. Typical for input on a routeplanner (eg kerkstraat 14 Hasselt or Achter De Kazerne ) . Limitied Geocoding service that retuns stops, adresses or POIs and its XY coordinates
![image](https://user-images.githubusercontent.com/8872614/199250985-4d5fec7e-290c-4c5d-b4e6-9d0880f7022c.png)

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
![image](https://user-images.githubusercontent.com/8872614/199251422-d9c8e533-3e07-4534-ad2b-238f08a88fcd.png)
![image](https://user-images.githubusercontent.com/8872614/199251449-aa06a034-a01e-412e-afa7-53fd5179b740.png)
![image](https://user-images.githubusercontent.com/8872614/199251471-4d31d356-d0b7-4b34-ac6c-026c18cfe532.png)


