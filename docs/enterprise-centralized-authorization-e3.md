# Enterprise Centralized Authorization (E3)

## Goal

E3 adds optional centralized policy decisions (PDP mode) for enterprise microservices while preserving local RBAC safety:

- Central decision point integration (OPA/OpenFGA-style response contracts).
- Configurable failure behavior (`FallbackToLocal` or `Deny`).
- Tenant-aware, correlation-aware decision logging.

## Runtime building blocks

- `MPolicyDecisionConfigs`:
  configuration surface for provider, endpoint, path, timeout, failure mode, logging, headers.
- `MPolicyDecisionRequest` / `MPolicyDecisionResult`:
  normalized request and decision contracts.
- `IMPolicyDecisionService` / `MPolicyDecisionService`:
  centralized decision execution and protocol parsing.
- `PermissionFilter<TPermission>`:
  PDP-aware bitmask permission filter.
- `AuthorizePermissionFilter<TDbContext>`:
  PDP-aware permission-key filter.

## Decision flow

1. Filter collects user/tenant/claims + required permissions.
2. Filter calls `IMPolicyDecisionService` when `MPolicyDecision.Enabled = true`.
3. If PDP response is authoritative:
   - `allow` => request continues.
   - `deny` => `PermissionDeniedException`.
4. If PDP call fails:
   - `FallbackToLocal` => existing local RBAC path executes.
   - `Deny` => request denied (fail-closed).

## Configuration

`appsettings.json`:

```json
{
  "MPolicyDecision": {
    "Enabled": true,
    "Provider": "Opa",
    "Endpoint": "http://localhost:8181",
    "DecisionPath": "/v1/data/authz/allow",
    "TimeoutSeconds": 5,
    "FailureMode": "FallbackToLocal",
    "EnableDecisionLogging": true,
    "DefaultHeaders": {
      "X-Policy-Api-Key": "replace-me"
    }
  }
}
```

Configuration notes:

- `Provider`:
  - `Opa` expects OPA-style response (`{"result": true}` or `{"result":{"allow":true}}`).
  - `OpenFga` supports OpenFGA-style response (`{"allowed": true}`).
- `DecisionPath` defaults:
  - OPA: `/v1/data/authz/allow`
  - OpenFGA: `/check`
- `FailureMode`:
  - `FallbackToLocal` is migration-friendly.
  - `Deny` is stricter for regulated production environments.

## Operations guidance

- Start rollout with `FallbackToLocal` in staging while validating policy parity.
- Promote to `Deny` only after decision parity and PDP reliability SLOs are met.
- Keep decision logging enabled in enterprise environments for auditability.

## Tests added in E3

- `tests/Muonroi.BuildingBlock.Test/MPolicyDecisionServiceTests.cs`
- `tests/Muonroi.BuildingBlock.Test/PermissionFilterPdpModeTests.cs`
- `tests/Muonroi.BuildingBlock.Test/AuthorizePermissionFilterPdpModeTests.cs`

## References

- OPA REST API: https://www.openpolicyagent.org/docs/rest-api
- OPA decision logs: https://www.openpolicyagent.org/docs/management-decision-logs
- OpenFGA API reference: https://openfga.dev/docs/getting-started/perform-check
- Keycloak authorization services:
  https://www.keycloak.org/docs/latest/authorization_services/index.html
- NIST SP 800-162 (ABAC): https://csrc.nist.gov/pubs/sp/800/162/upd2/final
