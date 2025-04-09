# Microsoft Learn Catalog
The Microsoft Learn Catalog API allows you to send a web-based query to Microsoft Learn and get back details about all published Learn content, such as titles, products covered, levels, links to the training, and other metadata. You can then take this information and display it in your own site or learning management system (LMS) experience so that your users can access it just-in-time when doing a task or displayed side-by-side with other training content.

## Publisher: Sean Kelly

## Prerequisites
There are no prerequisites for this connector.

## Supported Operations

There is a single action called **Get Learning Content**, this action can be used to access the entire catalog, or specific elements from the catalog by providing query parameters.

### Get Learning Content
This connector can return information from the Microsoft Learn Catalog about all publicly available Modules, Units, Learning Paths, Instructor-Led Courses, Exams, Certifications.

## Obtaining Credentials
No credentials are needed for this connector.

## Getting Started

Visit [Microsoft Learn Catalog API](https://learn.microsoft.com/training/support/catalog-api) for more information including details on the query parameters and fields.

## Known Issues and Limitations
There are no known issues at this time.

## Frequently Asked Questions

### Q: Is the training available in other languages?

It depends. Most content is available in the languages available for the product(s) that is/are being taught.

### Q: Does the data returned from the Catalog API remain static or change?

The data changes whenever new content is added, which is usually at least once a week.

### Q: Why are there HTML tags in the summary of the module?

Microsoft typically try to keep the formatting of the summary as raw as possible, but sometimes they need to emphasize or link certain text.

### Q: Is the UID unique for content?

Yes.

### Q: Why are there not links for the individual units?

The units aren't written as standalone content. They're intended to be taken in a specific order for the module. For this reason, Microsoft include the link to the module detail page and the first unit so that users can start there and proceed through the content.

### Q: How do I tell what content is new?

The last_modified value will tell you when the record was last modified.

### Q: Are all modules a part of a learning path?

No. Modules are standalone in the sense that they teach a scenario or concept end-to-end within them. For some, this is it and they aren't a part of a learning path. For others, they're bundled together in one or more learning paths that take a user through building more advanced concepts. A module doesn't have to be a part of a learning path, or it can be a part of one or more.

### Q: Will this API let our users consume the Learn content inside our platform?

No. The Catalog API provides metadata, including direct links to the content. Partners can integrate these links into any LMS or web experience.

### Q: Is there any reporting API to get data such as views, clicks, completion?

No. Microsoft do have organizational reporting functionality though that allows organizations to report on their users completions. Also, to explore ways to integrate this API with other features of Microsoft Learn, view the article Leverage Learn in your organization.

### Q: Is SSO enabled for these links?

Not at this time. Microsoft have SSO on their roadmap for a future iteration.