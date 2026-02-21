# UI Shells

This folder contains frontend shell starters that can be scaffolded by `dotnet new` with:

- `--ui angular`
- `--ui react`
- `--ui mvc`
- `--ui none` (default)

These shells are contract-first starters. UI state should come from backend `ui-engine` and generated API clients.

Hybrid upgrade:

- Backend defines runtime metadata via `ui-engine` endpoints.
- FE shells consume packages from `Muonroi.Ui.Engine` repository.
- UI teams focus on UI/UX and theme; permission and behavior gates come from backend metadata.
