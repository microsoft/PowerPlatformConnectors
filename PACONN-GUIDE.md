# paconn CLI Guide for Connector Development

## What is paconn?

`paconn` (Power Platform Connectors CLI) is Microsoft's command-line tool for creating, validating, and managing custom connectors. It's useful but **not required** for Independent Publisher connector submissions.

## Do You Need paconn?

### ✅ STRONGLY RECOMMENDED For:
- **Validating connector files** before submission (`paconn validate`)
- Catching issues early before Microsoft reviews
- Ensuring swagger and properties files are compatible

### ✅ Also Useful For:
- Downloading connectors from environments
- Uploading connectors to environments
- Converting between formats
- Batch validation of multiple connectors

### ⚠️ NOT Required For:
- Submitting to Microsoft (use GitHub PR)
- Creating package.zip (manual process)
- Generating SAS URLs (use Azure)

## Installation

### Prerequisites
- Python 3.5+ installed
- pip (Python package manager)

### Install paconn

```powershell
pip install paconn
```

Or from the PowerPlatformConnectors repo:

```powershell
cd f:/projects/Connectors/PowerPlatformConnectors/tools/paconn-cli
pip install -e .
```

## Common paconn Commands

### 1. Validate Connector Files

```powershell
paconn validate `
  --api-def f:/projects/Connectors/independent-publisher-connectors/MailChimp/apiDefinition.swagger.json `
  --api-prop f:/projects/Connectors/independent-publisher-connectors/MailChimp/apiProperties.json
```

**What it checks:**
- Swagger syntax
- Required fields present
- Policy templates valid
- File structure correct

### 2. Create New Connector

```powershell
paconn create `
  --api-def ./apiDefinition.swagger.json `
  --api-prop ./apiProperties.json `
  --icon ./icon.png `
  --environment [environment-id]
```

### 3. Update Existing Connector

```powershell
paconn update `
  --api-def ./apiDefinition.swagger.json `
  --api-prop ./apiProperties.json `
  --connector-id [connector-id] `
  --environment [environment-id]
```

### 4. Download Connector

```powershell
paconn download `
  --connector-id [connector-id] `
  --environment [environment-id] `
  --destination ./downloaded-connector
```

### 5. Login to Power Platform

```powershell
paconn login
```

This opens browser for authentication.

## For Independent Publisher Workflow

### Recommended Use

**1. Local Validation (Optional)**

```powershell
# Validate MailChimp connector
paconn validate `
  --api-def f:/projects/Connectors/independent-publisher-connectors/MailChimp/apiDefinition.swagger.json `
  --api-prop f:/projects/Connectors/independent-publisher-connectors/MailChimp/apiProperties.json
```

**2. Upload to Test Environment (Optional)**

```powershell
# Login first
paconn login

# Create connector in your environment
paconn create `
  --api-def f:/projects/Connectors/independent-publisher-connectors/MailChimp/apiDefinition.swagger.json `
  --api-prop f:/projects/Connectors/independent-publisher-connectors/MailChimp/apiProperties.json `
  --icon f:/projects/Connectors/independent-publisher-connectors/MailChimp/icon.png `
  --environment [your-environment-id]
```

## Alternative: Use Power Automate Portal (Easier)

Instead of paconn, you can use the web portal:

### Create Connector via Portal

1. Go to [Power Automate](https://make.powerautomate.com)
2. **Data** → **Custom connectors**
3. **+ New custom connector** → **Import an OpenAPI file**
4. Upload your `apiDefinition.swagger.json`
5. Test and create connection

### Why Portal is Better for Independent Publishers

- ✅ No CLI installation needed
- ✅ Visual interface for testing
- ✅ Easier to create test flows
- ✅ Simpler to export solutions
- ✅ Built-in connection testing

## When to Use paconn

### Good Use Cases

**1. Batch Validation**
Validate multiple connectors at once:

```powershell
foreach ($connector in Get-ChildItem -Path "./independent-publisher-connectors" -Directory) {
    Write-Host "Validating $($connector.Name)..."
    paconn validate `
        --api-def "$($connector.FullName)/apiDefinition.swagger.json" `
        --api-prop "$($connector.FullName)/apiProperties.json"
}
```

**2. CI/CD Pipeline**
Automate validation in build pipeline

**3. Downloading Existing Connectors**
Download connectors from environments for backup/modification

**4. Environment Management**
Manage connectors across multiple environments

### Not Useful For

- ❌ Creating package.zip (manual process)
- ❌ Generating SAS URLs (use Azure)
- ❌ Submitting to Microsoft (use GitHub)
- ❌ OneVet verification (web process)

## Validation: paconn vs Online Tools

### paconn validate
```powershell
paconn validate --api-def ./apiDefinition.swagger.json --api-prop ./apiProperties.json
```

**Checks:**
- File structure
- Required properties
- Policy templates
- Basic swagger syntax

### Online Swagger Validators

**Swagger Editor:** https://editor.swagger.io/
- Paste your swagger JSON
- See real-time validation
- More detailed error messages

**Swagger Validator:** https://validator.swagger.io/
- Upload or paste swagger
- Validates against OpenAPI 2.0 spec

## Recommended Workflow

### For Independent Publishers

1. **Edit files** in VS Code
2. **Validate with paconn** (strongly recommended):
   ```powershell
   paconn validate --api-def ./apiDefinition.swagger.json --api-prop ./apiProperties.json
   ```
3. **Validate swagger** at https://editor.swagger.io/ (double-check)
4. **Import to Power Automate** via portal
5. **Test operations** in Power Automate
6. **Create test flow** in Power Automate
7. **Export solutions** from portal
8. **Create package** manually
9. **Validate package** with ConnectorPackageValidator.ps1 (required)
10. **Upload to Azure** via portal
11. **Submit PR** on GitHub

**Two validators:**
- `paconn validate` - Validates connector files (swagger + properties)
- `ConnectorPackageValidator.ps1` - Validates package.zip structure (required)

## Summary

**paconn is:**
- Strongly recommended for validation
- Useful for automation/batch operations
- Optional but helpful CLI tool

**Two Validators You Need:**

1. **paconn validate** (strongly recommended)
   - Validates connector files (swagger + properties)
   - Catches issues before submission
   - Run before creating package

2. **ConnectorPackageValidator.ps1** (required)
   - Validates package.zip structure
   - Required before PR submission
   - Ensures package meets Microsoft requirements

**For your connectors:**
- ✅ Use `paconn validate` before creating package
- ✅ Use Power Automate portal for testing
- ✅ Use ConnectorPackageValidator.ps1 for package
- ✅ Use Azure portal for package upload
- ✅ Use GitHub for PR submission

**Bottom line:** Install paconn for validation, use portal for everything else.

---

## paconn Resources

- [paconn Documentation](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli)
- [paconn GitHub](https://github.com/microsoft/PowerPlatformConnectors/tree/dev/tools/paconn-cli)
- [Installation Guide](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/tools/paconn-cli/README.md)
