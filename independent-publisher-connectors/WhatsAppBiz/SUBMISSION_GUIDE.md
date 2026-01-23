# WhatsApp Business Power Platform Connector - Submission Guide

This guide walks you through the complete process of submitting your WhatsApp Business connector as an Independent Publisher to Microsoft Power Platform.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Files Overview](#files-overview)
3. [Testing Your Connector](#testing-your-connector)
4. [Submission Process](#submission-process)
5. [Certification Process](#certification-process)
6. [Post-Certification](#post-certification)

---

## Prerequisites

### Required Accounts

1. **GitHub Account** - For submitting the connector
2. **Microsoft Authenticator App** - For verified credentials
3. **Meta Developer Account** - For testing the WhatsApp API
4. **Power Platform Environment** - For testing (free developer environment works)

### Required Knowledge

- Basic understanding of REST APIs
- Familiarity with OpenAPI/Swagger specification
- Experience with Power Automate or Power Apps

---

## Files Overview

Your connector submission requires these files:

```
WhatsAppBusiness/
├── apiDefinition.swagger.json  # OpenAPI specification
├── apiProperties.json          # Connector properties (auth, branding)
├── readme.md                   # Documentation for users
└── intro.md                    # Initial proposal (if sharing early)
```

### File Descriptions

| File | Purpose |
|------|---------|
| `apiDefinition.swagger.json` | Defines all API operations, parameters, responses |
| `apiProperties.json` | Connection parameters, icon color (`#da3b01` for IP), policies |
| `readme.md` | How to get credentials, supported operations, limitations |
| `intro.md` | For early proposals to find collaborators |

---

## Testing Your Connector

### Step 1: Import as Custom Connector

1. Go to [make.powerautomate.com](https://make.powerautomate.com)
2. Navigate to **Data** > **Custom connectors**
3. Click **+ New custom connector** > **Import an OpenAPI file**
4. Upload your `apiDefinition.swagger.json`
5. Review and configure settings

### Step 2: Configure Authentication

1. In the custom connector editor, go to **Security**
2. Set Authentication type to **API Key**
3. Parameter label: `Access Token`
4. Parameter name: `Authorization`
5. Parameter location: `Header`

### Step 3: Create a Connection

1. Click **Create connector**
2. Go to **Test** tab
3. Click **+ New connection**
4. Enter your WhatsApp access token in format: `Bearer YOUR_TOKEN_HERE`

### Step 4: Test Operations

Test at least 3 unique operations. Recommended tests:

1. **Send Text Message** (within 24-hour window to a test number)
2. **Send Template Message** (use `hello_world` template)
3. **Get Message Templates** (list your WABA templates)

### Step 5: Capture Screenshots

For your PR, you need:
- Screenshot of successful test for each operation
- Screenshot showing the flow running successfully
- At least 3 operations demonstrated

---

## Submission Process

### Phase 1: Get Verified Credentials

1. **First Time Only** - When you submit your first PR, Microsoft will email you
2. Fill out the verification form they send
3. Complete identity verification via AU10TIX
4. Set up credentials in Microsoft Authenticator
5. This takes 1-5 business days

### Phase 2: Fork the Repository

```bash
# Fork microsoft/PowerPlatformConnectors on GitHub

# Clone your fork
git clone https://github.com/YOUR-USERNAME/PowerPlatformConnectors.git

# Add upstream remote
cd PowerPlatformConnectors
git remote add upstream https://github.com/microsoft/PowerPlatformConnectors.git

# Sync with upstream
git fetch upstream
git checkout dev
git merge upstream/dev
```

### Phase 3: Add Your Connector

```bash
# Create a new branch
git checkout -b whatsapp-business-connector

# Create your connector folder
mkdir -p independent-publisher-connectors/WhatsAppBusiness

# Copy your files
cp /path/to/your/files/* independent-publisher-connectors/WhatsAppBusiness/
```

### Phase 4: Validate Your Files

Before submitting, validate your swagger:

1. Use [Swagger Editor](https://editor.swagger.io/) - paste your JSON
2. Fix any errors or warnings
3. Ensure all required fields are present

**Key Requirements:**
- `x-ms-connector-metadata` must include Website, Privacy policy, Categories
- `iconBrandColor` must be `#da3b01` for IP connectors
- All operations need `operationId`, `summary`, `description`
- Response schemas should be defined (not dynamic unless necessary)

### Phase 5: Submit Pull Request

1. Commit your changes:
```bash
git add .
git commit -m "Add WhatsApp Business connector"
git push origin whatsapp-business-connector
```

2. Create Pull Request on GitHub:
   - Target: `microsoft/PowerPlatformConnectors` branch `dev`
   - Title: `WhatsApp Business (Independent Publisher)`

3. In the PR description, include:
   - Brief description of the connector
   - Screenshots of tested operations
   - Confirmation that you've tested the connector
   - Links to 3 operations working in flows

4. Add the label: `independent-publisher-connector`

---

## Certification Process

### What Happens After You Submit

1. **Automated Checks** (immediate)
   - Swagger validation
   - Breaking change detection
   - File structure validation

2. **Manual Review** (1-5 business days)
   - Microsoft team reviews your submission
   - May request changes or clarifications

3. **Certification** (after approval)
   - Team adds `certify-connector` comment
   - Connector goes through final validation
   - Deployed to Power Platform

### Common Review Feedback

| Issue | Solution |
|-------|----------|
| Missing response schemas | Add schema definitions for all responses |
| Unclear operation descriptions | Improve summaries and descriptions |
| Missing x-ms-summary | Add user-friendly labels to all parameters |
| Authentication issues | Verify policy template is correct |
| Swagger validation errors | Fix OpenAPI spec issues |

### Timeline

- **Initial Review**: 3-7 business days
- **Revisions** (if needed): Varies
- **Certification to Deployment**: 2-4 weeks
- **Total**: ~2-6 weeks typical

---

## Post-Certification

### Maintaining Your Connector

As the publisher, you're responsible for:

1. **Bug Fixes** - Address issues reported on GitHub
2. **Updates** - Keep up with API changes
3. **Support** - Respond to community questions

### Updating Your Connector

1. Make changes in a new branch
2. Submit PR to `dev` branch
3. Same review process applies
4. Breaking changes require special handling

### Your Connector in the Wild

Once published:
- Available in Power Automate, Power Apps, Logic Apps
- Listed in Microsoft documentation
- Your name appears as the publisher
- Users can submit issues on GitHub

---

## Resources

### Official Documentation

- [Independent Publisher Certification Process](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip)
- [Connector Certification Guidelines](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission)
- [Power Platform Connectors GitHub](https://github.com/microsoft/PowerPlatformConnectors)
- [IP Connector Manifesto](https://github.com/microsoft/PowerPlatformConnectors/wiki/Independent-Publisher-Connector-Group-%22Manifesto%22)

### WhatsApp API Documentation

- [Cloud API Overview](https://developers.facebook.com/docs/whatsapp/cloud-api)
- [Messages API Reference](https://developers.facebook.com/docs/whatsapp/cloud-api/reference/messages)
- [Message Templates](https://developers.facebook.com/docs/whatsapp/message-templates)

### Tools

- [Swagger Editor](https://editor.swagger.io/)
- [Postman WhatsApp Collection](https://www.postman.com/meta/whatsapp-business-platform/)
- [Power Platform CLI (paconn)](https://github.com/microsoft/PowerPlatformConnectors/tree/dev/tools/paconn-cli)

---

## Checklist Before Submission

- [ ] Swagger validates without errors
- [ ] All operations have `operationId`, `summary`, `description`
- [ ] All parameters have `x-ms-summary`
- [ ] Response schemas are defined
- [ ] `iconBrandColor` is set to `#da3b01`
- [ ] `x-ms-connector-metadata` includes Website, Privacy policy, Categories
- [ ] readme.md includes credential instructions
- [ ] readme.md includes supported operations table
- [ ] readme.md includes known issues/limitations
- [ ] Tested at least 3 operations successfully
- [ ] Have screenshots ready for PR
- [ ] No sensitive data in files (no real tokens/secrets)

---

Good luck with your submission! 🚀
