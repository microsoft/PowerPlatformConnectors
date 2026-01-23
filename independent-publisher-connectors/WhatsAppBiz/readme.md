# WhatsApp Business (Independent Publisher)

WhatsApp Business Cloud API connector enables businesses to communicate with customers through WhatsApp. Send text messages, images, documents, locations, templates, and interactive messages directly from Power Automate, Power Apps, and Logic Apps.

## Publisher: Your Name

## Prerequisites

To use this connector, you need:

1. **Meta Developer Account** - Register at [Meta for Developers](https://developers.facebook.com/)
2. **Meta Business Account** - Create or link a business account in [Meta Business Suite](https://business.facebook.com/)
3. **WhatsApp Business App** - Create a Business app in the Meta Developer Console
4. **Phone Number ID** - Register a phone number for WhatsApp Business
5. **Access Token** - Generate a permanent System User access token

## How to Get Credentials

### Step 1: Create a Meta Developer Account
1. Go to [developers.facebook.com](https://developers.facebook.com/)
2. Click "Get Started" and log in with your Facebook account
3. Complete the registration process

### Step 2: Create a Meta App
1. Go to [My Apps](https://developers.facebook.com/apps/)
2. Click "Create App"
3. Select "Business" as the app type
4. Enter your app name and contact email
5. Select or create a Business Portfolio
6. Click "Create App"

### Step 3: Add WhatsApp to Your App
1. In your app dashboard, scroll down to "Add products to your app"
2. Find "WhatsApp" and click "Set up"
3. This will create a WhatsApp Business Account (WABA) if you don't have one

### Step 4: Get Your Phone Number ID
1. In the left sidebar, go to WhatsApp > API Setup
2. You'll see a test phone number with its **Phone Number ID**
3. For production, register your own business phone number

### Step 5: Generate a Permanent Access Token

**For Development/Testing:**
1. In WhatsApp > API Setup, you'll see a temporary access token
2. This token expires in 24 hours - suitable only for testing

**For Production (Recommended):**
1. Go to Business Settings > Users > System Users
2. Create a new System User (Admin role recommended)
3. Click "Generate New Token"
4. Select your WhatsApp app
5. Add these permissions:
   - `whatsapp_business_messaging`
   - `whatsapp_business_management`
   - `business_management`
6. Generate and securely store your token

### Step 6: Verify Your Business (Required for Production)
1. Go to Business Settings > Security Center
2. Click "Start Verification"
3. Submit required business documents
4. Wait for Meta to verify your business (1-3 business days)

## Supported Operations

### Messages

| Operation | Description |
|-----------|-------------|
| **Send Text Message** | Send a plain text message to a WhatsApp user |
| **Send Template Message** | Send a pre-approved template message (works outside 24-hour window) |
| **Send Image Message** | Send an image with optional caption |
| **Send Document Message** | Send a PDF, DOC, or other document |
| **Send Location Message** | Send a location with coordinates and address |
| **Send Interactive Button Message** | Send a message with up to 3 reply buttons |
| **Mark Message as Read** | Mark a received message as read |

### Templates

| Operation | Description |
|-----------|-------------|
| **Get Message Templates** | List all message templates for your WABA |
| **Create Message Template** | Create a new template for approval |
| **Delete Message Template** | Delete an existing template |

### Phone Numbers

| Operation | Description |
|-----------|-------------|
| **Get Phone Number Info** | Get details about a registered phone number |

## Important Concepts

### 24-Hour Customer Service Window
- After a customer messages you, you have 24 hours to reply with **free-form messages** (text, media, etc.)
- Outside this window, you can only send **pre-approved template messages**
- Template messages can be sent anytime

### Message Templates
- Templates must be submitted for approval before use
- Approval typically takes 24-48 hours
- Categories: Marketing, Utility, Authentication
- Each category has different pricing

### Pricing
- WhatsApp Cloud API is free to set up
- You pay per conversation (not per message)
- Rates vary by country and conversation type
- First 1,000 service conversations per month are free

## Known Issues and Limitations

1. **Webhooks not supported** - This connector only supports outbound messaging. To receive messages, you need to set up webhooks separately.

2. **Media size limits**:
   - Images: 5 MB
   - Videos: 16 MB
   - Audio: 16 MB
   - Documents: 100 MB

3. **Rate limits**: Default 80 messages per second per phone number (can be increased)

4. **Template approval**: New templates require Meta approval (24-48 hours)

5. **Phone number registration**: Each phone number can only be registered with one WABA

## Frequently Asked Questions

**Q: Why can't I send messages to any number?**
A: During testing, you can only send to verified test numbers. For production, your business must be verified, and recipients must have opted in.

**Q: Why do I need templates?**
A: WhatsApp requires templates for business-initiated messages outside the 24-hour window to prevent spam.

**Q: Can I receive messages with this connector?**
A: This connector supports sending only. To receive messages, set up webhooks on your server.

**Q: How do I get a permanent access token?**
A: Create a System User in Business Settings and generate a token with the required permissions.

## Getting Started

1. Create a connection using your Access Token
2. Use "Send Template Message" with the built-in `hello_world` template to test
3. For the Phone Number ID, use the value from WhatsApp > API Setup

### Example: Send Hello World Template

```
Phone Number ID: 123456789012345
To: 14155238886
Template Name: hello_world
Language Code: en_US
```

## Useful Links

- [WhatsApp Business Platform Documentation](https://developers.facebook.com/docs/whatsapp)
- [Cloud API Overview](https://developers.facebook.com/docs/whatsapp/cloud-api)
- [Message Templates Guide](https://developers.facebook.com/docs/whatsapp/message-templates)
- [WhatsApp Business Policy](https://www.whatsapp.com/legal/business-policy)
- [Pricing Information](https://developers.facebook.com/docs/whatsapp/pricing)

## Deployment Instructions

1. Clone the connector files to your environment
2. Import as a custom connector in Power Platform
3. Create a connection using your Access Token
4. Test with the hello_world template

---

*This is an independent publisher connector. This connector is not affiliated with, endorsed by, or supported by Meta Platforms, Inc.*
