# React Shell Starter

This starter provides Muonroi runtime primitives:

- `createMApiClient`: fetch wrapper with auth/tenant/correlation headers
- `MUiManifestClient`: load backend UI manifest
- `mCanRender` / `mCanExecute`

Backend endpoint expected:

- `GET /api/v1/auth/ui-manifest/{userId}`
- `GET /api/v1/auth/ui-manifest/current` (recommended)
