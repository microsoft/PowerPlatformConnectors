# Nero AI Image Upscaler Connector

## Overview

The Nero AI Image Upscaler connector allows you to integrate AI-powered image enhancement capabilities into your Power Automate flows and Power Apps. This connector provides high-quality image upscaling with support for various image types and quality settings.

## Features

- **AI-Powered Upscaling**: Enhance image resolution using advanced AI algorithms
- **Multiple Scaling Options**: Support for 2x and 4x upscaling
- **Quality Control**: Adjustable quality factors from 80-100
- **Specialized Models**: Optimized processing for different image types
- **Asynchronous Processing**: Submit tasks and monitor progress
- **Real-time Status**: Check processing status and progress

## Supported Image Types

- **Standard**: General purpose upscaling for most images
- **Photograph**: Optimized for real-world photographs
- **Anime**: Specialized for animated and cartoon-style images
- **Face Enhancement**: Enhanced facial detail processing

## Authentication

This connector uses API key authentication. You'll need to obtain an API key from the Nero AI dashboard.

### Getting Your API Key

1. Visit the [Nero AI Business](https://ai.nero.com/ai-api?utm_source=power_automate)
2. Sign in to your account
3. Navigate to the API section
4. Generate a new API key
5. Copy the key and use it in the connector

## Operations

### Upscale Image

Submits an image for AI-powered upscaling.

**Parameters:**

- **Image URL**: Public HTTPS URL of the image to upscale
- **Type**: AI model type (Standard, Photograph, Anime, Face Enhancement)
- **Quality Factor**: Quality setting from 80-100 (default: 95)
- **Upscaling Rate**: 2x or 4x scaling (default: 4x)

**Returns:** Task ID for status monitoring

### Check Task Status

Monitors the progress of an upscaling task and retrieves results.

**Parameters:**

- **Task ID**: Unique identifier from the Upscale Image operation

**Returns:** Current status, progress, and result URL when complete

## Usage Examples

### Basic Image Upscaling

1. **Submit Image**: Use "Upscale Image" action with your image URL
2. **Monitor Progress**: Use "Check Task Status" to track processing
3. **Retrieve Result**: Get the final upscaled image URL when complete

### Quality Optimization

- Use quality factor 95-100 for professional results
- Use quality factor 80-90 for faster processing
- 4x scaling provides maximum detail enhancement
- 2x scaling offers balanced quality and speed

## Best Practices

1. **Image Preparation**: Ensure images are in common formats (JPEG, PNG)
2. **URL Accessibility**: Use publicly accessible HTTPS URLs
3. **Processing Time**: Allow sufficient time for high-quality upscaling
4. **Error Handling**: Implement retry logic for failed tasks
5. **Resource Management**: Monitor API usage and rate limits

## Rate Limits

- **Standard Plan**: 100 requests per hour
- **Professional Plan**: 500 requests per hour
- **Enterprise Plan**: Custom limits available

## Support

For technical support and questions:

- **Email**: feedback@nero.com
- **Documentation**: [Nero AI Documentation](https://docs.nero.com/ai)

## Privacy and Security

- All image processing is performed securely
- Images are not stored permanently
- API keys are encrypted and secure
- Compliant with GDPR and data protection regulations

## Changelog

### Version 1.0.0

- Initial release
- Support for 2x and 4x upscaling
- Multiple AI model types
- Quality factor control
- Asynchronous task processing

## License

This connector is provided by Nero AG under the Microsoft Independent Publisher program.
