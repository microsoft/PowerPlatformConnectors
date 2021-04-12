
# LegalBot AI Tools
LegalBot AI Tools lets you easily access Neuro Linguistic Programming (NLP) models, machine translation and legal content. You have the ability to customise the AI models to suit your particular needs. Functionality is specifically designed for the automation of tasks in the legal industry but is also useful for finance industry and any international business department. The functions specialise in handling and analysing documents and text.

##  Setup

1. Access to the API is through and API access key. To obtain an API key go to [LegalBot.io](https://legalbot.io) or email support@legalbot.io
2. Ensure you have sufficient credits to perform your operations. More details on pricing are available at [LegalBot.io](https://legalbot.io) or email support@legalbot.io
3. You will be requested to enter the API key when creating the new connector.

## Supported Actions

The following actions are supported:

* `Translate document`: Translates a document from the target language to source language whilst keeping the formatting of the document (55 languages supported). The machine translation uses legal translation dictionaries to give more accurate results for legal documents. Your files can be in  HTML, PDF, .docx, .doc, .odt, .rtf, .txt, .pptx, .ppt, .xlsx format. Files must be converted to Base64 string format before sending e.g. base64(outputs('Get_file_content')?['body']). Language codes follow ISO 639-1 Code two letter format other than Chinese simplified: zh-Hans and Chinese traditional: zh-Hant. Source language may be in Auto. It is also possible to write the English version of the name e.g. English, German, French, Russian. Possible language combinations can be found [here.](https://legalbot.io/translate/)

* `Translate text`: Translates text from the target language to source language (55 languages supported). The service supports a maximum 50,000,000 character limit (50 MB) per call however make sure your input fields will allow this length. The machine translation uses legal translation dictionaries to give more accurate results for legal texts. Language codes follow ISO 639-1 Code two letter format other than Chinese simplified: zh-Hans and Chinese traditional: zh-Hant. Source language may be in Auto. It is also possible to write the English version of the name e.g. English, German, French, Russian. Possible language combinations can be found [here.](https://legalbot.io/translate/)

* `Document similarity with NLP`: Compares how similar two documents are using NLP techniques. You can easily customise your AI model by changing the input parameters. The files can be in  HTML, PDF, .docx, .doc, .odt, .rtf or .txt format. Files must be converted to Base64 string format before sending e.g. base64(outputs('Get_file_content')?['body']). Input parameters include:
    * `Clean text`: if set to true this will convert words in the document text to lowercase and remove punctuation and whitespace
    * `Use stop words`: if set to true then the text will be processed through a stop word engine to remove your own custom stop words or those defined in a particular stop word model
    * `Stop word model`: if you have chosen to use stop words then you can customise the stop word model you use. Available are Basic: common words like and and I; Legal_V1: words specifically used in agreements; user_only: this allows you to develop and use your own custom stop words for processing
    * `Custom stop words`: enter any specific stop words you want excluded from analysis. This is useful to fine tune your NLP analysis. Words are separated by a comma like this: these, are, my, custom, stop, words
    * `Lemmatize string`: if set to true the words in the document text will be lemmatized to their lemma or part of speech. For example the words "liabilities and liability" are converted to the same lemma of "liability  and  liability" and "warranty and warranties" into "warranty  and  warranty".
    * `Stem string`: if set to true the words in the document will be converted to their stem origin. This can help you customise your analysis. For example the words "liabilities and liability" are converted to the same stem of "liabil  and  liabil" and "warranty and warranties" into "warranti and  warranti". Stemming is usually considered easier to implement tha lemmatizing.
    * `Exclude short words`: Set the number to higher than 0 to exclude words equal to or shorter than the number selected. For example if set to 3 all words with a charater length 3 or less will be excluded: e.g. "sue" would be excluded but not "sues". You must set use stop words to true to enable this feature.
    * `File content`: requires the content of two files in base64 string format along with their file names.  

* `Top Keywords`: Extracts and returns the top keywords from a document. You can easily customise your AI model by changing the input parameters. The files can be in  HTML, PDF, .docx, .doc, .odt, .rtf or .txt format. Files must be converted to Base64 string format before sending e.g. base64(outputs('Get_file_content')?['body']). Input parameters include:
    * `Number of keywords`: the number of keywords you would like to receive back
    * `Keyword model`: this can be set to either word model or noun model for keywords. An example of the difference is: the top 3 key words in the following sentence "LegalBot.io makes it fun, fun, fun and easy to use and create NLP and AI models!" would be 'fun', 'and', 'LegalBot.io' for the word model whilst the noun model would return 'legalbot.io', 'nlp', 'ai'. By customising the parameters below you can fine tune your results.
    * `Clean text`: if set to true this will convert words in the document text to lowercase and remove punctuation and whitespace
    * `Use stop words`: if set to true then the text will be processed through a stop word engine to remove your own custom stop words or those defined in a particular stop word model
    * `Stop word model`: if you have chosen to use stop words then you can customise the stop word model you use. Available are Basic: common words like and and I; Legal_V1: words specifically used in agreements; user_only: this allows you to develop and use your own custom stop words for processing
    * `Custom stop words`: enter any specific stop words you want excluded from analysis. This is useful to fine tune your NLP analysis. Words are separated by a comma like this: these, are, my, custom, stop, words
    * `Lemmatize string`: if set to true the words in the document text will be lemmatized to their lemma or part of speech. For example the words "liabilities and liability" are converted to the same lemma of "liability  and  liability" and "warranty and warranties" into "warranty  and  warranty".
    * `Stem string`: if set to true the words in the document will be converted to their stem origin. This can help you customise your analysis. For example the words "liabilities and liability" are converted to the same stem of "liabil  and  liabil" and "warranty and warranties" into "warranti and  warranti". Stemming is usually considered easier to implement tha lemmatizing.
    * `Exclude short words`: Set the number to higher than 0 to exclude words equal to or shorter than the number selected. For example if set to 3 all words with a charater length 3 or less will be excluded: e.g. "sue" would be excluded but not "sues". You must set use stop words to true to enable this feature.
    * `File content`: requires the content of a file in base64 string format along with its file name.

