# CI/CD Pipeline Documentation

## Overview

This document describes the continuous integration and deployment (CI/CD) pipelines configured for the ConcurrentProcessing project on .NET 10.

## GitHub Actions Workflows

### 1. Build and Test Workflow (`.github/workflows/dotnet.yml`)

**Triggers:**
- Push to `main` or `upgrade-to-NET10` branches
- Pull requests to `main` branch
- Manual workflow dispatch

**Jobs:**

#### `build-and-test`
Builds and tests the application across multiple operating systems.

- **Platforms:** Ubuntu (Linux), Windows, macOS
- **Steps:**
  1. Checkout code
  2. Setup .NET 10.0.x SDK
  3. Display .NET information
  4. Restore NuGet dependencies
  5. Build solution in Release configuration
  6. Run the application to verify functionality
  7. Check for vulnerable packages
  8. Upload build artifacts (Linux only)

**Matrix Strategy:** Runs in parallel on all three operating systems to ensure cross-platform compatibility.

#### `code-quality`
Performs static code analysis and quality checks.

- **Platform:** Ubuntu (Linux)
- **Steps:**
  1. Checkout code
  2. Setup .NET 10.0.x SDK
  3. Restore dependencies
  4. Build with .NET analyzers enabled
  5. Verify code formatting with `dotnet format`

#### `security-scan`
Scans for security vulnerabilities in dependencies.

- **Platform:** Ubuntu (Linux)
- **Permissions:** Write security-events, read contents
- **Steps:**
  1. Checkout code
  2. Setup .NET 10.0.x SDK
  3. Restore dependencies
  4. Analyze packages for known vulnerabilities
  5. Fail if vulnerabilities are detected

#### `publish`
Publishes the application when merged to main.

- **Platform:** Ubuntu (Linux)
- **Conditions:** Only runs on push to `main` branch after all other jobs succeed
- **Steps:**
  1. Checkout code
  2. Setup .NET 10.0.x SDK
  3. Restore dependencies
  4. Publish as self-contained single file executable
  5. Upload published artifacts with 30-day retention

---

### 2. Release Workflow (`.github/workflows/release.yml`)

**Triggers:**
- Git tags matching pattern `v*.*.*` (e.g., v1.0.0, v2.1.3)
- Manual workflow dispatch with version input

**Jobs:**

#### `create-release`
Creates GitHub releases with multi-platform binaries.

- **Platform:** Ubuntu (Linux)
- **Permissions:** Write contents
- **Steps:**
  1. Checkout code with full history
  2. Setup .NET 10.0.x SDK
  3. Extract version from tag or manual input
  4. Restore dependencies
  5. Build solution
  6. Run application for validation
  7. Publish for multiple runtimes:
     - **Windows x64** (`win-x64`) - ZIP archive
     - **Linux x64** (`linux-x64`) - TAR.GZ archive
     - **macOS Intel** (`osx-x64`) - TAR.GZ archive
     - **macOS Apple Silicon** (`osx-arm64`) - TAR.GZ archive
  8. Create compressed archives for each platform
  9. Generate release notes
  10. Create GitHub release with all platform binaries

**Published Artifacts:**
- Self-contained executables (no .NET runtime required)
- Single-file deployment
- Platform-specific optimizations
- Include native libraries

---

## Dependabot Configuration (`.github/dependabot.yml`)

Automated dependency management for security and updates.

### NuGet Packages
- **Schedule:** Weekly on Mondays at 09:00
- **Auto-grouping:**
  - Analyzer packages (`Microsoft.CodeAnalysis.*`)
  - Microsoft packages (`Microsoft.*`, `System.*`)
- **Configuration:**
  - Max 10 open PRs
  - Labels: `dependencies`, `nuget`
  - Assigned to: `markhazleton`
  - Ignores major version updates by default

### GitHub Actions
- **Schedule:** Weekly on Mondays at 09:00
- **Configuration:**
  - Max 5 open PRs
  - Labels: `dependencies`, `github-actions`
  - Assigned to: `markhazleton`
  - Keeps workflow actions up to date

---

## Pull Request Template (`.github/pull_request_template.md`)

Standardized template for all pull requests ensuring:
- Clear description of changes
- Change type categorization
- Testing checklist
- Code quality verification
- Related issue linking

**Benefits:**
- Consistent PR documentation
- Easier code reviews
- Better change tracking
- Quality gate enforcement

---

## Status Badges

The README now includes live status badges:

```markdown
[![.NET Build and Test](https://github.com/markhazleton/ConcurrentProcessing/actions/workflows/dotnet.yml/badge.svg)](...)
[![Release](https://github.com/markhazleton/ConcurrentProcessing/actions/workflows/release.yml/badge.svg)](...)
[![.NET Version](https://img.shields.io/badge/.NET-10.0-purple.svg)](...)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](...)
```

These provide quick visibility into:
- Build status (passing/failing)
- Release status
- .NET version
- License type

---

## Usage Instructions

### Running Builds Locally

Simulate CI/CD pipeline locally:

```bash
# Restore dependencies
dotnet restore

# Build in Release configuration
dotnet build --configuration Release --no-restore

# Run the application
dotnet run --project ConcurrentProcessing.csproj --configuration Release --no-build

# Check for vulnerabilities
dotnet list package --vulnerable --include-transitive

# Format code
dotnet format --verify-no-changes
```

### Creating a Release

**Option 1: Git Tag (Recommended)**
```bash
# Create and push a version tag
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
```

The release workflow will automatically:
1. Build for all platforms
2. Create release archives
3. Generate release notes
4. Publish to GitHub Releases

**Option 2: Manual Dispatch**
1. Navigate to Actions ? Release workflow
2. Click "Run workflow"
3. Enter version number (e.g., 1.0.0)
4. Click "Run workflow"

### Monitoring Pipeline Status

**GitHub Actions Dashboard:**
- Repository ? Actions tab
- View running/completed workflows
- Download artifacts
- Review logs for failures

**Email Notifications:**
- GitHub sends notifications for failed workflows
- Configure in Settings ? Notifications

---

## Environment Variables

Common environment variables used in workflows:

| Variable | Value | Description |
|----------|-------|-------------|
| `DOTNET_VERSION` | `10.0.x` | .NET SDK version to use |
| `CONFIGURATION` | `Release` | Build configuration |
| `GITHUB_TOKEN` | (automatic) | GitHub authentication token |

---

## Security Considerations

### Secrets Management
- No secrets currently required
- `GITHUB_TOKEN` is automatically provided by GitHub Actions
- If external services are added, use GitHub Secrets

### Vulnerability Scanning
- Runs on every build
- Checks direct and transitive dependencies
- Fails workflow if vulnerabilities detected
- Weekly Dependabot scans

### Code Signing
**Future Enhancement:** Consider adding code signing for released executables
- Windows: Authenticode signing
- macOS: Apple Developer signing

### SBOM Generation
**Implemented:** Software Bill of Materials (SBOM) generation for supply chain security
- Generated for all builds in security-scan job
- Included with releases for transparency
- SPDX 2.2 format for compatibility
- Helps with compliance and auditing

For more information, see [SECURITY.md](../SECURITY.md).

---

## Troubleshooting

### Build Failures

**Symptom:** Build fails on specific platform
**Solution:**
1. Check workflow logs in Actions tab
2. Verify .NET 10 SDK availability
3. Test locally on that platform
4. Review platform-specific code

### Release Creation Fails

**Symptom:** Release workflow fails at publish step
**Solution:**
1. Verify tag format matches `v*.*.*`
2. Check branch permissions
3. Ensure no duplicate releases exist
4. Review runtime-specific errors in logs

### Dependabot PRs Not Appearing

**Symptom:** No automated dependency PRs
**Solution:**
1. Verify Dependabot is enabled in repository settings
2. Check Dependabot logs in Insights ? Dependency graph
3. Ensure `dependabot.yml` syntax is correct
4. Verify schedule configuration

---

## Best Practices

### Commit Messages
Follow conventional commits format:
- `feat:` - New features
- `fix:` - Bug fixes
- `chore:` - Maintenance tasks
- `docs:` - Documentation changes
- `ci:` - CI/CD changes

### Branch Strategy
- `main` - Stable, production-ready code
- Feature branches - New development
- `upgrade-to-*` - Framework/version upgrades

### Pull Requests
- Always create PRs for changes
- Fill out PR template completely
- Wait for CI checks to pass
- Request reviews before merging

### Releases
- Use semantic versioning (MAJOR.MINOR.PATCH)
- Write meaningful release notes
- Test releases before making public
- Tag releases consistently

---

## Future Enhancements

Potential CI/CD improvements:

1. **Code Coverage Reporting**
   - Add test projects
   - Integrate coverage tools (Coverlet)
   - Report to Codecov or Coveralls

2. **Performance Benchmarks**
   - Add BenchmarkDotNet tests
   - Track performance over time
   - Alert on regressions

3. **Container Images**
   - Build Docker images
   - Push to container registry
   - Multi-stage builds for optimization

4. **Deployment Automation**
   - Deploy to Azure App Service
   - Deploy to AWS Lambda
   - Kubernetes manifests

5. **Enhanced Security**
   - SAST (Static Application Security Testing)
   - DAST (Dynamic Application Security Testing)
   - License compliance checking

---

## Support

For issues with CI/CD pipelines:
1. Check workflow logs in Actions tab
2. Review this documentation
3. Create an issue on GitHub
4. Reference specific workflow run URL

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2024 | Initial CI/CD setup for .NET 10 |

---

*This documentation is maintained alongside the CI/CD pipelines and should be updated when workflows change.*
