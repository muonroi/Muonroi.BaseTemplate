# Enterprise Compliance and Evidence (E4)

## Goal

E4 productizes compliance evidence generation from runtime and control-plane audit flows:

- Immutable export pipeline with hash-chain continuity.
- On-demand evidence pack generation for audit/review workflows.
- Built-in tamper-evidence verification tooling.
- Retention policy for generated evidence artifacts.

## Runtime building blocks

- `IMComplianceExportService` / `MComplianceExportService`
  - incremental export from:
    - `IFingerprintChainStore` (runtime audit trail),
    - control-plane audit records (`IMControlPlaneStore`, when registered).
  - append-only NDJSON file with per-record:
    - `PreviousHash`,
    - `RecordHash`,
    - deterministic sequence (`ExportSequence`).
  - checkpoint cursor for resume-safe exports.
- `IMComplianceEvidencePackService` / `MComplianceEvidencePackService`
  - filtered evidence pack generation (time, tenant, source),
  - summary + verification state + root hash,
  - HMAC signature over pack hash.
- `MComplianceExportHostedService`
  - optional periodic export execution.
- `MComplianceEndpointExtensions.MapMComplianceEndpoints(...)`
  - API endpoints for export, verify, evidence pack generation, and retention prune.

## Config surface

`LicenseConfigs.Compliance`:

- `Enabled`
- `ExportRootPath`
- `ExportFileName`
- `CheckpointFileName`
- `EvidencePackFolderName`
- `EnableBackgroundExport`
- `ExportIntervalMinutes`
- `EnableAutoPruneEvidencePacks`
- `EvidencePackRetentionDays`
- `MaxRecordsPerPack`

Example:

```json
{
  "LicenseConfigs": {
    "Compliance": {
      "Enabled": true,
      "ExportRootPath": "logs/compliance",
      "ExportFileName": "compliance-export.ndjson",
      "CheckpointFileName": "compliance-export.checkpoint.json",
      "EvidencePackFolderName": "evidence-packs",
      "EnableBackgroundExport": true,
      "ExportIntervalMinutes": 15,
      "EnableAutoPruneEvidencePacks": true,
      "EvidencePackRetentionDays": 365,
      "MaxRecordsPerPack": 100000
    }
  }
}
```

## Endpoint usage

```csharp
using Muonroi.BuildingBlock.Shared.Compliance;

var app = builder.Build();
app.MapMComplianceEndpoints();
```

Available endpoints:

- `POST /api/v1/compliance/export/run`
- `GET /api/v1/compliance/export/records`
- `GET /api/v1/compliance/verify`
- `POST /api/v1/compliance/evidence-packs/generate`
- `POST /api/v1/compliance/retention/prune`

## Verification model

For each exported record, hash material includes:

- export sequence,
- occurred timestamp,
- source and event type,
- tenant/entity identity,
- payload hash,
- previous hash.

Tamper evidence is validated by:

1. recomputing `RecordHash` from deterministic material,
2. validating `PreviousHash` continuity between records,
3. checking sequence ordering.

## Operational guidance

- Run with `EnableBackgroundExport=true` in enterprise production.
- Keep endpoint access restricted (admin/operator role only).
- Use evidence packs for audit windows instead of sharing raw export logs.
- Keep retention window policy-aligned with compliance requirements.

## References

- NIST SP 800-92 (Log Management Guide):
  https://csrc.nist.gov/pubs/sp/800/92/final
- NIST SP 800-53 Rev.5 (Security and Privacy Controls):
  https://csrc.nist.gov/pubs/sp/800/53/r5/upd1/final
- RFC 5848 (Signed Syslog Messages):
  https://www.rfc-editor.org/rfc/rfc5848
- RFC 9162 (Certificate Transparency v2 / append-only verifiable logs):
  https://www.rfc-editor.org/rfc/rfc9162
