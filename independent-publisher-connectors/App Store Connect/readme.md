# App Store Connect
App store connect connector allows you to automate tasks and workflows for your app store submissions and updates. You can use it to create, edit, and manage metadata, screenshots, in-app purchases, and more. With this tool, you can save time and avoid errors by streamlining your app store processes.

## Publisher: Farhan Latif

## Prerequisites
#### Requires `Private API KEY` & `JSON Web Tokens (JWTs) signed with your private key`
<br>


## Obtaining Credentials
# `Private API KEY`
> [!IMPORTANT]
> The App Store Connect API requires a JSON Web Token (JWT) to authorize each request you make to the API. You generate JWTs using an API key downloaded from App Store Connect.
> <br><br>**There are two types of API keys:**<br> - `Team` Access to all apps, with varying levels of access based on selected roles.<br>- `Individual` Access and roles of the associated user.

## Generate a Team Key and Assign It a Role

When you create an API key, assign it a role that determines the key’s access to areas of the App Store Connect API and permissions for performing tasks. For example, keys with the Admin role have broad permissions and can do things like create new users and delete users. Team API keys can access all apps, regardless of their role. The roles that apply to keys are the same roles that apply to users on your team; for more information, see Program Roles.

> [!NOTE] Team keys give access that’s not isolated to a single app, but individual key access is tied to the apps and permissions of the user.

To generate team keys, you must have an Admin account in App Store Connect. You can generate multiple API keys with any roles you choose.

To generate a team API key to use with the App Store Connect API, log in to [App Store Connect](https://appstoreconnect.apple.com/) and:
1) Select `Users and Access`, and then select the `API Keys` tab.<br><br>
2) Make sure the `Team Keys` tab is selected.<br><br>
3) Click `Generate API Key` or the Add `(+)` button.<br><br>
4) Enter a `name for the key`. The name is for your reference only and isn’t part of the key itself.<br><br>
5) Under Access, select the `role for the key`.<br><br>
5) Click `Generate`.<br><br>

The new key’s name, key ID, a download link, and other information appears on the page.

## Download and Store a Team Private Key
Once you’ve generated your API key, you can download the private half of the key. The private key is available for download a single time, to begin log in to App Store Connect and:
1) Select `Users and Access`, and then select the `API Keys` tab.
2) Click `Download API Key` link next to the new API key.<br><br>
The download link only appears if you haven’t downloaded the private key. Apple doesn’t keep a copy of the private key.

>[!IMPORTANT] Keep your API keys secure and private. Don’t share your keys, store keys in a code repository, or include keys in client-side code. If the key becomes lost or compromised, remember to revoke it immediately. See [Revoking API Keys](https://developer.apple.com/documentation/appstoreconnectapi/revoking_api_keys) for more information.

## Generate an Individual Key
To generate an individual API key, which has access and permissions of the associated user, for the App Store Connect API, log in to [App Store Connect](https://appstoreconnect.apple.com/) and:
1) Go to a your `user profile`.
2) Scroll down to `Individual API Key`.
3) Click `Generate API Key`.
The key ID, a download link, and other information appears on the page.

> [!NOTE]
> If you don’t have the Generate Individual API Keys permission, the Generate API Key button won’t show on your profile. A team Admin can grant you this permission.
> 
## Download and Store an Individual Private Key
Once you’ve generated your API key, you can download the private half of the key. The private key is available for download a single time, to begin log in to [App Store Connect](https://appstoreconnect.apple.com/) and:
1) Go to your `user profile`.
2) Scroll down to `Individual API Key`.
3) Click `Download API Key link`.
The download link only appears if you haven’t downloaded the private key. Apple doesn’t keep a copy of the private key.

# `JWT Token`
The App Store Connect API requires JWTs to authorize each API request. You create the token, and sign it with the private key you downloaded from App Store Connect.
To generate a signed JWT:
1) Create the `JWT header`.
2) Create the `JWT payload`.
3) Sign the `JWT`.<br><br>
 
Include the signed JWT in the authorization header of each App Store Connect API request.

> [!IMPORTANT]
> Adhere to the specified instructions for creating a JSON Web Token (JWT) outlined in [these guidelines](https://developer.apple.com/documentation/appstoreconnectapi/generating_tokens_for_api_requests)<br>You can use the libraries listed [here to create your JWT](https://jwt.io/libraries).

### Example Python Code
```from jwcrypto import jwt, jwk
import time

# Get the current time in seconds (issued at)
iat = int(time.time())

# Get the time 15 minutes from now in seconds (expiration time)
# Tokens that expire more than 20 minutes into the future are not valid except
# for resources listed in Determine the Appropriate Token Lifetime
exp = iat + (15 * 60)

print(f"iat: {iat}, exp: {exp}")

# Load the private key from a file
with open("YOUR_FILE_NAME.p8", "rb") as f:
    key = jwk.JWK.from_pem(f.read())

# Set the header fields
header = {
    "alg": "ES256", # Use the ES256 algorithm
    "kid": "KEY_ID_VALUE", # Use the key ID from App Store Connect
    "typ": "JWT" # Set the token type to JWT
}

# Set the payload fields
payload = {
    "iss": "ISSUER_ID_VALUE", # Use the issuer ID from App Store Connect
    "iat": iat,
    "exp": exp, # Set the expiration time to 20 minutes from now,
    "aud": "appstoreconnect-v1" ,# Set the audience to App Store Connect
    #scope:[
        #Add Scopes as per your needs e.g "GET /v1/apps?filter[platform]=IOS"  ]


}

# Create the JWT object
token = jwt.JWT(header=header, claims=payload)

# Sign the token with the private key¹[1]
token.make_signed_token(key)


# Get the signed token as a string
signed_token = token.serialize()

# Print the signed token
print("Bearer {}".format(signed_token))

```
> [!NOTE]
> In Custom Connector only use the `signed_token` value for `JWT Token | Put JWTs Value Only`


## Supported Operations

### Apps and App Metadata
 `Getting and modifying app information`

- **`List Apps`**
  - *HTTP Verb:* `GET`
  - *Description:* Find and list apps in App Store Connect.

- **`Read App Information`**
  - *HTTP Verb:* `GET`
  - *Description:* Get information about a specific app.



## Known Issues and Limitations
   At the current stage `NONE`

## Personal Note
> [!NOTE]
> [Personal Limitation] This connector provides access to a large and complex API that requires careful description, validation and testing of each method. Therefore, I am gradually adding more methods to the connector as I work on them. I am very passionate about this project and I hope to offer a comprehensive and reliable connector for the API users.



## API documentation
[Apple's Developer Documentation](https://developer.apple.com/documentation/appstoreconnectapi/app_store/apps)
