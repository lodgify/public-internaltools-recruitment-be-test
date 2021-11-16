## How we'd like to recieve the soluion?

1. Clone this repository and upload it as a new public repository in your github account
2. Create a new branch in your repository
3. Create a pull request with the requested functionality to unchanged master branch in your repository
4. Share link to the PR with us 

# Internal Tools Backend C# developer project

## Background

As a team focused on giving tools to our support and marketing teams, we usually need to deal with external API integrations. 

## The Code

This solution has 3 diferent projects in it. 
* One pretends to be a admin panel (SuperPanel.App) where our users list can be accessed, 
* there is also and API project (ExternalContact.Api). This API simulates an external integration where we send our users as contacts so we can use that external tool.
* Also there is a skeleton test project

### SuperPanel.App

This is a Asp.net core MVC project with one controller that renders all users in a table

### ExternalContacts.API

This api has just 4 endpoints

* /v1/contacts (GET): gets a list of all contacts in the external integration
* /v1/contacts/{email} (GET): gets a specific contact by searching for the email
* /v1/contacts/{id} (GET): gets a specific contact by searching for the id. Please note that the ID refers to the ExternalContacts ID.
* /v1/contacts/{id}/gdpr (put): places a GDPR request for the user with the id, this will anonymize any Personal Identifiable Information the External Contacts Integration has for that user. Please note that the ID refers to the ExternalContacts ID.

## The tasks

1. As the number of users has increased a lot, the SuperPanel user list is showing too much users, so we need to add a pagination feature. This includes changes from UI to the data access

   - SuperPanel is using Boostrap for convenience, feel free to use its UI features or change to another if you want.

2. We also need to implement a new feature that will allow the SuperPanel operators to do an GDPR deletion request for a given user. In order to do that we will make use of the External Contacts API, described above, where we will request for GDPR deletion for a user/s. To do this we will need to do a HTTP call to the External Contacts API.

   - The user may or may not exists on the External Contacts API, so you might consider this in your implementation
   - The External Contacts API is a bit unstable so you may expect random errors and it also has a very restrictive rate limiting in place

3. Please consider existing code as legacy. Improve it. Please focus on readability, maintainability and self-explanatory. Unit tests are more than welcome.

4. After all this hard work is done, support operators asked to provide some additional way to do a GDPR request for a batch of users. 

   - With the experience of the previous implementation, how you will approach this task. You can answer this in the description of a new empty PR.
   - **Optional** (nice to have): Implement this feature following approach described in the previous point

### What we are looking for

- How you deal with the existing code, understand it and work with it
- How you implement new features and understand the requirements (feel free to ask in case of doubts)
- How you work with MVC, css or js that you need in order to implement your changes. 
- How you approach the usage of external API's
- How you test the correctness of your solution

### Restrictions

- Please do not modify the External API project
- Besides it changing/adding everything is allowed, feel free to add new projects, libraries etc. - everything what you consider as useful
- If there is something that you want us to notice in your approach please add a text file describing it or do it on the PR request
