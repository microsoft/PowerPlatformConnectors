# Changelog

All notable changes to the Mailchimp Marketing connector will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-18

### Added
- Initial release of Mailchimp Marketing independent publisher connector
- 40+ operations covering Mailchimp Marketing API v3.0
- **Audience Management**
  - Create, read, and manage audiences (lists)
  - Get audience statistics and details
- **Member Operations**
  - Add, update, upsert, and delete members
  - Get member details and activity history
  - Member activity feed with detailed engagement data
- **Tag Management**
  - Apply and remove tags from members
  - Get all tags for an audience
  - Tag-based segmentation support
- **Campaign Operations**
  - Create, read, update, and delete campaigns
  - Send campaigns immediately or schedule for later
  - Send test emails
  - Get and set campaign content
- **Engagement Tracking**
  - Campaign open details (who opened)
  - Campaign click details (which URLs clicked)
  - Get members who clicked specific links
  - Unsubscribe tracking per campaign
  - Email activity reports
- **Reports & Analytics**
  - Campaign performance reports
  - Detailed recipient lists
  - Open, click, and unsubscribe metrics
  - Campaign-level statistics
- **Automation Support**
  - Get classic automations
  - Get automation emails
  - Automation workflow details
- **Batch Operations**
  - Bulk processing for high-volume scenarios
  - Create and monitor batch operations
  - Support for up to 500 operations per batch
- **Segment Operations**
  - Get all segments for an audience
  - Read-only segment access
- **Template Operations**
  - Get all templates
  - Filter by template type (user, base, gallery)
- **Comprehensive Documentation**
  - Detailed README with setup instructions
  - 1,427-line user guide with examples and patterns
  - Common workflows and use cases
  - Error handling guide

### Technical Details
- Basic authentication with API key
- Dynamic datacenter routing based on API key
- Support for all Mailchimp datacenters
- Pagination support (max 1000 records per request)
- MD5 hash support for subscriber operations
- Upsert operations for add-or-update logic

### Documentation
- Setup guide with Mailchimp API key instructions
- Datacenter configuration
- Operation reference with examples
- Common patterns (CRM sync, engagement import, Customer Journey triggers)
- Troubleshooting guide with common errors
- Best practices for API usage and rate limits

### Certification
- Passed Microsoft Power Platform connector certification
- Clean paconn validation with no warnings
- Complete schema definitions for all operations
- Publisher: Forceworks
- Stack Owner: Mailchimp (Intuit)

[1.0.0]: https://github.com/microsoft/PowerPlatformConnectors/tree/dev/independent-publisher-connectors/MailchimpMarketing
