# Lnk.Bio
This is a connector for Lnk.Bio. Lnk.Bio is a link-in-bio and website builder web tool and mobile app favored by 600,000 creators. 
With this connector, you can automate your Lnk.Bio Lnks creation, streamlining your workflow.

## Publisher: Lnk.Bio

## Prerequisites
You will need the following to proceed:

- A Lnk.Bio account, including FREE plans. [Sign up here](https://lnk.bio/signup)

## Supported Operations

### Create Lnk
Action to create a new Lnk on your public Lnk.Bio profile

## Obtaining Credentials
Authentication is via oAuth2, the only requisite is a Lnk.bio account [Sign up here](https://lnk.bio/signup). 

## Known Issues and Limitations
We do not have any known issues so far

## Deployment Instructions
Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png
```