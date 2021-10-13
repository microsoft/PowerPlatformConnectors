{
  "swagger": "2.0",
  "info": {
    "title": "IN-D Aadhaar Number Masking",
    "description": "Eliminate risks and ensure compliance by redacting first 8 digits of Aadhaar Number and QR Code from Aadhaar images",
    "version": "1.0",
    "contact": {
      "name": "IN-D Support",
      "url": "https://www.one.in-d.ai/developer/",
      "email": "explore@in-d.ai"
    }
  },
  "host": "vkyc.in-d.ai",
  "basePath": "/",
  "schemes": [
    "https"
  ],
  "consumes": [],
  "produces": [],
  "paths": {
    "/AadhaarMask": {
      "post": {
        "responses": {
          "200": {
            "description": "Success",
            "schema": {
              "type": "object",
              "properties": {
                "desc": {
                  "type": "string",
                  "description": "Error Description",
                  "title": "Success"
                },
                "result": {
                  "type": "string",
                  "description": "Base64 string of Masked Aadhaar",
                  "title": "Masked Aadhaar O/P"
                },
                "status": {
                  "type": "string",
                  "description": "Status of request either success or fail",
                  "title": "Status of request"
                }
              }
            }
          },
          "207": {
            "description": "Error in saving image",
            "schema": {
              "type": "object",
              "properties": {
                "desc": {
                  "type": "string",
                  "description": "Error description",
                  "title": "Description"
                },
                "flag": {
                  "type": "string",
                  "description": "Request Fail or Success",
                  "title": "Flag"
                },
                "status": {
                  "type": "string",
                  "description": "Request status either success or fail",
                  "title": "Status"
                }
              }
            }
          },
          "400": {
            "description": "Invalid base64 format",
            "schema": {
              "type": "object",
              "properties": {
                "status": {
                  "type": "string",
                  "description": "Request status either success or fail",
                  "title": "Status"
                },
                "desc": {
                  "type": "string",
                  "description": "Invalid base64 string",
                  "title": "Error Desctription"
                }
              }
            }
          },
          "401": {
            "description": "API Key expired or invlid",
            "schema": {
              "type": "object",
              "properties": {
                "message": {
                  "type": "string",
                  "description": "API key Invalid",
                  "title": "API Key Invalid"
                }
              }
            }
          },
          "403": {
            "description": "Authorization headers or api key not given",
            "schema": {
              "type": "object",
              "properties": {
                "message": {
                  "type": "string",
                  "description": "API Key Not Given",
                  "title": "API Key Missing"
                }
              }
            }
          }
        },
        "summary": "Aadhaar Number Masking",
        "description": "Eliminate risks and ensure compliance by redacting first 8 digits of Aadhaar No. from Aadhaar images",
        "operationId": "Aadharredaction",
        "parameters": [
          {
            "name": "Content-Type",
            "in": "header",
            "required": true,
            "type": "string",
            "default": "application/json",
            "description": "Content type of request",
            "x-ms-summary": "content type"
          },
          {
            "name": "body",
            "in": "body",
            "required": true,
            "schema": {
              "type": "object",
              "properties": {
                "image": {
                  "type": "string",
                  "description": "image",
                  "title": "base64",
                  "format": "byte"
                }
              },
              "required": [
                "image"
              ]
            }
          }
        ]
      }
    }
  },
  "x-ms-connector-metadata": [
    {
      "propertyName": "Website",
      "propertyValue": "https://www.in-d.ai"
    },
    {
      "propertyName": "Privacy policy",
      "propertyValue": "https://www.in-d.ai/privacy-policy"
    },
    {
      "propertyName": "Categories",
      "propertyValue": "AI;IT Operations"
    }
  ],
  "definitions": {},
  "parameters": {},
  "responses": {},
  "securityDefinitions": {
    "API Key": {
      "type": "apiKey",
      "in": "header",
      "name": "x-api-key"
    }
  },
  "security": [
    {
      "API Key": []
    }
  ],
  "tags": []
}
