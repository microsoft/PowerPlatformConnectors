# Open Trivia Database

The Open Trivia DB is a fun and interactive API that provides an easy way to acquire fun and random trivia questions at a moments notice.

## Publisher: Kiveshan Naidoo, Rishay Sunder


## Prerequisites

None

## Supported Operations

### Get a certain number of trivia questions, based on a criteria

Get a certain amount of questions from the database based on categories such as difficulty, Question Type and Category. Leaving
these options blank will return a random question

### Category Lookup
Returns the entire list of categories and ids in the database.

### Category Question Count Lookup:
Returns the number of questions in the database, in a specific category.

### Global Question Count Lookup: 
Returns the number of ALL questions in the database. Total, Pending, Verified, and Rejected.

## Obtaining Credentials
No credentials required

## Limitations
Only 1 Category can be requested per API Call. To get questions from any category, don't specify a category.
A Maximum of 50 Questions can be retrieved per call.

## API Documentation
Visit the [Open Trivia DB](https://opentdb.com/api_config.php) page for further details
