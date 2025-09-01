# Automaited
[Automaited](https://automaited.com) is a company specialized on document processing tasks. This connector focuses on validateing E-Invoice / E-Rechnung in all common formats such as XRechnung, ZUGFeRD and Factur-X.

## Publisher: Automaited

## Prerequisites
N/A

## Supported Operations

### Validate E-Invoice
Validate E-Invoice / E-Rechnung in all common formats such as XRechnung, ZUGFeRD and Factur-X. This operation takes an invoice file (PDF or XML) and an optional invoice type, and returns a list of validation issues.
To validate using the Factur-X spec you have to select the corresponding ZUGFeRD spec as these are identical.


## Obtaining Credentials
No authentication is required.


## Known Issues and Limitations
Currently, there are no know issues. If you run into problems reach out to us via support@automaited.com


## Deployment Instructions
You can use it as a standard connector in your automation flows.
Here is an example [e-invoice](https://github.com/automaited/dummy-e-invoice-data/blob/main/zugferd_demo_sample.pdf) you can use for testing.

The connector parameter `File/body` should be the `contentBytes` of the e-invoice and the parameter `File (file name)` should be the filename of the e-invoice.
The connector will return an empty list, since this e-invoice is a valid one.

You can also test the same connector with [XML e-invoice](https://github.com/automaited/dummy-e-invoice-data/blob/main/zugferd_demo_sample_with_validation_errors.xml) which contains errors. The connector will return a list of these errors.

