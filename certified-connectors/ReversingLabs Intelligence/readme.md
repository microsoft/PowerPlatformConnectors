# ReversingLabs Intelligence

ReversingLabs continually processes goodware and malware files providing early intelligence about attacks before they infiltrate customer infrastructures. This visibility to threats “in-the-wild” enables preparation for new attacks and quickly identifies the threat levels of new files as they arrive. ReversingLabs enables more effective and efficient threat identification, development of better threat intelligence, and implementation of proactive threat hunting programs.

## Prerequisites

To use this integration, you need to have a ReversingLabs account. Please contact `sales@reversinglabs.com` to get started.

## Known issues and limitations

Please note that some of our APIs will return a 404 to indicate that a resource was not found.  This is not an error state but simply informational.  To avoid the Logic App showing errors in the run state, we were advised to place calls to APIs in a Scope primitive.
