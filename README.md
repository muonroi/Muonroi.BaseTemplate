# Muonroi.BaseTemplate
[![Ask DeepWiki](https://raw.githubusercontent.com/muonroi/MuonroiBuildingBlock/main/src/Muonroi.BuildingBlock/Images/deep-wiki.png)](https://deepwiki.com/muonroi/Muonroi.BaseTemplate)

This repository provides a .NET solution template designed to accelerate the development of ASP.NET Core applications. It is structured based on Clean/Onion Architecture principles, providing a solid foundation with separate projects for the API, Core (Domain), and Data (Infrastructure) layers.

This template is built on .NET 9.0 and comes pre-configured with a suite of modern tools and patterns to support building robust and scalable web APIs.

## Features

*   **.NET Solution Template:** Easily create new projects using `dotnet new`.
*   **Clean Architecture:** A well-organized structure separating concerns into distinct layers:
    *   **API:** Handles presentation logic, controllers, and API endpoints.
    *   **Core:** Contains domain entities, interfaces, and core business logic.
    *   **Data:** Implements data access and other infrastructure concerns.
*   **CQRS with MediatR:** Implements the Command Query Responsibility Segregation pattern using the MediatR library for clean and decoupled application logic.
*   **Authentication & Authorization:** Includes a pre-built JWT-based authentication system with endpoints for login and token refresh. It also features a flexible, enum-based permission system (`Permission.cs`).
*   **Entity Framework Core:** Configured for data persistence with a `DbContext` and repository pattern implementation.
*   **Structured Logging with Serilog:** Integrated Serilog for powerful and configurable logging, with sinks for the console and Elasticsearch.
*   **Dependency Injection:** Properly configured using built-in .NET DI and supports Autofac.
*   **FluentValidation:** Includes request validation using FluentValidation.
*   **Localization Support:** Demonstrates localization for error messages with resource files for English (`en-US`) and Vietnamese (`vi-VN`).
*   **Centralized Building Blocks:** Utilizes the `Muonroi.BuildingBlock` NuGet package to provide shared components and abstractions.
*   **Service Discovery & Messaging:** Ready-to-use Consul registration, gRPC server setup and MassTransit message bus integration.
*   **Multi-Tenant & Dynamic Permission:** Includes tenant context middleware and automatic permission synchronization.

## Project Structure

The solution is organized into the following projects:

```
└── src/
    ├── Muonroi.BaseTemplate.API/   # Presentation Layer (API endpoints, Commands/Queries, DI setup)
    ├── Muonroi.BaseTemplate.Core/  # Domain Layer (Entities, Interfaces, Enums)
    └── Muonroi.BaseTemplate.Data/  # Infrastructure Layer (DbContext, Repositories, Migrations)
```

*   `Muonroi.BaseTemplate.API`: Contains the ASP.NET Core Web API project. It hosts the controllers and is the entry point of the application. It depends on the Core and Data layers.
*   `Muonroi.BaseTemplate.Core`: The core of the application. It contains domain entities (`SampleEntity`), business logic, and abstractions (interfaces) for repositories and other services. This project has no external dependencies on other layers.
*   `Muonroi.BaseTemplate.Data`: Implements the interfaces defined in the Core layer. It contains the Entity Framework `DbContext`, repository implementations, and any other infrastructure-related code like database configurations.

## Getting Started

### Prerequisites

*   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later.

### Installation

To install the template from this repository, clone it and run the following command from the root directory:

```bash
dotnet new install ./
```

### Usage

Once the template is installed, you can create a new solution using the following command. The short name for this template is `mr-base-sln`.

```bash
dotnet new mr-base-sln -n YourNewProjectName
```

This will create a new directory named `YourNewProjectName` with the complete solution structure.

#### Customization

The template allows you to specify a base name for classes like `DbContext`, which replaces `BaseTemplate` in the generated files. Use the `-C` or `--ClassName` parameter:

```bash
dotnet new mr-base-sln -n YourNewProjectName -C MyCoreName
```

This would generate files like `MyCoreNameDbContext.cs`, `MyCoreNameRepository.cs`, etc.

## Configuration

The application's behavior can be configured through `appsettings.Development.json` and `appsettings.Production.json`. Key settings include:

*   **`DatabaseConfigs`**: Set the `DbType` and connection strings.
*   **`TokenConfigs`**: Configure JWT issuer, audience, and keys for authentication.
*   **`Serilog`**: Configure logging levels and sinks (e.g., Elasticsearch nodes).
*   **`SecretKey`**: Provide a secret key used for encrypting sensitive configuration values.
*   **`MAllowDomains`**: Configure CORS origins.
*   **`RedisConfigs`**: Set up connection details for Redis caching.

## License

This project is licensed under the MIT License. See the [LICENSE.txt](LICENSE.txt) file for details.