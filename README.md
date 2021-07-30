# Overview
This is a sample project to implement a simple booking API using ASP.NET Core, MediatR and FluentValidation.

# Projects
There are 3 projects in this sample:
1. SettlementApi: ASP.NET Core API project which exposes an endpoint for appointment booking
2. Application: application layer which handles all application logic and validation rules.
The Domain entities are also put in here for simplicity. In reality, domain entities will be put in a separate project.
3. SettlementApi.Tests: unit tests to test the validation rules and business logic of this application.

# Technologies
* ASP.NET Core
* MediatR
* FluentValidation

# Design and implementation
* MediatR is used to segregate frontend controllers with business logic implemented in application layer.
* FluentValidation is used to write fluent validation rules on commands, queries sent from frontend controllers to application layer.

# How to test
1. Run SettlementApi project.
2. Use Postman to test with payloads in the following format:
{
    "bookingTime": "15:01",
    "name":"John Smith"
}
dfgsdg
dgfsfdh