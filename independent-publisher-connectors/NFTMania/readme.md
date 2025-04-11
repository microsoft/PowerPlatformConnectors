# NFTMania
This is a proposed connector for _Microsoft Power Platform_. Blockchain is s tech space which is gaining its prominence and is expected to be a key player in the upcoming web 3.0, besides this is provides various features for the security of any digital asset. NFT is one such branch based on blockchain which is being used by many to store the collectibles like art or any piece of data digitally. The NFTs are Non Fungible in nature so the owner holds the right for any collectible. It cant be manipulated as the transaction data will be stored on Blockchain. Developing the NFT is often overlooked as a tedious task due to the coding involved. It takes a long time to get ideas to life due to this. The main purpose of this connector is to help developers bring their NFT applications to market in hours instead of months. This is possible with the help of Microsoft Power Platform.This connector uses NFTPort APIs.

## Publisher: Shreyan J D Fernandes
My name is Shreyan J D Fernandes. I am a Gold Microsoft Learn Student Ambassador​ from India. I have a keen interest in Blockchain which is something that motivated me to plan this connector.I love to teach and explore new tech.

## Prerequisites
The connector uses the APIs from [_NFTPort_ ](https://www.nftport.xyz/)to run the backend services. This Connector requires an _API Key_ from [_NFTPort_ ](https://www.nftport.xyz/). Once a user has signed up on NFTPort platform, they can access the API key from their NFTPort Dashboard.
Apart from this a [_Polygon_](https://wallet.polygon.technology/)Wallet ID must be obtained from the website. this helps the user to mint the NFT to their personal wallet.

## Supported Operations
As of now the connector is capable of minting an Image into Non Fungible Token. The image must be hosted in any cloud storage and the URL must be provided.​

### Minting NFT with URL
The connector allows the user to choose any specific chain that they want the NFT to be deployed in. I have tried it out on Polygon Chain and it has been working fine for some NFTs that i minted for the purpose of testing.
Some details that must be filled for the API to run successfully include:
Chain:<Chain in which the user wants to publish the NFT>
Name:<The name of the particular NFT>
Description:<Description and details of the NFT being minted>
file_url:<Publicly viewable image of the Collectible to be minted as NFT. The link must be set to download and not to sharing mode>
mint_to_address:<This is the wallet address to which the Collectible will be minted as NFT>
Upon successful run. A success response of 200 will be displayed along with a message body in which the transaction hash and other details will be provided.

Response example: (Deployed on Polygon Chain)
{
  "response": "OK",
  "chain": "polygon",
  "contract_address": "0x47c7ff137d7a6644a9a96f1d44f5a6f857d9023f",
  "transaction_hash": "0x6eb71286f4875bf48be7834c1ff285910583705714f5a5acff67489f94e14954",
  "transaction_external_url": "https://polygonscan.com/tx/0x6eb71286f4875bf48be7834c1ff285910583705714f5a5acff67489f94e14954",
  "mint_to_address": "0x5FDd0881Ef284D6fBB2Ed97b01cb13d707f91e42",
  "name": "TestMinting",
  "description": "One-Stop & Simple NFT Infrastructure & APIs for Developers"
}


## Obtaining Credentials
To obtain the API Key, one must navigate to [_NFTPort_ ](https://www.nftport.xyz/) and Sign  Up providing details like Email id and create a new password. Upon signing up and confirming the mail, One can see the dashboard. This dashboard has an option of API Key. This API key must be used to create a connection and also must be provided in the header section while minting the NFT from Power Platform.

## Getting Started
In order to use this connector, The required Image file must be hosted on the cloud and the public link must be used in the "file_url" section. Incase of Polygon blockchain, the Polygon wallet ID must be used in the "mint_to_address" section.

## Known Issues and Limitations
Incase the image being hosted on the google drive, then the URL must be altered to download. The original URL obtained will be in sharing mode. It must be changed to download.This link must be set to _"Anyone with the link can view"_
The obtained link will be in the below format:
https://drive.google.com/file/d/1x2x34xx....../view?usp=sharing
Remove the text file/d/ from the link and replace it with uc?id=

Now remove the section after the file ID, including /view and replace it with &export=download in place of the text you have removed. 

This is what it should look like afterwards: https://drive.google.com/uc?id=1x2x34xx.......&export=download

