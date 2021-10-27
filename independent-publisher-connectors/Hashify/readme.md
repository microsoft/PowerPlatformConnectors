# Hashify
Hashify is a basic web application and public-accessible hashing API to generate hash digests of plain-text input or small file input to various output encodings such as hex, base32, and base64.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Hashify was made by [Darwin Smith II](https://dlsmi.com/) and is a public-accessible API with no subscription required. Hashify does not log hash inputs or digests, but IPs are temporarily cached, but not written to disk, for abuse prevention.

## Supported Operations
### Status
#### Check API status
Get current API status.
### Methods
#### Get methods
Retrieve list of hash options contains names, endpoints, and if key needed.
### MD4
#### Get MD4
Request a MD4 digest.
#### Post MD4
Create a MD4 digest.
#### Post an array for MD4
Create a MD5 digest from an array.
### MD5
#### Get MD5
Request a MD5 digest.
#### Post MD5
Create a MD5 digest.
#### Post an array for MD5
Create a MD5 digest from an array.
### Highway
#### Get Highway64 from random
Request a Highway64 digest from a random key.
#### Post Highway64
Create a Highway64 digest.
#### Post Highway64 from random
Create a Highway64 digest from a random key.
#### Get Highway128
Request a Highway128 digest.
#### Get Highway128 from random
Request a Highway128 digest from a random key.
#### Post Highway128 from random
Create a Highway128 digest from a random key.
#### Get Highway256 from random
Request a Highway256 digest from a random key.
#### Post Highway256
Create a Highway256 digest.
####Post Highway256 from random
Create a Highway256 digest from a random key.
### SHA
#### Get SHA1
Request a SHA1 digest.
#### Post SHA1 from an array
Create a SHA1 digest from an array.
#### Get SHA256
Request a SHA256 digest.
#### Post SHA256 from body
Create a SHA256 digest from a body query parameter.

## Obtaining Credentials
There are no credentials needed for this API.

## Known Issues and Limitations
This API does not have a 100% uptime. Please make use of the status action and/or make accommodations for error responses.
