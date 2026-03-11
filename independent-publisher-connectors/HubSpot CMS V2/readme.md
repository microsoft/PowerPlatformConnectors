# HubSpot CMS V2
Content management software that's flexible for marketers, powerful for developers, and gives your customers a personalized, secure experience. Includes hosting, flexible themes, dynamic content, drag-and-drop page editing, memberships, and more — all powered by a CRM platform that allows you to build seamless digital experiences for your customers.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Schedule blog post
Schedules a blog post to be published.
### Retrieve batch of blog posts
Retrieves the blog post objects identified in the request body.
### Update languages of multi-language group
Explicitly set new languages for each blog post in a multi-language group.
### Retrieve previous blog post
Retrieves a previous version of a blog post.
### Update batch of blog posts
Updates the blog post objects identified in the request body.
### Retrieve the full draft blog post
Retrieves the full draft version of the blog post.
### Update blog post draft
Sparse updates the draft version of a single blog post object identified by the identifier in the path. All the column values need not be specified. Only the that need to be modified can be specified.
### Create batch of blog posts
Creates the blog post objects detailed in the request body.
### Restore previous version blog post
Takes a specified version of a blog post, sets it as the new draft version of the blog post.
### Create new language variation
Creates a new language variation from an existing blog post.
### Clone blog post
Clones a blog post.
### Get blog posts
Retrieves the list of blog posts.
### Create new blog post
Creates a new blog post.
### Restore previous version blog post
Takes a specified version of a blog post and restores it.
### Detach blog post from group
Detaches a blog post from a multi-language group.
### Push blog post live
Takes any changes from the draft version of the blog post and apply them to the live version.
### Delete batch of blog posts
Deletes the blog post objects identified in the request body. Note: This is not the same as the in-app archive function. To perform a dashboard archive send an normal update with the archivedInDashboard field set to true.
### Reset blog post draft to live version
Discards any edits and resets the draft to the live version.
### Attach blog post to group
Attaches a blog post to a multi-language group.
### Retrieve previous versions blog post
Retrieves all the previous versions of a blog post.
### Set new primary language
Sets a blog post as the primary language of a multi-language group.
### Retrieve blog post
Retrieves the blog post object identified by the identifier in the path.
### Delete blog post
Deletes the blog post object identified by the identifier in the path.
### Update blog post
Sparse updates a single blog post object identified by the identifier in the path. All the column values need not be specified. Only the columns that need to be modified can be specified.
### Retrieve blog author
Retrieves the blog author object identified by the identifier in the path.
### Delete blog author
Deletes the blog author object identified by the identifier in the path.
### Update blog author
Sparse updates a single blog author object identified by the identifier in the path. All the column values need not be specified. Only the columns that need to be modified can be specified.
### Detach blog author from group
Detaches a blog author from a multi-language group.
### Update batch of blog authors
Updates the blog author objects identified in the request body.
### Set new primary language
Sets a blog author as the primary language of a multi-language group.
### Delete batch of blog authors
Deletes the blog author objects identified in the request body.
### Create batch of blog authors
Creates the blog author objects detailed in the request body.
### Retrieve batch of blog authors
Retrieves the blog author objects identified in the request body.
### Get blog authors
Retrieves the list of blog authors.
### Create new blog author
Creates a new blog author.
### Create new language variation
Creates a new language variation from an existing blog author.
### Attach blog author to a multi-language group
Attaches a blog author to a multi-language group.
### Update languages of group
Explicitly set new languages for each blog author in a multi-language group.
### Update languages of group
Explicitly set new languages for each blog in a multi-language group.
### Retrieve previous versions of a blog
Retrieves all the previous versions of a blog.
### Retrieve previous version blog
Retrieves a previous version of a blog.
### Retrieve blog
Retrieves the blog object identified by the identifier in the path.
### Create new language variation
Creates a new language variation from an existing blog.
### Get blogs
Retrieves the list of blogs.
### Set new primary language
Sets a blog as the primary language of a multi-language group.
### Detach blog from group
Detaches a blog from a multi-language group.
### Attach blog to group
Attaches a blog to a multi-language group.
### Update batch of blog tags
Updates the blog tag objects identified in the request body.
### Update languages of group
Explicitly set new languages for each blog tag in a multi-language group.
### Detach blog tag from a multi-language group
Detaches a blog tag from a multi-language group.
### Create batch of blog tags
Creates the blog tag objects detailed in the request body.
### Delete batch of blog tags
Deletes the blog tag objects identified in the request body.
### Set new primary language
Sets a blog tag as the primary language of a multi-language group.
### Retrieve batch of blog tags
Retrieves the blog tag objects identified in the request body.
### Get blog tags
Retrieves the list of blog tags.
### Create new blog tag
Creates a new blog tag.
### Attach blog tag to group
Attaches a blog tag to a multi-language group.
### Retrieve blog tag
Retrieves the blog tag object identified by the identifier in the path.
### Delete blog tag
Deletes the blog tag object identified by the identifier in the path.
### Update blog tag
Sparse updates a single blog tag object identified by the identifier in the path. All the column values need not be specified. Only the columns that need to be modified can be specified.
### Create new language variation
Creates a new language variation from an existing blog tag.
### Export draft table
Exports the draft version of a table to CSV format.
### Return all draft tables
Returns the details for each draft table defined in the specified account, including column definitions.
### Reset a draft table
Replaces the data in the draft version of the table with values from the published version. Any unpublished changes in the draft will be lost after this call is made.
### Export published table
Exports the published version of a table to CSV format.
### Clone table
Clone an existing HubDB table. The newName and newLabel of the new table can be sent as JSON in the body parameter. This will create the cloned table as a draft.
###Import data into draft table
Import the contents of a CSV file into an existing HubDB table. The data will always be imported into the draft version of the table.
### Get published table details
Returns the details for the published version of the specified table. This will include the definitions for the columns in the table and the number of rows in the table.
### Archive table
Archives (soft delete) an existing HubDB table. This archives both the published and draft versions.
### Get published tables
Returns the details for the published version of each table defined in an account, including column definitions.
### Create new table
Creates a new draft HubDB table given a JSON schema. The table name and label should be unique for each account.
### Unpublish table
Unpublishes the table, meaning any website pages using data from the table will not render any data.
### Get draft table
Retrieves the details for the draft version of a specific HubDB table. This will include the definitions for the columns in the table and the number of rows in the table.
### Update existing table
Updates an existing HubDB table. You can use this endpoint to add or remove columns to the table as well as restore an archived table. Tables updated using the endpoint will only modify the draft verion of the table. Use publish endpoint to push all the changes to the published version.
### Publish table from draft
Publishes the table by copying the data and table schema changes from draft version to the published version, meaning any website pages using data from the table will be updated.
### Get draft table rows
Returns rows in the draft version of the specified table. Row results can be filtered and sorted. Filtering and sorting options will be sent as query parameters to the API request.
### Get table row
Retrieves a single row by identifier from a table's published version.
### Clone row
Clones a single row in the draft version of the table.
### Get draft table row
Retrieves a single row by identifier from a table's draft version.
### Permanently delete row
Permanently deletes a row from a table's draft version.
### Replaces existing row
Replace a single row in the table's draft version. All the column values must be specified. If a column has a value in the target table and this request doesn't define that value, it will be deleted.
### Update existing row
Sparse updates a single row in the table's draft version. All the column values need not be specified. Only the columns or fields that needs to be modified can be specified.
### Get table rows
Returns a set of rows in the published version of the specified table. Row results can be filtered and sorted.
### Add row to table
Creates a new row to a HubDB table. New rows will be added to the draft version of the table. Use publish endpoint to push these changes to published version.
### Replace draft table rows in batch
Replaces multiple rows as a batch in the draft version of the table, with a maximum of 100 rows per call.
### Deletes rows in batch
Permanently deletes rows from the draft version of the table, given a set of row identifiers. Maximum of 100 row identifiers per call.
### Get rows in batch
Returns rows in the published version of the specified table, given a set of row identifiers.
### Create rows in batch
Creates rows in the draft version of the specified table, given an array of row objects. Maximum of 100 row object per call.
### Get draft table rows in batch
Returns rows in the draft version of the specified table, given a set of row identifiers.
### Clone rows in batch
Clones rows in the draft version of the specified table, given a set of row identifiers. Maximum of 100 row identifiers per call.
### Update draft table rows in batch
Updates multiple rows as a batch in the draft version of the table, with a maximum of 100 rows per call.
### Retrieve previous version site page
Retrieves a previous version of a site page.
### Update batch of site pages
Updates the site page objects identified in the request body.
### Restore previous version site page
Takes a specified version of a site page and restores it.
### Schedule site page published
Schedules a site page to be published.
### Attach site page to group
Attaches a site page to a multi-language group.
### Detach site page from group
Detaches a site page from a multi-language group.
### Retrieve previous versions of site page
Retrieves all the previous versions of a site page.
### Restore previous version site page
Takes a specified version of a site page, sets it as the new draft version of the site page.
### End an active AB test
Ends an active A/B test and designate a winner.
### Retrieve draft version of site page
Retrieves the full draft version of the site page.
### Update site page draft
Sparse updates the draft version of a single site page object identified by the identifier in the path. You only need to specify the column values that you are modifying.
### Retrieve batch of site pages
Retrieves the site page objects identified in the request body.
### Get site pages
Retrieves the list of site pages.
### Create new site page
Creates a new site page.
### Create new language variation
Creates a new language variation from an existing site page.
### Create new AB test variation
Creates a new A/B test variation based on the information provided in the request body.
### Update languages of group
Explicitly set new languages for each site page in a multi-language group.
### Create batch of site pages
Creates the site page objects detailed in the request body.
### Delete batch of site pages
Deletes the site page objects identified in the request body. Note: This is not the same as the dashboard archive function. To perform a dashboard archive send an normal update with the archivedInDashboard field set to true.
### Clone site page
Clones a site page.
### Set new primary language
Sets a site page as the primary language of a multi-language group.
### Push site page draft edits
Takes any changes from the draft version of the site page and apply them to the live version.
### Rerun a previous AB test
Reruns a previous A/B test.
### Reset the site page draft
Discards any edits and resets the draft to the live version.
### Retrieve site page
Retrieves the site page object identified by the identifier in the path.
### Delete site page
Deletes the site page object identified by the identifier in the path.
### Update site page
Sparse updates a single site page object identified by the identifier in the path. You only need to specify the column values that you are modifying.
### Retrieve previous versions of a landing page
Retrieves all the previous versions of a landing page.
### Retrieve previous versions of a folder
Retrieves all the previous versions of a folder.
### Get landing page folders
Retrieves the list of landing page folders.
### Create new folder
Creates a new folder.
### Create batch of folders
Creates the folder objects detailed in the request body.
### Retrieve draft version landing page
Retrieves the full draft version of the landing page.
### Update landing page draft
Sparse updates the draft version of a single landing page object identified by the identifier in the path. You only need to specify the column values that you are modifying.
### Get landing pages
Retrieves the list of landing pages.
### Create new landing page
Creates a new landing page.
### Create batch of landing pages
Creates the landing page objects detailed in the request body.
### Update batch of landing pages
Updates the landing page objects identified in the request body.
### Retrieve batch of landing pages
Retrieves the landing page objects identified in the request body.
### Clone landing page
Clone a landing page.
### Restore previous version folder
Takes a specified version of a folder and restores it.
### Set new primary language
Sets a landing page as the primary language of a multi-language group.
### Restore version landing page
Takes a specified version of a landing page and restores it.
### Retrieve folder
Retrieves the folder object identified by the identifier in the path.
### Delete folder
Deletes the folder object identified by the identifier in the path.
### Update folder
Sparse updates a single folder object identified by the identifier in the path. You only need to specify the column values that you are modifying.
### Create new AB test variation
Creates a new A/B test variation based on the information provided in the request body.
### Attach landing page to group
Attaches a landing page to a multi-language group.
### Retrieve previous version landing page
Retrieves a previous version of a landing page.
### End an active AB test
Ends an active A/B test and designate a winner.
### Push landing page draft live
Takes any changes from the draft version of the landing page and apply them to the live version.
### Detach landing page from a multi-language group
Detaches a landing page from a multi-language group.
### Schedule landing page to be published
Schedules a landing page to be published.
### Update languages of group
Explicitly set new languages for each landing page in a multi-language group.
### Rerun a previous AB test
Reruns a previous A/B test.
### Delete batch of folders
Deletes the folder objects identified in the request body.
### Update batch of folders
Updates the folder objects identified in the request body.
### Retrieve previous version folder
Retrieves a previous version of a folder.
### Reset the landing page draft
Discards any edits and resets the draft to the live version.
### Retrieve landing page
Retrieves the landing page object identified by the identifier in the path.
### Delete landing page
Deletes the landing page object identified by the identifier in the path.
### Update landing page
Sparse updates a single landing page object identified by the identifier in the path. You only need to specify the column values that you are modifying.
### Create new language variation
Creates a new language variation from an existing landing page.
### Delete batch of landing pages
Deletes the landing page objects identified in the request body. Note: This is not the same as the dashboard archive function. To perform a dashboard archive send an normal update with the archivedInDashboard field set to true.
### Restore previous version landing page
Takes a specified version of a landing page, sets it as the new draft version of the landing page.
### Retrieve batch of folders
Updates the folder objects identified in the request body.

## Known Issues and Limitations
There are no known issues at this time.
