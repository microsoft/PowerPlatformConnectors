# SOS Inventory 
SOS Inventory is an ERP system published by Saddle Oak Software.

# Publisher:  Harold Anderson (not affilitated with Saddle Oak Software)


# Getting Started

1.	You will need an account at SOSInventory.com.  
2.  Register for a developer account at developer.sosinventory.com
3.  Generate a Client ID and Secret key for a new app.
4.  Add ClientID and your desired resource group location to a parameters.json file.   I have provide an example json file for you.
5.  Deploy the Azure Resource Manager template for the custom connector (the file template.json) to Azure with this command:
```
    az deployment group create  --resource-group my-resource-group --template-file template.json --parameters parameters.json
```

# Disclaimer 

I am not associated with Saddle Oak Software, who produces SOS Inventory.  This software is provided as-is, with no guarantee.  Use this software at your own risk.  
