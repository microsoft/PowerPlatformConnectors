# SolarEdge

he SolarEdge API allows other software applications to access its monitoring system database for data analysis purposes, fleet management, displaying system data in other applications, etc

## Publisher: Richard Wierenga | Blis Digital

## Prerequisites

You need to have a SolarEdge account and SolarEdge equipment. You can [login](https://monitoring.solaredge.com/) and find all information here.

## Supported Operations

### Site List

A list of sites for a given account, with the information on each site. This list allows convenient search, sort and pagination

### Site Details

Details of a chosen site

### Site Overview

Site current power, energy production (today, this month, lifetime) and lifetime revenue

### Site Environmental Benefits

Summary of site’s positive impact on the environment

### Inventory

Information about the SolarEdge equipment, including: inverters/SMIs, batteries, meters, gateways and sensors

### Site Power

Site power measurements in a 15-minute resolution

### Site Energy

Site energy measurements

### Site Energy Detailed

Site energy measurements including meters such as consumption, export (feed-in), import (purchase), etc.

### Site Power Detailed

Site power measurements including meters such as consumption, export (feed-in), import (purchase), etc.

### Site Power Flow

Get the power flowchart of the site

### Site Energy – Time Period

Site energy for a requested timeframe

### Get Meters Data

Information about each meter in the site including: lifetime energy, metadata and the device to which it’s connected to.

### Storage

Get detailed storage information from batteries including the state of energy, power and lifetime energy

### Components List

List of inverters with name, model, manufacturer, serial number and status

### Get Sensor

List The list of sensors installed in the site

### Inverter Technical Data

Technical data on the inverter performance for a requested period of time

### Equipment Change Log

List of replacements for a given component

## Obtaining Credentials

An API key must be used in order to submit API requests.
Account users should generate an Account Level API Key, and system owners should generate a Site level API Key.

If you logon to the ![SolarEdge monitoring website](https://monitoring.solaredge.com/) you can create an API key.
In the Site Admin > Site Access tab > Access Control tab > API Access section:

1. Acknowledge reading and agreeing to the SolarEdge API Terms & Conditions.
2. Click Generate API key.
3. Copy the key.
4. Click Save.
5. Use this key when creating the connection.

for more info about the API see the following documentation ![link](https://knowledge-center.solaredge.com/sites/kc/files/se_monitoring_api.pdf)

## Known Issues and Limitations

No issues and limitations are known at this time.
