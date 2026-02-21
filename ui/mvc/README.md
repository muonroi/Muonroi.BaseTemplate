# MVC Shell Starter

This shell is an ASP.NET Core MVC starter to consume Muonroi UI engine metadata from backend API.

Use cases:

- Server-side rendering/BFF style app.
- Corporate intranet portals.
- Transitional UI while SPA is being built.

Backend endpoint expected:

- `GET /api/v1/auth/ui-engine/{userId}`
- `GET /api/v1/auth/ui-engine/current` (recommended)

Local package option (when package is not published yet):

1. Pack `Muonroi.Ui.Engine.Mvc` to local feed.
2. Ensure `NuGet.config` points to `D:\Personal\Project\LocalNuget`.
3. Restore `ui/mvc/src/Muonroi.MvcShell/Muonroi.MvcShell.csproj`.
