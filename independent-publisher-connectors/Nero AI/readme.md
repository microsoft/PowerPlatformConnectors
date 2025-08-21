# Nero AI Image Editing Tool Connector

## Overview

The Nero AI Image Editing Tool connector allows you to integrate multiple AI-powered image processing capabilities into your Power Automate flows and Power Apps. This connector provides comprehensive image processing including upscaling, colorization, background removal, noise reduction, compression, and face restoration.

## Features

- **AI-Powered Image Upscaling**: Enhance image resolution using advanced AI algorithms (2x and 4x scaling)
- **Photo Colorization**: Transform black and white photos to color using AI
- **Background Removal**: Automatically remove image backgrounds with precision
- **Image Denoising**: Reduce noise and improve image quality
- **Image Compression**: Optimize file size while maintaining quality
- **Face Restoration**: Enhance and restore facial features
- **Quality Control**: Adjustable quality factors from 80-100 for upscaling
- **Specialized Models**: Optimized processing for different image types
- **Asynchronous Processing**: Submit tasks and monitor progress
- **Real-time Status**: Check processing status and progress

## Supported Image Types

### Image Upscaling Models
- **Standard**: General purpose upscaling for most images
- **Photograph**: Optimized for real-world photographs
- **Anime**: Specialized for animated and cartoon-style images
- **Face Enhancement**: Enhanced facial detail processing

### Supported File Formats
- **Standard Formats**: JPG, JPEG, PNG, BMP, WEBP, JFIF, JFI, JPE, JIF, ICO
- **Advanced Formats**: HEIC, HEIF (for ColorizePhoto, BackgroundRemover, ImageDenoiser, ImageCompressor, FaceRestoration)

## Authentication

This connector uses API key authentication. You'll need to obtain an API key from the Nero AI dashboard.

### Getting Your API Key

1. Visit the [Nero AI Business](https://ai.nero.com/ai-api?utm_source=power_automate)
2. Sign in to your account
3. Navigate to the API section
4. Generate a new API key
5. Copy the key and use it in the connector

## Operations

### Create Image Task

Submits an image for AI-powered processing. Supports multiple task types:

**Parameters:**

- **Image URL**: Public HTTPS URL of the image to process
- **Type**: AI model type for the specific task:
  - **ImageUpscaler:Standard/Photograph/Anime/FaceEnhancement**: For image upscaling
  - **ColorizePhoto**: For black and white photo colorization
  - **BackgroundRemover**: For background removal
  - **ImageDenoiser**: For noise reduction
  - **ImageCompressor**: For image compression
- **Quality Factor**: Quality setting from 80-100 (default: 95, for upscaling only)
- **Upscaling Rate**: 2x or 4x scaling (default: 4x, for upscaling only)

**Returns:** Task ID for status monitoring

### Check Task Status

Monitors the progress of any image processing task and retrieves results.

**Parameters:**

- **Task ID**: Unique identifier from the Create Image Task operation

**Returns:** Current status, progress, and result URL when complete

## Usage Examples

### Basic Image Processing

1. **Submit Image**: Use "Create Image Task" action with your image URL and desired task type
2. **Monitor Progress**: Use "Check Task Status" to track processing
3. **Retrieve Result**: Get the final processed image URL when complete

### Task Type Examples

#### Image Upscaling
- Use quality factor 95-100 for professional results
- Use quality factor 80-90 for faster processing
- 4x scaling provides maximum detail enhancement
- 2x scaling offers balanced quality and speed

#### Photo Colorization
- Perfect for historical photos and family albums
- Automatically detects and applies appropriate colors
- Maintains original image quality and details

#### Background Removal
- Ideal for product photography and portraits
- Creates clean, professional-looking images
- Supports various image formats including HEIC/HEIF

#### Image Denoising
- Reduces grain and noise in low-light photos
- Improves overall image clarity and quality
- Works with both standard and advanced image formats

#### Image Compression
- Optimizes file size for web and storage
- Maintains visual quality while reducing bandwidth
- Perfect for bulk image processing workflows

#### Face Restoration
- Enhances facial features and details
- Restores damaged or low-quality portrait photos
- Professional-grade results for personal and commercial use

## Best Practices

1. **Image Preparation**: Ensure images are in supported formats (JPEG, PNG, WEBP, HEIC, HEIF)
2. **URL Accessibility**: Use publicly accessible HTTPS URLs
3. **Processing Time**: Allow sufficient time for high-quality processing (varies by task type)
4. **Error Handling**: Implement retry logic for failed tasks
5. **Resource Management**: Monitor API usage and rate limits
6. **Task Type Selection**: Choose the appropriate AI model for your specific use case
7. **Image Quality**: Use high-quality source images for best results

## Rate Limits

- **API Rate Limit**: 10 requests per second for all task operations
- **Plan Limits**: Vary by subscription tier
- **Enterprise Plans**: Custom limits available

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

- Initial release with comprehensive AI image processing capabilities
- **Image Upscaling**: Support for 2x and 4x upscaling with multiple AI models
- **Photo Colorization**: AI-powered black and white to color conversion
- **Background Removal**: Intelligent background removal for various image types
- **Image Denoising**: Advanced noise reduction and quality enhancement
- **Image Compression**: Smart compression while maintaining visual quality
- **Face Restoration**: Professional-grade facial feature enhancement
- Multiple AI model types for specialized processing
- Quality factor control for upscaling operations
- Asynchronous task processing with real-time status monitoring
- Support for advanced image formats (HEIC, HEIF)

## License

This connector is provided by Nero AG under the Microsoft Independent Publisher program.
