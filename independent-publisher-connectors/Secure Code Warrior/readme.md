# Secure Code Warrior
This connector uses your API key to access various Secure Code Warrior results and the creation of users and teams. Secure Code Warrior makes secure coding a positive and engaging experience for developers as they increase their software security skills. With our flagship Learning Platform, we guide each coder along their own preferred learning pathway, so that security-skilled developers become the everyday superheroes of our connected world.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Secure Code Warrior has two paid plans - Business and Enterprise. Both plans have the same access to the REST API.

## Obtaining Credentials
API access in Secure Code Warrior is disabled by default. An admin account can generate a new key from the Company Administration > Edit Company section.  The API key will be used in the header of the request, in the field X-API-Key.

## Supported Operations

### Assessments
#### Assign assessments
This endpoint will assign the given assessment to the specified users/teams. Returns a list of users that were assigned to the assessment with their email, inviteUrl and status indicating whether invite email was sent.

#### Get assessments
This endpoint will return a list of Assessment objects which contain a range of datapoints related to assessments. This includes assessment IDs, which may be used in conjunction with the other assessments API endpoints.

#### Get assessment attempts
Returns a detailed report for all developers who have taken the Assessment with the given ID. The scope of the report may be filtered in a number of ways. You may filter results to a given developer by supplying their email address with the query. If a start date is provided, only attempts started or completed after (and including) this date are included. If an end date is provided, only challenges started or completed before this date are included. Providing both indicates a date range to filter on. This includes all the data in the summary report plus details on all the challenges that are a part of the assessment.

#### Search assessment
This endpoint will return a list of Assessment objects which contain a range of datapoints related to assessments. This includes assessment IDs, which may be used in conjunction with the other assessments API endpoints. Results can be filtered based on status, name, supported language and difficulty.

#### Search assessment attempts
Returns a detailed report for all developers who have taken the Assessment with the given ID. The scope of the report may be filtered in a number of ways. You may filter results to given developer(s) by supplying their email address(es) with the query. If a status is provided, only assessment attempts with specified status are included. If team name(s) provided, only assessment attempts of users within those team(s) are included. If tags(s) is provided, only assessment attempts of users with those tag(s) are included. If a pass status is provided, only assessment attempts with specified pass status are included. This includes all the data in the summary report plus details on all the challenges that are a part of the assessment.

### Courses
#### Get all courses
Return a list of course objects which contain a range of datapoints related to courses. This includes course IDs, which may be used in conjunction with the other course API endpoints.

#### Get progress of all developers for the course
Return a list of the progress of all developers for the specified course.

#### Search course developers progress
Returns a list of the course progress of all developers within the organization.

### Learning
#### Get learning progress
Returns a list of learning resources which have be completed, read or watched.

#### Get resources
Returns a list of learning resources.

#### Search learning progress
Returns a list of detailed learning progress.

### Metrics
#### Get activity strengths and weaknesses
Returns the average strengths and weaknesses for a company, team or user (depending on the level of granularity of the query) for a specific language or all languages.

#### Get activity top performers
Retrieves a list of top performing users.

#### Get team activity most engaged
Returns the top 'N' most engaged teams.

#### Get users activity most engaged
Returns the top 'N' most engaged users.

#### Get time spent
Time spent on the platform across assessments, learning, tournaments and training.

### Teams
#### Create a team
Creates a team record.

#### Delete team
Deletes a team record.

#### Get team details
Returns a list of team details.

#### Get teams
Returns a list of team objects which contain a range of datapoints related to teams. This includes team IDs, which may be used in conjunction with the other team API endpoints.

#### Update team details
Update details of a team.

### Tournaments
#### Get tournaments
Returns a list of tournaments in the company.

#### Get tournament leaderboard
Returns the leaderboard for a single tournament. This includes all levels, challenges and stages that the developer participated in, with detailed metrics of each.

#### Search tournament leaderboard
Returns the leaderboard for a single tournament. This includes all levels, challenges and stages that the developer participated in, with detailed metrics of each. This endpoint can also return the leaderboard over a report period (which may be 1, 7 or 30 days) or a given date range. Results can be filtered based on developer emails, developer tags and team names.

### Training
#### Get developer activity
Returns the detailed challenge log of all developers within the organization, with challenge score, difficulty and challenge outcome of the developer.

#### Get developer leaderboard
Returns a list of all developers within the organization, with their current stats as well as the change in stats over the report period (which may be 1, 7 or 30 days).

#### Get developer progress
Returns the training progress of all developers within the organization, with current realm, level and quest progress.

#### Get team leaderboard
Returns a list of all teams within the organization, with their current stats as well as the change in stats over the report period (which may be 1, 7 or 30 days).

#### Search developers activity
Returns the detailed challenge log of all developers within the organization, with challenge score, difficulty and challenge outcome of the developer. This also lists the challenge log over the report period (which may be 1, 7 or 30 days) or a given date range. Results can be filtered based on developer emails, developer tags and team names.

#### Search developer leaderboard
Returns a list of all developers within the organization, with their current stats. This also lists the change in stats over the report period (which may be 1, 7 or 30 days) or a given date range. Results can be filtered based on developer emails, developer tags and team names.

#### Search developers progress
Returns the training progress of all developers within the organization, with current realm, level and quest progress. This also lists the change in developer progress over the report period (which may be 1, 7 or 30 days) or a given date range. Results can be filtered based on developer emails, developer tags and team names.

### URL Fetcher
#### Get course URL
Returns the course URL.

### Users
#### Create a user
Creates a user record.

#### Delete user
Deletes the user referenced by the given ID.

#### Get user
Retrieves a single user record. The response may be shaped by passing in a fields query parameter listing the attribute names to return.

#### Get users
Retrieves a list all user records. The response may be shaped by passing in a fields query parameter listing the attribute names to return.

#### Search users
Retrieves a list of all user records. The response may be shaped by passing in a fields query parameter listing the attribute names to return. Filter is achieved by passing the filter criteria through the body.

#### Update user
Updates a user with the given data. Tags will be replaced.

## Known Issues and Limitations
At this time, there are no known issues.
