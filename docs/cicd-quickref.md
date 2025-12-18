# CI/CD Quick Reference

Quick commands and procedures for working with the CI/CD pipelines.

## ? Quick Commands

### Local Testing (Simulate CI)

```bash
# Full CI simulation
dotnet restore && \
dotnet build --configuration Release --no-restore && \
dotnet run --project ConcurrentProcessing.csproj --configuration Release --no-build && \
dotnet list package --vulnerable

# Just build and test
dotnet build --configuration Release
dotnet run --configuration Release

# Check code formatting
dotnet format --verify-no-changes

# Security scan
dotnet list package --vulnerable --include-transitive
```

### Git Workflow

```bash
# Create feature branch
git checkout -b feature/my-feature

# Make changes, then stage and commit
git add .
git commit -m "feat: Add new feature"

# Push to GitHub
git push origin feature/my-feature

# Create Pull Request on GitHub
# Workflows will automatically run

# After PR approval and merge to main
git checkout main
git pull origin main
```

### Creating Releases

```bash
# Tag a release version
git tag -a v1.0.0 -m "Release version 1.0.0 - .NET 10 upgrade"
git push origin v1.0.0

# Release workflow runs automatically
# Check: https://github.com/markhazleton/ConcurrentProcessing/actions
```

## ?? Monitoring

### Check Build Status
- **Live badges**: Check README for real-time status
- **Actions tab**: https://github.com/markhazleton/ConcurrentProcessing/actions
- **Email**: GitHub sends failure notifications

### View Build Logs
1. Go to Actions tab
2. Click on workflow run
3. Click on failed job
4. Expand failed step
5. Review error logs

### Download Artifacts
1. Go to Actions tab
2. Click on successful workflow run
3. Scroll to "Artifacts" section
4. Click to download

## ?? Common Tasks

### Update .NET Version

Edit these files:
1. `global.json` - SDK version
2. `ConcurrentProcessing.csproj` - TargetFramework
3. `.github/workflows/dotnet.yml` - DOTNET_VERSION env var
4. `.github/workflows/release.yml` - DOTNET_VERSION env var
5. `README.md` - Badge version

### Add New GitHub Actions Workflow

1. Create file in `.github/workflows/`
2. Use YAML format
3. Test with workflow_dispatch first
4. Commit and push
5. Monitor in Actions tab

### Fix Failed Workflow

**Common Issues:**
- **Build failure**: Check compiler errors in logs
- **Test failure**: Review test output
- **Security scan failure**: Update vulnerable packages
- **Format check failure**: Run `dotnet format` locally

**Steps:**
1. Identify failed job in Actions
2. Review error logs
3. Fix issue locally
4. Test locally
5. Commit and push fix
6. Verify workflow passes

### Update Dependencies

**Manual:**
```bash
# Update specific package
dotnet add package Microsoft.CodeAnalysis.NetAnalyzers --version 10.0.100

# Update all packages (carefully!)
dotnet list package --outdated
```

**Automatic (Dependabot):**
- Dependabot creates PRs weekly
- Review changes in PR
- Approve and merge if CI passes

## ?? Release Checklist

Before creating a release:

- [ ] All tests pass locally
- [ ] Main branch CI is green
- [ ] Version number decided (semantic versioning)
- [ ] Release notes prepared
- [ ] Breaking changes documented
- [ ] Security scan clean

**Create Release:**
```bash
# Example for version 2.1.0
VERSION=2.1.0

# Create annotated tag
git tag -a v${VERSION} -m "Release ${VERSION}

## What's New
- Feature 1
- Feature 2
- Bug fix 3

## Breaking Changes
- None

## Upgrade Notes
- Standard upgrade, no special steps required"

# Push tag
git push origin v${VERSION}
```

## ?? Workflow Triggers

| Workflow | Trigger | When |
|----------|---------|------|
| Build & Test | Push to main/upgrade-to-NET10 | Every commit |
| Build & Test | Pull Request to main | Every PR |
| Build & Test | Manual dispatch | On demand |
| Release | Git tag v*.*.* | Tag push |
| Release | Manual dispatch | On demand |
| Dependabot | Schedule | Weekly Monday 9am |

## ?? Security

### Vulnerability Detected

**Workflow fails with vulnerability:**
```bash
# Identify vulnerable package
dotnet list package --vulnerable

# Update package
dotnet add package <PackageName> --version <SafeVersion>

# Test
dotnet build && dotnet run

# Commit
git commit -am "fix: Update vulnerable package"
git push
```

### Dependabot Security Alert

1. Review alert in Security tab
2. Dependabot may auto-create PR
3. Review PR changes
4. Test locally if needed
5. Merge if safe

## ?? Best Practices

### Commits
```bash
# Good commit messages
git commit -m "feat: Add caching to processor"
git commit -m "fix: Resolve race condition in semaphore"
git commit -m "docs: Update API documentation"
git commit -m "chore: Update dependencies"

# Conventional Commits format
<type>(<scope>): <subject>

Types: feat, fix, docs, style, refactor, test, chore
```

### Pull Requests
1. Create from feature branch
2. Fill out PR template completely
3. Wait for CI checks (green ?)
4. Request review
5. Address feedback
6. Squash and merge

### Branches
- `main` - Always deployable
- `feature/*` - New features
- `fix/*` - Bug fixes
- `docs/*` - Documentation
- `upgrade-to-*` - Major upgrades

## ?? Getting Help

**CI/CD Issues:**
1. Check this quick reference
2. Review [full CI/CD documentation](cicd.md)
3. Search workflow logs for errors
4. Create GitHub issue with:
   - Workflow run URL
   - Error messages
   - Steps to reproduce

**Workflow References:**
- GitHub Actions: https://docs.github.com/en/actions
- Dependabot: https://docs.github.com/en/code-security/dependabot
- .NET CLI: https://docs.microsoft.com/en-us/dotnet/core/tools/

## ?? Maintenance

### Weekly
- [ ] Review Dependabot PRs
- [ ] Check for failed workflows
- [ ] Monitor security alerts

### Monthly
- [ ] Review workflow efficiency
- [ ] Update documentation if needed
- [ ] Clean up old artifacts

### Per Release
- [ ] Create git tag
- [ ] Verify release artifacts
- [ ] Update changelog
- [ ] Announce release

---

**Last Updated:** 2024 (with .NET 10 upgrade)

**Quick Links:**
- [Full CI/CD Docs](cicd.md)
- [GitHub Actions](https://github.com/markhazleton/ConcurrentProcessing/actions)
- [Releases](https://github.com/markhazleton/ConcurrentProcessing/releases)
