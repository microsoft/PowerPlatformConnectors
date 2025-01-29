# AssemblyAI

With the [AssemblyAI](https://www.assemblyai.com) Connector, you can use AssemblyAI's models to process audio data by transcribing it with speech recognition models, analyzing it with audio intelligence models, and building generative features on top of it with LLMs.

## Publisher: AssemblyAI

## Prerequisites

You will need the following to proceed:

- An AssemblyAI API key ([get one for free](https://www.assemblyai.com/dashboard))

## Supported Operations

The connector supports the following operations:

### Upload a File

Upload your media file directly to the AssemblyAI API if it isn't accessible via a URL already.

### Transcribe Audio

Create a transcript from an audio or video file that is accessible via a URL.

### List transcripts

Retrieve a list of transcripts you created.
Transcripts are sorted from newest to oldest.
The previous URL always points to a page with older transcripts.

### Get Transcript

Get the transcript resource. The transcript is ready when the "status" is "completed".

### Delete Transcript

Delete the transcript.
Deleting does not delete the resource itself, but removes the data from the resource and marks it as deleted.

### Get Subtitles of Transcript

Export your transcript in SRT or VTT format, to be plugged into a video player for subtitles and closed captions.

### Get Sentences of Transcript

Get the transcript split by sentences.
The API will attempt to semantically segment the transcript into sentences to create more reader-friendly transcripts.

### Get Paragraphs of Transcript

Get the transcript split by paragraphs.
The API will attempt to semantically segment your transcript into paragraphs to create more reader-friendly transcripts.

### Search Transcript for Words

Search through the transcript for a specific set of keywords.
You can search for individual words, numbers, or phrases containing up to five words or numbers.

### Get Redacted Audio

Retrieve the redacted audio object containing the status and URL to the redacted audio.

### Run a Task using LeMUR

Use the LeMUR task endpoint to input your own LLM prompt.

### Purge LeMUR Request Data

Delete the data for a previously submitted LeMUR request.
The LLM response data, as well as any context provided in the original request will be removed.

## Obtaining Credentials

You can get an AssemblyAI API key for free by [signing up for an account](https://www.assemblyai.com/dashboard/signup) and copying the API key from [the dashboard](https://www.assemblyai.com/dashboard).

## Known Issues and Limitations

No known issues currently.
We don't support Streaming Speech-To-Text (realtime) as it is not possible using Custom Connectors.

## Deployment Instructions

Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
