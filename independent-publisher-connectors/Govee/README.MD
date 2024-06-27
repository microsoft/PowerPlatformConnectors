# Govee
Offers seamless integration with [Govee's](https://us.govee.com) smart lighting systems, enabling users to effortlessly control and manage their devices. It allows for a wide range of functionalities including adjusting light settings, changing colors, modifying brightness, and accessing real-time device status.

The ability to automate and customize lighting setups makes this connector particularly useful for enhancing ambiance in smart homes, creating dynamic lighting scenes for various occasions, and integrating smart lighting into broader home automation systems. It offers a convenient and intuitive way for users to personalize their lighting experience and fully leverage the capabilities of Govee's smart lighting technology.

## Publisher: Richard Wilson

## Prerequisites

Before you can use the Govee connector, you must obtain a Govee Developer API Key. Follow these steps to acquire the key:

### Obtaining a Govee Developer API Key

1. **Download the Govee Home App:**
   - For iOS: [Govee Home on the App Store](https://apps.apple.com/us/app/govee-home/id1395696823)
   - For Android: [Govee Home on Google Play](https://play.google.com/store/apps/details?id=com.govee.home)

2. **Access Your Profile:**
   - Open the Govee Home App and navigate to the 'My Profile' page by tapping the profile icon (👤).

3. **Go to Settings:**
   - Click on the settings icon (⚙️) located at the top right corner to access the Settings menu.

4. **Apply for API Key:**
   - Select 'Apply for API Key' within the Settings menu.

5. **Complete Application Form:**
   - Fill in the required fields for 'Name' and 'Reason for application'.
     - Example reasons include home automation, third-party integration, or educational purposes.

6. **Agree to Terms of Service:**
   - Carefully read the Govee Developer API Terms of Service. Once you have understood the terms, check the box to indicate your acceptance.

7. **Submit Application:**
   - Click 'Submit' to send your application for an API key. Once approved, you will receive your Govee API Key, which is necessary to use this connector.

Please be aware that the API key approval process may take some time, and approval is at the discretion of Govee. Ensure that your application includes a valid and clear reason for requesting the API key to increase the likelihood of approval.

## Supported Operations

### 1. Run Command on Device

This operation allows you to send various commands to control your Govee device.

- **Input Properties:**
  - **Device MAC Address:** The unique MAC address of the Govee device to be controlled.
  - **Device Model:** The model number of the Govee device.
  - **Command Details:** An object containing the command to be executed. It includes:
    - **Name:** The command name (e.g., 'turn', 'brightness', 'color', 'colorTem').
  - Additional Parameters (based on the command):
    - **Turn:** (Optional) 'on' or 'off' - used when the command is 'turn'.
    - **Brightness:** (Optional) Numeric value (0-100) - used when the command is 'brightness'.
    - **Color:** (Optional) Object with 'r', 'g', 'b' values - used when the command is 'color'.
    - **Color Temperature:** (Optional) Numeric value for color temperature - used when the command is 'colorTem'.

### 2. Get Device Information

Retrieve information about a specific Govee device or all devices associated with your account.

- **Input Properties:**
  - **Device MAC Address:** (Optional) The MAC address of the specific Govee device for which information is being requested. If omitted, information for all devices is returned.
  - **Device Model:** (Optional) The model of the Govee device. If specified along with the MAC address, it refines the query to that specific device.

## API Documentation
For detailed information about the Govee Lights Developer API, including its capabilities, endpoints, parameters, and responses, refer to the official [Govee API Documentation](https://govee-public.s3.amazonaws.com/developer-docs/GoveeDeveloperAPIReference.pdf). The documentation provides comprehensive guidance and examples to help you effectively integrate and utilize the API within your applications and workflows.

## Known Issues and Limitations
Currently, no known issues or limitations exist. Always refer to this section for updated information.