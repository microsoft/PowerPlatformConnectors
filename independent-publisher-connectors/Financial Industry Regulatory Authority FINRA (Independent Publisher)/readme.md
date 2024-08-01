## Financial Regulatory Authority (FINRA) Connector

	- The FINRA Data Connector provides access to a wealth of financial market data maintained by the Financial Industry Regulatory Authority (FINRA).
		- This includes information on OTC market activity, trading volume, and regulatory filings.
	- The scope of this connector is freemium endpoints Equity and Fixed Income endpoints.

## Publisher

	- Dan Romano (swolcat)

## Prerequisites

	- Bookmark and use this link: https://developer.finra.org/docs
	- Authorization: https://developer.finra.org/docs#getting_started-api_platform_basics-authorization

## FINRA Resources


	- FINRA support - https://developer.finra.org/support
	- Connect to FINRA via Postman - https://developer.finra.org/UsingPostmantocalltheFINRAAPIPlatform
	- Fees - Free accounts are available with restrictions. Premium accounts https://developer.finra.org/fees

## Obtaining Credentials

	- To access the FINRA API, you will need an API key. You can obtain this by registering on the FINRA API portal and requesting access.
	- It is important to note that FINRA requires OAuth2 authentication, only a credential and secret that are generated within the FINRA console.
		- INSTRUCTIONS: https://developer.finra.org/docs
			- These docs will walk you through the particular process of creating a credential and token. Follow these only.
			- Further instructions: https//developments.substack.com/
	- It is recommended to create two credentials, one for Public datasets and another for Mock datasets.

## Supported Operations:

	- Freemium to start, which includes:
		- Summaries from Equity endpoints (Weekly, Monthly, OTC)
			COMING SOON: All other free Equity endpoints plus metadata.
		
	- The API platform supports both synchronous and asynchronous requests.
		- The request type is specified using the "async" query parameter on GET requests: api.finra.org/{resource path}?async=true and as a request parameter on POST requests: "async" : true. 
		- The async parameter defaults to false if not provided, which indicates a synchronous request.

	- Request headers: https://developer.finra.org/docs#getting_started-api_platform_basics-request_headers

	- Response headers: https://developer.finra.org/docs#getting_started-api_platform_basics-response_headers

## Known Issues and Limitations

	- There are no known issues at this time. It is important to note, however, that the free version is limited to two endpoints.

## REST API Capabilities

	- The FINRA REST API enables various calls to access and manage financial market data from a REST endpoint.

	## ENDPOINTS

		## Equity

			- The Equity API endpoint provides access to OTC trade and equity data. 

				- Blocks Summary:
					- What It Is: This is like looking at all the big moves in the market. Imagine if a bunch of big players, like those heavy hitters in the neighborhood, are buying or selling a ton of stock. This shows you where the action is.
					- Why It Matters: If these big shots are moving a lot of money into or out of a stock, it might be a good idea to pay attention. They usually know something.
					- The data: Aggregated ATS trade data in NMS stocks that meets certain share based and dollar based thresholds.

				- Consolidated Short Interest:
					- What It Is: Think of this as a list of all the people betting against a stock, like folks in the neighborhood who think a business is going to fail.
					- Why It Matters: If a lot of people are betting against a stock, there could be trouble ahead. But if they’re wrong, the price might shoot up when they have to cover their bets.
					- The data: The Consolidated Short Interest data provides a single view of OTC short interest positions submissions across all exchanges. 

				- Monthly Summary:
					- What It Is: This is like getting a monthly report from the family business. It tells you how things are going overall.
					- Why It Matters: It helps you see the big picture and spot trends over time.
					- The data: Monthly aggregate trade data for OTC (non-ATS) trading data for each firm with trade reporting obligations under FINRA rules.

				- OTC Block Summary:
					- What It Is: Similar to the Blocks Summary but for over-the-counter trades, like dealing under the table in the back room.
					- Why It Matters: Keeps you informed about big trades in the less regulated parts of the market.
					- The data: Aggregated OTC (Non-ATS) trade data in NMS stocks that meets certain share based and dollar based thresholds.

				- Reg SHO Daily Short Sale Volume:
					- What It Is: This shows you the daily bets against stocks, as per the rules everyone’s gotta follow.
					- Why It Matters: You can track who’s trying to make a quick buck betting a stock will drop.
					- The data: Daily short sale aggregated volume by security for all short sale trades executed and reported to a FINRA trade reporting facility.

				- Threshold List:
					- What It Is: A list of stocks that are in trouble, like the businesses in the neighborhood that owe everyone money and can’t seem to pay up.
					- Why It Matters: It’s a warning sign to be careful with these stocks.
					- The data: OTC Regulation SHO and Rule 4320 Threshold Securities.

				- Weekly Summary:
					- What It Is: Just like the Monthly Summary, but every week. It’s like getting a weekly update from the boss.
					- Why It Matters: Helps you keep a close eye on what’s happening in the short term.
					- The data: Weekly aggregate trade data for OTC (ATS and non-ATS) trading data for each ATS/firm with trade reporting obligations under FINRA rules.

				- How you can use these:
					- Personal investing
						- Follow the big players with the Blocks Summary and OTC Block Summary to make smarter moves.
						- Watch the short interest to see if people are betting against stocks, maybe you can make a move before things get bad.

					- Market analysis
						- Use the Monthly and Weekly Summaries to see the trends and decide if the market is hot or not
						- Check the Threshold List to avoid stocks that are in trouble.

		## Fixed Income

			- The Fixed Income endpoint provides access to OTC secondary market transaction data for fixed income securities as reported to TRACE.
				- Key highlights:
					- Weekly Treasury Aggregates - TRACE reported trading volume in US Treasury Securities aggregated weekly by security subtype.
					- Market Breadth & Sentiment - TRACE fixed income market breadth and sentiment data.
					- Capped Volume - TRACE structured product and corporate/agency capped volume data.

			- Agency Debt Market Breadth:
				- What It Is: This tells you how many different agency bonds are being traded. Think of it as counting all the different types of pasta dishes at a big family dinner.
				- Why It Matters: The more variety, the healthier the market. It means people are interested in lots of different bonds, not just a few.
				- The data: Market breadth calculations for Agency debt securities.
	
			- Agency Debt Market Sentiment:
				- What It Is: This is like taking the temperature of the room. It tells you if people are feeling good or bad about agency bonds.
				- Why It Matters: If sentiment is high, it means people trust these bonds. If it’s low, maybe it’s time to be cautious.
				- The data: Market sentiment calculations for Agency debt securities.
	
			- Corporate 144A Debt Market Breadth:
				- What It Is: Similar to the agency breadth but for corporate 144A bonds, which are like the secret recipes that only the family knows about.
				- Why It Matters: It shows you how many different corporate bonds are being traded under these special rules. More variety usually means more interest.
				- The data: Market breadth calculations for Rule 144A corporate debt securities.
	
			- Corporate 144A Debt Market Sentiment:
				- What It Is: This tells you how people are feeling about these special corporate bonds.
				- Why It Matters: Helps you gauge trust and interest in these particular investments.
				- The data: Market sentiment calculations for Rule 144A corporate debt securities.
	
			- Corporate And Agency Capped Volume:
				- What It Is: This shows the total trading volume for corporate and agency bonds, but with a cap. Think of it as counting all the dishes at dinner but only up to a certain number.
				- Why It Matters: Gives you an idea of trading activity without getting overwhelmed by outliers.
				- The data: Capped volume calculations for corporate and agency debt.
	
			- Corporate Debt Market Breadth:
				- What It Is: Like the agency breadth but for all corporate bonds. Counting all the different kinds of corporate bonds on the table.
				- Why It Matters: More variety means a healthier market with diverse interests.
				- The data: Market breadth calculations for corporate debt securities.
	
			- Corporate Debt Market Sentiment:
				- What It Is: This tells you how people are feeling about corporate bonds in general.
				- Why It Matters: High sentiment means people are confident, low sentiment means they’re wary.
				- The data: Market sentiment calculations for corporate debt securities.
	
			- Securitized Product Capped Volume:
				- What It Is: Shows the total trading volume for securitized products but capped. It’s like saying, “We’ll count the dishes, but only up to a certain point.”
				- Why It Matters: Helps you see trading activity without getting skewed by huge trades.
				- The data: Capped volume calculations for securitized products.
	
			- Treasury Daily Aggregates:
				- What It Is: This gives you the daily totals for trading activity in treasury bonds. It’s like counting the number of plates used every day.
				- Why It Matters: Daily data helps you keep a close eye on what’s happening in the short term.
				- The data: Daily trading volume in US Treasury Securities reported to TRACE.
	
			- Treasury Monthly Aggregates:
				- What It Is: This gives you the monthly totals for treasury bond trading. Think of it as the big monthly family meeting where you go over everything that happened.
				- Why It Matters: Helps you see longer-term trends and make better decisions.
				- The data: Monthly trading volume in US Treasury Securities reported to TRACE.
		
			- Firm Registration Types - Breakdown of securities industry registered firms by type of registration.

			- How you can use these:
	
				- Independent investment:
					- Use market breadth data to see how diverse the trading is. A healthy market has lots of different bonds being traded.
					- Check market sentiment to see if people are feeling good about the bonds you’re interested in.
	
				- Market Analysis:
					- Look at capped volume to get a sense of trading activity without being misled by outliers.
					- Use daily and monthly aggregates to track trends and spot opportunities.
					- Explain the market breadth and sentiment to help stakeholders and decision-makers understand how the bond market is doing.

		## METADATA

			- Schema for each Equity endpoint included.


	## Paging

		- There are a number of  response headers that can be used to page through larger datasets in compliance with the response size and offset limits, including:

			- Record-Total response header: Total records found at the time of the request. Use this value when paging through large datasets to determine when all data has been retrieved.
			- Record-Offset response header: Set to match the offset value provided with the request.
			- Record-Limit response header: Set to match the limit value provided with the request or the platform maximum limit, whichever is smaller. 
			- Record-Max-Limit response header: Returns the current maximum number of records that will be returned by the API platform. Use to build a flexible integration that can adapt to future record limit changes.
			- Response-Payload_Max_Size response header: Returns the current maximum payload size (in MB) returned by the API platform. Use to build a flexible integration that can adapt to future payload response size changes. Applies only to synchronous API requests.
		
			- Notes:
				- To maximize the number of records returned in a response minimize the number of fields you return and use the text/plain Accept header on  requests whenever possible (e.g. the Equity datasets support text/plain).
				- API clients should count the number of records returned in each response in order to ensure all expected data is returned, and to adapt future requests if less data than expected was returned because the record and/or the response payload size limits are reached.