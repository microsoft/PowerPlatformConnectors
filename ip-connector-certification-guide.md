# Microsoft Independent Publisher Connector Certification Guide

*Comprehensive reference based on Microsoft documentation as of January 2026*

---

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [OneVet Verification Process](#onevet-verification-process)
4. [Package Requirements](#package-requirements)
5. [Connector File Requirements](#connector-file-requirements)
6. [Submission Process](#submission-process)
7. [Certification Workflow](#certification-workflow)
8. [Policy Error Codes](#policy-error-codes)
9. [Testing Post-Certification](#testing-post-certification)
10. [Updates and Breaking Changes](#updates-and-breaking-changes)
11. [Moving from Preview to GA](#moving-from-preview-to-ga)
12. [Connector Integrity](#connector-integrity)
13. [FAQ and Limits](#faq-and-limits)
14. [Key Contacts and Resources](#key-contacts-and-resources)

---

## Overview

### What is an Independent Publisher Connector?

Independent publishers **do not own** the underlying service behind their connector. Anyone can submit a connector for a third-party API (like Apollo.io, Mailchimp, etc.) to the Power Platform ecosystem.

### Benefits

- **Free certification** - No cost to register, certify, or update
- **Listed in official Microsoft connector gallery** - Showcased on Power Automate, Power Apps, and Copilot Studio websites
- **Your name as publisher** - Visibility across Microsoft products and documentation
- **Marketing benefits** - Featured in blog posts, YouTube videos, monthly demos, social media
- **Technical review** - Microsoft's team provides feedback based on 1,200+ connector launches

### Key Limitations

- **OAuth connectors are currently UNSUPPORTED** for Independent Publishers
- All connectors are deployed as **Premium tier** (cannot be changed)
- **Same publisher rule** - Updates must come from the original publisher

---

## Prerequisites

### Before Starting

1. **Familiarize yourself with the GitHub repository**: [github.com/microsoft/PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors)
2. **Review learning content**: [Learning Content for Independent Publisher Connectors](https://github.com/microsoft/PowerPlatformConnectors/wiki/Learning-Content-for-Independent-Publisher-Connectors)
3. **Read and agree to the manifesto**: [Independent Publisher Connector Group Manifesto](https://github.com/microsoft/PowerPlatformConnectors/wiki/Independent-Publisher-Connector-Group-%22Manifesto%22)
4. **Get verified credentials** (OneVet) - Required for all PR submissions

### Verify Connector Doesn't Already Exist

Check these locations before building:

- [Microsoft connector reference list](https://learn.microsoft.com/en-us/connectors/connector-reference/)
- [Power Platform Connectors independent publisher folder](https://github.com/microsoft/PowerPlatformConnectors/tree/dev/independent-publisher-connectors)

| If your proposed connector... | Action |
|-------------------------------|--------|
| Already exists for Power Platform | Cannot build |
| Already exists as IP connector | Can add more functionality |
| Is currently a PR AND a proposal | Contact publisher to collaborate |
| Is a PR AND not a proposal | Wait for certification, then update |

---

## OneVet Verification Process

### Why Required?

Microsoft uses verified credentials (VCs) from trusted identity verification vendors to prevent fraud, consent phishing, and impersonation.

### What You Need

- Government-issued ID (passport or driver's license)
- GitHub profile that **matches your government ID**
- Email associated with your GitHub account
- Mobile device (iOS/Android) with Microsoft Authenticator app installed

### Process Steps

1. **Submit a PR** to the PowerPlatformConnectors repository
2. **Receive form link** - If you don't have VCs, you'll get an auto-generated link
3. **Fill out form** with:
   - First name, Last name
   - Email address
   - Country/Region
   - Full address (street, city, state, zip)
4. **Receive verification email** from `maccount@microsoft.com` (Account No Reply)
5. **Open link in private/incognito browser** to start AU10TIX verification
6. **Complete AU10TIX process**:
   - Enter email (same as GitHub)
   - Enter PIN from AU10TIX email
   - Enter phone number
   - Scan QR code with mobile device
   - Take photo of government ID
   - Take selfie
7. **Add to Microsoft Authenticator** - Select "Open Authenticator" then "Add"

### Important Notes

| Item | Details |
|------|---------|
| Time to complete | ~15 minutes |
| Deadline | 30 days from receiving email |
| Expiration | 1 year OR government ID expiration (whichever comes first) |
| Expired email? | Contact `connectorpartnermgmtteam@microsoft.com` |

---

## Package Requirements

### Package Structure

```
package.zip
├── PackageAssets/
│   └── [solution files]
├── ConnectorSolution.zip
├── FlowSolution.zip
├── intro.md
└── customProperties.json (if multi-auth)
```

### Creating the Package

1. **Create custom connector in a solution** in Power Automate/Power Apps
2. **Run Solution Checker** on connector solution
3. **Export connector solution**
4. **Create test flow** using the connector and add to solution
5. **Export flow solution**
6. **Create package** with both solutions using Package Deployer
7. **Create intro.md file**
8. **Zip everything** in the required structure
9. **Upload to Azure blob storage** and generate SAS URL (valid for 15+ days)

### Package Validation

Download and run: [ConnectorPackageValidator.ps1](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/scripts/ConnectorPackageValidator.ps1)

```powershell
# Run in PowerShell Admin mode
Set-ExecutionPolicy -ExecutionPolicy Unrestricted
.\ConnectorPackageValidator.ps1 "C:\path\to\package.zip"
```

**Success message**: "Validation successful: The package structure is correct."

---

## Connector File Requirements

### Title Requirements

| Requirement | Details |
|-------------|---------|
| Language | English |
| Uniqueness | Must be unique from all existing connectors |
| Pattern | `Connector Name (Independent Publisher)` |
| Max length | 30 characters |
| Forbidden words | API, Connector, Copilot Studio, Power Apps, etc. |
| Ending | Must end with alphanumeric character |

**Good**: `Apollo (Independent Publisher)`
**Bad**: `Apollo.io Power Apps Connector API`

### Description Requirements

| Requirement | Details |
|-------------|---------|
| Language | English |
| Grammar | Free of spelling/grammatical errors |
| Length | 30-500 characters |
| Content | Concise description of purpose and value |
| Forbidden | Power Platform product names |

### Icon Requirements (Independent Publishers)

- **Icon brand color**: Must be `#da3b01` (vivid orange)
- No custom icon needed - generic icon used

### Operation Requirements

| Element | Requirement |
|---------|-------------|
| summaries | ≤80 characters, alphanumeric + parentheses only |
| descriptions | Full sentences with punctuation |
| responses | Exact schema only (no default responses with schema) |
| empty schemas | Not allowed (except for dynamic responses) |
| empty operations | Not allowed |

### Swagger/OpenAPI Requirements

- **Version**: OpenAPI 2.0 only (3.0 not supported)
- **Host URL**: Production URL required (no staging/dev/test)
- **Security**: HTTPS with TLS 1.2+ required

### intro.md File Requirements

Must include:

- Connector features and functionality
- **Known Issues and Limitations** section
- Instructions for obtaining credentials (if OAuth)
- Steps to create app in third-party service

Example template: [Azure Key Vault Readme.md](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/custom-connectors/AzureKeyVault/Readme.md)

---

## Submission Process

### Step 1: Create Proposal PR

1. Fork [microsoft/PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors)
2. Create PR titled: `Proposal - ConnectorName` (e.g., `Proposal - Apollo`)
3. Commit `intro.md` file with connector details
4. Share verified credentials (or get auto-link if not set up)

### Step 2: Build Connector

Follow [Create a custom connector from scratch](https://learn.microsoft.com/en-us/connectors/custom-connectors/define-blank)

### Step 3: Submit Artifacts

1. Add all connector artifacts to your proposal PR
2. Add `package.zip` file
3. Fill out [PR template checklist](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/.github/pull_request_template.md)
4. Remove `Proposal -` from PR title
5. Certification team adds `certify-connector` comment to start process

### Step 4: Certification

- If package is valid → Success message, deployment begins
- If errors → Details in PR comments, fix and resubmit
- **Same PR preferred** for fixes (can create fresh PR if needed)

---

## Certification Workflow

### Timeline

| Phase | Duration |
|-------|----------|
| Identity validation (OneVet) | 30 days max |
| Certification review | Varies by submission quality |
| Deployment | ~15 business days |
| Production schedule | Fridays, PST/PDT |

### Deployment Process

1. Certification team completes code review
2. Initiates deployments
3. PR updated and merged when complete
4. Connector deployed incrementally to regions worldwide

### Key Policies

- **Folder name locked** - Cannot change connector folder name after added
- **Same publisher required** - Different publisher PRs are rejected
- **Lost account?** - Contact certification team for verification and unblocking

---

## Policy Error Codes

### Package Structure Errors (5000.1.x.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.1.1.1 | Icon validation failure | Include icon in package |
| 5000.1.1.2 | Missing readme file | Add intro.md/readme file |
| 5000.1.1.3 | Missing apiDefinition.json | Include apiDefinition.json |
| 5000.1.1.5 | Missing files | Need: CRM package, intro.md, customProperties.json (if multi-auth) |
| 5000.1.1.6 | Missing flows | Include flow + connector solutions |
| 5000.1.1.11 | Incorrect file placement | Package.zip missing from root (extra folder added) |
| 5000.1.1.13 | Missing solution zip | Add solution zip files |

### Icon Errors (5000.2.1.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.2.1.1 | Invalid brand color | Use valid hex, not #ffffff or #007ee5 |
| 5000.2.1.2 | Icon too large | Keep under 1 MB |
| 5000.2.1.3 | Wrong format | Use PNG as icon.png |
| 5000.2.1.4 | Wrong dimensions | 100x100 to 230x230 pixels, square |

### Title Errors (5000.2.2.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.2.2.1 | Too long | Max 30 characters |
| 5000.2.2.2 | Reserved words | Remove API, Connector, Power Apps, etc. |
| 5000.2.2.3 | Bad ending | End with alphanumeric |
| 5000.2.2.5 | Not unique | Change to unique name |

### Description Errors (5000.2.3.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.2.3.1 | Not English | Write in English |
| 5000.2.3.2 | Grammar errors | Fix spelling/grammar |
| 5000.2.3.4 | Wrong length | 30-500 characters |
| 5000.2.3.5 | Restricted keywords | Remove product names |

### Operation Errors (5000.2.4.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.2.4.1 | Bad response schema | Use exact schema only |
| 5000.2.4.2 | Default response with schema | Don't use default responses |
| 5000.2.4.4 | Empty response schema | Add schema (unless dynamic) |
| 5000.2.4.5 | Empty operations | Remove or complete operations |

### Swagger Errors (5000.2.6.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.2.6.2 | Invalid swagger | Validate against OpenAPI 2.0 |
| 5000.2.6.3 | Wrong version | Use OpenAPI 2.0 only |
| 5000.2.6.4 | Bad host URL | Use production URL |

### Submission Errors (5000.3.x.x)

| Code | Error | Fix |
|------|-------|-----|
| 5000.3.1.2 | Package URI expired | SAS token must be valid 4+ days |
| 5000.3.1.3 | Invalid website URL | Provide valid URL |
| 5000.3.1.4 | Invalid support email | Provide valid email |
| 5000.3.1.5 | Invalid privacy policy | Provide valid link |
| 5000.3.1.6 | Security protocol | Use HTTPS with TLS 1.2+ |

---

## Testing Post-Certification

### Preview Environment

After certification, connector is deployed to preview region.

1. Sign in to [Partner Center](https://partner.microsoft.com/dashboard/home)
2. Select **MarketPlace offers** > **Microsoft 365 and Copilot Program**
3. Open your offer → **Product Overview**
4. Find **Store Link** under **Publisher Sign Off**

### Testing Requirements

- Create connections and flows
- Test **all triggers and actions**
- Verify OAuth ClientID and ClientSecret (if applicable)

### Timeline

- Preview environment expires in **2 business days**
- Must complete testing and confirm go-live
- If not done, offer is rejected and requires resubmission

### Post Go-Live

- Deployment to all public regions: **5-6 weeks**
- Connector released as **Premium tier** (cannot be changed)

---

## Updates and Breaking Changes

### Submitting Updates

1. Follow same process as initial certification
2. **Don't create new offer** - reopen existing offer in Partner Center
3. Deployment takes **15 business days** regardless of update size

### Breaking Change Types

| Change | Impact |
|--------|--------|
| AuthenticationMethodRemoved | **Not allowed** - cannot change auth type |
| ConnectionParametersRemoved | May break existing flows |
| InputFieldDeleted | Breaks flows using that field |
| InputFieldVisibilityChangedToInternal | Default may replace existing values |
| OperationDeleted | Breaks existing flows |
| OperationHidden | Breaks designer |
| OutputFieldDeleted | Breaks flows using that field |
| OutputFieldTypeChanged | May break flows |
| TierChangedToPremium | Free plan users can't use connector |

### Best Practices

- Detail all changes in intro.md
- Use [operational versioning](https://learn.microsoft.com/en-us/connectors/custom-connectors/operational-versioning) for breaking changes
- Notify customers of breaking changes through documentation

---

## Moving from Preview to GA

### Required Criteria

| Metric | Requirement |
|--------|-------------|
| Backend APIs | Must be in production |
| Availability | 99.95% |
| SLO | 99.5% |
| Success rate | >80% |
| Support | Match your backend service support model |

### Recommended Criteria

| Metric | Recommendation |
|--------|----------------|
| Weekly active users | 500+ over last 3 weeks |
| Monthly active connections | 50+ at any given time |

### Exception Path

If customer requires production connector to use it:

- Connector in preview for **6 months** AND
- Meets all required criteria with **≥50% success rate**

### Process

1. Contact Microsoft Team when criteria are met
2. Request removal of preview tag
3. Process takes approximately **1 month**

---

## Connector Integrity

### Success Rate Formula

```
Success Rate = ((100s + 200s + 300s - 400s - 500s) × (1/total)) × 100
```

*Throttling failures are excluded*

### Common Error Codes

| Code | Cause | Fix |
|------|-------|-----|
| 400 | Bad Request (invalid inputs) | Add tooltips, update documentation |
| 401 | Authentication failed | Check auth configuration |
| 403 | Rate limit exceeded | Handle throttling |
| 404 | Operation deprecated/missing | Remove or update operation |
| 429 | Too Many Requests | Implement rate limiting |
| 500 | Internal Server Error | Check API/server |
| 502 | Server Side Error | Check hosting web server |
| 504 | Timeout | Optimize API performance |

### Improving Integrity

- Update **Known Issues and Limitations** in intro.md
- Add **tooltips** to swagger for user guidance
- Ensure API and hosting server are stable
- Use correct HTTP/HTTPS requests with appropriate responses

---

## FAQ and Limits

### Connector Limits

| Platform | Custom Connectors Limit |
|----------|------------------------|
| Azure Logic Apps | 1,000 per subscription |
| Power Platform (Free) | 1 |
| Power Platform (Office 365/Dynamics) | 1 |
| Power Platform (Per User) | 50 |

### Request Limits

| Platform | Requests per Minute |
|----------|---------------------|
| Logic Apps | 500 per connection |
| Power Platform | 10,000 per connection |

### Other Limits

- OpenAPI file: **<1 MB**
- Max schema count per body: **512**
- Max operations per swagger: **256**
- Max schemas per operation: **16,384**
- Max request content-length: **3,182,218 bytes**

### Supported Authentication

- OAuth 2.0 (for specific services)
- Generic OAuth 2.0
- Basic authentication
- API Key (except on-premises gateway)

**Note**: Client credentials grant type is **not supported**

### Supported Triggers

- Webhook-based triggers
- Polling triggers

---

## Key Contacts and Resources

### Email Contacts

| Purpose | Email |
|---------|-------|
| Certification Team | connectorpartnermgmtteam@microsoft.com |
| Development Help | condevhelp@microsoft.com |

### Office Hours

**When**: Every Tuesday, 3:30 PM - 4:30 PM UTC
**Platform**: Microsoft Teams
**Meeting**: [Office Hours Meeting](https://teams.microsoft.com/meet/245494443760?p=kjoTSmiPQ9wKzMjlWN)

### Key Links

| Resource | URL |
|----------|-----|
| GitHub Repository | https://github.com/microsoft/PowerPlatformConnectors |
| IP Connectors Folder | https://github.com/microsoft/PowerPlatformConnectors/tree/dev/independent-publisher-connectors |
| Connector Reference | https://learn.microsoft.com/en-us/connectors/connector-reference/ |
| IP Certification Process | https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip |
| Package Validator | https://github.com/microsoft/PowerPlatformConnectors/blob/dev/scripts/ConnectorPackageValidator.ps1 |
| PR Template | https://github.com/microsoft/PowerPlatformConnectors/blob/dev/.github/pull_request_template.md |
| Manifesto | https://github.com/microsoft/PowerPlatformConnectors/wiki/Independent-Publisher-Connector-Group-%22Manifesto%22 |

### Community Resources

- [Power Automate Forum - Connector Development](https://powerusers.microsoft.com/t5/Connector-Development/bd-p/ConnectorDevelopment)
- [Power Apps Community Plan](https://powerapps.microsoft.com/communityplan/) (free for development)

---

## Checklist Before Submitting

### Connector Files

- [ ] Title ≤30 characters, follows naming pattern, no forbidden words
- [ ] Description 30-500 characters, English, no product names
- [ ] All operations have summary, description, visibility
- [ ] All operations have exact response schemas
- [ ] Swagger validates against OpenAPI 2.0
- [ ] Production host URL (no staging/dev/test)
- [ ] iconBrandColor set to `#da3b01`

### Package

- [ ] Package validated with ConnectorPackageValidator.ps1
- [ ] Connector solution included
- [ ] Flow solution with test flow included
- [ ] intro.md with Known Issues section
- [ ] SAS URL valid for 15+ days

### Testing

- [ ] All operations tested (minimum 10 successful calls each)
- [ ] No runtime or schema validation errors
- [ ] Credentials work in production environment

### PR Submission

- [ ] Verified credentials ready
- [ ] PR checklist completed
- [ ] Support email matches GitHub account email
- [ ] Privacy policy URL provided
- [ ] Detailed operation descriptions

---

*Document generated January 2026. Always verify against current Microsoft documentation for the latest requirements.*
