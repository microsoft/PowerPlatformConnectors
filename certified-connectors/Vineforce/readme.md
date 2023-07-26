# Vineforce
Vineforce is a unified workspace designed for Microsoft 365 that smartly integrates your projects, tasks, contacts, and files. Enhance collaboration, streamline planning, and boost productivity with AI-powered task suggestions and reminders.

## Publisher: Vineforce, Inc.
Vineforce is a remote workplace tool built for Microsoft 365 users.

## Prerequisites
Vineforce subscription

## Supported Operations
1. Create Task

2. Alert

3. Create Project

4. Update Task

5. Create Company

6. Create Contact

7. Create Contact Phone

8. Create Contact Address

9. Create Contact Family

10. Create Contact Note

### Create Task
This operation facilitates the creation of a new task within the Vineforce App. By specifying parameters like task name, due date, and assigned user, you can effectively delegate responsibilities and manage your workflow.

### Alert
This operation allows you to generate alerts and notify users based on defined parameters. You can set the alert's message, recipient, and message, ensuring that vital information is promptly delivered.

### Create Project
This operation enables the creation of a new project within the Vineforce App, complete with sections and file links. By providing information such as project name, sections, members and creator's email address, you can create a well-structured project environment.

### Update Task
This operation allows you to modify the details of a pre-existing task using the unique TaskID returned by the Create Task action. You can update various aspects of the task, such as name, description, due date, or assigned user, ensuring that tasks always reflect current requirements or changes.

### Create Company
This operation allows you to create a new company in your system. Parameters like company name, address details, tax id, and website URL can be specified to create a comprehensive company profile.

### Create Contact
This operation enables you to add a new contact to your database. It requires parameters such as the contact's first, email address and contact owner's email address.

### Create Contact Phone
This operation allows you to associate a new phone number with an existing contact. By providing the unique Contact email address and the phone number, you can expand a contact's details.

### Create Contact Address
This operation enables you to add a new address to a specific contact in your database. It requires parameters like the unique Contact email address, address type, street address, city, state, zip code, and country.

### Create Contact Family
This operation allows you to add a new family contact. By specifying parameters such as the unique Contact email address, their relationship to the primary contact, and the first name, you can expand a contact's family details.

### Create Contact Note
This operation lets you create a new note associated with a specific contact. By specifying the unique Contact email address and the content of the note, you can add additional context or information to a contact.

## Obtaining Credentials
API key can be obtained from admin user. It is available within vineforce application under company settings.​

## Deployment Instructions
https://www.vineforce.com/en/help/what-is-the-vineforce-power-automate-connector