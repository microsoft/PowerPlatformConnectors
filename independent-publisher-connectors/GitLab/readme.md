# GitLab
GitLab is an alternative to GitHub and Azure DevOps.  GitLab offers a robust suite of APIs for automating CI/CD pipelines.  This connector allows a user to orchestrate basic DevOps workflows within Power Apps, Logic Apps, and Power Automate.

## Publisher: Roy Paar

## Prerequisites
You must provision an API Key within your GitLab account.

## Obtaining Credentials
You must provision an API Key within your GitLab account.

## Supported Operations
### Get Version
Retrieve version information for this GitLab instance. Responds 200 OK for authenticated users.

### Create a Project
Create a new project owned by the authenticated user.

### Fork a Project
Fork a project into the user namespace of the authenticated user or the one provided.

### Compare a branch, tag, or commit
Get the diff of a branch, tag, or commit in a project.

### Create a Branch
Create a new branch in the repository.

### Create a Commit
Create a commit by posting a JSON payload.

### Create a Merge Request
Create a new merge request.

### Merge a Merge Request
Accept and merge changes submitted with MR.

### Update a Merge Request
Update a merge request, for example close or reopen an MR.

### Get a File From a Repository
Fetch, create, update, and delete files in your repository.

### Create a CI/CD Pipeline Trigger
Create a pipeline trigger so that you can trigger your CI/CD pipeline with a webservice call.

### Trigger a CI/CD Pipeline
Use your pipeline trigger to trigger a CI/CD pipeline.

### Enable a Runner
Pass in the ID of a shared runner with the image and tools you need to execute your pipeline so that your project can build on that runner.

## API Documentation
https://docs.gitlab.com/ee/api/ 

## Known Issues and Limitations
None.
