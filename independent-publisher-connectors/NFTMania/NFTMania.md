**Proposal for NFTMania Connector**

This is a proposed connector for _Microsoft Power Platform_. Blockchain is s tech space which is gaining its prominence and is expected to be a key player in the upcoming web 3.0, besides this is provides various features for the security of any digital asset. NFT is one such branch based on blockchain which is being used by many to store the collectibles like art or any piece of data digitally. The NFTs are Non Fungible in nature so the owner holds the right for any collectible. It cant be manipulated as the transaction data will be stored on Blockchain. Developing the NFT is often overlooked as a tedious task due to the coding involved. It takes a long time to get ideas to life due to this. The main purpose of this connector is to help developers bring their NFT applications to market in hours instead of months. This is possible with the help of Microsoft Power Platform. The further update of this connector will include many  exciting features like Batch NFT Minting, Deploying  Contracts and even file upload on Interplanetary file System (IPFS).

The connector uses the APIs from [_NFTPort_ ](https://www.nftport.xyz/)to run the backend services. _NFTPort_ provides one-stop, simple and developer-friendly NFT infrastructure & APIs for developers. 

This will be available in two versions. For any individual working on a simple project there is a limited free usage quota so that they can still go ahead and use this application. For any organization, there is a premium plan offered by NFT Port.

This connector requires _API Key_ which can be easily availed from the NFTPort website.The user can see the metrics as well from the NFTPort Website.
:::image type="content" source="../media/Dashboard.png" alt-text="Dashboard of NFTPort":::
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

This is the beta version of this connector. The easy NFT mint feature of the connector is ready and the further updates are also being developed. The final connector will contain 40 functionalities including the Easy minting of NFT.


About me:

My name is Shreyan Fernandes, Final Year B.Tech Student from Manipal Institute of Technology. I am a Gold Microsoft Learn Student Ambassador from India. I have a keen interest in Blockchain which is something that motivated me to plan this connector.I love to teach and explore new tech.
email:shreyanfernandes@gmail.com / shreyanfernandes@studentambassadors.com