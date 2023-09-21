## RegEx Matching
This connector extends Power Automate capabilities for performing tests whether a text string matches a pattern. There are pre-built patterns and also an option for user to provide their own regular expressions (regex) for matching the text string. 

## Publisher: Mitanshu Garg

## Prerequisites
There are no prerequisities for this connector

## Supported Operations

### valid Email
Returns `true` if the text string is a valid email address

### valid GUID
Returns `true` if the text string is in a valid GUID format

### valid SSN
Returns `true` if the text string is in United States Social Security Number format

### valid Credit Card
Returns `true` if the text string is in credit card number format

### valid DATETIME
Returns `true` if the text string is in YYYYMMDDhhmmss format

### contains Digit
Returns `true` if the text string contains a digit (0-9) anywhere in the input

### startsWith
Returns `true` if the text string starts with the given keyword

### endsWith
Returns `true` if the text string ends with the given keyword

### match pattern
Returns `true` if the text string matches the custom regex pattern

## Obtaining Credentials
Anonymous authentication

## Known Issues and Limitations
- This connector only checks whether the provided input matches the expected format for the corresponding action. Other logic (such as checksum) is not performed by this connector.
- Datetime action does not limit number of days for different months and it accepts 31 days for all 12 months.
