@echo off
setlocal enabledelayedexpansion

:: =============================================================================
:: EF Core Migration Helper Script for Windows
:: =============================================================================

set "SCRIPT_DIR=%~dp0"
set "ROOT_DIR=%SCRIPT_DIR%.."

:: Find projects
for /d %%d in ("%ROOT_DIR%\src\*.Data") do set "DATA_PROJECT=%%d"
for /d %%d in ("%ROOT_DIR%\src\*.API") do set "API_PROJECT=%%d"

if not defined DATA_PROJECT (
    echo [ERROR] Could not find Data project in src folder
    exit /b 1
)

if not defined API_PROJECT (
    echo [ERROR] Could not find API project in src folder
    exit /b 1
)

:: Parse command
set "CMD=%~1"
set "ARG=%~2"

if "%CMD%"=="" goto :show_usage_error
if "%CMD%"=="help" goto :show_usage
if "%CMD%"=="-h" goto :show_usage
if "%CMD%"=="--help" goto :show_usage

if "%CMD%"=="add" goto :add_migration
if "%CMD%"=="init" goto :init_database
if "%CMD%"=="update" goto :update_database
if "%CMD%"=="remove" goto :remove_migration
if "%CMD%"=="list" goto :list_migrations
if "%CMD%"=="status" goto :show_status

echo [ERROR] Unknown command: %CMD%
echo.
goto :show_usage_error

:show_usage
echo.
echo EF Core Migration Helper
echo.
echo Usage: ef ^<command^> [options]
echo.
echo Commands:
echo   add ^<name^>        Add a new migration
echo   init [name]       Create initial migration if missing, then update database
echo   update [target]   Update database to latest or specific migration
echo   remove [--force]  Remove the last migration
echo   list              List all migrations
echo   status            Show migration status
echo   help              Show this help message
echo.
echo Examples:
echo   ef add InitialCreate      # Add migration named 'InitialCreate'
echo   ef init                   # Create initial migration if needed and update database
echo   ef update                 # Apply all pending migrations
echo   ef update InitialCreate   # Update to specific migration
echo   ef remove                 # Remove last unapplied migration
echo   ef list                   # List all migrations
echo.
echo Prerequisites:
echo   dotnet tool install --global dotnet-ef
echo.
goto :eof

:show_usage_error
call :show_usage
exit /b 1

:add_migration
echo.
echo [INFO] Projects found:
echo   Data: %DATA_PROJECT%
echo   API:  %API_PROJECT%
echo.

if "%ARG%"=="" set "ARG=Migration"
echo [INFO] Adding migration '%ARG%'...
dotnet ef migrations add "%ARG%" -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%" -o Persistence/Migrations
if errorlevel 1 goto :error
echo.
echo [SUCCESS] Migration '%ARG%' created successfully!
echo [INFO] Run 'ef update' to apply the migration.
goto :eof

:init_database
echo.
echo [INFO] Projects found:
echo   Data: %DATA_PROJECT%
echo   API:  %API_PROJECT%
echo.

set "INIT_NAME=%ARG%"
if "%INIT_NAME%"=="" set "INIT_NAME=InitialCreate"
for %%f in ("%DATA_PROJECT%") do set "DATA_NAME=%%~nf"
set "MIGRATIONS_LIST=%TEMP%\\ef_migrations_%DATA_NAME%_%RANDOM%_%RANDOM%_%RANDOM%.txt"

dotnet ef migrations list -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%" > "%MIGRATIONS_LIST%" 2>&1
if errorlevel 1 (
    type "%MIGRATIONS_LIST%"
    del "%MIGRATIONS_LIST%" >nul 2>&1
    goto :error
)

findstr /C:"No migrations were found" "%MIGRATIONS_LIST%" >nul
if not errorlevel 1 (
    echo [INFO] No migrations found. Creating initial migration '%INIT_NAME%'...
    dotnet ef migrations add "%INIT_NAME%" -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%" -o Persistence/Migrations
    if errorlevel 1 (
        del "%MIGRATIONS_LIST%" >nul 2>&1
        goto :error
    )
) else (
    echo [INFO] Existing migrations detected. Skip creating initial migration.
)

del "%MIGRATIONS_LIST%" >nul 2>&1

echo [INFO] Applying migrations...
dotnet ef database update -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%"
if errorlevel 1 goto :error
echo.
echo [SUCCESS] Init completed successfully!
goto :eof

:update_database
echo.
echo [INFO] Projects found:
echo   Data: %DATA_PROJECT%
echo   API:  %API_PROJECT%
echo.
echo [INFO] Updating database...
if "%ARG%"=="" (
    dotnet ef database update -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%"
) else (
    echo   Target: %ARG%
    dotnet ef database update "%ARG%" -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%"
)
if errorlevel 1 goto :error
echo.
echo [SUCCESS] Database updated successfully!
goto :eof

:remove_migration
echo.
echo [INFO] Projects found:
echo   Data: %DATA_PROJECT%
echo   API:  %API_PROJECT%
echo.
echo [INFO] Removing last migration...
if "%ARG%"=="--force" (
    dotnet ef migrations remove -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%" --force
) else (
    dotnet ef migrations remove -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%"
)
if errorlevel 1 goto :error
echo.
echo [SUCCESS] Migration removed successfully!
goto :eof

:list_migrations
echo.
echo [INFO] Projects found:
echo   Data: %DATA_PROJECT%
echo   API:  %API_PROJECT%
echo.
echo [INFO] Listing migrations...
echo.
dotnet ef migrations list -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%"
goto :eof

:show_status
echo.
echo [INFO] Projects found:
echo   Data: %DATA_PROJECT%
echo   API:  %API_PROJECT%
echo.
echo [INFO] Checking migration status...
echo.
dotnet ef migrations list -p "%DATA_PROJECT%" --startup-project "%API_PROJECT%"
goto :eof

:error
echo.
echo [ERROR] Command failed with error code %errorlevel%
exit /b %errorlevel%
