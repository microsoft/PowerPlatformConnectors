# FishWatch.gov

Get the most up-to-date information on popular seafood harvested or farmed in the United States to help you make educated seafood choices. This Connector helps you to work with the FishWatch.gov API for a sustainable future.

## Publisher: Fördős András

## Prerequisites

None.

## Deprecation

**This connector is deprecated.** The underlying FishWatch.gov API has been decommissioned: the standalone FishWatch.gov site was folded into fisheries.noaa.gov, and the `www.fishwatch.gov/api` endpoints no longer return data (requests now redirect away from the API). As a result, the connector's operations are non-functional.

If you are using it in your flows or apps, please migrate away from it. See the [Recommended Alternative](#recommended-alternative) below, or reach out if you need help migrating.

## Supported Operations

### List Species (deprecated)

Lists all species data from the FishWatch database.

### Get Species (deprecated)

Get specific species data from the FishWatch database.

## Known Issues and Limitations

This connector is deprecated and its operations are non-functional, because the underlying FishWatch.gov API has been retired (see [Deprecation](#deprecation) above).

The API previously had the limitation that:
* it only allowed requesting either all data or just one specific species at a time
* some parameters were returned with markup included

## Getting Started

The FishWatch.gov API is no longer maintained, and its endpoints no longer return data (see [Deprecation](#deprecation) above). The original developer documentation at `https://www.fishwatch.gov/developers` now redirects to NOAA Fisheries and is no longer available.

For a maintained data source going forward, see the [Recommended Alternative](#recommended-alternative) below.

## Recommended Alternative

As the FishWatch.gov API is retired, the maintained NOAA Fisheries developer API is the **Fisheries One Stop Shop (FOSS)**: [NOAA Fisheries FOSS](https://www.fisheries.noaa.gov/foss/). Note that FOSS exposes a different dataset (commercial landings and related fisheries statistics) and is not a drop-in replacement for FishWatch's seafood species profiles, so evaluate it against your needs before migrating.

## Disclaimer

This connector is provided on a best-effort basis. If you face any issues, please let me know immediately!
