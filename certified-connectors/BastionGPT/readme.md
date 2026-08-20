# BastionGPT

## Publisher: Bastion Intelligence

BastionGPT is a HIPAA-compliant AI assistant built for healthcare by Bastion Intelligence (FortaTech Security, LLC). This connector lets you generate text with AI chat completions, ask questions about uploaded documents, and transcribe audio recordings with speaker separation, all from your automations and agents. Customer content is never used to train AI models.

## Prerequisites

- A BastionGPT API subscription. Request one at https://bastiongpt.com/api-signup. Keys are typically issued the same business day.
- Your BastionGPT API subscription key. Keep it confidential and rotate it if you suspect it has been exposed.

## Obtaining Credentials

When you create a connection, paste your BastionGPT API subscription key into the **API Key** field. The connector sends the key in the `key` request header on every call. No other configuration is required.

## Supported Operations

### Ask a question

The simplest way to use BastionGPT. Sends a single question and returns the answer as plain text.

- **Question** (required): what you want to ask or have generated.
- **Instructions** (optional): guidance that shapes the response, such as the role to adopt, the audience, or the output format. When omitted, the default BastionGPT healthcare assistant persona is used.
- **Document ID** (optional): the ID returned by **Upload a document**, to ask about that document.
- **Max Tokens** (required, default 1000) and **Temperature** (advanced, default 0).

The response is flat: **Answer**, **Finish Reason**, **Response ID**, and **Prompt/Completion/Total Tokens**. Drag **Answer** directly into the next step.

### Create a chat completion

Sends a multi-turn conversation to BastionGPT and returns an AI-generated text response. Use this when you need to send earlier turns or full control over the messages array; for a single question, **Ask a question** is simpler.

- **Messages** (required): the conversation, oldest first. Each message has a **Role** (`system`, `user`, or `assistant`) and **Content**. Include earlier user and assistant turns to keep context. An optional `system` message as the first item overrides the default BastionGPT healthcare assistant persona.
- **Max Tokens** (required, default 1000): the maximum number of tokens to generate.
- **Temperature** (advanced, default 0): 0.0 to 1.0. Lower values give more consistent output.
- **Document ID** (optional): the ID returned by **Upload a document**, to ask questions about that document.

The response contains **Choices** (an array with one item whose **Message Content** holds the generated text), a **Response ID**, a **Created** timestamp, and **Usage** token counts. To read the generated text in an expression, use:

```
first(outputs('Create_a_chat_completion')?['body/choices'])?['message']?['content']
```

### Upload a document

Uploads a single document (PDF, plain text, HTML, CSV, Markdown, PNG, or JPEG; 10 MB maximum) and returns a **Document ID**. Supply the **File** content (for example the *File Content* output of a SharePoint or OneDrive *Get file content* action) and the **File name** (for example *File name with extension*). Documents are retained for 30 days after upload and can be referenced by any number of chat completion requests during that time. A document ID is owned by the API subscription key that uploaded it.

### Submit audio for transcription

Uploads an audio recording (WAV, MP3, M4A, OGG, WEBM, and other common formats) for asynchronous transcription with speaker separation. Supply the **File** content and the **File name**. Returns a **Transcript ID** immediately, plus the audio **File Duration** in seconds.

### Get a transcript

Retrieves a transcript by **Transcript ID**. The operation always succeeds with a **Status** field:

- `processing`: the transcript is not ready yet. **Segments** is empty and **Text** is blank. Wait 10 to 20 seconds and call the operation again. A typical pattern is a *Do until* loop that repeats until **Status** equals `completed`, with a *Delay* of 15 seconds inside the loop.
- `completed`: **Segments** holds the speaker-separated transcript (each with **Timestamp** in HH:MM:SS, **Speaker**, and **Text**), and **Text** holds the full transcript with one line per segment, ready to drop into an email, a document, or a record.

If the transcription failed or the transcript ID is missing, the operation returns an error with the service's message.

## Getting Started

1. Request an API subscription at https://bastiongpt.com/api-signup and receive your key.
2. Add a BastionGPT action to your automation or agent and create a connection with your API key.
3. For a first test, use **Ask a question** with the Question `Summarize the purpose of a SOAP note in two sentences.` and Max Tokens `200`, then use **Answer** in a following step.
4. To work with a file, use **Upload a document** first, then pass the returned Document ID to **Ask a question** or **Create a chat completion**.
5. To transcribe audio, use **Submit audio for transcription**, then poll **Get a transcript** until **Status** is `completed`.

## Known Issues and Limitations

- **Max Tokens is required.** Requests without it are rejected by the service.
- **One document per chat completion request.** Documents expire 30 days after upload; upload again to get a new ID.
- **Chat completion size limits.** Request body 10 MB; about 1.6 million characters per message and about 2.5 million characters of total message content. Very large prompts are automatically served by a large-context model.
- **Get a transcript reports `processing` for unknown transcript IDs** as well as for in-progress transcriptions. If the status stays `processing` well beyond the audio duration, verify the transcript ID. Give *Do until* loops a sensible limit.
- **Streaming responses are not exposed** by this connector. Use the non-streaming chat completion operation.
- **Rate limits** depend on your subscription tier. On status code 429, wait and retry with exponential backoff.
- **File inputs need a content type.** File content from SharePoint, OneDrive, Outlook attachments, or forms carries its content type automatically. If you build file content yourself from base64, use `dataUriToBinary(concat('data:<content type>;base64,', <base64>))` so the service can recognize the file type.
- **Protected health information.** BastionGPT is designed to process PHI under a Business Associate Agreement with Bastion Intelligence. Review your organization's policies before sending PHI through automations, and consider enabling secure inputs and outputs on actions so PHI is not written to run history.

## Frequently Asked Questions

### Which AI model answers my request?

BastionGPT automatically selects an appropriate enterprise-grade AI model for each request. The response format is the same regardless of the model used.

### Is my data used to train AI models?

No. Customer content is never used to train AI models.

### Where can I get help?

Visit https://support.bastiongpt.com or contact support@bastiongpt.com.
