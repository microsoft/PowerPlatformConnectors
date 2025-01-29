# Amazon S3 Bucket

Amazon Simple Storage Service (Amazon S3) is an object storage service.

## Publisher

Michael Megel

## Prerequisites

You will need the following to proceed:

* You must have an AWS account.
* You need an IAM (Identity and Access Management) user or role with appropriate permissions to access the S3 bucket.
* You need the AWS Access Key ID and AWS Secret Access Key of the IAM user or role.
* You need the name of the S3 bucket you want to access.

## Obtaining Credentials

Use your AWS Access Key ID as the username and AWS Access Key Secret as the password. AWS Access Key ID and AWS Access Key Secret are used to authenticate the request to AWS. For more information, see [AWS Signature Version 4](#aws-signature-version-4).

## Supported Operations

AWS S3 Rest API operations is documented here: [AWS S3 Rest API Documentation](https://docs.aws.amazon.com/AmazonS3/latest/API/API_Operations.html)

This connector supports the following operations:

* [List Objects v2](https://docs.aws.amazon.com/AmazonS3/latest/API/API_ListObjectsV2.html) 
* [Get Object](https://docs.aws.amazon.com/AmazonS3/latest/API/API_GetObject.html)
* [Put Object](https://docs.aws.amazon.com/AmazonS3/latest/API/API_PutObject.html)
* [Delete Object](https://docs.aws.amazon.com/AmazonS3/latest/API/API_DeleteObject.html)

## Known Issues and Limitations

* List Objects v2
  * The maximum number of objects returned is 1000.
* Get Object
  * The content is returned as a string.
* Put Object  
  * Large file requests might run into timeout issues such as HTTP 500 "Request to the backend service timed out". This is caused by custom connector script that creates the AWS Signature Version 4. The script must be finished within 5 seconds. For large files, the script might take longer than 5 seconds to finish. ([Microsoft FAQ: Script must be finished within 5 seconds](https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code#custom-code-faq))

### Fixed Issues

* Put Object
  * The connector now supports files in sub folders (`folder1/folder2/myfile.csv`).
  * The connector now supports filenames with characters such as spaces (`folder 1/my file.csv`).
  * The connector now supports binary content such as PDF files.
    *Note: Large files might run into timeout issues.([Microsoft FAQ: Script must be finished within 5 seconds](https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code#custom-code-faq))

## AWS Signature Version 4

The AWS Signature Version 4 is used to authenticate requests to AWS services. The signature is calculated based on the following documentation:

* [Authenticating Requests (AWS Signature Version 4)](https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-authenticating-requests.html)
* [AWS Signature Version 4 C# example for Amazon S3 Buckets](https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-examples-using-sdks.html)
