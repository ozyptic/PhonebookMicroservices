# PhonebookMicroservices

### Scenario : 

By designing a structure with at least two communicating microservices, a simple phone directory application will be created.

Expected functionalities:

- Creating a contact in the directory
- Removing a contact from the directory
- Adding contact information to a contact in the directory
- Removing contact information from a contact in the directory
- Listing all contacts in the directory
- Retrieving detailed information, including contact details, for a specific contact in the directory
- Requesting a report that provides statistics based on the location of contacts in the directory
- Requesting a report that provides statistics based on the location of contacts in the directory
- Listing the generated reports by the system
- Retrieving detailed information for a generated report by the system

### Technical Design : 

Contacts: The system should be capable of theoretically unlimited contact records. Contact-specific communication details should also be able to be added in an unlimited manner.

The required fields in the expected data structure are as follows:

- UUID
- First Name
- Last Name
- Company
- Contact Information
  - Information Type: Phone Number, Email Address, Location
  - Information Content

Report: Report requests will work asynchronously. When a user requests a report, the system will handle this process sequentially in the background without creating bottlenecks; once the report is completed, the user should be able to observe the status of the report as "completed" through the "list of reports" endpoint.

The report will simply contain the following information:

- Location Information
- Number of contacts registered in the directory at that location
- Number of phone numbers registered in the directory at that location

In terms of data structure:

- UUID
- Request Date of the Report
- Report Status (Preparing, Completed)

### Used Techs
- .NET 7
- MongoDB
- RabbitMQ

### HTTP Requests 
#### Contact
`
  POST http://localhost:5239/contacts/AddContact
`

`
  DELETE http://localhost:5239/contacts/DeleteContact/{contactId}
`

`
  GET http://localhost:5239/contacts/GetContactById/{contactId}
`

`
  GET http://localhost:5239/contacts/GetAllContactsAsList
`

#### ContactInfo
`
  POST http://localhost:5239/contactInfo/AddContactInfo
`

`
  GET http://localhost:5239/contactInfo/GetAllContactInfosAsList
`

`
  DELETE http://localhost:5239/contactInfo/DeleteContactInfo/{contactId}
`

`
  GET http://localhost:5239/contactInfo/GetContactInfoById/{contactId}
`

(Contact-ContactInfo Swagger : http://localhost:5239/swagger/index.html)

(Contact-ContactInfo HealthCheck : http://localhost:5239/contactapi-health)

#### Reports

`
  GET http://localhost:5194/Reports/GetAllReports
`

`
  GET http://localhost:5194/Reports/GetReportById/{id}
`

`
  POST http://localhost:5194/Reports/CreateReport
`

(Report Swagger : http://localhost:5194/swagger/index.html)

(Report HealthCheck : http://localhost:5194/reportapi-health)

![phonebook-diagram](https://raw.githubusercontent.com/ozyptic/PhonebookMicroservices/dev/phonebook_diagram.png)

