# Bitskout
Create smart plugins powered by A.I. and automatically extract information from files, emails, documents, etc. to your tasks and write data to your tools. Save more than 5 hours per week on typing data into tools.

# Publisher
**Name**: Bitskout
**Email**: admin@bitskout.com

# Prerequisites
To run the Bitskout plugins you have to do the following: 
1. Create an Account at Bitskout. You can sign up [here](https://app.bitskout.com/signup)
2. Get your Bitskout API Key. Login to Bitskout, go to your Profile and select **Tokens and Passwords** tab. Click **Generate Token** button to generate your API Key  

# API Limits
There is 14 days trial. After that you need to purchase a specific plan. 

# Supported Operations

## List Plugins
This action retrieves a list of your Bitskout plugins

## Run Plugin for a File
This action is used to run the specific Bitskout plugin for a Provided file. 

### Input

This action expects two parameters: 
* **plugin** - Plugin ID that user can select from the Dropdown list
* **file_url** - a link to download file. Please note that this should be **a direct download link** because Bitskout needs to download this file and pass it to the further processing. Please also note that **Share Link** is not always a Direct Download link

### Output
This action returns a JSON-encoded object of the following structure:
```
{
    "outputs": {
        "<key1>": "<value1>",
        "<key2>": "<value2>",
        ...
        "<key3>": "<value3>",
    }
}
```

**key** and **value** may differ based on the Plugin being run

## Run Plugin for a Text
This action is used to run the Bitskout plugin for a provided text. 

### Input
This action expects two parameters: 
* **plugin** - Plugin ID that user can select from the Dropdown list
* **text** - a text you want to your plugin for

### Output
This action returns a JSON-encoded object of the following structure:
```
{
    "outputs": {
        "<key1>": "<value1>",
        "<key2>": "<value2>",
        ...
        "<key3>": "<value3>",
    }
}
```

**key** and **value** may differ based on the Plugin being run

# Issues and Limitations
The number of times you can run the Bitskout plugin is limited by your plan. Please check [this](https://www.bitskout.com/pricing) page