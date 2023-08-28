
# Eden AI
Enhance your apps and workflows with the power of [Eden AI](https://www.edenai.co?referral=power-apps)! Integrate advanced AI features such as [text analysis](https://www.edenai.co/technologies/text?referral=power-apps) (sentiment analysis, keyword extraction, text moderation, etc.) , [image recognition](https://www.edenai.co/technologies/image?referral=power-apps) (object detection, face detection, explicit content detection, etc.), [document parsing](https://www.edenai.co/technologies/ocr-document-parsing?referral=power-apps) (invoice, receipt, IDs, resumes, etc.), [audio processing](https://www.edenai.co/technologies/speech?referral=power-apps), [machine translation](https://www.edenai.co/technologies/translation?referral=power-apps), [generative AI](https://www.edenai.co/technologies/generative-ai?referral=power-apps) (text generation, image generation, speech generation) and more.

## Publisher: Eden AI

## Prerequisites
You will need to [create your Eden AI account for free](https://app.edenai.run/user/register?referral=power-apps).

## Obtaining Credentials
Once you have sign up, get your API key from your account:
![image](https://github.com/queSaDiLLaSS/try/assets/118369217/b72d6510-6a10-4047-9656-536823f4cba3)

When you create a connection to Eden AI, do not forget the keyword **Bearer** before your API key:
![image](https://github.com/queSaDiLLaSS/try/assets/118369217/7154fbab-0c90-4e7a-9fab-053a2c1b504f)

## Supported Operations
The connector supports the following operations:
| Action | Description |
| :---    | :---       |
|[Convert Text into Speech](#convert-text-into-speech) | Converts normal language text into speech. |
|[Detect Explicit Content in Images](#detect-explicit-content-in-images) | Detects adult explicit content in images, that is generally inappropriate for people under the age of 18 and includes nudity, sexual activity and pornography... |
|[Text Generation](#text-generation) | Generates text based on a given prompt. |
|[Generate Chat Responses](#generate-chat-responses) | Generates human-like responses to various inputs and queries. |
|[Extract Topic from Text](#extract-topic-from-text) | Extracts general topics in a text. |
|[Extract Keywords from Text](#extract-keywords-from-text) | Extracts the most important words and expressions in a text. |
|[Extract Named Entities in Text](#extract-named-entities-in-text) | Identifies named entities in a text and classifies them into predefined categories. |
|[Anonymize Images](#anonymize-images) | Anonymizes an image by bluring sensitive parts (faces, car plates, etc.) |
|[Detect Faces in Images](#detect-faces-in-images) | Identifies human faces in digital images: position, sex, age, smile, glasses, etc. |
|[Image Generation](#image-generation) | Generates compelling images based on a given prompt. |
|[Translate Text into another Language](#translate-text-into-another-language) | Translates a text into another language using rules, statics or ML technics. |
|[Moderate Texts](#moderate-texts) | Moderates a text by detecting explicit content. |
|[Summarize a Text](#summarize-a-text) | Extracts the most important sentences from a text in order to create a smaller version of the text. |
|[Detect Language of Text](#detect-language-of-text) | Identifies the specific linguistic features that are unique to each language. |
|[Identify General Sentiment of a Text](#identify-general-sentiment-of-a-text) | Identifies general sentiment of a text and returns positive, negative or neutral. |
|[Extract Information in Invoices](#extract-information-in-invoices) | Extracts the data in contains (items, prices, addresses, vendor name, etc.) to automate the invoice processing. |
|[Extract Information in Resumes](#extract-information-in-resumes) | Extracts structured data (name, job list, education, skills) to automate the resume processing. |
|[Extract Informations in Identity Documents](#extract-informations-in-identity-documents) | Extracts structured information in identity documents (passports, identity cards, driver license, etc.). |
|[Extract Information in Receipts](#extract-information-in-receipts) | Extracts structured information like products purchased, quantity, price, date and VAT from receipts. |

### Convert Text into Speech
Converts normal language text into speech.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/audio_text_to_speech_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/audio_text_to_speech_create).
* `text`: Enter the text you want to convert into audio.
* `option`: Enter MALE or FEMALE to choose the voice gender.
* `settings`: Specify specific models to use for some providers. It can be in the following format: {'google' : 'google_model', 'microsoft': 'microsoft_model'...}. Check the model available [here](https://api.edenai.run/v2/info/provider_subfeatures?subfeature__name=text_to_speech&feature__name=audio).
* `rate`: Increase or decrease the speaking rate by expressing a positif or negatif number ranging between 100 and -100 (a relative value as percentage varying from -100% to 100%).
* `pitch`: Increase or decrease the speaking pitch by expressing a positif or negatif number ranging between 100 and -100 (a relative value as percentage varying from -100% to 100%).
* `volume`: Increase or decrease the audio volume by expressing a positif or negatif number ranging between 100 and -100 (a relative value as percentage varying from -100% to 100%).
* `audio_format`: Optional parameter to specify the audio format in which the audio will be generated. By default, audios are encoded in MP3, except for lovoai which use the wav container.
* `sampling_rate`: The synthesis sample rate (in hertz) for this audio. When this is specified, the audio will be converted either to the right passed value, or to a the nearest value acceptable by the provider.

### Detect Explicit Content in Images
Detects adult explicit content in images, that is generally inappropriate for people under the age of 18 and includes nudity, sexual activity and pornography...
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/image_explicit_content_create).
* `file`: Choose the file you want to analyze.

### Text Generation
Generates text based on a given prompt.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_generation_create).
* `text`: Enter your prompt.
* `temperature`: A value between 0 and 1. Higher values mean the model will take more risks and value 0 (argmax sampling) works better for scenarios with a well-defined answer.
* `max_tokens`: A value between 1 and 2048. The maximum number of tokens to generate in the completion. The token count of your prompt plus max_tokens cannot exceed the model's context length.
* `settings`: Specify specific models to use for some providers. It can be in the following format: {'openai' : 'openai_model', 'cohere': 'cohere_model'...}. Check the model available [here](https://api.edenai.run/v2/info/provider_subfeatures?subfeature__name=generation&feature__name=text).

### Generate Chat Responses
Generates human-like responses to various inputs and queries.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_chat_create).
* `text`: Enter your prompt.
* `chat_global_action`: A system message that helps set the behavior of the assistant. For example, 'You are a helpful assistant'.
* `temperature`: A value between 0 and 1. Higher values mean the model will take more risks and value 0 (argmax sampling) works better for scenarios with a well-defined answer.
* `max_tokens`: A value between 1 and 2048. The maximum number of tokens to generate in the completion. The token count of your prompt plus max_tokens cannot exceed the model's context length.
* `settings`: Specify specific models to use for some providers. It can be in the following format: {'openai' : 'openai_model', 'ibm': 'ibm_model'...}. Check the model available [here](https://api.edenai.run/v2/info/provider_subfeatures?subfeature__name=chat&feature__name=text).

### Extract Topic from Text
Extracts general topics in a text.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_topic_extraction_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/text_topic_extraction_create).
* `text`: Tap or paste the text you want to analyze.

### Extract Keywords from Text
Extracts the most important words and expressions in a text.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_keyword_extraction_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/text_keyword_extraction_create).
* `text`: Tap or paste the text you want to analyze.

### Extract Named Entities in Text
Generates human-like responses to various inputs and queries.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_named_entity_recognition_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/text_named_entity_recognition_create).
* `text`: Tap or paste the text you want to analyze.

### Anonymize Images
Anonymizes an image by bluring sensitive parts (faces, car plates, etc.)
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/image_anonymization_create).
* `file`: Choose the file you want to analyze.

### Detect Faces in Images
Identifies human faces in digital images: position, sex, age, smile, glasses, etc.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/image_face_detection_create).
* `file`: Choose the file you want to analyze.

### Image Generation
Generates compelling images based on a given prompt.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/image_generation_create).
* `text`: Tap or paste the text you want to analyze.
* `resolution`: Enter the resolution, it can only be 256x256, 512x512 or 1024x1024.
* `num_images`: Enter the number of images you want.

### Translate Text into another Language
Translates a text into another language using rules, statics or ML technics.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/translation_automatic_translation_create).
* `text`: Tap or paste the text you want to analyze.
* `source_language`: Enter the language of your text. Check languages supported [here](https://docs.edenai.co/reference/translation_automatic_translation_create).
* `target_language`: Enter the language of the output text. Check languages supported [here](https://docs.edenai.co/reference/translation_automatic_translation_create).

### Moderate Texts
Moderates a text by detecting explicit content.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_moderation_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/text_moderation_create).
* `text`: Tap or paste the text you want to analyze.

### Summarize a Text
Extracts the most important sentences from a text in order to create a smaller version of the text.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_summarize_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/text_summarize_create).
* `text`: Tap or paste the text you want to analyze.
* `output_sentences`: Enter the sentence number of the summary.
* `settings`: Specify specific models to use for some providers. It can be in the following format: {'google' : 'google_model', 'ibm': 'ibm_model'...}. Check the model available [here](https://api.edenai.run/v2/info/provider_subfeatures?subfeature__name=summarize&feature__name=text).

### Detect Language of Text
identify the specific linguistic features that are unique to each language.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/translation_language_detection_create).
* `text`: Tap or paste the text you want to analyze.

### Identify General Sentiment of a Text
Identifies general sentiment of a text and returns positive, negative or neutral.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/text_sentiment_analysis_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/text_sentiment_analysis_create).
* `text`: Tap or paste the text you want to analyze.

### Extract Information in Invoices
Extracts the data in contains (items, prices, addresses, vendor name, etc.) to automate the invoice processing.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/ocr_invoice_parser_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/ocr_invoice_parser_create).
* `file`: Choose the file you want to analyze.

### Extract Information in Resumes
Extracts structured data (name, job list, education, skills) to automate the resume processing.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/ocr_resume_parser_create).
* `file`: Choose the file you want to analyze.

### Extract Informations in Identity Documents
Extracts structured information in identity documents (passports, identity cards, driver license, etc.).
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/ocr_identity_parser_create).
* `file`: Choose the file you want to analyze.

### Extract Information in Receipts
Extracts structured information like products purchased, quantity, price, date and VAT from receipts.
* `providers`: Enter the selected providers seperated by a coma. Check the providers available [here](https://docs.edenai.co/reference/ocr_receipt_parser_create).
* `language`: Check languages supported [here](https://docs.edenai.co/reference/ocr_receipt_parser_create).
* `file`: Choose the file you want to analyze.

## Known Issues and Limitations
Here are some issues you could face while using the connector:
* `Error 400`: Appears when you make a bad request, it means that you wrongly enter the parameters. Check how to configure them correctly with our [doc](https://docs.edenai.co/reference/start-your-ai-journey-with-edenai).
* `Error 403`: Appears when you wrongly enter your API Key. When you create a connection, don't forget the keyword **Bearer** before your API_Key: Bearer API_KEY

If you have any interrogation about our APIs, check our [documentation](https://docs.edenai.co/reference/start-your-ai-journey-with-edenai) or feel free to [contact us](https://www.edenai.co/contact). 

## Deployment Instructions
Please use the instructions on [Microsoft Power Platform Connectors CLI](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate or Power Apps.


