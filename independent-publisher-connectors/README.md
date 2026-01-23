# Independent Publisher Connectors

This directory contains custom Power Platform connectors in development.

## Structure

Each connector is organized in its own folder with the following standard files:

```
independent-publisher-connectors/
├── Apollo/
│   ├── apiDefinition.swagger.json
│   ├── apiProperties.json
│   ├── readme.md
│   └── [additional connector files]
├── ExchangeRate/
├── Kit/
├── MailChimp/
├── PartnerCenter/
├── QuickBooks/
└── WhatsAppBiz/
```

## Connectors

- **Apollo** - Apollo.io connector
- **ExchangeRate** - Exchange rate API connector
- **Kit** - Kit connector
- **MailChimp** - MailChimp Marketing connector
- **PartnerCenter** - Microsoft Partner Center connector
- **QuickBooks** - QuickBooks Online connector
- **WhatsAppBiz** - WhatsApp Business connector

## Development Status

All connectors in this directory are currently in development and not yet ready for pull requests.

## Git Structure

This repository is set up as a fork of the [Microsoft PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors) repository.

### Branches

Each connector has its own dedicated branch for future pull requests:

- `apollo-enrichment-connector` - Apollo connector
- `exchangerate` - ExchangeRate connector
- `kit` - Kit connector
- `mailchimp-marketing-ip` - MailChimp connector
- `partnercenter` - Partner Center connector
- `quickbooks-online-forceworks` - QuickBooks connector
- `whatsappbiz` - WhatsApp Business connector

### Workflow

1. Work on each connector in its dedicated branch
2. When ready for submission, the branch will be used to create a pull request to the upstream repository
3. The `master` branch contains all connectors for local development

## Standard Files

Each connector folder should contain:

- `apiDefinition.swagger.json` - OpenAPI/Swagger definition
- `apiProperties.json` - Connector properties and metadata
- `readme.md` - Connector documentation
- `icon.png` - Connector icon (optional)
- Additional screenshots and documentation as needed

## Future Pull Requests

When ready for submission, each connector will be prepared as a separate pull request to the official Power Platform Connectors repository following the [Independent Publisher Connector submission guidelines](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/independent-publisher-connectors/README.md).
