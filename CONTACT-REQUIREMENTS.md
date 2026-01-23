# Contact Information Requirements for Independent Publisher Connectors

## Microsoft's Requirements

For Independent Publisher connector submissions, Microsoft requires **individual contact information**, not company information.

### What's Required

| Field | Requirement | Example |
|-------|-------------|---------|
| **Publisher Name** | Your personal name | "John Smith" |
| **Publisher Email** | Your personal email (must match GitHub account) | "john.smith@email.com" |
| **Support Email** | Your personal email (must match GitHub account) | "john.smith@email.com" |
| **GitHub Account** | Must match your government ID name | "johnsmith" |

### Why Individual Names?

1. **OneVet Verification**: Microsoft uses verified credentials tied to your government-issued ID
2. **Accountability**: Individual publishers are accountable for their connectors
3. **Same Publisher Rule**: Updates must come from the same verified individual
4. **Identity Verification**: Your GitHub profile must match your government ID

### ❌ NOT Allowed

- Company names as publisher
- Generic company emails
- Team/group names
- Aliases that don't match your ID

### ✅ Allowed

- Your personal name (as on government ID)
- Your personal email (linked to GitHub)
- Your verified GitHub account

## Contact Information in Connector Files

### In apiDefinition.swagger.json

```json
{
  "info": {
    "title": "Kit (Independent Publisher)",
    "description": "...",
    "version": "1.0.0",
    "contact": {
      "name": "Your Name",  // Your personal name
      "url": "https://github.com/yourusername",  // Your GitHub profile
      "email": "your.email@example.com"  // Your personal email
    }
  }
}
```

### In PR Submission

When submitting the PR, you'll provide:

- **Publisher Name**: Your personal name
- **Publisher Email**: Your personal email (matches GitHub)
- **Support Email**: Your personal email (matches GitHub)
- **Website URL**: Can be company website, personal site, or GitHub profile
- **Privacy Policy URL**: Can be company privacy policy or service privacy policy

## Special Cases

### Working for a Company

If you're building connectors for your company:

- **Publisher Name**: Still your personal name
- **Contact**: Still your personal email
- **Website**: Can be company website
- **Privacy Policy**: Can be company privacy policy
- **Description**: Can mention you're building it for/with the company

Example:
```
Publisher: John Smith
Email: john.smith@company.com (must be linked to your GitHub)
Website: https://www.company.com
Description: "Built by John Smith for Company XYZ..."
```

### Multiple Connectors

If building multiple connectors:

- **Same publisher name** for all (your name)
- **Same email** for all (your verified email)
- **Consistent identity** across all submissions

### Lost Access

If you lose access to your account:

- Contact: connectorpartnermgmtteam@microsoft.com
- Provide verification of identity
- Microsoft will help transfer ownership

## Best Practices

1. **Use your real name** as it appears on your government ID
2. **Use a permanent email** you'll have long-term access to
3. **Link email to GitHub** before starting OneVet verification
4. **Keep credentials current** (they expire after 1 year)
5. **Document your role** in readme if building for a company

## FAQ

### Can I use my company email?

✅ **Yes**, as long as:
- It's linked to your personal GitHub account
- You have long-term access to it
- It matches your verified identity

### Can I list my company as co-author?

✅ **Yes**, in the readme.md you can mention:
- "Built by [Your Name] for [Company]"
- "Developed in collaboration with [Company]"
- Company logo/branding in documentation

### What if I leave the company?

⚠️ **Important**: 
- You remain the publisher (it's tied to your identity)
- You're responsible for updates
- Consider using personal email if this is a concern
- Can transfer ownership through Microsoft support

### Can a team submit connectors?

❌ **No**, submissions must be from individuals
✅ **But**: One person can be the publisher, others can contribute via GitHub

## Summary

**For Independent Publisher Connectors:**
- Publisher = Individual person (you)
- Contact = Your personal information
- Verification = Your government ID
- Accountability = You personally

**This is different from Certified Connectors** where companies can be publishers.

---

**Reference**: [Microsoft Independent Publisher Certification Guide](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip)
