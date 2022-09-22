#  Qiita
Qiita is a service for recording and sharing engineering knowledge. You can easily record and publish your programming tips, know-how, and notes.
Web Site:https://qiita.com/

## Publisher: Miyake Hideo

## Prerequisites
There are no prerequisites to use this service.

## Obtaining Credentials
There are no credentials needed to use this service.

## Supported Operations

* [Get Items](/api/v2/items)
	- This endpoint returns a list of articles in descending order by creation date.
* [Get Items by ID](/api/v2/items/{Post_Id})
	- This endpoint retrieves articles with the article ID as a parameter.
* [Get Items by Tag](/api/v2/tags/{Tag}/items)
	- This endpoint returns a list of articles tagged with the specified tag, in descending order by the date and time of tagging.
* [Get Items by UserID](/api/v2/users/{User_Id}/items)
	- This endpoint returns a list of articles for the specified user in descending order by creation date.
* [Get Stocks by UserID](/api/v2/users/{User_Id}/stocks)
	- This endpoint returns a list of articles stocked by the specified user in descending order of stocked date and time.

## API documentation
Visit [Qiita API v2 documentation page](https://qiita.com/api/v2/docs) for further detail

## Known Issues and Limitations
There are no known issues at this time.
