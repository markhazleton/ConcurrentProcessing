# Contributing to ConcurrentProcessing

We want to make contributing to this project as easy and transparent as possible, whether it's:
- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer

## Prerequisites

Before contributing, ensure you have:
- **.NET 10 SDK** installed ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **Git** for version control
- A code editor (Visual Studio 2022, VS Code, or Rider recommended)

Verify your setup:
```bash
dotnet --version  # Should show 10.0.x or later
git --version
```

## Development Setup

1. **Fork and clone the repository**
```bash
git clone https://github.com/YOUR-USERNAME/ConcurrentProcessing.git
cd ConcurrentProcessing
```

2. **Create a feature branch**
```bash
git checkout -b feature/my-new-feature
```

3. **Build and test**
```bash
dotnet restore
dotnet build --configuration Release
dotnet run --configuration Release
```

## We Develop with GitHub

We use GitHub to host code, to track issues and feature requests, as well as accept pull requests.

## We Use GitHub Flow - All Code Changes Happen Through Pull Requests

Pull requests are the best way to propose changes to the codebase. We actively welcome your pull requests:

1. **Fork the repo** and create your branch from `main` (not `master`)
2. **Make your changes** following our coding standards
3. **Add tests** if you've added code that should be tested
4. **Update documentation** if you've changed APIs or behavior
5. **Ensure CI passes** - All GitHub Actions workflows must succeed
6. **Ensure code quality**:
   ```bash
   dotnet build --configuration Release  # Must build with 0 errors
   dotnet format --verify-no-changes      # Code must be formatted
   dotnet list package --vulnerable       # No vulnerable packages
   ```
7. **Write clear commit messages** using [Conventional Commits](https://www.conventionalcommits.org/):
   - `feat:` - New features
   - `fix:` - Bug fixes
   - `docs:` - Documentation changes
   - `chore:` - Maintenance tasks
   - `test:` - Test additions/changes
8. **Submit your pull request** using the provided template

## Pull Request Process

1. **Create PR** against the `main` branch
2. **Fill out PR template** completely (auto-loaded)
3. **Wait for CI checks** to complete (build, test, security scan)
4. **Address review feedback** from maintainers
5. **Ensure all checks pass** before requesting final review
6. **Squash and merge** when approved

**Automated Checks:**
- ? Build on Linux, Windows, macOS
- ? Code quality analysis
- ? Security vulnerability scanning
- ? Application execution validation

## Coding Standards

### .NET 10 Best Practices
- Target .NET 10 (`net10.0` in project files)
- Use latest C# language features appropriately
- Follow async/await best practices
- Use nullable reference types where appropriate
- Leverage pattern matching and modern C# syntax

### Code Style
```bash
# Format code before committing
dotnet format

# Verify formatting
dotnet format --verify-no-changes
```

### Performance
- Profile performance-critical code
- Use `System.Diagnostics.Stopwatch` for timing
- Consider memory allocations in hot paths
- Use appropriate collection types

### Documentation
- XML comments for public APIs
- Markdown for user-facing documentation
- Code examples should be runnable
- Update relevant docs when changing behavior

## Any Contributions You Make Will Be Under the MIT Software License

In short, when you submit code changes, your submissions are understood to be under the same [MIT License](http://choosealicense.com/licenses/mit/) that covers the project. Feel free to contact the maintainers if that's a concern.

## Report Bugs Using GitHub's Issues

We use GitHub issues to track public bugs. Report a bug by [opening a new issue](https://github.com/markhazleton/ConcurrentProcessing/issues/new).

## Write Bug Reports with Detail, Background, and Sample Code

**Great Bug Reports** tend to have:

- **Quick summary** and/or background
- **Steps to reproduce**
  - Be specific!
  - Give sample code if you can
  - Include system information (.NET version, OS)
- **What you expected** would happen
- **What actually happens**
- **Notes** (possibly including why you think this might be happening, or stuff you tried that didn't work)

**Bug Report Template:**
```markdown
## Description
Brief description of the bug

## Environment
- .NET Version: 10.0.x
- OS: Windows 11 / Ubuntu 22.04 / macOS 14
- Project Version: (git commit or release tag)

## Steps to Reproduce
1. Step one
2. Step two
3. ...

## Expected Behavior
What should happen

## Actual Behavior
What actually happens

## Code Sample (if applicable)
```csharp
// Minimal reproducible example
```

## Additional Context
Any other relevant information
```

People *love* thorough bug reports!

## Proposing Features

Feature requests are welcome! To propose a new feature:

1. **Check existing issues** to avoid duplicates
2. **Open a new issue** with the "feature request" label
3. **Describe the feature** clearly
4. **Explain the use case** - why is this needed?
5. **Consider alternatives** - what other approaches exist?
6. **Discuss implementation** - how might this work?

## Testing

### Running Tests
```bash
# Run the application (serves as integration test)
dotnet run --configuration Release

# Check for vulnerabilities
dotnet list package --vulnerable
```

### Future: Unit Tests
We welcome contributions adding unit test coverage:
- Use xUnit, NUnit, or MSTest
- Test concurrent scenarios carefully
- Mock external dependencies
- Aim for >80% code coverage

## Security

**Reporting Security Issues:**
- **DO NOT** open public issues for security vulnerabilities
- Email security concerns to maintainers privately
- Use GitHub Security Advisories for sensitive issues

**Dependency Security:**
- Dependabot automatically monitors dependencies
- Review and merge Dependabot PRs promptly
- Run `dotnet list package --vulnerable` before committing

## Code of Conduct

### Our Standards

**Positive behaviors:**
- Using welcoming and inclusive language
- Being respectful of differing viewpoints
- Gracefully accepting constructive criticism
- Focusing on what is best for the community
- Showing empathy towards other community members

**Unacceptable behaviors:**
- Harassment or discriminatory language
- Trolling, insulting/derogatory comments
- Public or private harassment
- Publishing others' private information
- Other conduct reasonably considered inappropriate

## Getting Help

- **Documentation**: Check [docs/](../docs/) folder first
- **CI/CD Questions**: See [CI/CD Guide](cicd.md)
- **General Questions**: Open a GitHub Discussion
- **Bugs**: Open a GitHub Issue
- **Security**: Contact maintainers privately

## Recognition

Contributors are recognized in:
- Git commit history
- Release notes
- GitHub Contributors page

Thank you for contributing to ConcurrentProcessing! ??

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

**Additional Resources:**
- [Getting Started Guide](getting-started.md)
- [CI/CD Pipeline Documentation](cicd.md)
- [Project README](../README.md)
