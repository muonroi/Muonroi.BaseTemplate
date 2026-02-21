# Enterprise Secure-By-Default Profile (E2)

## Goal

E2 enforces deterministic production defaults for Enterprise tier:

- Signed policy is required (fail-closed).
- Remote trust path is hardened (trusted host + certificate pinning + server response signature).
- Fail-closed matrix is explicit by capability area.

## Runtime building blocks

- `MEnterpriseSecurityProfile`:
  computes effective secure defaults for Enterprise + Production.
- `MEnterpriseFailClosedMatrix`:
  maps failure reason -> capability block scope.
- `LicenseGuard`:
  enforces fail-closed for policy requirements at runtime.
- `ChainSubmitter`:
  enforces remote trust checks for enterprise remote audit path.

## Fail-closed matrix (E2)

| Failure reason | Blocked capability scope |
|---|---|
| Missing signed policy | All enterprise capabilities (`core.runtime` + premium capabilities) |
| Invalid signed policy | All enterprise capabilities (`core.runtime` + premium capabilities) |
| Policy expired | All enterprise capabilities (`core.runtime` + premium capabilities) |
| Endpoint trust failure | `audit.remote` |
| Certificate pinning misconfigured | `audit.remote` |
| Server response signature missing/invalid | `audit.remote` |

## Config surface

`LicenseConfigs.Enterprise`:

- `EnableSecureDefaults` (default `true`)
- `AllowPolicyBypassInProduction` (default `false`)
- `AllowEndpointTrustBypassInProduction` (default `false`)
- `RequireCertificatePinningInProduction` (default `true`)
- `RequireTrustedEndpointInProduction` (default `true`)
- `RequireServerResponseSignatureInProduction` (default `true`)
- `TrustedLicenseServerHosts` (default: Muonroi license hosts)

## Example (strict enterprise production)

```json
{
  "LicenseConfigs": {
    "Mode": "Online",
    "LicenseFilePath": "licenses/license.json",
    "PolicyFilePath": "licenses/policy.json",
    "PublicKeyPath": "licenses/public.pem",
    "Online": {
      "Endpoint": "https://license.muonroi.com",
      "EnableCertificatePinning": true,
      "ExpectedCertificateThumbprint": "AA11BB22CC33DD44EE55FF66778899AA00112233445566778899AABBCCDDEEFF"
    },
    "Enterprise": {
      "EnableSecureDefaults": true,
      "AllowPolicyBypassInProduction": false,
      "AllowEndpointTrustBypassInProduction": false,
      "RequireCertificatePinningInProduction": true,
      "RequireTrustedEndpointInProduction": true,
      "RequireServerResponseSignatureInProduction": true
    }
  }
}
```

## References

- OWASP ASVS V4.0.3 (Fail Securely): https://owasp.org/www-project-application-security-verification-standard/
- RFC 6125 (service identity / certificate host validation):
  https://datatracker.ietf.org/doc/html/rfc6125
- .NET `ServerCertificateCustomValidationCallback`:
  https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler.servercertificatecustomvalidationcallback
- .NET cert validation analyzer CA5359:
  https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5359
