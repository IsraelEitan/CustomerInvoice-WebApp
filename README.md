Customer Invoices Web Application (Server )

Description
  This is a .NET 7 Core web-based application designed to manage customer invoices.
  It allows users to view, create, and edit all invoices in the system with a focus on clean architecture and best coding practices.

Features
  .NET 7 Core: Utilizes the latest features of .NET 7 Core for optimal performance and reliability.
  
  Entity Framework Core: Implements Code First approach with Entity Framework Core for SQL Server.
  
  AutoMapper: Simplifies object-to-object mappings.
  
  Swagger: Integrated Swagger UI for API testing and documentation.
  
  Dependency Injection: Implements Dependency Injection for better testing and maintainability.
  
  Model Validation: Robust model validation to ensure data integrity.
  
  Repository and Unit of Work Patterns: Implements Repository and Unit of Work patterns to abstract the data layer.
  
  Simple Caching: Utilizes MemoryCache for efficient caching and retrieval of data.

  Simple Paging for better performance 

  SOLID Principles: Adheres to SOLID principles for clean and scalable software design.
  
  Clean Architecture: Follows Clean Architecture guidelines for a maintainable and flexible codebase.


NOTE:
    The focus here is only on the Invoices hance the InvoicesController is the only controller that is written here.
    The Customer models ( dto, model ) is for model demonstration and data purposes only. 

Getting Started

Prerequisites
  .NET 7 SDK
  SQL Server
  
Running Migrations
  To set up your database, run the following command:
  dotnet ef database update
    
Running the Application
  dotnet run
    
Access the Swagger UI to test the application functionality:
  http://localhost:[port]/swagger

Usage
  The application provides endpoints to manage customer invoices. These include:

Viewing Invoices: Fetch a list of all invoices or specific invoices based on certain criteria.
Creating Invoices: Add new invoices to the system.
Editing Invoices: Update the details of existing invoices.
...and more.

Refer to the Swagger UI for the full list of endpoints and their usage.
