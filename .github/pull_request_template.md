
---
### When submitting a connector, please make sure that you follow the requirements below, otherwise your PR might be rejected. We want to make you have a well-built connector, a smooth certification experience, and your users are happy :) 

- [ ] I attest that the connector works and I verified by deploying and testing all the operations. 
- [ ] I attest that I have added detailed descriptions for all operations and parameters in the swagger file.
- [ ] I attest that I have added response schemas to my actions, unless the response schema is dynamic. 
- [ ] I validated the swagger file, `apiDefinition.swagger.json`, by running `paconn validate` command.
- [ ] If this is a certified connector, I confirm that `apiProperties.json` has a valid brand color and doesn't use an invalid brand color, `#007ee5` or `#ffffff`. If this is an independent publisher connector, I confirm that I am not submitting a connector icon.

If you are an Independent Publisher, you must also attest to the following to ensure a smooth publishing process:
- [ ] I have named this PR after the pattern of "Connector Name (Independent Publisher)" ex: HubSpot Marketing (Independent Publisher)
- [ ] Within this PR markdown file, I have pasted screenshots that show: 3 unique operations (actions/triggers) working within a Flow. This can be in one flow or part of multiple flows. For each one of those flows, I have pasted in screenshots of the Flow succeeding. 
- [ ] Within this PR markdown file, I have pasted in a screenshot from the Test operations section within the Custom Connector UI.
- [ ] If the connector uses OAuth, I have provided detailed steps on how to create an app in the readme.md. 


