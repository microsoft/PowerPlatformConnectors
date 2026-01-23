# Pull Request Submission Checklist

Based on Microsoft's Independent Publisher Connector Certification Requirements

## Pre-Submission Requirements

### ✅ OneVet Verification
- [ ] Government-issued ID ready (passport or driver's license)
- [ ] GitHub profile matches government ID name
- [ ] Microsoft Authenticator app installed on mobile device
- [ ] Verified credentials added to Microsoft Authenticator
- [ ] Verification valid (not expired)

### ✅ Connector Uniqueness
- [ ] Checked [Microsoft connector reference list](https://learn.microsoft.com/en-us/connectors/connector-reference/)
- [ ] Checked [independent-publisher-connectors folder](https://github.com/microsoft/PowerPlatformConnectors/tree/dev/independent-publisher-connectors)
- [ ] Confirmed connector doesn't already exist
- [ ] If exists, confirmed adding new functionality (not duplicating)

## Connector File Requirements

### ✅ Title (apiProperties.json)
- [ ] Written in English
- [ ] Follows pattern: `[Service Name] (Independent Publisher)`
- [ ] ≤30 characters total
- [ ] No forbidden words (API, Connector, Power Apps, Power Automate, Copilot Studio, Logic Apps, Flow)
- [ ] Ends with alphanumeric character
- [ ] Unique from all existing connectors

### ✅ Description (apiProperties.json)
- [ ] Written in English
- [ ] 30-500 characters
- [ ] Free of spelling/grammatical errors
- [ ] Concise description of purpose and value
- [ ] No Power Platform product names

### ✅ Icon (apiProperties.json)
- [ ] `iconBrandColor` set to `#da3b01` (vivid orange - required for Independent Publishers)
- [ ] No custom icon needed (generic icon will be used)

### ✅ Operations (apiDefinition.swagger.json)
- [ ] All operations have `summary` (≤80 characters, alphanumeric + parentheses only)
- [ ] All operations have `description` (full sentences with punctuation)
- [ ] All operations have `visibility` set appropriately
- [ ] All responses have exact schemas (no default responses with schema)
- [ ] No empty response schemas (except for dynamic responses)
- [ ] No empty operations
- [ ] All parameters have descriptions
- [ ] All parameters have appropriate `x-ms-summary` for display names

### ✅ Swagger/OpenAPI (apiDefinition.swagger.json)
- [ ] OpenAPI 2.0 format (3.0 not supported)
- [ ] Valid swagger (validated against OpenAPI 2.0 spec)
- [ ] Production host URL (no staging/dev/test URLs)
- [ ] HTTPS with TLS 1.2+ required
- [ ] File size <1 MB
- [ ] ≤256 operations per swagger
- [ ] ≤512 schema count per body
- [ ] ≤16,384 schemas per operation

### ✅ Authentication
- [ ] **OAuth is NOT supported** for Independent Publishers
- [ ] Using supported auth type: API Key or Basic Authentication
- [ ] Auth configuration tested and working
- [ ] Clear instructions for obtaining credentials in readme.md

## Package Requirements

### ✅ Package Structure
```
package.zip
├── PackageAssets/
│   └── [solution files]
├── ConnectorSolution.zip
├── FlowSolution.zip
├── intro.md
└── customProperties.json (if multi-auth)
```

### ✅ Package Contents
- [ ] Connector solution exported from Power Platform
- [ ] Flow solution with test flow included
- [ ] intro.md file with required sections
- [ ] customProperties.json (if using multiple auth types)
- [ ] Package validated with [ConnectorPackageValidator.ps1](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/scripts/ConnectorPackageValidator.ps1)

### ✅ intro.md / readme.md Requirements
- [ ] Connector features and functionality described
- [ ] **Known Issues and Limitations** section included
- [ ] Instructions for obtaining credentials
- [ ] Example use cases or scenarios
- [ ] Clear, professional writing

### ✅ Azure Blob Storage
- [ ] Package uploaded to Azure blob storage
- [ ] SAS URL generated
- [ ] SAS URL valid for **15+ days** from submission
- [ ] SAS URL tested and accessible

## Testing Requirements

### ✅ Connector Testing
- [ ] All operations tested (minimum 10 successful calls each)
- [ ] No runtime errors
- [ ] No schema validation errors
- [ ] Credentials work in production environment
- [ ] Test flow created and working
- [ ] Test flow added to solution

### ✅ Solution Checker
- [ ] Solution Checker run on connector solution
- [ ] All critical issues resolved
- [ ] All high-priority issues resolved

## GitHub PR Requirements

### ✅ Repository Setup
- [ ] Forked [microsoft/PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors)
- [ ] Working on `dev` branch (not `master`)
- [ ] Connector files in correct folder: `independent-publisher-connectors/[ConnectorName]/`

### ✅ Required Files in PR
- [ ] apiDefinition.swagger.json
- [ ] apiProperties.json
- [ ] readme.md (or intro.md)
- [ ] icon.png (optional but recommended)
- [ ] Screenshots (optional but recommended)

### ✅ PR Details
- [ ] PR title: `[ConnectorName] (Independent Publisher)` (no "Proposal -" prefix)
- [ ] PR description filled out completely
- [ ] Support email matches GitHub account email
- [ ] Privacy policy URL provided
- [ ] Package SAS URL included in PR
- [ ] All checklist items in PR template completed

### ✅ Submission Information
- [ ] Publisher name (your name)
- [ ] Publisher email (matches GitHub)
- [ ] Support email (matches GitHub)
- [ ] Website URL (valid)
- [ ] Privacy policy URL (valid)
- [ ] Package URI (SAS URL, valid 15+ days)

## Post-Submission

### ✅ Certification Process
- [ ] Verified credentials shared (if requested)
- [ ] Responded to certification team comments within 30 days
- [ ] Fixed any policy errors reported
- [ ] Maintained same PR for fixes (or created fresh PR if needed)

### ✅ Preview Testing (After Certification)
- [ ] Accessed preview environment in Partner Center
- [ ] Created test connections
- [ ] Tested all triggers and actions
- [ ] Verified OAuth ClientID/ClientSecret (if applicable)
- [ ] Confirmed go-live within 2 business days

## Common Pitfalls to Avoid

❌ **Don't:**
- Use OAuth authentication (not supported for Independent Publishers)
- Include "API" or "Connector" in the title
- Use staging/dev/test URLs
- Submit with expired SAS URL
- Create default responses with schemas
- Leave empty operations or schemas
- Use OpenAPI 3.0 (only 2.0 supported)
- Change authentication type in updates (breaking change)
- Create new offer for updates (reopen existing)

✅ **Do:**
- Use production URLs only
- Set iconBrandColor to #da3b01
- Include Known Issues section in readme
- Test all operations thoroughly
- Validate package before submission
- Keep SAS URL valid for 15+ days
- Respond to certification team promptly
- Detail all changes in updates

## Timeline Expectations

| Phase | Duration |
|-------|----------|
| OneVet verification | Complete within 30 days |
| Certification review | Varies by quality |
| Deployment to preview | ~15 business days |
| Preview testing window | 2 business days |
| Production deployment | Fridays (PST/PDT) |
| Global rollout | 5-6 weeks |

## Resources

- **Certification Guide**: [ip-connector-certification-guide.md](./ip-connector-certification-guide.md)
- **Workflow Guide**: [WORKFLOW.md](./WORKFLOW.md)
- **Package Validator**: [ConnectorPackageValidator.ps1](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/scripts/ConnectorPackageValidator.ps1)
- **PR Template**: [pull_request_template.md](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/.github/pull_request_template.md)
- **Office Hours**: Every Tuesday, 3:30-4:30 PM UTC
- **Email Support**: connectorpartnermgmtteam@microsoft.com

---

**Last Updated**: January 2026
