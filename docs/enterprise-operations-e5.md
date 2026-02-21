# Enterprise Operations Package (E5)

## Goal

E5 makes enterprise upgrades and operations repeatable with built-in safety gates:

- Upgrade compatibility checker (version/license/policy/config drift).
- SLO preset catalog for runtime quality gates.
- CI scripts to apply compatibility + SLO gates consistently.
- LTS/release runbook for operational handoff.

## Runtime building blocks

- `IMUpgradeCompatibilityService` / `MUpgradeCompatibilityService`
  - evaluates compatibility from in-memory payloads or file-based artifacts.
  - emits issue list with severity (`Info`, `Warning`, `Blocking`).
- `IMEnterpriseSloPresetService` / `MEnterpriseSloPresetService`
  - provides enterprise SLO presets:
    - `balanced`
    - `strict`
    - `regulated`
- `MEnterpriseOperationsEndpointExtensions.MapMEnterpriseOperationsEndpoints(...)`
  - `/api/v1/enterprise-ops/upgrade/compatibility/check`
  - `/api/v1/enterprise-ops/upgrade/compatibility/check-files`
  - `/api/v1/enterprise-ops/slo/presets`
  - `/api/v1/enterprise-ops/slo/presets/{presetName}`

## CI gate scripts

- Upgrade compatibility gate:

```powershell
pwsh ./scripts/check-enterprise-upgrade-compat.ps1 `
  -BaselineLicensePath .\baseline\license.json `
  -TargetLicensePath .\current\license.json `
  -BaselinePolicyPath .\baseline\policy.json `
  -TargetPolicyPath .\current\policy.json `
  -BaselinePackageVersion 1.9.10 `
  -TargetPackageVersion 1.10.0
```

- SLO preset gate:

```powershell
pwsh ./scripts/check-enterprise-slo-gates.ps1 `
  -CurrentMetricsRoot .\metrics\current `
  -BaselineMetricsRoot .\metrics\baseline `
  -PresetName strict
```

Preset files:

- `deploy/enterprise/slo-presets/balanced.json`
- `deploy/enterprise/slo-presets/strict.json`
- `deploy/enterprise/slo-presets/regulated.json`

## Compatibility check scope

Blocking examples:

- target license removes baseline capabilities,
- target policy relaxes `FailMode` from `Hard`,
- target config disables signed policy / audit chain / compliance export.

Warning examples:

- major package version jump,
- reduced rate limits or quotas,
- server validation disabled.

## LTS and release runbook (baseline)

1. Prepare release candidate:
   - run full tests,
   - run compatibility gate against previous LTS baseline,
   - run SLO gates (`strict` or `regulated` based on environment).
2. Security and governance validation:
   - confirm signed policy + trust-path requirements for enterprise production,
   - confirm compliance export and evidence pack generation are healthy.
3. Release decision:
   - block on any compatibility `Blocking` issue,
   - block on SLO gate failures,
   - escalate warnings to risk review.
4. Post-release support:
   - capture release notes and known migration impacts,
   - keep previous LTS artifacts for rollback window,
   - monitor runtime SLO + audit telemetry for first 24h.

## References

- Semantic Versioning 2.0.0: https://semver.org/
- NuGet package versioning guidance:
  https://learn.microsoft.com/en-us/nuget/concepts/package-versioning
- OpenSLO specification:
  https://github.com/OpenSLO/OpenSLO
- .NET support policy:
  https://dotnet.microsoft.com/en-us/platform/support/policy
