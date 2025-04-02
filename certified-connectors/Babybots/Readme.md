# Babybots Connector
Assists in streamlining workflows and automate tasks with ease. The Babybots Connector can preform complex operations such as data aggregation with just a few click. Babybots Connector is a premium connector that requires a Subscription.

## Publisher: Babybots LLC

## Prerequisites
To utilize the Babybots Connector one must have a susbscription.
* Visit https://buy.stripe.com/bIY3fN6sNeLacEwfYY and purchase a subscription.
* Check your email for a message from hello@babybots.ai which will contain your Subscription Key. 
* Using your email and the Subscription Key add the connector to your Power Automate workflow.

## Supported Operations
### Sum Numbers
Accepts a multi-formatted string of numbers which all will be summed together and output will be a single float value.

### Sanitize Input
Will parse out any HTML/XML tags, and special characters from text input. 

## Obtaining Credentials
Visit https://buy.stripe.com/bIY3fN6sNeLacEwfYY and purchase a subscription. You will recieve an email with your subscription key. Your email address and the subscription key will be used for credentials for this connector.

## Known Issues and Limitations
Sanitize Input will leave special characters such as . and @ as they are valid in email addresses.

## Deployment Instructions
Run the following commands and follow the prompts:

```
paconn login
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
