# Mediastac

Feed the latest and most popular news articles into your application or website, fully automated & updated every minute.

## Publisher: Fördős András

## Prerequisites

Mediastack is available both as a free and as a subscription based service. You will need to sign up and choose the fitting plan: [https://mediastack.com/](https://mediastack.com?utm_source=FirstPromoter&utm_medium=Affiliate&fpr=mediastack)

## Obtaining Credentials

The connector uses API key for authentication, which you can obtain through your Medistack profile, once signed up, through your Account dashboard.

## Supported Operations

### List news
The full set of available real-time news articles can be accessed and filtered down with additional parameters.

### List sources
Using the sources endpoint together with a series of search and filter parameters you will be able to search all news sources supported by the mediastack API.

## Known Issues and Limitations

Current version of the connector only supports a subset of the API endpoints and response data. Contact me if you see a need to bring in any of the other ones!

The underlying API might have additional applicable limitations, especially around response data and rate limitation, depending on your subscription.

### HTTP vs HTTPS

The underlying service provides the option to both use HTTP and HTTPS, however the current implementation of the connector only supports HTTP. In case you would want to use HTTPS (too), please get in touch!