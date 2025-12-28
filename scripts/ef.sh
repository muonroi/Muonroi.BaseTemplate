#!/usr/bin/env bash

# =============================================================================
# EF Core Migration Helper Script
# Cross-platform: Windows (Git Bash/WSL), macOS, Linux
# =============================================================================

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Get script directory and root directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

# Find Data and API projects
find_projects() {
    DATA_PROJECT=$(find "$ROOT_DIR/src" -maxdepth 1 -type d -name "*.Data" | head -1)
    API_PROJECT=$(find "$ROOT_DIR/src" -maxdepth 1 -type d -name "*.API" | head -1)

    if [ -z "$DATA_PROJECT" ]; then
        echo -e "${RED}Error: Could not find Data project in src folder${NC}"
        exit 1
    fi

    if [ -z "$API_PROJECT" ]; then
        echo -e "${RED}Error: Could not find API project in src folder${NC}"
        exit 1
    fi

    echo -e "${CYAN}Projects found:${NC}"
    echo -e "  Data: $DATA_PROJECT"
    echo -e "  API:  $API_PROJECT"
    echo ""
}

# Show usage
show_usage() {
    echo -e "${CYAN}EF Core Migration Helper${NC}"
    echo ""
    echo "Usage: $0 <command> [options]"
    echo ""
    echo "Commands:"
    echo "  add <name>        Add a new migration"
    echo "  update [target]   Update database to latest or specific migration"
    echo "  remove [--force]  Remove the last migration"
    echo "  list              List all migrations"
    echo "  status            Show migration status"
    echo "  help              Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0 add InitialCreate      # Add migration named 'InitialCreate'"
    echo "  $0 update                 # Apply all pending migrations"
    echo "  $0 update InitialCreate   # Update to specific migration"
    echo "  $0 remove                 # Remove last unapplied migration"
    echo "  $0 remove --force         # Force remove last migration"
    echo "  $0 list                   # List all migrations"
    echo "  $0 status                 # Show pending migrations"
    echo ""
    echo -e "${YELLOW}Prerequisites:${NC}"
    echo "  dotnet tool install --global dotnet-ef"
    echo ""
}

# Add migration
add_migration() {
    local name="${1:-Migration}"

    echo -e "${CYAN}Adding migration '$name'...${NC}"

    dotnet ef migrations add "$name" \
        -p "$DATA_PROJECT" \
        --startup-project "$API_PROJECT" \
        -o Persistence/Migrations

    echo ""
    echo -e "${GREEN}Migration '$name' created successfully!${NC}"
    echo -e "${YELLOW}Run '$0 update' to apply the migration.${NC}"
}

# Update database
update_database() {
    local target="$1"

    echo -e "${CYAN}Updating database...${NC}"

    if [ -n "$target" ]; then
        echo -e "  Target: $target"
        dotnet ef database update "$target" \
            -p "$DATA_PROJECT" \
            --startup-project "$API_PROJECT"
    else
        dotnet ef database update \
            -p "$DATA_PROJECT" \
            --startup-project "$API_PROJECT"
    fi

    echo ""
    echo -e "${GREEN}Database updated successfully!${NC}"
}

# Remove migration
remove_migration() {
    local force=""

    if [ "$1" == "--force" ] || [ "$1" == "-f" ]; then
        force="--force"
        echo -e "${YELLOW}Force mode enabled${NC}"
    fi

    echo -e "${CYAN}Removing last migration...${NC}"

    dotnet ef migrations remove \
        -p "$DATA_PROJECT" \
        --startup-project "$API_PROJECT" \
        $force

    echo ""
    echo -e "${GREEN}Migration removed successfully!${NC}"
}

# List migrations
list_migrations() {
    echo -e "${CYAN}Listing migrations...${NC}"
    echo ""

    dotnet ef migrations list \
        -p "$DATA_PROJECT" \
        --startup-project "$API_PROJECT"
}

# Show migration status
show_status() {
    echo -e "${CYAN}Checking migration status...${NC}"
    echo ""

    dotnet ef migrations list \
        -p "$DATA_PROJECT" \
        --startup-project "$API_PROJECT" \
        --no-connect 2>/dev/null || dotnet ef migrations list \
        -p "$DATA_PROJECT" \
        --startup-project "$API_PROJECT"
}

# Main
main() {
    local command="$1"
    shift 2>/dev/null || true

    case "$command" in
        add)
            find_projects
            add_migration "$1"
            ;;
        update)
            find_projects
            update_database "$1"
            ;;
        remove)
            find_projects
            remove_migration "$1"
            ;;
        list)
            find_projects
            list_migrations
            ;;
        status)
            find_projects
            show_status
            ;;
        help|--help|-h|"")
            show_usage
            ;;
        *)
            echo -e "${RED}Unknown command: $command${NC}"
            echo ""
            show_usage
            exit 1
            ;;
    esac
}

main "$@"
