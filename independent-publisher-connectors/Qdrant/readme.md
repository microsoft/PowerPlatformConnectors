# Qdrant

[Qdrant](https://qdrant.tech/) is a vector similarity search engine. It provides a production-ready service with a convenient API to store, search, and manage vectors with additional payload and extended filtering support. It makes it useful for all sorts of neural network or semantic-based matching, faceted search, and other applications.

## Publisher: Anush, Qdrant

## Obtaining Credentials

If using Qdrant cloud, you can refer to the [Authentication Documentation](https://qdrant.tech/documentation/cloud/authentication/) to learn how to create an API key.

## Supported Operations

### Delete Index

Delete index for a field in a collection.

### Create Index

Create index for a field in a collection.

### Check Collection Existence

Check the existence of a collection.

### Delete Points

Delete points.

### Delete vectors

Delete named vectors from the given points.

### Update vectors

Update specified named vectors on points, keep unspecified vectors intact.

### Delete payload

Delete specified key payload for points.

### Clear payload

Remove all payload for specified points.

### Set payload

Set payload values for points.

### Overwrite payload

Replace full payload of points with new one.

### Batch update points

Apply a series of update operations for points, vectors and payloads.

### Scroll points

Scroll request - paginate over all points which matches given filtering condition.

### Count points

Count points which matches given filtering condition.

### Query points in batch

Universally query points in batch. This endpoint covers all capabilities of search, recommend, discover, filters. But also enables hybrid and multi-stage queries.

### Query points with grouping

Universally query points, grouped by a given payload field.

### Query points

Universally query points. This endpoint covers all capabilities of search, recommend, discover, filters. But also enables hybrid and multi-stage queries.

### Get points

Retrieve multiple points by specified IDs.

### Upsert points

Perform insert + updates on points. If point with given ID already exists - it will be overwritten.

### Collection info

Get detailed information about specified existing collection.

### Delete collection

Drop collection and all associated data.

### Create collection

Create new collection with given parameters.

### Update collection parameters

Update parameters of the existing collection

### List collections

Get list name of all existing collections

### Returns information about the running Qdrant instance

Returns information about the running Qdrant instance like version and commit id

## Known Issues and Limitations

There are no known issues at this time.
