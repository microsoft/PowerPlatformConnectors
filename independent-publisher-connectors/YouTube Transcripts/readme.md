# YouTube Transcript (Independent Publisher)
A custom service to retrieve transcripts from YouTube videos using the internal YouTube service.

## Publisher: Troy Taylor

## Prerequisites
There are no prerequisites needed for this service.

## Obtaining Credentials
This connector does not require authentication. YouTube transcripts are accessed through public API endpoints.

## Supported Operations
### Get Video Transcript
Retrieves and transforms the transcript for a specified YouTube video into a clean, Power Platform-friendly format with enhanced metadata and text processing.

## Known Issues and Limitations
- Transcripts must be available for the video (auto-generated or manually uploaded by creator)
- Only works with public YouTube videos
- Uses YouTube's internal API which may change without notice
- Custom code transforms complex responses into simplified Power Platform format
- Please ensure compliance with YouTube's Terms of Service