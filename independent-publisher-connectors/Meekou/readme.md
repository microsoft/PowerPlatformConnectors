
# Meekou Connector

Meekou connector provides a powerful base functions. Using this connector, you can focus on implementing business logic without write duplicate operation to do basic operations.

## Publisher: Meekou

## Prerequisites

You will need the following to proceed:

* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools

## Supported Operations

### Regex

use Regex to extract value from content



### ImageInfo
  
get image base information like height, width and size by image base64

* PowerApps get image info
  
```yaml
Set(
    ImageInfo,
    Meekou.ImageInfo(
        {
            imageBase64: Substitute(
                JSON(
                    Image1.Image,
                    JSONFormat.IncludeBinaryData
                ),
                """",
                ""
            )
        }
    )
);
```

![imageinfo-powerapps](http://qiniu.meekou.cn/Blogs/imageinfo-powerapps.png)

* Roundup

get number round up value

* SharePoint.BatchRequest.Build

build sharepoint request body for batch operation

* Array.Map

create a new array based on existing array and property path

## Obtaining Credentials

No Authenticate required.

## Getting Started

Todo.

## Known Issues and Limitations

Todo.

## Frequently Asked Questions

To do

## Deployment Instructions

To Do.
