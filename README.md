# Muonroi.BaseTemplate
[![Ask DeepWiki](https://raw.githubusercontent.com/muonroi/MuonroiBuildingBlock/main/src/Muonroi.BuildingBlock/Images/deep-wiki.png)](https://deepwiki.com/muonroi/Muonroi.BaseTemplate)

This repository provides a .NET solution template designed to accelerate the development of ASP.NET Core applications. It builds directly on the Muonroi.BuildingBlock library and follows Clean/Onion Architecture principles, providing a solid foundation with separate projects for the API, Core (Domain), and Data (Infrastructure) layers.

This template is built on .NET 9.0 and comes pre-configured with a suite of modern tools and patterns to support building robust and scalable web APIs.

## Features

*   **.NET Solution Template:** Easily create new projects using `dotnet new`.
*   **Clean Architecture:** A well-organized structure separating concerns into distinct layers:
    *   **API:** Handles presentation logic, controllers, and API endpoints.
    *   **Core:** Contains domain entities, interfaces, and core business logic.
    *   **Data:** Implements data access and other infrastructure concerns.
*   **CQRS with MediatR:** Implements the Command Query Responsibility Segregation pattern using the MediatR library for clean and decoupled application logic.
*   **Authentication & Authorization:** Uses Muonroi.BuildingBlock for JWT validation, permission filters, and dynamic permission sync. Enum-based permission system (`Permission.cs`).
*   **Entity Framework Core:** Configured for data persistence with a `DbContext` and repository pattern implementation.
*   **Structured Logging with Serilog:** Integrated via Muonroi.BuildingBlock helpers, with console sink by default (Elasticsearch optional).
*   **Dependency Injection:** Properly configured using built-in .NET DI and supports Autofac.
*   **FluentValidation:** Includes request validation using FluentValidation.
*   **Localization Support:** Demonstrates localization for error messages with resource files for English (`en-US`) and Vietnamese (`vi-VN`).
*   **Centralized Building Blocks:** Utilizes the `Muonroi.BuildingBlock` NuGet package to provide shared components and abstractions.
*   **Service Discovery & Messaging:** Ready-to-use Consul registration, gRPC server setup and Kafka (MassTransit) integration.
*   **Multi‑Tenancy & Dynamic Permission:** TenantContext middleware, default filters, and automatic permission synchronization.

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

## Configuration (aligns with Muonroi.BuildingBlock)

The application's behavior can be configured through `appsettings.Development.json` and `appsettings.Production.json`. Key settings include:

*   `DatabaseConfigs`: Set `DbType` (Sqlite/SqlServer/Postgres) and connection strings.
*   `TokenConfigs`: JWT issuer/audience/keys; supports RSA (UseRsa=true) as in the template.
*   `MultiTenantConfigs`: Toggle multi‑tenancy (Enabled=true). For default tenant fallback, use `TenantConfigs:DefaultTenant`.
*   `Serilog`: Logging levels and sinks. Console by default; add Elasticsearch if needed.
*   `MAllowDomains`: CORS origins.
*   `RedisConfigs`: Redis caching; set `AllMethodsEnableCache` for global cache policy.
*   `MessageBusConfigs`: Kafka bus (host/topic/groupId) via BuildingBlock integration.
*   `ConsulConfigs`: Service discovery registration + health checks.
*   `GrpcServices`: Sample outbound gRPC endpoints.
*   `PaginationConfigs`, `ResourceSetting`, `SecretKey`, `EnableEncryption`: auxiliary settings used by the blocks.

See the full documentation: ../../docs/introduction.md and grouped guides under ../../docs/.

## License

This project is licensed under the MIT License. See the [LICENSE.txt](LICENSE.txt) file for details.
