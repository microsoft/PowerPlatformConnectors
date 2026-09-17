# Entra ID Password Reset
Microsoft Entra ID is an integrated cloud identity and access solution, and a leader in the market for managing directories, enabling access to applications, and protecting identities. This connector allows authorized users to reset Entra ID passwords.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
The user of this connector will need at least the Authentication Administrator or Privileged Authentication Administrator Microsoft Entra role. Admins with User Administrator, Helpdesk Administrator, or Password Administrator roles can also reset passwords for non-admin users and a limited set of admin roles.

## Obtaining Credentials
The OAuth configuration for this connector requires the UserAuthenticationMethod.ReadWrite.All permission.

## Supported Operations
### List password
Retrieve a list of the passwords registered to a user.
### Reset password
Reset a user's password.

## Known Issues and Limitations
There are no known issues at this time.
