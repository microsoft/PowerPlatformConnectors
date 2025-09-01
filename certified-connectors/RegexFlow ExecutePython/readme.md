# RegexFlow ExecutePython
The RegexFlow ExecutePython connector enables PowerAutomate users who want to add Python's functionality to their Flows. 

## Publisher: Epicycle

## Prerequisites
You will need the following to proceed:
* A RegexFlow ExecutePython subscription

## Supported Operations

### ExecutePython
Executes the Python (v3.10) script received in the body of the POST request. The script has to put the intended results into a JSON variable named output.

## Known Issues and Limitations
* The usage usage quota is 31 calls per month and there is a rate limit in place of 10 calls per minute.
* Max runtime of the python script is 10s
* Only the built-in modules and openpyxl are available.

## Obtaining Credentials
You can get your own API key from the [RegexFlow Portal](https://portal.publicapi.regexflow.com/).