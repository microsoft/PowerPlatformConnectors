# PokéAPI Core

All the Core Pokémon data you'll ever need in one place, easily accessible through a modern free open-source solution. This means, that any data related to pokémons are found within with connector.

## Publisher: Fördős András

## Obtaining Credentials

The underlying service is opensource and public, hence there is no need for credentials.

## Supported Operations

### List abilities
Abilities provide passive effects for Pokémon in battle or in the overworld. Pokémon have multiple possible abilities but can have only one ability at a time.

### Get ability
Abilities provide passive effects for Pokémon in battle or in the overworld. Pokémon have multiple possible abilities but can have only one ability at a time.

### List characteristics
Characteristics indicate which stat contains a Pokémon's highest IV. A Pokémon's Characteristic is determined by the remainder of its highest IV divided by 5 (gene_modulo).

### Get characteristic
Characteristics indicate which stat contains a Pokémon's highest IV. A Pokémon's Characteristic is determined by the remainder of its highest IV divided by 5 (gene_modulo).

### List genders
Genders were introduced in Generation II for the purposes of breeding Pokémon but can also result in visual differences or even different evolutionary lines.

### Get gender
Genders were introduced in Generation II for the purposes of breeding Pokémon but can also result in visual differences or even different evolutionary lines.

### List growth rates
Growth rates are the speed with which Pokémon gain levels through experience.

### Get growth rate
Growth rates are the speed with which Pokémon gain levels through experience.

### List pokemons
Pokémon are the creatures that inhabit the world of the Pokémon games. They can be caught using Pokéballs and trained by battling with other Pokémon. Each Pokémon belongs to a specific species but may take on a variant which makes it differ from other Pokémon of the same species, such as base stats, available abilities and typings.

### Get pokemon
Pokémon are the creatures that inhabit the world of the Pokémon games. They can be caught using Pokéballs and trained by battling with other Pokémon. Each Pokémon belongs to a specific species but may take on a variant which makes it differ from other Pokémon of the same species, such as base stats, available abilities and typings.

### List types
Types are properties for Pokémon and their moves. Each type has three properties: which types of Pokémon it is super effective against, which types of Pokémon it is not very effective against, and which types of Pokémon it is completely ineffective against.

### Get type
Types are properties for Pokémon and their moves. Each type has three properties: which types of Pokémon it is super effective against, which types of Pokémon it is not very effective against, and which types of Pokémon it is completely ineffective against.

### List evolution chains
Evolution chains are essentially family trees. They start with the lowest stage within a family and detail evolution conditions for each as well as Pokémon they can evolve into up through the hierarchy.

### Get evolution chain
Evolution chains are essentially family trees. They start with the lowest stage within a family and detail evolution conditions for each as well as Pokémon they can evolve into up through the hierarchy.

### List evolution triggers
Evolution triggers are the events and conditions that cause a Pok\u00e9mon to evolve.

### Get evolution trigger
Evolution triggers are the events and conditions that cause a Pok\u00e9mon to evolve.

## Known Issues and Limitations
There are no known issues with the connector, but the latest status can be checked in the repository (issues). As limitations, this specific connector only implements the core endpoints of the underlying API (mainly pokemon related), and other connectors joining endpoints (locations, battles, items).That said, there are also endpoints and data, that are not foreseen to be implemented. Please reach out and let us collaborate, if you are missing something. 

The underlying service expects a fair use policy, so please don't abuse it with frequent calls, and rather implement resource caching whenever possible.