# Agent Working Standard

This file defines the unified working rules for Muonroi repositories.

## Scope

- `MuonroiBuildingBlock` (core library)
- `Muonroi.BaseTemplate`
- `Muonroi.Modular.Template`
- `Muonroi.Microservices.Template`
- `Muonroi.Docs` (documentation hub)

## Core Rules

- No quick workaround. Always do research first, then plan, then implement.
- Done means:
1. Plan is completed.
2. Unit tests pass 100%.
3. New test cases are added for each upgrade behavior.
- Developer-facing API naming must use `M` prefix for Muonroi branding:
1. Classes: `MRepository`, `MQuery`, ...
2. Extension classes/method groups: `M...Extensions`
3. Helper/service abstractions for external developer use.
- Exceptions to `M` prefix:
1. Framework-mandated types (`Program`, ASP.NET handlers, EF migration classes).
2. Third-party contracts/interfaces that must keep original names.

## Git Rules

- Commit by logical scope, per repository.
- Do not rewrite shared history unless explicitly requested.
- Default branches:
1. `MuonroiBuildingBlock`: `develop`
2. Templates/Docs: `main`

## Version Bump And Local Package Flow

All steps are local-only (no publish to public NuGet).

1. Bump library version and update template package refs:

```powershell
cd D:\Personal\Project\MuonroiBuildingBlock
.\scripts\bump-version.ps1 -Version 1.9.11
```

2. Local package outputs:
1. `D:\Personal\Project\LocalNuget`
2. `D:\Personal\Project\LocalNuGetFeed`

3. Bump template package versions (`.csproj` and `.nuspec`) to same version.

4. Pack each template to local feed:

```powershell
cd D:\Personal\Project\Muonroi.BaseTemplate
dotnet pack .\Muonroi.BaseTemplate.csproj -c Release -o D:\Personal\Project\LocalNuget

cd D:\Personal\Project\Muonroi.Modular.Template
dotnet pack .\Muonroi.Modular.csproj -c Release -o D:\Personal\Project\LocalNuget

cd D:\Personal\Project\Muonroi.Microservices.Template
dotnet pack .\Muonroi.Microservices.csproj -c Release -o D:\Personal\Project\LocalNuget
```

5. Reinstall local templates:

```powershell
dotnet new install D:\Personal\Project\LocalNuget\Muonroi.BaseTemplate.1.9.11.nupkg --force
dotnet new install D:\Personal\Project\LocalNuget\Muonroi.Modular.Template.1.9.11.nupkg --force
dotnet new install D:\Personal\Project\LocalNuget\Muonroi.Microservices.Template.1.9.11.nupkg --force
```

## Generate New Projects And Verify

For each generated project:

1. Create from template (`dotnet new ...`)
2. Run EF scripts:

```powershell
cd <generated-project>
.\scripts\ef.cmd init
.\scripts\ef.cmd update
dotnet restore
dotnet run
```

## License Keys And Tier Setup (Free/Paid/Enterprise)

1. Generate master/child key assets:

```powershell
cd D:\Personal\Project\MuonroiBuildingBlock
.\scripts\flow-license-server.ps1 -Organization "Muonroi Local Verify" -NoRunServer
```

2. Optional run mock server:

```powershell
cd D:\Personal\Project\MuonroiBuildingBlock\tools\MockLicenseServer
dotnet run --project .\MockLicenseServer.csproj
```

3. Use signed local licenses per generated app:
1. `licenses\paid-license.json`
2. `licenses\enterprise-license.json`
3. `licenses\control-plane-public.pem`

4. Configure app (`appsettings` or env vars):
1. `LicenseConfigs:Mode=Offline`
2. `LicenseConfigs:LicenseFilePath=<license-json>`
3. `LicenseConfigs:PublicKeyPath=<public-key-pem>`

## Tier Verification Matrix

1. `Free`:
1. Register/Login/CRUD must still work.
2. Premium endpoints must be blocked.
2. `Paid`:
1. Login returns token.
2. Premium endpoints (for paid scope) return success.
3. `Enterprise`:
1. Login returns token.
2. Enterprise endpoints/features enabled by config and license.

## Runtime Verification Requirements

- Verify by tests and runtime logs:
1. `dotnet test` green.
2. Log contains `[License] Verified tier: ...`
3. Login response contains `result.accessToken`.
4. Authenticated endpoint (for example `Auth/roles`) behaves by tier as expected.

## Docs Rule

- Enterprise docs are centralized in `Muonroi.Docs` under `docs/enterprise/`.
- Template README files must reference `Muonroi.Docs` as source of truth.
