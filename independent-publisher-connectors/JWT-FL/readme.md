# JWT-FL
Generates a signed JSON Web Token using your private key


## Publisher: Farhan Latif

## Prerequisites
`NIL`


## Supported Operations

- ## **`1) Create a Signed JWT Token`**
  - *HTTP Verb:* `POST`
  - *Description:* Generates a JWT token signed with your key

<img width="497" alt="Create a Signed JWT Token" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/3b8b1b12-a708-49bd-9b95-8f24a6d17990">
<br><br>


>[!IMPORTANT]
> 1) Please refrain from including the JWT `exp` (Expiration Time) or `iat` (Issued At) values in the `Payload | Content`. To simplify the process for developers, these values are automatically included in all JWT tokens. The `Expiry | Minutes` field is provided for you to specify the desired token validity duration in minutes
> 2) The `Your JWT Header` and `Payload | Content` fields require a map/dictionary value. As this datatype is not directly supported, you would typically need to use the `parseJson` connector each time, which can be cumbersome. To alleviate this, you can input these values as a single string. This string will be converted into a map/dictionary on the backend. For improved readability, you can include spaces between strings; the backend will structure it accordingly
> <br> For example, you can write <br><br> `alg: ES256, kid:   abcd,   typ: JWT, ` or <br><br> `alg:ES256, kid:   abcd,typ: JWT` or <br><br> `alg: ES256, kid:abcd, typ:JWT`<br><br> You have the flexibility to write the header and payload values in a manner that suits you best, and it will be appropriately converted

<br>


 ## **`2) Validate JWT Token`**
  - *HTTP Verb:* `POST`
  - *Description:* This method performs a validation check on the JWT token based on its `exp` (Expiration Time). It returns a boolean value indicating the validity of the token and provides the remaining duration in minutes before the token expires

## Known Issues and Limitations
   At the current stage `NONE`


## Support
For any suggestions, collaborations, or ideas related to the JWT-FL connector, please feel free to [contact me](farhanlatif027@outlook.com). When reaching out, kindly use one of the following subjects for a more efficient response:

- `JWT-FL connector | Suggestion`: If you have any suggestions for improvements or enhancements.
- `JWT-FL connector | Collaboration`: If you’re interested in collaborating on this project.
- `JWT-FL connector | Idea`: If you have a new idea or concept that you’d like to discuss.





