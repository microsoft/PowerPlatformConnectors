
## Cloudmersive Data Validation Connector
The [data validation APIs](https://cloudmersive.com/validate-api) help you validate data. Check if an E-mail address is real. Check if a domain is real. Check up on an IP address, and even where it is located. All this and much more is available in the validation API.


## Pre-requisites
N/A


## API documentation
[Cloudmersive Data Validation API Documentation](https://api.cloudmersive.com/docs/validate.asp) is available on the Cloudmersive website


## Supported Operations
### Fully validate an email address	
Performs a full validation of the email address. Checks for syntactic correctness, identifies the mail server in question if any, and then contacts the email server to validate the existence of the account - without sending any emails.

### Geolocate an IP address	
Identify an IP address Country, State/Provence, City, Zip/Postal Code, etc. Useful for security and UX applications.

### Get the gender of a first name	
Determines the gender of a first name (given name)

### Get WHOIS information for a domain	
Validate whether a domain name exists, and also return the full WHOIS record for that domain name. WHOIS records include all the registration details of the domain name, such as information about the domain's owners.

### Lookup a VAT code	
Checks if a VAT code is valid, and if it is, returns more information about it

### Parse an HTTP User-Agent string, identify robots	
Uses a parsing system and database to parse the User-Agent into its structured component parts, such as Browser, Browser Version, Browser Engine, Operating System, and importantly, Robot identification.

### Parse an unstructured input text string into an international, formatted address	
Uses machine learning and Natural Language Processing (NLP) to handle a wide array of cases, including non-standard and unstructured address strings across a wide array of countries and address formatting norms.

### Parse and validate a full name	
Parses a full name string (e.g. "Mr. Jon van der Waal Jr.") into its component parts (and returns these component parts), and then validates whether it is a valid name string or not

### Validate a code identifier	
Determines if the input name is a valid technical / code identifier. Configure input rules such as whether whitespace, hyphens, underscores, etc. are allowed. For example, a valid identifier might be "helloWorld" but not "hello*World".

### Validate a domain name	
Check whether a domain name is valid or not. API performs a live validation by contacting DNS services to validate the existence of the domain name.

### Validate a first name	
Determines if a string is a valid first name (given name)

### Validate a last name	
Determines if a string is a valid last name (surname)

### Validate a URL fully	
Validate whether a URL is syntactically valid (does not check endpoint for validity), whether it exists, and whether the endpoint is up and passes virus scan checks. Accepts various types of input and produces a well-formed URL as output.

### Validate a URL syntactically	
Validate whether a URL is syntactically valid (does not check endpoint for validity). Accepts various types of input and produces a well-formed URL as output.

### Validate phone number (basic)	
Validate a phone number by analyzing the syntax


## How to get credentials
To use this connector, you need a Cloudmersive account.  You can sign up with a Microsoft Account or create a Cloudmersive account.  Follow the steps below to get your API Key.

### Get the API Key and Secret
- [Register](https://account.cloudmersive.com/signup) for a Cloudmersive Account
- [Sign In](https://account.cloudmersive.com/login) with your Cloudmersive Account and click on API Keys

Here you can create and see your API key(s) listed on the API Keys page.  Simply copy and paste this API Key into the Cloudmersive Document Convert Connector.

Now you are ready to start using the Cloudmersive Document Convert Connector.


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

