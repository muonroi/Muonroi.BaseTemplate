# Angular Shell Starter

This starter provides bindings to Muonroi UI engine packages:

- `@muonroi/ui-engine-core`
- `@muonroi/ui-engine-angular`
- runtime exports from `src/muonroi/m-ui-runtime.ts`

Backend endpoint expected:

- `GET /api/v1/auth/ui-engine/{userId}`
- `GET /api/v1/auth/ui-engine/current` (recommended)

Local package option (when package is not published yet):

```bash
npm install file:/absolute/path/Muonroi.Ui.Engine/packages/m-ui-engine-core \
  file:/absolute/path/Muonroi.Ui.Engine/packages/m-ui-engine-angular
```

PrimeNG option:

```bash
npm install file:/absolute/path/Muonroi.Ui.Engine/packages/m-ui-engine-primeng
```
