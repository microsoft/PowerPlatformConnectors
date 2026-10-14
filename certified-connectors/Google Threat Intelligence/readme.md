# Google Threat Intelligence

This connector allows you to integrate the power of Google Threat Intelligence (GTI) directly into your Microsoft Power Automate flows and Power Apps applications. Leverage GTI's vast knowledge base and analysis capabilities to enhance your security workflows and automate threat response.

## Publisher: Google

## With this connector, you can:

- Analyze suspicious files and URLs: Submit files and URLs to GTI for analysis, retrieving comprehensive reports on potential threats.
- Detect various types of malware: Identify a wide range of malware, including viruses, trojans, ransomware, and more.
- Automatically share threat information: Contribute to the security community by automatically sharing analyzed files and URLs with GTI.
- Streamline security workflows: Automate tasks like malware analysis, and incident response within your Power Platform applications.
- Enhance threat intelligence: Integrate GTI's rich threat data into your existing security systems and processes.

## Pre-requisites

To use this integration, you need:

A GTI account: Follow the steps on https://developers.virustotal.com/v3.0/reference#getting-started to get your API key.

Microsoft Power Platform environment: Access to Power Automate or Power Apps to create and deploy flows or applications.

API documentation
For detailed information on the GTI API, please refer to the official documentation: https://developers.virustotal.com/v3.0/reference

Deployment instructions
To deploy this connector as a custom connector in Microsoft Power Automate and Power Apps, follow these instructions: https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli   

## Supported Operations

This document provides detailed descriptions of all actions available in the Google Threat Intelligence connector for Microsoft Power Platform.

### File Analysis

* **Upload File**

    * **Description:** Submits a file to VirusTotal for analysis. This action initiates the analysis process and returns a unique identifier for tracking the analysis progress and retrieving the report later.
    * **Input:** File content (binary data).
    * **Output:** Analysis ID (string).

* **Get File Report**

    * **Description:** Retrieves a detailed analysis report for a specific file identified by its unique ID (SHA-256 hash). The report includes information such as antivirus detection rates, community ratings, behavioral analysis, and more.
    * **Input:** File ID (string).
    * **Output:** File analysis report (object).


### URL Analysis

* **Analyze URL**

    * **Description:**  Submits a URL to VirusTotal for analysis. This action starts the analysis process and returns a unique identifier for tracking the analysis progress and retrieving the report.
    * **Input:** URL (string).
    * **Output:** Analysis ID (string).

* **Get URL Report**

    * **Description:** Retrieves an analysis report for a specific URL. The report contains information such as website safety ratings, malware detection results, phishing indicators, and more.
    * **Input:** URL (string).
    * **Output:** URL analysis report (object).


### IP Address Analysis

* **Get IP Report**

    * **Description:** Retrieves analysis and reputation information for a given IP address. This includes details like geolocation, associated domain names, malware connections, and security assessments.
    * **Input:** IP address (string).
    * **Output:** IP address report (object).


### Domain Analysis

* **Get Domain Report**

    * **Description:** Retrieves analysis and reputation information for a specified domain name. The report includes details such as registration information, associated IP addresses, malware distribution history, and security assessments.
    * **Input:** Domain name (string).
    * **Output:** Domain report (object).


### Analysis Retrieval

* **Retrieve URL/File Analysis**

    * **Description:**  Retrieves information about a specific file or URL analysis request. This action allows you to check the status of an ongoing analysis or fetch the results of a completed analysis.
    * **Input:** Analysis ID (string).
    * **Output:** Analysis information (object) including status and results.

### Threat List

* **Retrieve Threat Lists**

    * **Description:**  Retrieves information about a Threats given a timestamp and a category name. 
    * **Input:** Category (string) // Timestamp (YYYYMMDDHH).
    * **Output:** List of Threats in STIX format.

**Note:** All actions require an API key for authentication.  Please refer to the "Pre-requisites" section in the `intro.md` file for instructions on obtaining and using your API key.

## Obtaining Credentials

https://docs.virustotal.com/docs/please-give-me-an-api-key

## Known Issues and Limitations

N/A