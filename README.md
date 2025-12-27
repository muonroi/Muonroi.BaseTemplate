# Muonroi.BaseTemplate

[![Ask DeepWiki](https://raw.githubusercontent.com/muonroi/MuonroiBuildingBlock/main/src/Muonroi.BuildingBlock/Images/deep-wiki.png)](https://deepwiki.com/muonroi/Muonroi.BaseTemplate)

A .NET solution template for building ASP.NET Core applications using Clean/Onion Architecture with the Muonroi.BuildingBlock library.

## Quick Start

```bash
# 1. Install template
dotnet new install Muonroi.BaseTemplate

# 2. Create new project
dotnet new mr-base-sln -n MyProject -C MyCore

# 3. Setup
cd MyProject
dotnet restore

# 4. Run migrations (optional)
./scripts/ef.sh add InitialCreate
./scripts/ef.sh update

# 5. Run
cd src/MyProject.API
dotnet run
```

Open: `https://localhost:5001/swagger`

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- (Optional) [EF Core CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

## Installation

### From NuGet (recommended)

```bash
dotnet new install Muonroi.BaseTemplate
```

### From source

```bash
git clone https://github.com/muonroi/MuonroiBuildingBlock.git
cd MuonroiBuildingBlock/src/Muonroi.Base.Template
dotnet new install ./
```

### Verify installation

```bash
dotnet new list | grep "mr-base-sln"
```

## Usage

### Create new project

```bash
dotnet new mr-base-sln -n <ProjectName> [-C <ClassName>]
```

| Parameter | Short | Description | Default |
|-----------|-------|-------------|---------|
| `--name` | `-n` | Solution/project name | (required) |
| `--ClassName` | `-C` | Base name for classes (DbContext, etc.) | `BaseTemplate` |

### Examples

```bash
# Basic
dotnet new mr-base-sln -n MyApp

# With custom class name
dotnet new mr-base-sln -n TruyenTM -C TruyenTM
# Generates: TruyenTMDbContext, TruyenTMRepository, etc.
```

## Project Structure

```
MyProject/
├── MyProject.sln
├── scripts/
│   └── ef.sh                    # Cross-platform migration helper
├── src/
│   ├── MyProject.API/           # Presentation Layer
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Production.json
│   │   ├── appsettings.Example.json   # Configuration reference
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   └── Application/
│   ├── MyProject.Core/          # Domain Layer
│   │   ├── Entities/
│   │   └── Interfaces/
│   └── MyProject.Data/          # Infrastructure Layer
│       ├── MyDbContext.cs
│       └── Repositories/
└── README.md
```

## Configuration

### Supported Database Types

| DbType | Connection String Key |
|--------|----------------------|
| `Sqlite` | `SqliteConnectionString` |
| `SqlServer` | `SqlServerConnectionString` |
| `MySql` | `MySqlConnectionString` |
| `PostgreSql` | `PostgreSqlConnectionString` |
| `MongoDb` | `MongoDbConnectionString` |

### Example Configuration

```json
{
  "DatabaseConfigs": {
    "DbType": "Sqlite",
    "ConnectionStrings": {
      "SqliteConnectionString": "Data Source=app.db"
    }
  },
  "TokenConfigs": {
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:5001",
    "SigningKeys": "your-secret-key-minimum-32-characters!",
    "UseRsa": false,
    "ExpiryMinutes": 60
  },
  "EnableEncryption": false
}
```

> See `appsettings.Example.json` for all available options with documentation.

### Feature Flags

Toggle optional features to reduce startup time:

```json
{
  "FeatureFlags": {
    "UseGrpc": false,
    "UseServiceDiscovery": false,
    "UseMessageBus": false,
    "UseBackgroundJobs": false,
    "UseEnsureCreatedFallback": true
  }
}
```

## Database Migrations

### Using helper script (recommended)

**Linux/macOS (or Git Bash on Windows):**
```bash
./scripts/ef.sh add InitialCreate
./scripts/ef.sh update
./scripts/ef.sh list
./scripts/ef.sh help
```

**Windows Command Prompt/PowerShell:**
```cmd
scripts\ef add InitialCreate
scripts\ef update
scripts\ef list
scripts\ef help
```

### Using dotnet ef directly

```bash
dotnet ef migrations add "InitialCreate" \
    -p ./src/MyProject.Data \
    --startup-project ./src/MyProject.API \
    -o Persistence/Migrations

dotnet ef database update \
    -p ./src/MyProject.Data \
    --startup-project ./src/MyProject.API
```

## Features

- **Clean Architecture** - Separate layers: API, Core, Data
- **CQRS with MediatR** - Command/Query separation
- **Authentication & Authorization** - JWT, permissions, roles
- **Entity Framework Core** - Multiple database support
- **Structured Logging** - Serilog with Console/Elasticsearch
- **Caching** - Memory, Redis, or Multi-level
- **Multi-tenancy** - Data isolation by tenant
- **Service Discovery** - Consul integration
- **Message Bus** - Kafka/RabbitMQ via MassTransit
- **Background Jobs** - Hangfire/Quartz

## Documentation

- [Template Quickstart Guide](../../docs/template-quickstart.md) - Detailed setup guide
- [Architecture Overview](../../docs/architecture-overview.md)
- [Appsettings Guide](../../docs/appsettings-guide.md)
- [Auth Module Guide](../../docs/auth-module-guide.md)
- [Permission Guide](../../docs/permission-guide.md)

## Troubleshooting

### "Connection string is not provided"

Ensure `DbType` matches the connection string key:

```json
{
  "DatabaseConfigs": {
    "DbType": "MySql",  // Must match key below
    "ConnectionStrings": {
      "MySqlConnectionString": "..."  // ✓ Correct key
    }
  }
}
```

### "The input is not a valid Base-64 string"

Set `"EnableEncryption": false` in appsettings.

### API slow on startup

Disable unused features in `FeatureFlags`.

## License

MIT License. See [LICENSE.txt](LICENSE.txt) for details.
