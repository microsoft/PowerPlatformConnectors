# Updating an Existing PR

## Microsoft's Preference

**✅ DO:** Update the same PR with fixes  
**❌ DON'T:** Create a new PR for the same connector

Microsoft's certification team prefers you keep the same PR and push updates to it. This maintains the conversation history and review context.

---

## How to Update Your Apollo PR

### Step 1: Find Your PR Branch

Check which branch you used for the original PR:

```powershell
cd f:/projects/Connectors
git branch -a
```

Look for your Apollo branch (likely `apollo-enrichment-connector`)

### Step 2: Checkout the PR Branch

```powershell
git checkout apollo-enrichment-connector
```

### Step 3: Update Files

Copy the updated files from master:

```powershell
# Copy updated files from master
git checkout master -- independent-publisher-connectors/Apollo/apiDefinition.swagger.json
git checkout master -- independent-publisher-connectors/Apollo/apiProperties.json
git checkout master -- independent-publisher-connectors/Apollo/readme.md
```

Or make changes directly on the branch.

### Step 4: Commit Changes

```powershell
git add independent-publisher-connectors/Apollo/
git commit -m "update Apollo connector files"
```

### Step 5: Push to Your Fork

```powershell
# If you haven't set up your fork as origin yet
git remote add origin https://github.com/forceworks/PowerPlatformConnectors.git

# Push the updated branch
git push origin apollo-enrichment-connector --force
```

### Step 6: PR Automatically Updates

The PR on GitHub will automatically update with your new commits. No need to create a new PR!

---

## What to Update in Your Apollo PR

Based on the changes we made:

### Files to Update

1. **apiDefinition.swagger.json**
   - ✅ Title: "Apollo Enrichment (Independent Publisher)"
   - ✅ Contact: Steve Mordue, steve@forceworks.com
   - ✅ Website: https://forceworks.com

2. **apiProperties.json**
   - ✅ Created (if missing)
   - ✅ Publisher: Steve Mordue
   - ✅ iconBrandColor: #da3b01

3. **readme.md**
   - ✅ Publisher section updated
   - ✅ Known Issues section (if missing)

### Package Updates

If you need to update the package:

1. **Create new package.zip** with updated connector
2. **Upload to Azure Blob Storage**
3. **Generate new SAS URL** (valid 15+ days)
4. **Update PR description** with new SAS URL

---

## Responding to Certification Team Comments

If the certification team left comments on your PR:

### Step 1: Read All Comments

Review every comment from the certification team carefully.

### Step 2: Fix Issues

Make the requested changes to your files.

### Step 3: Commit and Push

```powershell
git add .
git commit -m "address certification feedback"
git push origin apollo-enrichment-connector
```

### Step 4: Reply to Comments

On GitHub, reply to each comment:
- "Fixed in commit [commit-hash]"
- Explain what you changed
- Ask for clarification if needed

### Step 5: Request Re-Review

Comment on the PR:
```
@[certification-team-member] - All feedback addressed. Ready for re-review.
```

---

## When to Create a Fresh PR

Only create a new PR if:

1. **Certification team explicitly asks** you to
2. **PR was closed/rejected** and you're starting over
3. **You're submitting a different connector** (not Apollo)

For updates to the same connector, **always use the same PR**.

---

## Common Update Scenarios

### Scenario 1: Missing Files

**Certification Team:** "Missing apiProperties.json"

**Fix:**
```powershell
git checkout apollo-enrichment-connector
# Create apiProperties.json
git add independent-publisher-connectors/Apollo/apiProperties.json
git commit -m "add apiProperties.json"
git push origin apollo-enrichment-connector
```

### Scenario 2: Title Issues

**Certification Team:** "Title must include (Independent Publisher)"

**Fix:**
```powershell
git checkout apollo-enrichment-connector
# Update apiDefinition.swagger.json title
git add independent-publisher-connectors/Apollo/apiDefinition.swagger.json
git commit -m "update title"
git push origin apollo-enrichment-connector
```

### Scenario 3: Package Issues

**Certification Team:** "Package validation failed"

**Fix:**
1. Create new package.zip
2. Validate with ConnectorPackageValidator.ps1
3. Upload to Azure
4. Generate new SAS URL
5. Update PR description with new URL
6. Comment: "Updated package with fixes. New SAS URL in description."

---

## Timeline for Updates

| Action | Timeline |
|--------|----------|
| Push updates to PR | Immediate |
| Certification team sees updates | Within 1-2 business days |
| Re-review | Varies (usually 3-5 business days) |
| Response deadline | 30 days from last comment |

**Important:** If you don't respond within 30 days, the PR may be closed.

---

## Best Practices

### DO:
- ✅ Keep the same PR
- ✅ Respond to all comments
- ✅ Test changes before pushing
- ✅ Update package SAS URL if needed
- ✅ Be responsive (reply within a few days)

### DON'T:
- ❌ Create new PR for same connector
- ❌ Force push without explanation
- ❌ Ignore certification team comments
- ❌ Let PR sit for weeks without response
- ❌ Submit incomplete fixes

---

## Checking Your Existing Apollo PR

### Find Your PR

1. Go to https://github.com/microsoft/PowerPlatformConnectors/pulls
2. Search for "Apollo" or your GitHub username
3. Check PR status:
   - **Open** - Still under review
   - **Closed** - Rejected or withdrawn
   - **Merged** - Accepted and deployed!

### PR States

**If PR is Open:**
- Update the same PR with your changes
- Respond to any comments
- Wait for certification team

**If PR is Closed (Not Merged):**
- Check why it was closed
- If you can fix issues, create fresh PR
- Reference old PR number in new PR

**If PR is Merged:**
- Connector is certified!
- For updates, create new PR with "Update" in title
- Reference original PR

---

## Summary

**For your Apollo connector:**

1. Check if PR exists and is open
2. If open → Update the same PR
3. If closed → Can create fresh PR
4. If merged → Create update PR

**Never create duplicate PRs for the same connector submission.**

---

## Next Steps for Apollo

1. Check your existing PR status
2. If open, update it with the new files
3. If closed, create fresh PR with updated files
4. Include updated package with new SAS URL
