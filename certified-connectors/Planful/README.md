# Planful Connector
Planful is the pioneer of financial performance management cloud software. The Planful platform is used by the Office of the CFO around the globe to streamline business-wide planning, budgeting, consolidations, reporting, and visual analytics. Planful transforms the way you plan, close, and report so you can drive more impactful decisions across your business. 

## Publisher: Planful

## Prerequisites

- An active acount with Planful

- Access to 'Data Load Rules' within Planful

- User to have 'Web services Access' enabled. Account admin can enable this in the "User & Role Management" section for any user

## Supported Operations
The connector supports the following operations:

### Get DLR Rules
Within Planful platform UI, you need to first create "Data Load Rules" (DLRs). This operations allows you to fetch all the existing data load rules within your account

### File load
This operation allows you to load data from flat file (csv) into Planful. You also need to specify the 'Data Load Rule' when using this operation to send the file to Planful.

## Ontaining credentials

- Step1: Sign in to your Planful account

- Step2: Click on your profile icon at the top right and navigate to "Access Keys"

- Step3: Give a name for the access key to create a new access key

- Step4: Copy the Access ID and Access Key to use them during authenticating the connector in Power Automate

![Screenshot 2023-05-04 at 7 32 24 PM](https://user-images.githubusercontent.com/408872/236231570-9541a18e-29fc-4146-af0e-003df69015bf.png)


## Known Issues and Limitations
- Supports only csv file types for data load.

## Deployment Instructions
- N/A