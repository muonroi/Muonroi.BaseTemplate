# License Capability Model (E0)

## Goal

Unify paid-license entitlements by using capability profiles instead of fragile per-action keys.

This model keeps backward compatibility with existing `AllowedFeatures` payloads.

## Capability Schema v1

- `core.runtime`
- `auth.rbac_plus`
- `tenancy.strict`
- `rules.runtime`
- `transport.grpc`
- `transport.message_bus`
- `cache.distributed`
- `audit.trail`
- `runtime.anti_tampering`
- `audit.remote`

## Compatibility Mapping

| Legacy feature key | Capability key |
|---|---|
| `advanced-auth` | `auth.rbac_plus` |
| `multi-tenant` | `tenancy.strict` |
| `rule-engine` | `rules.runtime` |
| `grpc` | `transport.grpc` |
| `message-bus` | `transport.message_bus` |
| `distributed-cache` | `cache.distributed` |
| `audit-trail` | `audit.trail` |
| `anti-tampering` | `runtime.anti_tampering` |
| `server-validation` | `audit.remote` |

## Runtime Resolution Rules

1. Invalid license state only allows Free baseline features.
2. Free baseline is always allowed for valid licenses.
3. Enterprise tier is allow-all (`*` behavior unchanged).
4. Paid valid tiers (`Licensed`, `Enterprise`) automatically get `core.runtime`.
5. Core action prefixes map to `core.runtime` automatically:
   - `api.*`
   - `db.*`
   - `http.*`
6. `AllowedFeatures` can contain either legacy keys or capability keys (or mixed).

## Why This Removes Manual Action Keys

Before E0, paid payloads often needed explicit action entries (`api.list`, `db.savechanges`, ...).
After E0, these actions are resolved by capability (`core.runtime`), so action-key enumeration is no longer required.

## Migration Guidance

1. Existing licenses continue to work unchanged.
2. For new paid licenses, emit capability keys in `AllowedFeatures`.
3. Keep wildcard `["*"]` for Enterprise full-access contracts.
4. Generate payloads from control-plane services to avoid entitlement drift.

## Validation

Tests added in:

- `tests/Muonroi.BuildingBlock.Test/LicenseCapabilityResolverTests.cs`

Coverage includes:

- Legacy-to-capability and capability-to-legacy mapping.
- Automatic core runtime action resolution for paid tiers.
- Invalid-license fallback behavior.
- Wildcard compatibility.
