# r/SpaceX
r/SpaceX is an open source REST API for SpaceX launch, rocket, core, capsule, Starlink, launchpad, and landing pad data. r/SpaceX is not affiliated, associated, authorized, endorsed by, or in any way officially connected with Space Exploration Technologies Corp (SpaceX), or any of its subsidiaries or its affiliates. The names SpaceX as well as related names, marks, emblems and images are registered trademarks of their respective owners.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
This API requires no authentication for all of the actions provided in this connector. All create, update, and delete endpoints are not included. The query actions support pagination, custom queries, and other output controls. See the [pagination + query guide](https://github.com/r-spacex/SpaceX-API/blob/master/docs/queries.md) for more details and examples.

## Supported Operations

### Get all capsules
Detailed info for all serialized Dragon capsules.
### Get a capsule
Detailed info for a serialized Dragon capsule.
### Query capsules
Detailed info for queried serialized Dragon capsules.
### Get company info
Detailed info about SpaceX as a company.
### Get all cores
Detailed info for all serialized first stage cores.
### Get a core
Detailed info for a serialized first stage core.
### Query cores
Detailed info for queried serialized first stage cores.
### Get all crew members
Detailed info on all Dragon crew members.
### Get a crew member
Detailed info on a Dragon crew member.
### Query crew members
Detailed info on queried Dragon crew members.
### Get all Dragons
Detailed info about all Dragon capsule versions.
### Get a Dragon
Detailed info about a Dragon capsule version.
### Query Dragons
Detailed info about queried Dragon capsule versions.
### Get all history events
Detailed info on all SpaceX historical events.
### Get a historic event
Detailed info on a SpaceX historical event.
### Query historic events
Detailed info on queried SpaceX historical events.
### Get all landpads
Detailed info about all landing pads and ships.
### Get a landpad
Detailed info about a landing pad or ship.
### Query landpads
Detailed info about queried landing pads and ships.
### Get all launches
Detailed info about all launches.
### Get a launch
Detailed info about a launch.
### Query launches
Detailed info about queried launches.
### Get past launches
Detailed info about past launches.
### Get upcoming launches
Detailed info about upcoming launches.
### Get latest launch
Detailed info about latest launch.
### Get next launch
Detailed info about next launches.
### Get all launchpads
Detailed info about all launchpads.
### Get a launchpad
Detailed info about a launchpad.
### Query launchpads
Detailed info about queried launchpads.
### Get all payloads
Detailed info about all launch payloads.
### Get a payload
Detailed info about a launch payload.
### Query payloads
Detailed info about queried launch payloads.
### Get Roadster Info
Detailed info about Elon's Tesla Roadster's current position.
### Get all rockets
Detailed info about all rockets.
### Get a rocket
Detailed info about a rocket.
### Query rockets
Detailed info about queried rockets.
### Get all ships
Detailed info about all ships in the SpaceX fleet.
### Get a ship
Detailed info about a ship in the SpaceX fleet.
### Query ships
Detailed info about queried ships in the SpaceX fleet.
### Get all Starlink sats
Detailed info about all Starlink satellites and orbits.
### Get a Starlink sat
Detailed info about a Starlink satellite and orbit.
### Query Starlink sats
Detailed info about queried Starlink satellites and orbits.

#### Launch dates
Why does the date appear wrong? This is usually due to the way partial dates are stored and displayed in the API. For example, a launch scheduled for July 2020 would be represented as 2020-07-01T00:00:00.000Z. In this case, the field date_precision would be set as month, meaning the date is only valid to the month level, or 2020-07.
