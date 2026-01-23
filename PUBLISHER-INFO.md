# Publisher Information

This file contains the standard publisher information to be used across all connectors.

## Publisher Details

**Name:** Steve Mordue
**Email:** steve@forceworks.com
**Support Email:** steve@forceworks.com
**GitHub:** https://github.com/forceworks
**Website:** https://forceworks.com

## Usage in Connector Files

### apiDefinition.swagger.json

```json
{
  "info": {
    "title": "[Connector Name] (Independent Publisher)",
    "description": "...",
    "version": "1.0.0",
    "contact": {
      "name": "Steve Mordue",
      "url": "https://github.com/forceworks",
      "email": "steve@forceworks.com"
    }
  },
  ...
  "x-ms-connector-metadata": [
    {
      "propertyName": "Website",
      "propertyValue": "https://forceworks.com"
    },
    {
      "propertyName": "Privacy policy",
      "propertyValue": "[Service Privacy Policy URL]"
    },
    {
      "propertyName": "Categories",
      "propertyValue": "[Appropriate Categories]"
    }
  ]
}
```

### readme.md

```markdown
## Publisher: Steve Mordue

[Steve Mordue](https://github.com/forceworks) | [Forceworks](https://forceworks.com)
```

## PR Submission Information

When submitting pull requests, use:

- **Publisher Name:** Steve Mordue
- **Publisher Email:** steve@forceworks.com
- **Support Email:** steve@forceworks.com
- **Website URL:** https://forceworks.com
- **Privacy Policy:** [Use the service's privacy policy URL]

## OneVet Verification

Ensure your OneVet verification is completed with:
- Government ID matching: Steve Mordue
- GitHub account: forceworks
- Email: steve@forceworks.com

## Notes

- All connectors must use consistent publisher information
- Email must match GitHub account email
- GitHub account must match government ID for OneVet verification
- Privacy policy can be the service's policy (e.g., Kit's privacy policy for Kit connector)
