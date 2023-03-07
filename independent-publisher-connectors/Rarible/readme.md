# Rarible
The Rarible Multichain Protocol is a blockchain-agnostic and decentralized tool to query, issue, and trade NFTs.  This connector specifically uses the Ethereum network and allows a user to pull this data into their app, flow, or report.

## Publisher: Roy Paar

## Prerequisites
None.

## Supported Operations
### getErc20Balance
Return ERC20 token balance.

### getNftOwnershipById
Return NFTs by Ownership ID.

### getNftOwnershipsByItem
Return NFTs by Item.

### getNftAllOwnerships
Return all NFTs by Owner.

### getNftItemMetaById
Returns the NFT item meta by identifier.

### getNftLazyItemById
Returns the NFT lazy item by identifier.

### getNftItemById
Returns the NFT item by identifier.

### getNftItemsByOwner
Returns the NFT item by owner.

### getNftItemsByCreator
Returns the NFT item by creator.

### getNftItemsByCollection
Returns the NFT items by collection.

### getNftAllItems
Returns all NFT items.

### getNftItemRoyaltyById
Returns item royalty by identifier.

### generateNftTokenId
Returns next available tokenId for minter.

### getNftCollectionById
Returns collection by address.

### searchNftCollectionsByOwner
Returns collection by owner.

### searchNftAllCollections
Returns all NFT collections.

### getOrderByHash
Returns the order by order hash.

### updateOrderMakeStock
Update stock of the order by order hash.

### getSellOrdersByMakerAndByStatus
Returns all orders for sale by maker and order status.

### getSellOrdersByItemAndByStatus
Returns all orders for sale by item and order status.

### getCurrenciesBySellOrdersOfItem
Returns currencies taken as payment in all sell orders of the item.

### getCurrenciesByBidOrdersOfItem
Returns currencies made as payment in all bid orders of the item.

### getSellOrdersByCollectionAndByStatus
Returns all orders for sale by collection and order status.

### getSellOrdersByStatus
Returns all orders for sale by order status.

### getOrderBidsByMakerAndByStatus
Returns all order bids by maker and order status.

### getOrderBidsByItemAndByStatus
Returns all order bids by item and order status.

### getOrdersAllByStatus
Returns all orders with status sorting.
 
### aggregateNftSellByMaker
Aggregate NFT sell order by maker.

### aggregateNftPurchaseByTaker
Aggregate NFT purchase by taker.

### aggregateNftPurchaseByCollection
Aggregate NFT purchase by collection.

## API Documentation
https://ethereum-api.rarible.org/v0.1/doc

## Known Issues and Limitations
None.
