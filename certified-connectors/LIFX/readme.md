
## LIFX Connector
LIFX make Wi-Fi-enabled LED smart lighting. Deep colors, bright whites, and unique features, effects and integrations make for Smarter Light. Use this connector to use your LIFX to provide add immersive visual feedback into your workflows.

## Pre-requisites
You will need the following to proceed:
- A Microsoft Power Apps or Microsoft Power Automate plan enabling the use of Premium connectors
- [LIFX](https://www.lifx.co) Account

## API documentation
Introduction: https://api.developer.lifx.com/
Selectors: https://api.developer.lifx.com/docs/selectors
Colors: https://api.developer.lifx.com/docs/colors

Set State: https://api.developer.lifx.com/docs/set-state
Set States: https://api.developer.lifx.com/docs/set-states
Toggle Power: https://api.developer.lifx.com/docs/toggle-power
Breathe Effect: https://api.developer.lifx.com/docs/breathe-effect
Move Effect: https://api.developer.lifx.com/docs/move-effect
Morph Effect: https://api.developer.lifx.com/docs/morph-effect
Pulse Effect: https://api.developer.lifx.com/docs/pulse-effect
Effects Off: https://api.developer.lifx.com/docs/effects-off
Activate Scene: https://api.developer.lifx.com/docs/activate-scene

## Supported Operations
Set State: Set state of the lights.
Set States: Set different states on multiple sets of lights.
Toggle Power: Turn off lights if any of them are on, or turn them on if they are all off.
Breathe Effect: Performs a breathe effect by slowly fading between the given colors. 
Move Effect: Performs a move effect on a linear device with zones, by moving the current pattern across the device. Use the parameters to tweak the effect.
Morph Effect: Performs a morph effect on the tiles in your selector. Note that the brightness of the morph is determined by the brightness of the tile, rather than the brightness of the colours in the palette. To change the brightness, use the SetState endpoint.
Pulse Effect: Quickly flashes lights between given colors.
Effects Off: Turn off any running effects on the lights.
Activate Scene: ACtivates a LIFX scene, that has been defined within the LIFX app.


## How to get credentials
To test the connectors you need a LIFX account. You will then be prompted to sign in to your account when creating a connection.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
