# Sealed Rose

Sealed Rose is a multimodal AI media forensics and deepfake detection platform. It provides neural verification across video, audio, image, and text to detect synthetic face swaps, AI voice clones, and generative media artifacts. An interactive browser-based verification suite is available at [https://sealedrose.com/verify-video](https://sealedrose.com/verify-video).

## Publisher: Derek Gallardo

## Prerequisites

You will need a Sealed Rose account and API key, or you can use the free interactive web verification suite directly at [https://sealedrose.com/verify-video](https://sealedrose.com/verify-video).

## Supported Operations

### Detect AI-generated text (`DetectText`)
Analyzes text content for perplexity, burstiness, and token-distribution signatures characteristic of large language models.

### Verify media URL for deepfake manipulation (`VerifyMediaUrl`)
Evaluates a remote video, audio, or image URL using multimodal neural decomposition and temporal optical flow analysis.

## Obtaining Credentials

1. Visit the Sealed Rose Developer Portal at [https://sealedrose.com/developers](https://sealedrose.com/developers).
2. Generate an API Key and supply it in the `Authorization` header as `Bearer YOUR_API_KEY`.

## Known Issues and Limitations

Media files larger than 32 MB should be verified directly using the client-side browser laboratory at [https://sealedrose.com/verify-video](https://sealedrose.com/verify-video) so zero media payload is transmitted over external networks.
