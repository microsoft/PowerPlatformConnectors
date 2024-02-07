# haveibeenpwned
Check if your email address or phone number is in a data breach.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
There are no prerequisites to using this service.

## Obtaining Credentials
An API key must be obtained to call these actions, which costs at least one month at $3.50. The page to purchase the key can be found [here](https://haveibeenpwned.com/API/Key).

## Supported Operations
### Get all breaches for an account
The most common use of this service is to return a list of all breaches a particular account has been involved in. The action takes a single parameter which is the account to be searched for. The account is not case sensitive and will be trimmed of leading or trailing white spaces.
### Get all breached sites in the system
A breach is an instance of a system having been compromised by an attacker and the data disclosed. It is possible to return the details of each of breach in the system.
### Get a single breached site
Sometimes just a single breach is required and this can be retrieved by the breach name. This is the stable value which may or may not be the same as the breach title (which can change).
### Get all data classes in the system
A "data class" is an attribute of a record compromised in a breach. For example, many breaches expose data classes such as "Email addresses" and "Passwords". The values returned by this service are ordered alphabetically in a string array and will expand over time as new breaches expose previously unseen classes of data.
### Get all pastes for an account
This action takes a single parameter which is the email address to be searched for. The email is not case sensitive and will be trimmed of leading or trailing white spaces.
### Get the most recently added breach
Retrieve the most recently added breach.

## Known Issues and Limitations
There are no known issues at this time.
