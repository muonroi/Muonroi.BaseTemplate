# Angular Shell Starter

This starter provides Muonroi runtime primitives:

- `mAuthInterceptor`: attach bearer token
- `mTenantInterceptor`: attach tenant header
- `mCorrelationInterceptor`: attach correlation id
- `mErrorInterceptor`: normalize API errors
- `MUiManifestService`: load backend UI manifest

Backend endpoint expected:

- `GET /api/v1/auth/ui-manifest/{userId}`
