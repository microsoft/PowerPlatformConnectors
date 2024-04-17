# Title
Swirl is metasearch software that uses AI to simultaneously search multiple content and data sources, finds the best results using a reader LLM, then prompts Generative AI, enabling you to get answers from your own data. 

This Connector is intended to make it easy to use Swirl in Power Automate flows. Please [contact support](#support) if you need help getting it working.

## Publisher: Swirl Corporation

For more information, please visit [swirl.today](https://swirl.today)

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with Custom Connector feature
* [A Swirl installation](https://go.swirl.today/azure/) such as search.your-domain.com
* Credentials for one (or more) Swirl user accountss

## Supported Operations
Swirl currently supports a single method: `Search Swirl`. 

### Search Swirl
The following parameters are supported for this operation:

* `Swirl host`: the hostname of the Swirl installation, optionally including port number
* `Search for`: the search query to execute
* `RAG results`: true to have Swirl RAG using the top results; false otherwise
* `Content-type`: application/json

## Obtaining Credentials
* [Install Swirl](https://docs.swirl.today/Quick-Start.html)
* [Install Swirl in your Azure environment via the marketplace](https://go.swirl.today/azure)
* Create a Swirl username and password
* Use this username and password when adding the Swirl Connector to a Power Automate flow
* Set the `swirl_host` to the IP address of the installation, or a proxy server (not included) if used

## Known Issues and Limitations
Review the [release notes](https://github.com/swirlai/swirl-search/releases) for known issues and limitations. See below to create a new issue or obtain support.

## Deployment Instructions
* Create a flow that produces a text output intended to be a user query
* Add the Swirl Connector
* Select the Search Swirl operation
* Configure Swirl so the *Search for* field receives the flow text output as input
* Use a Compose Data Operation to select either the `results` or `ai_summary` blocks
* Deliver the output into most any Power Automate connector such as `Create file`, `Send an email`, etc. 

## Support
* [Join the Swirl Slack](https://join.slack.com/t/swirlmetasearch/shared_invite/zt-1qk7q02eo-kpqFAbiZJGOdqgYVvR1sfw)
* [GitHub Issues](https://github.com/swirlai/swirl-search/issues)
* [Swirl Website](https://swirl.today/)

