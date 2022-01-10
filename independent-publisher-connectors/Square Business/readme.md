# Square Business
Square helps millions of sellers run their business - from secure credit card processing to point of sale solutions. This connector contains actions for the following endpoints: Loyalty, Gift Cards, Bookings, Business, Team, Financials, Online and Auth.

## Publisher: Troy Taylor

## Prerequisites
Use of this connector will require permission from the owner of a seller account for production use with OAuth 2. You should also sign up for a [Square Developer account](https://developer.squareup.com/us/en) to gain access to a sandbox environment, also using OAuth 2.

## Obtaining Credentials
In your Square Developer account, you will need to create an application registration to be granted a sandbox OAuth 2 application ID and access token. You will also need to set the OAuth redirect URL to https://global.consent.azure-apim.net/redirect. Once you are ready to use the connector in production, the owner will need to register a production application with the same redirect URL.

## Supported Operations

#### List bank accounts
Returns a list of BankAccount objects linked to a Square account.
#### Get bank account by V1 ID
Returns details of a BankAccount identified by V1 bank account ID.
#### Get bank account
Returns details of a BankAccount linked to a Square account.
### Bookings
#### Create booking (Beta)
Creates a booking.
#### Search availability (Beta)
Searches for availabilities for booking.
#### Retrieve business booking profile (Beta)
Retrieves a seller's booking profile.
#### List team member booking profiles (Beta)
Lists booking profiles for team members.
### Cash Drawers
#### List cash drawer shifts
Provides the details for all of the cash drawer shifts for a location in a date range.
#### Retrieve cash drawer shift
Provides the summary details for a single cash drawer shift.
#### List cash drawer shift events
Provides a paginated list of events for a single cash drawer shift.
### Devices
#### List device codes
Lists all DeviceCodes associated with the merchant.
#### Create device code
Creates a DeviceCode that can be used to login to a Square Terminal device to enter the connected terminal mode.
#### Get device code
Retrieves DeviceCode with the associated ID.
### Gift Cards
#### List gift card activities (Beta)
Lists gift card activities. By default, you get gift card activities for all gift cards in the seller's account. You can optionally specify query parameters to filter the list. For example, you can get a list of gift card activities for a gift card, for all gift cards in a specific region, or for activities within a time window.
#### Create gift card activity (Beta)
Creates a gift card activity.
#### Retrieve gift card from GAN (Beta)
Retrieves a gift card using the gift card account number (GAN).
#### Retrieve gift card from nonce (Beta)
Retrieves a gift card using a nonce (a secure token) that represents the gift card.
#### Link customer to gift card (Beta)
Links a customer to a gift card
#### Unlink customer from gift card (Beta)
Unlinks a customer from a gift card.
#### Retrieve gift card (Beta)
Retrieves a gift card using its ID.
### Labor
#### List break types
Returns a paginated list of BreakType instances for a business.
#### Create break type
Creates a new BreakType. A BreakType is a template for creating Break objects.
#### Get break type
Returns a single BreakType specified by id.
#### Delete break type
Deletes an existing BreakType. A BreakType can be deleted even if it is referenced from a Shift.
#### Update break type
Updates an existing BreakType.
#### Create shift
Creates a new Shift. A Shift represents a complete workday for a single employee
#### Search shifts
Returns a paginated list of Shift records for a business.
#### Get shift
Returns a single Shift specified by id.
#### Delete shift
Deletes a Shift.
#### Update shift
Updates an existing Shift.
#### List team member wages
Returns a paginated list of TeamMemberWage instances for a business.
#### Get team member wage
Returns a single TeamMemberWage specified by id.
#### List workweek configs
Returns a list of WorkweekConfig instances for a business.
#### Update workweek config
Updates a WorkweekConfig.
### Locations
#### List locations
Provides information of all locations of a business.
#### Create location (Beta)
Creates a location.
#### Retrieve location
Retrieves details of a location. You can specify "main" as the location ID to retrieve details of the main location.
#### Update location (Beta)
Updates a location.
### Loyalty
#### Create loyalty account
Creates a loyalty account.
#### Search loyalty accounts
Searches for loyalty accounts in a loyalty program.
#### Retrieve loyalty account
Retrieves a loyalty account.
#### Accumulate loyalty points
Adds points to a loyalty account.
#### Adjust loyalty points
Adds points to or subtracts points from a buyer's account. Use this endpoint only when you need to manually adjust points.
#### Search loyalty events
Searches for loyalty events. A Square loyalty program maintains a ledger of events that occur during the lifetime of a buyer's loyalty account. Each change in the point balance (for example, points earned, points redeemed, and points expired) is recorded in the ledger. Using this endpoint, you can search the ledger for events.
#### Retrieve loyalty program
Retrieves the loyalty program in a seller's account, specified by the program ID or the keyword main.  Loyalty programs define how buyers can earn points and redeem points for rewards.
#### Calculate loyalty points
Calculates the points a purchase earns. An application might call this endpoint to show the points that a buyer can earn with the specific purchase.
#### Create loyalty reward
Creates a loyalty reward. After a reward is created, the points are locked and not available for the buyer to redeem another reward.
#### Search loyalty rewards
Searches for loyalty rewards in a loyalty account.
#### Retrieve loyalty reward
Retrieves a loyalty reward.
#### Delete loyalty reward
Deletes a loyalty reward. Returns the loyalty points back to the loyalty account. You cannot delete a reward that has reached the terminal state (REDEEMED).
#### Redeem loyalty reward
Redeems a loyalty reward. After the reward reaches the terminal state, it cannot be deleted. In other words, points used for the reward cannot be returned to the account.
### Merchants
#### List merchants
Returns Merchant information for a given access token.
#### Retrieve merchant
Retrieve a Merchant object for the given merchant_id.
### Mobile
#### Create mobile authorization code
Generates code to authorize a mobile application to connect to a Square card reader. Authorization codes are one-time-use and expire 60 minutes after being issued.
### Sites
#### List sites
Lists the Square Online sites that belong to a seller.
#### Retrieve snippet
Retrieves your snippet from a Square Online site. A site can contain snippets from multiple snippet applications, but you can retrieve only the snippet that was added by your application.
#### Delete snippet
Removes your snippet from a Square Online site.
#### Upsert snippet
Adds a snippet to a Square Online site or updates the existing snippet on the site. The snippet code is appended to the end of the head element on every page of the site, except checkout pages. A snippet application can add one snippet to a given site.
### Team Members
#### Create team member
Creates a single TeamMember object.
#### Bulk create team members
Creates multiple TeamMember objects.
#### Bulk update team members
Updates multiple TeamMember objects.
#### Search team members
Returns a paginated list of TeamMember objects for a business.
#### Retrieve team member
Retrieves a TeamMember object for the given TeamMember id.
#### Update team member
Updates a single TeamMember object.
#### Retrieve wage setting
Retrieves a WageSetting object for a team member specified by TeamMember id.
#### Update wage setting
Creates or updates a WageSetting object.

## Known Issues and Limitations
There are some actions that have '(Beta)' appended to the action name. These actions may not return all fields while still in beta.
