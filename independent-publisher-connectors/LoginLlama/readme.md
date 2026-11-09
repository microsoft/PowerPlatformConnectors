# LoginLlama
LoginLlama scores each sign-in to your app for risk. Send the user's identity, IP address and user agent, and it returns a score from 0 to 10 with the reasons: a new device or location, impossible travel, Tor, VPN and proxy addresses, and bursts of failed attempts.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to sign up for an account with [LoginLlama](https://loginllama.app/). The free tier includes 1,000 login checks.

## Obtaining Credentials
Create an API key on the [API keys page](https://loginllama.app/account/api) of your LoginLlama dashboard, and paste it in when you create the connection.

## Supported Operations
### Check a login for risk
Score a sign-in for risk and get the reasons. Only the identity key is required. Send the IP address and user agent as well for a useful score. Set the authentication outcome to failed for wrong passwords, so LoginLlama can spot password guessing.

## Known Issues and Limitations
New risk codes may be added over time, so treat the list in the response as open-ended.
