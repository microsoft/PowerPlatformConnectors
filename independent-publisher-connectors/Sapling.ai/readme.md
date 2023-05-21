# Sapling.ai
Sapling is building the AI assistant for business communication, one that helps teams communicate more efficiently and effectively with their clients, across diverse use cases.

## Publisher: Fördős András

## Prerequisites
You will need an API key to be able to use the underlying service. You can register for a free (dev) account or request a personalized production access: [https://sapling.ai/user/register](https://sapling.ai/user/register)

## Supported Operations
### AI Detector
The endpoint computes the likelihood a piece of text was AI generated.

### Autocomplete
Sapling's autocomplete provides predictions of the next few characters or words given the current context in a particular editable. The predictions are based on the previous text.

### Rephrase
The paraphrase and rephrase endpoints offer paraphrasing for particular styles. Given an input sentence, the endpoint returns output sentences that preserve meaning but use alternative phrasings or styles.

### Spellcheck
For some use cases, grammar and phrase-level edits may not be necessary. If your application only requires spellcheck, the Sapling spellcheck endpoint is a faster, cheaper solution.

## Known Issues and Limitations
There are no known issues or limitations for the connector at this time.

Be aware that the underlying service itself (Sapling.ai) does come with some limitations, such as 5.000 character reqeust per 24 hours for the free account. You can get more information about the actual limitations at: [https://sapling.ai/docs/api/api-access](https://sapling.ai/docs/api/api-access)