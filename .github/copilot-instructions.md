# GitHub Copilot Instructions for CliScape

## Development Workflow

**IMPORTANT**: This project should be worked on using **console commands only**. Do not use VS Code tasks, launch configurations, or IDE-specific tooling.

## Command-Line Operations

### Building
```bash
dotnet build CliScape.slnx
```

### Running
```bash
dotnet run --project src/CliScape.Cli
# Or use the convenience wrapper:
./cliscape.sh
```

### Testing
```bash
dotnet test tests/CliScape.Content.Tests
```

### Cleaning
```bash
dotnet clean CliScape.slnx
```

### Restoring Dependencies
```bash
dotnet restore CliScape.slnx
```

### Adding Packages
```bash
dotnet add <project-path> package <package-name>
```

### Creating New Files
Use standard file operations (`touch`, `mkdir`, etc.) or direct file creation rather than VS Code commands.

### Project Information
```bash
dotnet list CliScape.slnx reference  # List project references
dotnet list package                   # List installed packages
```

## Rationale

This approach ensures:
- Consistency across different development environments
- Platform independence
- Clear, reproducible build steps
- No IDE-specific dependencies
- Easy CI/CD integration

## Additional Context

Refer to `AGENTS.md` for comprehensive project guidelines, conventions, and architecture patterns.
