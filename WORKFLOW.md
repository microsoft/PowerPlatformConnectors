# Connector Development Workflow

## Daily Development

### Work on a Specific Connector

```powershell
cd f:/projects/Connectors
git checkout quickbooks-online-forceworks
# Make changes to independent-publisher-connectors/QuickBooks/
git add independent-publisher-connectors/QuickBooks/
git commit -m "update QuickBooks connector"
```

### Switch Between Connectors

```powershell
git checkout mailchimp-marketing-ip
# Work on MailChimp

git checkout apollo-enrichment-connector
# Work on Apollo

git checkout exchangerate
# Work on ExchangeRate
```

### Keep All Connectors in Sync on Master

```powershell
git checkout master
git merge quickbooks-online-forceworks
git merge mailchimp-marketing-ip
# Merge other branches as needed
```

## Preparing for Pull Request

### 1. Fetch Latest from Microsoft's Repository

```powershell
git fetch upstream
git checkout quickbooks-online-forceworks
git rebase upstream/dev
```

### 2. Push to Your GitHub Fork

First, add your fork as origin:
```powershell
git remote add origin https://github.com/YOUR-USERNAME/PowerPlatformConnectors.git
```

Then push the branch:
```powershell
git push origin quickbooks-online-forceworks
```

### 3. Create Pull Request on GitHub

1. Go to your fork on GitHub
2. Click "Compare & pull request" for the branch you pushed
3. Set base repository: `microsoft/PowerPlatformConnectors`
4. Set base branch: `dev`
5. Set compare branch: your branch (e.g., `quickbooks-online-forceworks`)
6. Fill in PR description following the template
7. Submit the pull request

## Branch Structure

- **master** - Contains all connectors for local development
- **apollo-enrichment-connector** - Apollo connector only
- **exchangerate** - ExchangeRate connector only
- **kit** - Kit connector only
- **mailchimp-marketing-ip** - MailChimp connector only
- **partnercenter** - Partner Center connector only
- **quickbooks-online-forceworks** - QuickBooks connector only
- **whatsappbiz** - WhatsApp Business connector only

## Recommended Approach

**Option 1: Work on Master (Simpler)**
- Do all development on `master` branch
- When ready for PR, create a clean branch with only that connector
- Cherry-pick or copy files to the PR branch

**Option 2: Work on Feature Branches (Cleaner PRs)**
- Work directly on connector-specific branches
- Merge to master periodically to keep everything in sync
- Branch is already clean for PR submission

## Testing Before PR

1. Validate connector files:
```powershell
cd f:/projects/Connectors/PowerPlatformConnectors
python tools/paconn-cli/paconn/paconn.py validate --api-def ../independent-publisher-connectors/QuickBooks/apiDefinition.swagger.json --api-prop ../independent-publisher-connectors/QuickBooks/apiProperties.json
```

2. Test in Power Platform:
   - Import connector to test environment
   - Create test connections
   - Verify all operations work as expected

3. Review checklist:
   - [ ] apiDefinition.swagger.json is valid
   - [ ] apiProperties.json is complete
   - [ ] readme.md follows template
   - [ ] icon.png is included (112x112 or 160x160)
   - [ ] Screenshots included
   - [ ] All operations tested
