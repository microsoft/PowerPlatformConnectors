## Swirl Search Connector
Swirl is metasearch software that uses AI to simultaneously search multiple content and data sources, finds the best results using a reader LLM, then prompts Generative AI, enabling you to get answers from your own data. 

This Connector is intended to make it easy to use Swirl in Power Automate flows. Please [contact support](#support) if you need help getting it working.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with Custom Connector feature
* [A Swirl installation](https://go.swirl.today/azure/) such as search.your-domain.com
* Credentials for one (or more) Swirl user accountss

## Deploying the Connector
To use the connector, add it to a flow.

## Supported Operations
Swirl currently supports a single method: `Search Swirl`. 

## Supported Parameters
* `Swirl host`: the hostname of the Swirl installation, optionally including port number
* `Search for`: the search query to execute
* `RAG results`: true to have Swirl RAG using the top results; false otherwise
* `Content-type`: application/json

## Testing the Connector
To test the connector: 
* Create a flow that provides a text output
* Configure Swirl such that the *Search for* field is the text output
* Use a Compose Data Operation to select either the `results` or `ai_summary` blocks
* Deliver those blocks into most any Power Automate connector such as `Create file`, `Send an email`, and much more.

## Support
* [Join the Swirl Slack](https://join.slack.com/t/swirlmetasearch/shared_invite/zt-1qk7q02eo-kpqFAbiZJGOdqgYVvR1sfw)
* [GitHub Issues](https://github.com/swirlai/swirl-search/issues)
* [Swirl Website](https://swirl.today/)

