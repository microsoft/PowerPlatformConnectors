# Khalibre LMS connector
Khalibre is the provider of the Khalibre Platform, used by Crosswired and other collaboration and capacity building portals. We are a social enterprise, formed in 2010 in Singapore and Cambodia to deliver business technology solutions, capacity building and learning platform for medium and large organizations globally. With the Khalibre LMS Connector, you can extend LMS capabilities by integrating with the system of your choice through the APIs.

# Prerequisites
You will need the following to proceed:
1. Khalibre platform account
2. Your organization enables API Integrations for learning management system
3. OAuth 2.0 credentials generated for secure API Integrations

# How to get OAuth 2.0 credentials

Login as an organization admin and navigate to organization **Settings** > **API Integrations**. Under **OAuth 2.0 credentials** section, you can obtain the client ID & secret. You can also delete the credentials or generate new credentials.

# Supported triggers
With **When an HTTP request is received** as a trigger, you can provide the endpoint where Khalibre platform will send trigger action for the following operations:
1. Course created (event type: course.created)
   - Course status is changed from draft to public.
2. Course updated (event type: course.updated) 
   - Course information is updated.
3. Course deleted (event type: course.deleted) 
   - Course status is changed from public to draft.
   - Course status is changed from public to archive.
   - Course is deleted.
4. Community updated (event type: community.updated)
   - Community name is updated.
5. Course share (event type: course.shared)
   - Community admin added shared course into the community.
6. Course unshared (event type: course.unshared)
   - Shared course is removed from the community.
7. Course progress status (event type: course.status)
   - Learner made progress and course status is changed. 
8. Course progress activity (event type: course.activity)
   - Learner made progress and completed new course activity.

You can register your endpoint to Khalibre platform through organization **Settings** > **API Integrations**. Under **Webhooks** section, enable the webhooks and provide your endpoint to Webhook URL field. Khalibre platform will make an HTTP post to this URL when there is any event related to the course.

# Supported Actions
* `Read courses`: Returns all organization courses with visibility set to API.
* `Read course detail`: Returns course detail information for a given course ID.
* `Progress by course ID`: Returns all learner progresses for a given course ID.
* `Progress by email`: Returns all learner progresses for a given learner email address.
* `Book course`: Book a learner to a given course ID.

Note - Course ID here is equivalent to class ID in Khalibre platform.

You can also obtain a link to **API developer doc** through organization **Settings** > **API Integrations** > **API developer doc**.