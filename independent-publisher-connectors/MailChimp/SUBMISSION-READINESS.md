# MailChimp Marketing Connector - Submission Readiness

**Connector:** Mailchimp Marketing (Independent Publisher)  
**Publisher:** Steve Mordue  
**Status:** Files compliant, package/testing pending

---

## ✅ Completed

### Files
- [x] [`apiDefinition.swagger.json`](./apiDefinition.swagger.json) - Compliant
- [x] [`apiProperties.json`](./apiProperties.json) - Compliant
- [x] [`readme.md`](./readme.md) - Compliant with Known Issues section
- [x] [`icon.png`](./icon.png) - Present

### Compliance
- [x] Title: "Mailchimp Marketing (Independent Publisher)"
- [x] Publisher: Steve Mordue
- [x] Contact: steve@forceworks.com
- [x] Website: https://forceworks.com
- [x] Privacy Policy: https://www.intuit.com/privacy/statement/
- [x] iconBrandColor: #da3b01
- [x] All operations have summaries and descriptions
- [x] Known Issues section in readme

---

## ⚠️ Pending - Before Submission

### 1. Testing (Required)

**Test these core operations** (10+ successful calls each):

**Audiences:**
- [ ] GetLists
- [ ] GetListMembers
- [ ] AddListMember
- [ ] UpsertListMember
- [ ] UpdateMemberTags

**Campaigns:**
- [ ] GetCampaigns
- [ ] CreateCampaign
- [ ] SetCampaignContent
- [ ] SendTestEmail

**Reports:**
- [ ] GetCampaignReport
- [ ] GetCampaignOpenDetails
- [ ] GetCampaignClickDetails

**Spot check** (3-5 calls each):
- [ ] GetSegments
- [ ] GetListTags
- [ ] GetAutomations
- [ ] GetBatches
- [ ] GetTemplates

### 2. Create Test Flow (Required)

Create a Power Automate flow that demonstrates the connector:

**Example Flow:**
```
Trigger: Manual trigger
↓
Action: Get all audiences (MailChimp)
↓
Action: Get audience members (MailChimp)
  - List ID: From previous step
  - Count: 10
↓
Action: Compose
  - Inputs: Member count and first member email
```

**Requirements:**
- [ ] Flow created in Power Automate
- [ ] Flow uses MailChimp connector
- [ ] Flow runs successfully (10+ times)
- [ ] Flow added to a solution
- [ ] Solution exported as FlowSolution.zip

### 3. Create Connector Solution (Required)

- [ ] Connector imported to Power Platform
- [ ] Connector tested and working
- [ ] Connector added to a solution
- [ ] Solution Checker run (no critical/high issues)
- [ ] Solution exported as ConnectorSolution.zip

### 4. Create Package (Required)

**Package Structure:**
```
package.zip
├── ConnectorSolution.zip
├── FlowSolution.zip
└── intro.md
```

**Steps:**
- [ ] Create intro.md (can use readme.md content)
- [ ] Create package.zip with correct structure
- [ ] Validate with ConnectorPackageValidator.ps1
- [ ] Package validation passes

### 5. Upload to Azure (Required)

- [ ] Azure Storage Account created
- [ ] Container created (e.g., "connector-packages")
- [ ] package.zip uploaded
- [ ] SAS URL generated (Read permission, valid 15+ days)
- [ ] SAS URL tested (downloads successfully)

### 6. OneVet Verification (Required)

- [ ] Government ID ready (matches "Steve Mordue")
- [ ] GitHub account email is steve@forceworks.com
- [ ] Microsoft Authenticator app installed
- [ ] OneVet verification completed
- [ ] Verified credentials valid (not expired)

### 7. GitHub PR (Required)

- [ ] Fork microsoft/PowerPlatformConnectors
- [ ] Create branch from `dev`
- [ ] Add connector files to `independent-publisher-connectors/MailchimpMarketing/`
- [ ] Create PR with title: "Mailchimp Marketing (Independent Publisher)"
- [ ] Fill out PR template completely
- [ ] Include package SAS URL in PR description

---

## PR Submission Information

When creating the PR, provide:

| Field | Value |
|-------|-------|
| **Publisher Name** | Steve Mordue |
| **Publisher Email** | steve@forceworks.com |
| **Support Email** | steve@forceworks.com |
| **Website URL** | https://forceworks.com |
| **Privacy Policy URL** | https://www.intuit.com/privacy/statement/ |
| **Package SAS URL** | [Generate from Azure] |

---

## Testing Notes

### Authentication Testing

MailChimp uses **API Key + Datacenter** authentication:
- API Key format: `abc123def456-us21`
- Datacenter: `us21` (extracted from API key)
- Auth method: Bearer token in Authorization header
- Dynamic host: `{datacenter}.api.mailchimp.com`

**Test:**
- [ ] Connection creates successfully
- [ ] API key is validated
- [ ] Datacenter routing works
- [ ] Requests go to correct datacenter

### Operations to Prioritize

**Most Important** (test thoroughly):
1. GetLists - Core operation
2. AddListMember - Most common use case
3. UpsertListMember - Prevents duplicates
4. UpdateMemberTags - Key for segmentation
5. GetCampaignReport - Analytics use case

**Important** (test well):
6. CreateCampaign
7. SetCampaignContent
8. SendTestEmail
9. GetCampaignOpenDetails
10. GetCampaignClickDetails

**Nice to Have** (spot check):
- Automation operations
- Batch operations
- Template operations
- Segment operations

### Known Limitations to Document

Already documented in readme:
- ✅ Customer Journeys limited support
- ✅ Rate limits (10 requests/second)
- ✅ Pagination (max 1000 records)
- ✅ Email hash requirement
- ✅ No webhook triggers

---

## Estimated Timeline

| Task | Time Estimate |
|------|---------------|
| Testing operations | 2-4 hours |
| Create test flow | 30 minutes |
| Export solutions | 15 minutes |
| Create package | 15 minutes |
| Upload to Azure | 15 minutes |
| Create PR | 30 minutes |
| **Total** | **4-6 hours** |

Plus OneVet verification (15 minutes, one-time)

---

## After Submission

1. **Respond to certification team** within 30 days
2. **Fix any policy errors** they identify
3. **Test in preview environment** (2 business days window)
4. **Confirm go-live**
5. **Wait for production deployment** (5-6 weeks)

---

## Resources

- [WORKFLOW.md](../../../WORKFLOW.md) - Development workflow
- [PR-SUBMISSION-CHECKLIST.md](../../../PR-SUBMISSION-CHECKLIST.md) - Full checklist
- [MICROSOFT-TESTING-PROCESS.md](../../../MICROSOFT-TESTING-PROCESS.md) - What Microsoft tests
- [Package Validator](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/scripts/ConnectorPackageValidator.ps1)

---

**Next Step:** Start testing operations in Power Automate