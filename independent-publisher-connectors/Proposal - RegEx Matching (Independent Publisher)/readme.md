## RegEx Matching
This connector extends Power Automate capabilities for performing tests whether a text string matches a pattern. There are pre-built patterns and also an option for user to provide their own regular expressions (regex) for matching the text string. 

## Publisher
Mitanshu Garg

## Prerequisites
There are no prerequisities for this connector

## Supported Operations
| Action | Description |
| --- | --- |
| `valid Email` | Returns `true` if the text string is a valid email address |
| `valid GUID` | Returns `true` if the text string is in a valid GUID format |
| `valid DATETIME` | Returns `true` if the text string is in `YYYYMMDDHHMMSS` format |
| `contains Digit` | Returns `true` if the text string contains a digit `0-9` anywhere in the input |
| `match pattern` | Returns `true` if the text string matches the custom `regex pattern` |
| `startsWith` | Returns `true` if the text string starts with the given keyword |
| `endsWith` | Returns `true` if the text string ends with the given keyword |
