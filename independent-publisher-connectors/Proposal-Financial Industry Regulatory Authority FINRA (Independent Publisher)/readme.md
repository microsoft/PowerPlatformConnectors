## Financial Regulatory Authority (FINRA) Connector

	- The FINRA Data Connector provides access to a wealth of financial market data maintained by the Financial Industry Regulatory Authority (FINRA). This includes information on market activity, trading volume, and regulatory filings. The API enables seamless integration of this data into your workflows using Power Automate custom connectors.

## Publisher

	- Dan Romano (swolcat)

## Prerequisites

	- The actions for this connector should be reviewed on the FINRA API documentation page: https://developer.finra.org/docs

## FINRA Resources


	- FINRA support - https://developer.finra.org/support
	- Connect to FINRA via Postman - https://developer.finra.org/UsingPostmantocalltheFINRAAPIPlatform
	- Fees - Free accounts are available with restrictions. Premium accounts https://developer.finra.org/fees

## Obtaining Credentials

	- To access the FINRA API, you will need an API key. You can obtain this by registering on the FINRA API portal and requesting access.
	- It is important to note that FINRA does not require OAuth2 authentication, only a credential and secret that are generated within the FINRA console.
		- 

## Supported Operations:

	- Market Summary - Retrieve a summary of the overall market activity including volume and trade counts.

	- Trading Volume by Symbol - Retrieve the trading volume for a specific security symbol.

	- Regulatory Filings - Retrieve information about regulatory filings submitted to FINRA.

	- Recent Market Data - Retrieves recent market data for a given date or range of dates.

	- Search for Securities  - Retrieves a list of securities based on specific criteria such as name, symbol, or sector.

	- Security Details - Retrieves detailed information about a specific security, including historical performance and key metrics.

	- Trading Activity by Firm - Retrieves trading activity data for a specific brokerage firm.

	- Reported Trades - Retrieves details of trades reported to FINRA, including trade price, volume, and timestamp.

	- Short Interest Data - Retrieves short interest data for a specific security, including the number of shares shorted and the short interest ratio.

	- Market Maker Status - Retrieves the market maker status for a specific security, including quotes and trading activity.
 
	- Recent Filings by Firm - Retrieves the most recent regulatory filings submitted by a specific brokerage firm.

	- Get Daily Short Volume - Retrieves daily short volume for a specific security.

## Known Issues and Limitations

	- There are no known issues at this time.

## REST API Capabilities

	- The FINRA REST API enables various calls to access and manage financial market data. Here are some of the key capabilities:

		- Market Summary: Fetches a comprehensive summary of the day's market activities.

		- Trading Volume: Provides detailed trading volume data for specific securities.

		- Regulatory Filings: Accesses information on regulatory filings submitted to FINRA, useful for compliance and monitoring.

		- Market Data: Retrieves historical and real-time market data, essential for analysis and decision-making.

		- Security Search: Allows for the search and retrieval of securities information based on different criteria.

		- Firm Activity: Monitors and reports on trading activities by brokerage firms.

		- Trade Details: Offers detailed reports on trades, including prices, volumes, and timestamps.

		- Short Interest: Provides data on short interest, helping to understand market sentiment and risk.

		- Market Maker Status: Tracks market maker activities and their impact on trading.

## ENDPOINTS
## Equity

	- Blocks Summary:
		- What It Is: This is like looking at all the big moves in the market. Imagine if a bunch of big players, like those heavy hitters in the neighborhood, are buying or selling a ton of stock. This shows you where the action is.
		- Why It Matters: If these big shots are moving a lot of money into or out of a stock, it might be a good idea to pay attention. They usually know something.

	- Consolidated Short Interest:
		- What It Is: Think of this as a list of all the people betting against a stock, like folks in the neighborhood who think a business is going to fail.
		- Why It Matters: If a lot of people are betting against a stock, there could be trouble ahead. But if they’re wrong, the price might shoot up when they have to cover their bets.

	- Monthly Summary:
		- What It Is: This is like getting a monthly report from the family business. It tells you how things are going overall.
		- Why It Matters: It helps you see the big picture and spot trends over time.

	- OTC Block Summary:
		- What It Is: Similar to the Blocks Summary but for over-the-counter trades, like dealing under the table in the back room.
		- Why It Matters: Keeps you informed about big trades in the less regulated parts of the market.

	- Reg SHO Daily Short Sale Volume:
		- What It Is: This shows you the daily bets against stocks, as per the rules everyone’s gotta follow.
		- Why It Matters: You can track who’s trying to make a quick buck betting a stock will drop.

	- Threshold List:
		- What It Is: A list of stocks that are in trouble, like the businesses in the neighborhood that owe everyone money and can’t seem to pay up.
		- Why It Matters: It’s a warning sign to be careful with these stocks.

	- Weekly Summary:
		- What It Is: Just like the Monthly Summary, but every week. It’s like getting a weekly update from the boss.
		- Why It Matters: Helps you keep a close eye on what’s happening in the short term.

	- How You Can Use These:
		- Personal investing
			- Follow the big players with the Blocks Summary and OTC Block Summary to make smarter moves.
			- Watch the short interest to see if people are betting against your stocks, maybe you can make a move before things get bad.

		- Market analysis
			- Use the Monthly and Weekly Summaries to see the trends and decide if the market is hot or not
			- Check the Threshold List to avoid stocks that are in trouble.

## FIXED INCOME

	- Agency Debt Market Breadth:
		- What It Is: This tells you how many different agency bonds are being traded. Think of it as counting all the different types of pasta dishes at a big family dinner.
		- Why It Matters: The more variety, the healthier the market. It means people are interested in lots of different bonds, not just a few.
	
	- Agency Debt Market Sentiment:
		- What It Is: This is like taking the temperature of the room. It tells you if people are feeling good or bad about agency bonds.
		- Why It Matters: If sentiment is high, it means people trust these bonds. If it’s low, maybe it’s time to be cautious.
	
	- Corporate 144A Debt Market Breadth:
		- What It Is: Similar to the agency breadth but for corporate 144A bonds, which are like the secret recipes that only the family knows about.
		- Why It Matters: It shows you how many different corporate bonds are being traded under these special rules. More variety usually means more interest.
	
	- Corporate 144A Debt Market Sentiment:
		- What It Is: This tells you how people are feeling about these special corporate bonds.
		- Why It Matters: Helps you gauge trust and interest in these particular investments.
	
	- Corporate And Agency Capped Volume:
		- What It Is: This shows the total trading volume for corporate and agency bonds, but with a cap. Think of it as counting all the dishes at dinner but only up to a certain number.
		- Why It Matters: Gives you an idea of trading activity without getting overwhelmed by outliers.
	
	- Corporate Debt Market Breadth:
		- What It Is: Like the agency breadth but for all corporate bonds. Counting all the different kinds of corporate bonds on the table.
		- Why It Matters: More variety means a healthier market with diverse interests.
	
	- Corporate Debt Market Sentiment:
		- What It Is: This tells you how people are feeling about corporate bonds in general.
		- Why It Matters: High sentiment means people are confident, low sentiment means they’re wary.
	
	- Securitized Product Capped Volume:
		- What It Is: Shows the total trading volume for securitized products but capped. It’s like saying, “We’ll count the dishes, but only up to a certain point.”
		- Why It Matters: Helps you see trading activity without getting skewed by huge trades.
	
	- Treasury Daily Aggregates:
		- What It Is: This gives you the daily totals for trading activity in treasury bonds. It’s like counting the number of plates used every day.
		- Why It Matters: Daily data helps you keep a close eye on what’s happening in the short term.
	
	- Treasury Monthly Aggregates:
		- What It Is: This gives you the monthly totals for treasury bond trading. Think of it as the big monthly family meeting where you go over everything that happened.
		- Why It Matters: Helps you see longer-term trends and make better decisions.
	
	- How can normal people use them?
	
		- Independent investment:
			- Use market breadth data to see how diverse the trading is. A healthy market has lots of different bonds being traded.
			- Check market sentiment to see if people are feeling good about the bonds you’re interested in.
	
		- Market Analysis:
			- Look at capped volume to get a sense of trading activity without being misled by outliers.
			- Use daily and monthly aggregates to track trends and spot opportunities.
			- Explain the market breadth and sentiment to help stakeholders and decision-makers understand how the bond market is doing.
	