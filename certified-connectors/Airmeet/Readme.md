## Airmeet Connector

Airmeet is the fastest growing platform for hosting a rich variety of virtual events. From Hackathons to ice-breakers, panel discussions to workshops, Airmeet's intuitive experience delivers on organisers expectations for a wide range of events and not just conferences.

## Prerequisites

### To Generate Access key and Secret key

You will need the following to proceed:

Step 1: Sign in to your airmeet account ( https://www.airmeet.com/signup )

Step 2: Click on the "Settings" tab and select the "API Access Key" section.

Step 3: Click on "Generate access key" and provide a name/label to your key (can be anything that can be remembered later)
![Generate Access key and Secret key](https://s3-ap-south-1.amazonaws.com/ind-cdn.freshdesk.com/data/helpdesk/attachments/production/82017079115/original/VFPjpANY-BGgR06r6sZs0lSa-v8sUiPl1A.png?1634818045)

Step 4: After providing your label name 'X-Airmeet-Access-Key' with 'X-Airmeet-Secret-Key' would be generated, which can be used in API integration

Access Key = X-Airmeet-Access-Key

Secret Key = X-Airmeet-Secret-Key

![Copy Access key and Secret key](https://s3-ap-south-1.amazonaws.com/ind-cdn.freshdesk.com/data/helpdesk/attachments/production/82017079116/original/3UadSalyOAf1fqCDujELbc6FkkKJ7eNtmA.png?1634818046)

## Supported Operations

The connector supports the following operations:

- `Get Airmeets`: Get airmeet list from your community.
- `Create an Airmeet`: Create an Airmeet in your community.
- `Add a speaker`: Add a speaker to your Airmeet event.
- `Create Session`: Add a Session to your Airmeet.
- `Add Registrant`: Add Registrant to an Airmeet.
- `Register an Airmeet Trigger`: Select a trigger from the available options. Setup reminders and alerts based on event registration, attendance, event start and end time.
  Avaible triggers are "trigger.airmeet.registrant.added","trigger.airmeet.attendee.added","trigger.airmeet.attendee.joined","trigger.airmeet.created","trigger.airmeet.started","trigger.airmeet.finished","trigger.airmeet.reminder"
