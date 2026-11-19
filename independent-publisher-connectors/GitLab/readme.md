# GitLab



GitLab is a complete DevOps platform delivered as a single application, covering the entire software development lifecycle: project management, source code management, CI/CD pipelines, security, and monitoring. This connector enables Power Automate makers to monitor merge requests, CI/CD pipelines, and issues in GitLab projects — and route notifications to Microsoft Teams — without writing code or managing webhooks.



## Publisher



### Aaron Mah



## Prerequisites



You need a GitLab account (free tier or above) with a Personal Access Token (PAT) that has the `api` scope.



To create a Personal Access Token:



1. Log in to [GitLab.com](https://gitlab.com).

2. Click your avatar (upper-right) → **Edit profile**.

3. In the left sidebar, select **Access Tokens**.

4. Click **Add new token**.

5. Enter a name (e.g., `Power Automate Connector`).

6. Set an expiration date (maximum 365 days).

7. Select scope: **`api`** (grants full read/write access — required for all operations).

8. Click **Create personal access token**.

9. **Copy the token immediately** — it is only shown once.



## Supported Operations



### Get Version

Retrieve version information for this GitLab instance.



### Create Project

Create a new project owned by the authenticated user.



### Get Project

Retrieves details of a single GitLab project by its numeric ID.



### Fork Project

Fork a project into the user namespace of the authenticated user.



### Compare Repository

Compare a branch, tag, or commit in a project.



### Create Branch

Create a new branch in the repository.



### Create Commit

Create a commit by posting a JSON payload.



### Get File

Get a file from a repository.



### Create Merge Request

Create a new merge request.



### List Project Merge Requests

Lists merge requests for a project with optional filters for state, date range, labels, and milestone.



### Get Merge Request

Retrieves full details of a single merge request by project ID and merge request IID.



### Update Merge Request

Update a merge request (title, description, state, etc.).



### Merge Merge Request

Accept and merge changes submitted with a merge request.



### Create Merge Request Note

Adds a comment (note) to an existing merge request.



### List Project Pipelines

Lists CI/CD pipelines for a project with optional filters for status, branch, and date range.



### Get Pipeline

Retrieves full details of a single CI/CD pipeline including duration, user, and status information.



### List Pipeline Jobs

Lists all jobs for a specific CI/CD pipeline, optionally filtered by job status.



### Retry Pipeline

Retries all failed and canceled jobs in a CI/CD pipeline.



### Get Job Log

Retrieves the raw text log (trace) output of a CI/CD job.



### List Project Issues

Lists issues for a project with optional filters for state, labels, milestone, assignee, and date range.



### Get Issue

Retrieves full details of a single issue by project ID and issue IID.



### Create Issue

Creates a new issue in a GitLab project.



### Create Issue Note

Adds a comment (note) to an existing issue.



### Create Trigger

Create a pipeline trigger for a project.



### Trigger Pipeline

Trigger a CI/CD pipeline using a trigger token.



### Enable Runner

Enable a runner in a project.



## API Documentation



Visit [GitLab REST API Documentation](https://docs.gitlab.com/ee/api/rest/) for further details.



## Known Issues and Limitations



- **Rate limits:** GitLab.com enforces 2,000 authenticated requests per minute. Note creation is limited to 60 per minute per project.

- **Pagination:** List operations return a maximum of 100 items per page. Use the `per_page` and `page` parameters for pagination.

- **Token expiration:** Personal Access Tokens expire on the configured date (maximum 365 days). Plan for rotation before expiry.

- **Job log size:** The Get Job Log operation returns the full raw log as a string. Very large logs may be truncated by Power Automate's string size limits.

- **`iid` vs `id`:** Merge requests and issues use `iid` (project-scoped ID shown in the UI, e.g., `!42` or `#15`). Pipelines and jobs use globally unique `id` values.



## License



Distributed under the MIT License.

