# Enterprise Control Plane MVP (E1)

## Scope

E1 introduces a control-plane service API so teams can operate licenses and signed policy bundles
without hand-editing `license.json` and `policy.json`.

Implemented in:

- `src/Muonroi.BuildingBlock/Shared/ControlPlane/EnterpriseControlPlaneService.cs` (`MEnterpriseControlPlaneService`)
- `src/Muonroi.BuildingBlock/Shared/ControlPlane/ControlPlaneEndpointExtensions.cs` (`MControlPlaneEndpointExtensions`)

## Delivered APIs

### License lifecycle

- `IssueLicense`: generate signed `LicensePayload` and persist managed license metadata.
- `RevokeLicense`: revoke by `LicenseId` with reason and audit record.
- `AssignTenants`: assign normalized tenant set to a managed license.

### Policy bundle lifecycle

- `CreatePolicyDraft`: create versioned draft bundle.
- `ApprovePolicyBundle`: sign policy payload and move draft -> approved.
- `ActivatePolicyBundle`: move approved -> activated and supersede prior active bundle.
- `RollbackPolicyBundle`: move current active -> rolled back and reactivate target version.

### Audit + signing

- Every write operation emits an audit record with SHA-256 data hash and RSA-SHA256 signature.
- License payload and policy bundle signatures are verifiable through service helpers.

## Status model

- `MPolicyBundleStatus.Draft`
- `MPolicyBundleStatus.Approved`
- `MPolicyBundleStatus.Activated`
- `MPolicyBundleStatus.Superseded`
- `MPolicyBundleStatus.RolledBack`

## Minimal integration

```csharp
using Muonroi.BuildingBlock.Shared.ControlPlane;

var signer = MRsaControlPlaneSigner.FromPrivateKeyFile("licenses/control-plane-private.pem", "cp-key-2026");
builder.Services.AddMEnterpriseControlPlane("licenses/control-plane-registry.json", signer);

var app = builder.Build();
app.MapMEnterpriseControlPlaneEndpoints();
```

## Security references used

- OPA bundle lifecycle/version distribution:
  https://www.openpolicyagent.org/docs/management-bundles
- Keycloak authorization services (central policy administration):
  https://www.keycloak.org/docs/latest/authorization_services/index.html
- JSON Web Signature (JWS) for detached signature model:
  https://datatracker.ietf.org/doc/html/rfc7515
- OAuth2 scope semantics for entitlement boundary language:
  https://datatracker.ietf.org/doc/html/rfc6749#section-3.3
- JSON Canonicalization Scheme considerations for signed JSON payloads:
  https://datatracker.ietf.org/doc/html/rfc8785


