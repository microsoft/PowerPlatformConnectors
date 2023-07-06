# Tumblr
Tumblr is a microblogging and social networking website. The service allows users to post multimedia and other content to a short-form blog.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Use of the Tumblr service requires a [Tumblr account](https://www.tumblr.com/register).

## Obtaining Credentials
Tumblr uses OAuth for authentication. You will need to [register an application](https://www.tumblr.com/oauth/register), providing a name, website, and description. For Default Callback URL and OAuth Redirect URLs use: https://global.consent.azure-apim.net/redirect


## Supported Operations
### Get blog info
Retrieves general information about the blog, such as the title, number of posts, and other high-level data.
### Get blog's blocks
Retrieve the blogs that the requested blog is currently blocking.
### Remove a block
Allows you to remove a block on a post by identifier.
### Block a blog
Allows you to block a post by identifier.
### Block a list of posts
Allows you to block a list of posts by identifier.
### Get blog's likes
Retrieve a list of all likes for a blog.
### Get blog's following
Retrieve the publicly exposed list of blogs that a blog follows, in order from most recently-followed to first.
### Get a blog's followers
Retrieve a list of a blog's followers
### Check if followed by blog
Used to check if one of your blogs is followed by another blog.
### Get queued posts
Retrieves a list of the currently queued posts for the specified blog.
### Reorder queued posts
Allows you to reorder a post within the queue, moving it after an existing queued post, or to the top.
### Shuffle queued posts
Randomly shuffles the queue for the specified blog.
### Get draft posts
Retrieve a list of draft posts.
### Get submission posts
Retrieve a list of submission posts.
### Get blog's activity feed
Retrieve the activity items for a specific blog, in reverse chronological order.
### Create or reblog a post (Neue Post Format)
Allows you to create posts and reblogs using the Neue Post Format.
### Fetch a post (Neue Post Format)
Retrieve a post.
### Edit a post (Neue Post Format)
Allows you to edit posts using the Neue Post Format.
### Delete a post
Deletes a post by the identifier.
### Get notes for a post
Retrieve a list of notes for a specific post.
### Get user information
Retrieve the user account information.
### Get user limits
Retrieve information about the various user limits.
### Get user dashboard
Retrieve the information in the user dashboard.
### Get user likes
Retrieve the user liked posts.
### Get blogs following
Retrieve the list of blogs the user is following.
### Follow a blog
Follow a blog by the URL or email address.
### Unfollow a blog
Unfollow a blog by the URL or email address.
### Like a post
Like a post by the post identifier and reblog key.
### Unlike a post
Unlike a post by post identifier and reblog key.
### Get posts with tag
Retrieve a list of posts with a given tag.


## Known Issues and Limitations
There are no known issues at this time.
