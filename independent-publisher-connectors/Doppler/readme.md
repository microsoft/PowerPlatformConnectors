# Doppler Secrets
Doppler Secrets is a secrets management platform for developers and teams. It allows efficient handling of sensitive data like API keys and tokens across projects. It offers version control, environment-specific configurations, and integrations, simplifying secret management. The connector enables automation of secret, config, and project management, suitable for both small and large applications.

## Publisher: Farhan Latif

## Prerequisites
> [!IMPORTANT]
> To access the following features, you'll need Doppler's "Team" subscription plan and the "Custom Roles" add-on:

1. **Secrets:**
   - Update Secret's Note <br>
<br>- **NOTE**<br>
  The actions of listing, updating, retrieving, and deleting are accessible with a free developer account

2. **Config:**
   - Create/Update/Clone/Lock/Unlock Config

3. **Project:**
   - Create/Delete/Update

4. **Project Roles:**
   - List/Create/Retrieve/Delete

5. **Project Members:**
   - List/Add/Retrieve/Delete/Update

## Supported Operations
# SECRETS

- **`List`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve a list of secrets.

- **`Retrieve`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve details of a specific secret.

- **`Delete`**
  - *HTTP Verb:* `DELETE`
  - *Description:* Delete a secret.

- **`Update`**
  - *HTTP Verb:* `POST`
  - *Description:* Update an existing secret.

- **`Update Note`**
  - *HTTP Verb:* `POST`
  - *Description:* Update the note associated with a secret.

# PROJECTS

- **`List`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve a list of projects.

- **`Create`**
  - *HTTP Verb:* `POST`
  - *Description:* Create a new project.

- **`Retrieve`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve details of a specific project.

- **`Update`**
  - *HTTP Verb:* `POST`
  - *Description:* Update an existing project.

# PROJECT ROLES

- **`List`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve a list of project roles.

- **`Retrieve`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve details of a specific project role.

- **`Update`**
  - *HTTP Verb:* `PATCH`
  - *Description:* Update an existing project role.

# PROJECT MEMBERS

- **`List`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve a list of project members.

- **`Retrieve`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieve details of a specific project member.

- **`Add`**
  - *HTTP Verb:* `POST`
  - *Description:* Add a new member to the project.

- **`Update`**
  - *HTTP Verb:* `PATCH`
  - *Description:* Update details of an existing project member.

- **`Delete`**
  - *HTTP Verb:* `DELETE`
  - *Description:* Remove a project member.


## Obtaining Credentials
- `Step 1 - Register Account`
You can create a free developer account by registering [here](https://dashboard.doppler.com/register)
- `Step 2 - Create Project e.g 'example-project'`
<img width="1330" alt="Creating Project" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/afe7a5af-a58b-4ac6-84ae-d0532ccb620f">
<br>
<br>


- `Step 3 - Select Config e.g 'dev'`
<img width="1330" alt="Selecting Config" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/d7d8428a-ff61-48f9-a1ef-3b689f118b3d">
<br>
<br>


- `Step 4 - Generate Service Token For That Config e.g 'dev'`
<img width="1160" alt="Generating Token" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/2c251a31-c116-49b6-b0eb-94982f7ea1c7">
<br>
<br>


- `Step 5 - Add The Generated Service Token To My Custom Connector`
<img width="1330" alt="Add To Custom Connector" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/eef00028-5fda-414d-b758-3f91cdabaa6b">
<br>
<br>



**Creating Teams Service Account Token**
- `Step 1 - Register Account`
You can create a free developer account by registering [here](https://dashboard.doppler.com/register)

- `Step 2 - Sign Up For Teams`
<img width="1440" alt="Signing Up For Teams" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/016596cc-99fb-45c1-94da-395deac42a43">
<br>
<br>



- `Step 3 - Create Service Account`
<img width="1440" alt="After Teams - Create Service Account" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/c0abb62f-98a9-42c0-905c-cd3e2ed1d860">
<br>
<br>



- `Step 4 - Get Into Your Service Account And Assign This Service Account Permissions`
> [!IMPORTANT]
> Follow The Principle Of Least Privilege

If your service account is allowed to do something, it can perform specific tasks. For example, if your service account has permission to work on projects, the token it generates can be used to do things like creating or deleting projects.
<img width="1440" alt="Get Into Your Service Account " src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/6c3618b6-8b7e-4b03-8bcc-2b4cc901a145">
<img width="1440" alt="Assign Permissions" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/069cc1f3-8016-4789-a860-93e1395430a3">
<br>
<br>



- `Step 5 - Add The Generated Service Token To My Custom Connector`
<img width="1440" alt="Add To Custom Connector" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/eef00028-5fda-414d-b758-3f91cdabaa6b">
<br>
<br>


## Known Issues and Limitations
*Secrets Update Secret Method*

To address the issue with the "Secrets Update Secret" method, make sure to provide a JSON value for the "secrets" property. To resolve this, you can utilize the "parseJson" Built-in Action. Set its body as the value for the "secrets" field.

for content copy the below:
>[!NOTE]
> replace the `Keys` with your `secret's name` and `values` with your `secret's updated value`

```
{
"NAME_OF_YOUR_SECRET_1": "Value1",
"NAME_OF_YOUR_SECRET_2": "Value2",
"NAME_OF_YOUR_SECRET_3": "Value3"
}

```

for schema copy the below:
```{
    "type": "object",
    "properties": {
        "type": {
            "type": "string"
        },
        "properties": {
            "type": "object",
            "properties": {
                "STRIPE": {
                    "type": "object",
                    "properties": {
                        "type": {
                            "type": "string"
                        },
                        "properties": {
                            "type": "object",
                            "properties": {
                                "raw": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                },
                                "computed": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                },
                                "note": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                "ALGOLIA": {
                    "type": "object",
                    "properties": {
                        "type": {
                            "type": "string"
                        },
                        "properties": {
                            "type": "object",
                            "properties": {
                                "raw": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                },
                                "computed": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                },
                                "note": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                "DATABASE": {
                    "type": "object",
                    "properties": {
                        "type": {
                            "type": "string"
                        },
                        "properties": {
                            "type": "object",
                            "properties": {
                                "raw": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                },
                                "computed": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                },
                                "note": {
                                    "type": "object",
                                    "properties": {
                                        "type": {
                                            "type": "string"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```
<img width="1162" alt="Schema" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/d2d2cc7e-832d-4ae7-bf02-c264b057ac44">
<img width="1162" alt="Adding Json" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/7a044c5f-7137-4287-aee5-b1308a65a59b">
<img width="1162" alt="Flow Success" src="https://github.com/microsoft/PowerPlatformConnectors/assets/82877513/443fac1d-c9ba-4ee1-80ee-87c2ca76b872">



## API documentation
[Doppler Documentation Hub](https://docs.doppler.com/reference/api)
