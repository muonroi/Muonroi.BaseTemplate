#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Sync Muonroi UI engine runtime and generate API clients for template UI shells.

Usage:
  ./scripts/sync-ui-engine.sh \
    [--ui-engine-path <path>] \
    [--openapi <openapi-file-or-url>] \
    [--framework angular|react|mvc|all]

Examples:
  ./scripts/sync-ui-engine.sh --ui-engine-path ../Muonroi.Ui.Engine --openapi http://localhost:5000/swagger/v1/swagger.json --framework all
  ./scripts/sync-ui-engine.sh --openapi ./_tmp/openapi.json --framework angular
EOF
}

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/.." && pwd)"
UI_ENGINE_PATH="${REPO_ROOT}/../Muonroi.Ui.Engine"
OPENAPI_SOURCE="http://localhost:5000/swagger/v1/swagger.json"
FRAMEWORK="all"

while [[ $# -gt 0 ]]; do
  case "$1" in
    --ui-engine-path)
      UI_ENGINE_PATH="${2:-}"
      shift 2
      ;;
    --openapi)
      OPENAPI_SOURCE="${2:-}"
      shift 2
      ;;
    --framework)
      FRAMEWORK="${2:-}"
      shift 2
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown argument: $1" >&2
      usage
      exit 1
      ;;
  esac
done

if [[ ! -d "${UI_ENGINE_PATH}" ]]; then
  echo "Muonroi.Ui.Engine path not found: ${UI_ENGINE_PATH}" >&2
  exit 1
fi

case "${FRAMEWORK}" in
  angular|react|mvc|all) ;;
  *)
    echo "Invalid --framework value: ${FRAMEWORK}" >&2
    exit 1
    ;;
esac

install_ui_packages() {
  local shell_path="$1"
  shift
  local packages=("$@")

  if [[ ! -f "${shell_path}/package.json" ]]; then
    return
  fi

  (
    cd "${shell_path}"
    npm install --no-fund --no-audit "${packages[@]}"
  )
}

if [[ "${FRAMEWORK}" == "angular" || "${FRAMEWORK}" == "all" ]]; then
  install_ui_packages "${REPO_ROOT}/ui/angular" \
    "file:${UI_ENGINE_PATH}/packages/m-ui-engine-core" \
    "file:${UI_ENGINE_PATH}/packages/m-ui-engine-angular"
fi

if [[ "${FRAMEWORK}" == "react" || "${FRAMEWORK}" == "all" ]]; then
  install_ui_packages "${REPO_ROOT}/ui/react" \
    "file:${UI_ENGINE_PATH}/packages/m-ui-engine-core" \
    "file:${UI_ENGINE_PATH}/packages/m-ui-engine-react"
fi

if [[ "${FRAMEWORK}" == "all" ]]; then
  if [[ -f "${REPO_ROOT}/ui/angular/package.json" ]]; then
    install_ui_packages "${REPO_ROOT}/ui/angular" \
      "file:${UI_ENGINE_PATH}/packages/m-ui-engine-primeng"
  fi
fi

OUTPUT_DIR="${REPO_ROOT}/ui/generated-clients"
bash "${UI_ENGINE_PATH}/scripts/generate-ui-clients.sh" \
  --openapi "${OPENAPI_SOURCE}" \
  --framework "${FRAMEWORK}" \
  --output "${OUTPUT_DIR}"

echo "UI engine sync completed. Generated clients at ${OUTPUT_DIR}."