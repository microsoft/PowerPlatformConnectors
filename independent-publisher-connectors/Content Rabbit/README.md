# Content Rabbit Power Platform Connector

Content Rabbit is a headless social-scheduling core. This connector is a thin wrapper over the Content Rabbit public REST API, allowing you to schedule and publish to multiple social networks (X, LinkedIn, Mastodon, etc.) directly from Microsoft Power Automate, Power Apps, and Logic Apps.

## Features
- **List Accounts**: Retrieve a list of your connected social media accounts.
- **Create Post**: Create a new post or schedule one for later, specifying the target platform (e.g. "twitter", "linkedin").
- **List Posts**: View your scheduled and published posts.
- **Get Analytics**: Access high-level analytics for your connected accounts.

## Authentication
This connector uses an API Key for authentication.
1. Go to your Content Rabbit Dashboard.
2. Navigate to **Settings -> Team -> API & Integrations**.
3. Generate a new API key.
4. When creating the connection in Power Automate/Power Apps, enter your API key in the format: `Bearer <your_api_key>`.

## Local Development and Testing
You can use the Power Platform Connectors CLI (`paconn`) to download, validate, and test this connector locally.

1. Install the CLI:
   ```bash
   pip install paconn
   ```
2. Validate the Swagger file:
   ```bash
   paconn validate --api-def apiDefinition.swagger.json
   ```

## Submission Instructions (Independent Publisher)
To submit this connector to the official open-source repository:
1. Fork the [microsoft/PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors) repository.
2. Clone your fork locally.
3. Create a new branch.
4. Copy the contents of this directory (`integrations/powerplatform-connector`) into a new folder: `independent-publisher-connectors/ContentRabbit/`.
5. Ensure the structure includes `apiDefinition.swagger.json`, `apiProperties.json`, `README.md`, and `icon.png`.
6. Run `paconn validate --api-def apiDefinition.swagger.json` to ensure there are no validation errors.
7. Commit your changes and push the branch to your fork.
8. Open a Pull Request against the `microsoft/PowerPlatformConnectors` repository. Ensure you check the boxes in the PR template.

**Notes**:
- The icon should be a PNG file (approx. 230x230 pixels) and under 1MB.
- `iconBrandColor` in `apiProperties.json` is set to `#ff5ba8` to match Content Rabbit's brand guidelines.
- The authentication is strictly via standard API key (Header: Authorization).