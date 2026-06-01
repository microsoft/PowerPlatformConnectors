# PokéAPI World

Access Pokemon world data including machines, locations, regions, and berries.

## Publisher: Fördős András

## Obtaining Credentials

The underlying service is opensource and public, hence there is no need for credentials.

## Supported Operations

### List berries
Berries are small fruits that can provide HP and status condition restoration, stat enhancement, and even damage negation when eaten by Pokemon.

### Get berry
Berries are small fruits that can provide HP and status condition restoration, stat enhancement, and even damage negation when eaten by Pokemon.

### List enounters
Methods by which the player might can encounter Pokemon in the wild, e.g., walking in tall grass.

### Get encounter
Methods by which the player might can encounter Pokemon in the wild, e.g., walking in tall grass.

### List encounter conditions
Conditions which affect what pokemon might appear in the wild, e.g., day or night.

### Get encounter condition
Conditions which affect what pokemon might appear in the wild, e.g., day or night.

### List encounter condition values
Encounter condition values are the various states that an encounter condition can have, i.e., time of day can be either day or night.

### Get encounter condition value
Encounter condition values are the various states that an encounter condition can have, i.e., time of day can be either day or night.

### List items
An item is an object in the games which the player can pick up, keep in their bag, and use in some manner. They have various uses, including healing, powering up, helping catch Pokemon, or to access a new area.

### Get item
An item is an object in the games which the player can pick up, keep in their bag, and use in some manner. They have various uses, including healing, powering up, helping catch Pokemon, or to access a new area.

### List locations
Locations that can be visited within the games. Locations make up sizable portions of regions, like cities or routes.

### Get location
Locations that can be visited within the games. Locations make up sizable portions of regions, like cities or routes.

### List location areas
Location areas are sections of areas, such as floors in a building or cave. Each area has its own set of possible Pokemon encounters.

### Get location area
Location areas are sections of areas, such as floors in a building or cave. Each area has its own set of possible Pokemon encounters.

### List machines
Machines are the representation of items that teach moves to Pokemon. They vary from version to version, so it is not certain that one specific TM or HM corresponds to a single Machine.

### Get machine
Machines are the representation of items that teach moves to Pokemon. They vary from version to version, so it is not certain that one specific TM or HM corresponds to a single Machine.

### List regions
A region is an organized area of the Pokemon world. Most often, the main difference between regions is the species of Pokemon that can be encountered within them.

### Get region
A region is an organized area of the Pokemon world. Most often, the main difference between regions is the species of Pokemon that can be encountered within them.

## Known Issues and Limitations
There are no known issues with the connector, but the latest status can be checked in the repository (issues). As limitations, this specific connector only implements the core endpoints of the underlying API (mainly pokemon related), and other connectors joining endpoints (locations, battles, items).That said, there are also endpoints and data, that arenot foreseen to be implemented. Please reach out and let us collaborate, if you are missing something. 

The underlying service expects a fair use policy, so please don't abuse it with frequent calls, and rather implement resource caching whenever possible.