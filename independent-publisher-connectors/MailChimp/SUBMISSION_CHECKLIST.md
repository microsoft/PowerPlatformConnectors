# Independent Publisher Connector Submission Checklist

## Files Prepared

- [x] `apiDefinition.swagger.json` - Swagger definition with x-ms-summary on all params
- [x] `apiProperties.json` - Connection parameters and policy templates
- [x] `readme.md` - Documentation with API key instructions

## Pre-Submission Steps

### 1. Update Contact Information
In both files, replace placeholder email:
- `apiDefinition.swagger.json` → `info.contact.email`
- `apiProperties.json` → `publisher`
- `readme.md` → Support section

### 2. Verify Connector Locally
```bash
# Install Power Platform CLI
npm install -g paconn

# Login
paconn login

# Validate swagger
paconn validate --api-def apiDefinition.swagger.json
```

### 3. Test All Operations
Take screenshots of successful tests for these key operations:
- GetLists
- GetListMembers
- AddListMember
- UpdateMemberTags
- GetCampaigns
- GetCampaignReport
- GetCampaignOpenDetails
- GetCampaignClickDetails
- GetClickLinkMembers
- GetCampaignUnsubscribes

### 4. Fork and Clone Repository
```bash
git clone https://github.com/microsoft/PowerPlatformConnectors.git
cd PowerPlatformConnectors
git checkout dev
git checkout -b mailchimp-marketing-ip
```

### 5. Add Connector Files
```
PowerPlatformConnectors/
└── independent-publisher-connectors/
    └── MailchimpMarketing/
        ├── apiDefinition.swagger.json
        ├── apiProperties.json
        └── readme.md
```

### 6. Create Pull Request
- Title: `Mailchimp Marketing (Independent Publisher)`
- Include test screenshots
- Reference the manifesto agreement

## Key Requirements Met

| Requirement | Status |
|-------------|--------|
| Title includes "(Independent Publisher)" | ✅ |
| iconBrandColor = #da3b01 | ✅ |
| x-ms-summary on all parameters | ✅ |
| x-ms-url-encoding on path parameters | ✅ |
| Response schemas defined | ✅ |
| OAuth/API key instructions in readme | ✅ |
| Contact email provided | ⚠️ Update |
| Privacy policy URL | ✅ (Intuit) |
| Operations descriptions | ✅ |

## Differentiation from Official Connector

Your connector includes operations NOT in the official Mailchimp connector:

**Reports & Analytics (Official has NONE)**
- GetCampaignReport
- GetCampaignOpenDetails  
- GetCampaignClickDetails
- GetClickLinkMembers
- GetCampaignUnsubscribes
- GetCampaignRecipients
- GetEmailActivity

**Automations (Official has NONE)**
- GetAutomations
- GetAutomation
- GetAutomationEmails
- GetAutomationEmail

**Member Activity (Official has NONE)**
- GetMemberActivity
- GetMemberActivityFeed
- UpdateMemberTags

**Campaign Management (Official limited)**
- GetCampaigns (with filters)
- GetCampaign
- GetCampaignContent
- SetCampaignContent
- SendCampaign
- ScheduleCampaign
- SendTestEmail

**Batch Operations (Official has NONE)**
- CreateBatch
- GetBatch
- GetBatches

**Templates (Official has NONE)**
- GetTemplates

## Expected Timeline

- PR Review: 5-10 business days
- Validation & Feedback: 1-2 weeks
- Deployment to Preview: After approval
- Preview Testing: 2 days
- Production Deployment: ~15 business days after preview approval

## Resources

- [Independent Publisher Certification Process](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip)
- [Connector Preparation Best Practices](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission)
- [GitHub Repository](https://github.com/microsoft/PowerPlatformConnectors)
- [Independent Publisher Manifesto](https://github.com/microsoft/PowerPlatformConnectors/wiki/Independent-Publisher-Connector-Group-Manifesto)
