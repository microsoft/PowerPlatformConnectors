# Sealed Rose (Independent Publisher)

Sealed Rose is a multimodal AI media forensics and deepfake detection platform providing neural verification across video, audio, image, and text to detect synthetic face swaps, AI voice clones, and generative media artifacts. The service is a REST API at `sealedrose.com/api/v1` with Bearer API key authentication, documented at https://sealedrose.com/developers and https://sealedrose.com/verify-video.

The connector exposes three operations: evaluate remote video, audio, or image URLs for synthetic deepfake manipulation (`VerifyMediaUrl`), analyze text passages for large language model generation signatures (`DetectText`), and inspect current API quota and verification usage (`GetUsage`).

No connector on the Power Platform provides multimodal video, voice clone, and image deepfake forensic verification via Sealed Rose today.

Contact: derek@sealedrose.com
