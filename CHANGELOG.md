# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2024 - .NET 10 LTS Upgrade

### ?? Major Release - .NET 10 LTS

This major release upgrades the entire solution to .NET 10.0 LTS (Long Term Support), bringing significant performance improvements, security enhancements, and modern development practices.

### Added

#### CI/CD Infrastructure
- **GitHub Actions Workflows**
  - Multi-platform build and test pipeline (Ubuntu, Windows, macOS)
  - Automated security vulnerability scanning
  - Code quality analysis with .NET analyzers
  - Automated release creation for multiple platforms
  - Self-contained executable publishing
  
- **Dependabot Configuration**
  - Automated weekly dependency updates for NuGet packages
  - Automated GitHub Actions version updates
  - Smart grouping of related package updates
  - Security-first update strategy

- **Quality Assurance**
  - Pull request template for consistent contributions
  - Automated formatting checks with `dotnet format`
  - Security scanning on every build
  - Multi-OS validation (Linux, Windows, macOS)

#### Documentation
- **CI/CD Documentation** (`docs/cicd.md`) - Comprehensive 400+ line guide covering:
  - Workflow architecture and jobs
  - Security considerations
  - Troubleshooting procedures
  - Best practices and future enhancements
  
- **CI/CD Quick Reference** (`docs/cicd-quickref.md`) - Fast-access guide with:
  - Common commands and workflows
  - Release procedures
  - Monitoring instructions
  - Quick troubleshooting

- **Enhanced Documentation**
  - Updated all documentation for .NET 10
  - Added prerequisites and setup instructions
  - Included .NET 10-specific optimizations
  - Comprehensive API references
  - Updated external links to Microsoft Learn

- **Changelog** (`CHANGELOG.md`) - This file!

#### Project Updates
- **Status Badges** in README:
  - Live build status
  - Release pipeline status
  - .NET 10 version badge
  - MIT License badge

### Changed

#### Framework and Runtime
- **Upgraded from .NET 9.0 to .NET 10.0 LTS**
  - `TargetFramework`: `net9.0` ? `net10.0`
  - Better performance through runtime optimizations
  - Improved async/await implementation
  - Enhanced garbage collection
  - Faster JIT compilation

- **SDK Version**
  - Updated `global.json`: `9.0.305` ? `10.0.101`
  - Enables .NET 10-specific features and tooling

#### Packages
- **Microsoft.CodeAnalysis.NetAnalyzers**: `9.0.0` ? `10.0.100`
  - .NET 10-specific code analysis rules
  - Enhanced code quality checks
  - Better performance diagnostics

#### Code Improvements
- **TimeSpan API Clarity**
  - Added explicit `(double)` cast to `TimeSpan.FromMilliseconds` calls
  - Resolves potential overload ambiguity introduced in .NET 9+
  - File: `Sample/SampleTaskProcessor.cs` (Line 9)
  - Improves code clarity and maintainability

#### Documentation Structure
- **Getting Started** (`docs/getting-started.md`)
  - Added .NET 10 SDK prerequisites
  - Included project setup instructions
  - Added performance considerations
  - Included quick start examples

- **Contributing Guidelines** (`docs/CONTRIBUTING.md`)
  - Updated for .NET 10 requirements
  - Added CI/CD integration information
  - Included modern Git workflow
  - Enhanced security reporting procedures

- **ConcurrentProcessor Documentation** (`docs/concurrentprocessor.md`)
  - Expanded from basic overview to comprehensive guide
  - Added architecture and design pattern explanations
  - Included .NET 10 performance optimizations
  - Added usage examples and best practices
  - Detailed each method with parameters and behavior

- **References** (`docs/references.md`)
  - Updated all Microsoft documentation links
  - Added .NET 10-specific resources
  - Included modern development tools
  - Added CI/CD and DevOps references

### Fixed

- **Build Compatibility**
  - Resolved `global.json` SDK pinning preventing .NET 10 targeting
  - Fixed package version compatibility (10.0.0 ? 10.0.100)
  - All projects now build successfully on .NET 10

- **API Compatibility**
  - Resolved `TimeSpan.FromMilliseconds` overload ambiguity
  - No breaking changes to public API surface

### Security

- **Automated Security Scanning**
  - Vulnerability checks on every build
  - Dependabot monitoring for security updates
  - No vulnerable packages detected in dependencies

- **Package Verification**
  - All packages scanned with `dotnet list package --vulnerable`
  - Transitive dependencies included in security checks
  - Clean security report: 0 vulnerabilities

### Performance

#### .NET 10 Runtime Improvements
- **Async/Await Optimizations**
  - Reduced allocations in async state machines
  - Faster task scheduling
  - Better thread pool work distribution

- **Garbage Collection**
  - Improved Gen0/Gen1 collection efficiency
  - Reduced GC pause times
  - Better memory pressure handling

- **JIT Compilation**
  - Enhanced optimization of hot paths
  - Better inlining decisions
  - Improved SIMD vectorization

#### Application Performance
- **Concurrent Processing**
  - Leverages .NET 10 threading improvements
  - Optimized SemaphoreSlim performance
  - Reduced overhead in task coordination

### Testing

- **Multi-Platform Validation**
  - Automated testing on Ubuntu (Linux)
  - Automated testing on Windows
  - Automated testing on macOS
  - All platforms verified with .NET 10

- **Functional Testing**
  - Application execution verified on all platforms
  - Concurrent processing validated at multiple scales:
    - 100 tasks @ 1, 10, 50, 100 concurrency
    - 1000 tasks @ 500, 1000 concurrency
  - Task delays functioning correctly (10-20ms range)
  - Exit code 0 (success) confirmed

### Infrastructure

- **Source Control**
  - Branch: `upgrade-to-NET10`
  - Commits: 6 total for complete upgrade
  - All changes tracked and documented
  - Ready for merge to `main`

- **Build Artifacts**
  - Automated artifact publishing
  - Multi-platform release binaries:
    - Windows x64 (ZIP)
    - Linux x64 (TAR.GZ)
    - macOS Intel (TAR.GZ)
    - macOS Apple Silicon (TAR.GZ)
  - Self-contained executables (no runtime required)

### Deployment

- **Release Automation**
  - Tag-based release creation
  - Automatic multi-platform builds
  - GitHub Releases integration
  - Release notes generation

### Breaking Changes

?? **Runtime Requirement**
- **Requires .NET 10 SDK** for development
- **Requires .NET 10 Runtime** for execution (or use self-contained builds)
- **No backward compatibility** with .NET 9 or earlier

?? **Build Changes**
- `global.json` now specifies .NET 10 SDK
- Projects target `net10.0` framework
- Older SDKs cannot build this version

### Migration Guide

For users upgrading from .NET 9 version:

1. **Install .NET 10 SDK**
   ```bash
   # Download from: https://dotnet.microsoft.com/download/dotnet/10.0
   dotnet --version  # Verify 10.0.x
   ```

2. **Pull Latest Changes**
   ```bash
   git checkout main
   git pull origin main
   ```

3. **Build and Run**
   ```bash
   dotnet restore
   dotnet build --configuration Release
   dotnet run --configuration Release
   ```

4. **Verify Security**
   ```bash
   dotnet list package --vulnerable
   # Should report: 0 vulnerable packages
   ```

### Acknowledgments

- .NET Team at Microsoft for .NET 10 LTS release
- Community contributors and issue reporters
- GitHub Actions team for excellent CI/CD platform

### Links

- **Full Upgrade Plan**: `.github/upgrades/plan.md`
- **Assessment Report**: `.github/upgrades/assessment.md`
- **Task Execution Log**: `.github/upgrades/tasks.md`
- **CI/CD Documentation**: `docs/cicd.md`

---

## [1.0.0] - Previous Release (.NET 9)

### Initial Release
- ConcurrentProcessor base implementation
- Sample task processor example
- Basic documentation
- .NET 9 support

---

## Version History Summary

| Version | .NET Version | Release Date | Status |
|---------|--------------|--------------|--------|
| 2.0.0 | .NET 10 LTS | 2024 | ? Current |
| 1.0.0 | .NET 9 | 2024 | ?? Deprecated |

---

**Note:** This changelog follows [Keep a Changelog](https://keepachangelog.com/) format and [Semantic Versioning](https://semver.org/).

**Repository**: https://github.com/markhazleton/ConcurrentProcessing  
**Issues**: https://github.com/markhazleton/ConcurrentProcessing/issues  
**Releases**: https://github.com/markhazleton/ConcurrentProcessing/releases
