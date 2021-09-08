
## Cloudmersive NLP
[NLP API](https://cloudmersive.com/nlp-api) lets you analyze and process text using Natural Language Processing to perform text analytics, automatic rephrasing, sentiment analysis and other NLP operations.


## Pre-requisites
N/A


## API documentation
[Cloudmersive NLP API Documentation](https://api.cloudmersive.com/docs/nlp.asp) is available on the Cloudmersive website


## Supported Operations

### Perform Sentiment Analysis and Classification on Text
Analyze input text using advanced Sentiment Analysis to determine if the input is positive, negative, or neutral.  Supports English language input.  Consumes 1-2 API calls per sentence.

### Perform Profanity and Obscene Language Analysis and Detection on Text
Analyze input text using advanced Profanity and Obscene Language Analysis to determine if the input contains profane language.  Supports English language input.  Consumes 1-2 API calls per sentence.

### Perform Subjectivity and Objectivity Analysis on Text
Analyze input text using advanced Subjectivity and Objectivity Language Analysis to determine if the input text is objective or subjective, and how much.  Supports English language input.  Consumes 1-2 API calls per sentence.

### Extract entities from string
Extract the named entitites from an input string.

### Detect language of text
Automatically determine which language a text string is written in.  Supports Danish (DAN), German (DEU), English (ENG), French (FRA), Italian (ITA), Japanese (JPN), Korean (KOR), Dutch (NLD), Norwegian (NOR), Portuguese (POR), Russian (RUS), Spanish (SPA), Swedish (SWE), Chinese (ZHO).

### Parse string to syntax tree
Parses the input string into a Penn Treebank syntax tree

### Part-of-speech tag a string
Part-of-speech (POS) tag a string and return result as JSON

### Part-of-speech tag a string, filter to verbs
Part-of-speech (POS) tag a string, find the verbs, and return result as JSON

### Part-of-speech tag a string, filter to nouns
Part-of-speech (POS) tag a string, find the nouns, and return result as JSON

### Part-of-speech tag a string, filter to adjectives
Part-of-speech (POS) tag a string, find the adjectives, and return result as JSON

### Part-of-speech tag a string, filter to adverbs
Part-of-speech (POS) tag a string, find the adverbs, and return result as JSON

### Part-of-speech tag a string, filter to pronouns
Part-of-speech (POS) tag a string, find the pronouns, and return result as JSON

### Rephrase, paraphrase English text sentence-by-sentence using Deep Learning AI
Automatically rephrases or paraphrases input text in English sentence by sentence using advanced Deep Learning and Neural NLP.  Creates multiple reprhasing candidates per input sentence, from 1 to 10 possible rephrasings of the original sentence.  Seeks to preserve original semantic meaning in rephrased output candidates.  Consumes 1-2 API calls per output rephrasing option generated, per sentence.

### Extract sentences from string
Segment an input string into separate sentences, output result as a string.

### Get words in input string
Get the component words in an input string

### Find spelling corrections
Find spelling correction suggestions and return result as JSON

### Check if sentence is spelled correctly
Checks whether the sentence is spelled correctly and returns the result as JSON




## How to get credentials
- [Register](https://account.cloudmersive.com/signup) for a Cloudmersive Account
- [Sign In](https://account.cloudmersive.com/login) with your Cloudmersive Account and click on API Keys

Here you can create and see your API key(s) listed on the API Keys page.  Simply copy and paste this API Key into the Cloudmersive NLP Connector.

Now you are ready to start using the Cloudmersive NLP Connector.


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

