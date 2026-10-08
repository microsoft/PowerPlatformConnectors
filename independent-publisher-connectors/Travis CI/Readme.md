# Travis CI

Travis CI is a hosted continuous integration service that automatically builds and tests code changes in GitHub repositories. This connector enables Power Automate users to monitor builds and explore repository, job, and branch details — powering CI/CD alerting and daily reporting flows.

## Publisher: Aaron Mah

## Prerequisites

You need a Travis CI account (free tier available). Travis CI authenticates exclusively via GitHub — sign up at [https://app.travis-ci.com](https://app.travis-ci.com) with your GitHub account. At least one GitHub repository must be activated on Travis CI to use most operations.

## Obtaining Credentials

1. Log in at [https://app.travis-ci.com](https://app.travis-ci.com) with your GitHub account.
2. Click your avatar in the upper right corner and select **Settings**.
3. Scroll to the **API Token** section.
4. Click **Copy Token** to copy your API token.
5. Paste the token when creating the Travis CI connection in Power Automate.

## Supported Operations

### Get Build
Gets a single build by its numeric ID, including commit, branch, and job details.

### List Builds for Repository
Lists builds for a specific repository, with optional filtering by state, event type, and branch.

### Get Repository
Gets details for a single repository, including its current build status.

### List Repositories
Lists repositories the authenticated user has access to, with optional filtering.

### Get Job
Gets details for a single job within a build.

### Get Job Log
Gets the full text log for a job. Note: logs can be large; use for targeted debugging.

### List Jobs for Build
Lists all jobs belonging to a specific build.

### Get Branch
Gets a branch and its last build information for a repository.

### Get Current User
Gets the profile of the currently authenticated user.

## API Documentation

Visit [Travis CI Developer Docs](https://developer.travis-ci.com/) for further details.

## Known Issues and Limitations

- **Repository slug encoding**: Repository slugs use `owner/name` format. Power Automate handles the URL encoding automatically — enter the slug as `myorg/my-repo` (not `myorg%2Fmy-repo`).
- **API rate limits**: Travis CI may rate-limit API requests. If you experience throttling, increase the interval between flow runs.
- **Large log responses**: The Get Job Log operation can return multi-megabyte responses for long-running builds. Use it for targeted debugging rather than bulk log retrieval.
- **Single token per user**: Travis CI uses a single API token per user account. There are no granular scopes — the token inherits permissions from your GitHub repository access.
- **Required API version header**: All requests require the `Travis-API-Version: 3` header. This connector injects it automatically via a policy template.

## License

Distributed under the MIT License.
