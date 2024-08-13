
# Papara Bootcamp Final Case

This repository contains the final project for the Papara Bootcamp. The project is based on a modular monolithic architecture, utilizing the CQRS pattern for managing business logic. RabbitMQ is used for sending notifications, and Redis is employed for caching.

## Table of Contents

- [Technologies](#technologies)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
  - [CommerceHub.API](#commercehubapi)
  - [CommerceHub.Base](#commercehubbase)
  - [CommerceHub.Business](#commercehubbusiness)
  - [CommerceHub.Data](#commercehubdata)
  - [CommerceHub.Schema](#commercehubschema)
- [Setup and Installation](#setup-and-installation)
- [API Documentation](#api-documentation)
- [Usage](#usage)

## Technologies

- **.NET Core 8.0** - Main framework of the project
- **Entity Framework Core** - ORM for database access
- **RabbitMQ** - Message broker for notifications
- **Redis** - In-memory data store for caching
- **PostgreSQL** - Relational database management system
- **Swagger** - API documentation and testing tool
- **Postman** - Tool for API testing
- **CQRS** - Pattern for separating commands and queries

## Architecture

This project follows a monolithic architecture. The business logic layer implements the CQRS pattern. RabbitMQ is used for sending notifications, while Redis is utilized for caching purposes.

## Project Structure

### CommerceHub.API

This layer contains the API endpoints and middleware. It manages requests coming from the external world.

### CommerceHub.Base

This layer contains base classes used by other modules. Base classes are defined here to provide common functionality.

### CommerceHub.Business

This layer contains the business logic. The CQRS pattern is implemented here, separating command and query responsibilities.

### CommerceHub.Data

This layer contains the entities interacting with the database. It is used to manage database operations.

### CommerceHub.Schema

This layer defines the models used for requests and responses in the API. Request and response models are defined here.

## Setup and Installation

### Requirements

- .NET Core 8.0 SDK
- PostgreSQL
- RabbitMQ
- Redis
- Docker (optional)

### Installation Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/ramazanefegurkan/Papara-Bootcamp-Final-Case.git
   cd Papara-Bootcamp-Final-Case
   ```

2. Set up the database:
   ```bash
   dotnet ef database update
   ```

3. Configure the PostgreSQL connection string, RabbitMQ, and Redis settings in the `appsettings.json` file:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=your_database;Username=your_username;Password=your_password",
     "Redis": "localhost:6379"
   },
   ```
4. Configure RabbitMQ in Program.cs using MassTransit:
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<OrderPlacedConsumer>();
    
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host("rabbitmq://localhost", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
    
            cfg.ReceiveEndpoint("order-placed-queue", e =>
            {
                e.ConfigureConsumer<OrderPlacedConsumer>(context);
            });
        });
    });
5. Run the application:
   ```bash
   dotnet run
   ```
6.Default Admin User
```
   username:test
   password:test
```

## API Documentation

The API is documented using Swagger. Once the application is running, you can access the documentation at:

```
https://localhost:7276/swagger/index.html
```

Alternatively, you can import the provided Postman collection in the repository to test the API endpoints using Postman.
```
https://documenter.getpostman.com/view/34773448/2sA3s4kVct
```

## Usage

You can interact with the application directly via the API using Swagger or Postman, or you can integrate the modules into your own applications. The application supports user registration, order management, report generation, and notification sending.
