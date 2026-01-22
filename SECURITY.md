# Security Policy

## Supported Versions

We actively maintain and provide security updates for the following versions:

| Version | .NET Target | Supported          | End of Support |
| ------- | ----------- | ------------------ | -------------- |
| 2.x     | .NET 10.0   | :white_check_mark: | May 2027       |
| 1.x     | .NET 9.0    | :x:                | Ended Jan 2026 |

## Reporting a Vulnerability

We take the security of ConcurrentProcessing seriously. If you believe you've found a security vulnerability, please report it to us responsibly.

### How to Report

**Please DO NOT report security vulnerabilities through public GitHub issues.**

Instead, please report them via one of the following methods:

1. **GitHub Security Advisories** (Preferred)
   - Navigate to the [Security tab](https://github.com/markhazleton/ConcurrentProcessing/security)
   - Click "Report a vulnerability"
   - Fill out the advisory form with details

2. **Email**
   - Send details to: markhazleton@gmail.com
   - Use subject line: `[SECURITY] ConcurrentProcessing Vulnerability`
   - Include detailed description and reproduction steps

### What to Include

Please include as much of the following information as possible:

- **Type of vulnerability** (e.g., buffer overflow, SQL injection, cross-site scripting)
- **Full paths** of source file(s) related to the vulnerability
- **Location** of the affected source code (tag/branch/commit or direct URL)
- **Step-by-step instructions** to reproduce the issue
- **Proof-of-concept or exploit code** (if possible)
- **Impact** of the vulnerability
- **Suggested fix** (if you have one)

### Response Timeline

- **Initial Response**: Within 48 hours of receiving your report
- **Status Update**: Within 7 days with assessment and timeline
- **Fix Timeline**: Critical issues within 30 days, others within 90 days
- **Disclosure**: Coordinated disclosure after fix is available

### Security Update Process

1. **Acknowledgment**: We'll confirm receipt of your report
2. **Investigation**: We'll investigate and validate the vulnerability
3. **Fix Development**: We'll develop and test a fix
4. **Release**: We'll release a security patch
5. **Disclosure**: We'll publish a security advisory
6. **Credit**: We'll credit you in the advisory (unless you prefer to remain anonymous)

## Security Best Practices for Users

### Dependency Management

- **Keep Updated**: Regularly update to the latest version
- **Monitor Dependencies**: Watch for security advisories
- **Use Dependabot**: Enable Dependabot alerts in your repository

### Runtime Security

- **Use Latest .NET**: Always use the latest .NET 10 SDK/Runtime
- **Least Privilege**: Run with minimum required permissions
- **Validate Input**: Sanitize all user input when extending the library
- **Secure Configuration**: Follow secure coding guidelines

### Code Review

When contributing or using this library:
- Review code changes carefully
- Use static analysis tools
- Enable all compiler warnings
- Run security scans regularly

## Automated Security Measures

This project implements several automated security measures:

### Continuous Security Scanning

- **Every Build**: Vulnerability scanning via `dotnet list package --vulnerable`
- **Weekly Scans**: Automated Dependabot dependency reviews
- **Pull Requests**: Security checks on all PRs

### Dependency Management

- **Dependabot**: Automated dependency updates
- **Grouped Updates**: Smart grouping of related packages
- **Conservative Strategy**: Major versions reviewed manually

### CI/CD Security

- **Multi-Platform Testing**: Ubuntu, Windows, macOS validation
- **Code Analysis**: Microsoft.CodeAnalysis.NetAnalyzers enabled
- **Quality Gates**: Builds fail on security issues

## Known Security Considerations

### Concurrent Processing

This library uses:
- **SemaphoreSlim**: For concurrency control
- **Task Parallel Library**: For async operations
- **Generic Types**: For type-safe processing

### Thread Safety

- All public APIs are thread-safe
- Internal state uses proper synchronization
- No shared mutable state without protection

### Resource Management

- Proper disposal of resources via `IDisposable`
- Semaphore cleanup on completion
- Memory-efficient task processing

## Security Enhancements Roadmap

### Planned Improvements

- [ ] **Code Signing**: Authenticode (Windows), Developer ID (macOS)
- [ ] **SBOM Generation**: Software Bill of Materials for releases
- [ ] **Supply Chain Security**: Enhanced provenance tracking
- [ ] **Advanced Scanning**: CodeQL integration if available

## Security Disclosure History

No security vulnerabilities have been reported or discovered to date.

We will maintain a record of all security advisories here:

| CVE | Severity | Affected Versions | Fixed In | Date |
| --- | -------- | ----------------- | -------- | ---- |
| -   | -        | -                 | -        | -    |

## Additional Resources

- **OWASP**: [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- **.NET Security**: [Microsoft Security](https://docs.microsoft.com/en-us/dotnet/standard/security/)
- **CWE**: [Common Weakness Enumeration](https://cwe.mitre.org/)
- **NVD**: [National Vulnerability Database](https://nvd.nist.gov/)

## Contact

- **Security Issues**: markhazleton@gmail.com (Subject: [SECURITY])
- **General Questions**: [GitHub Issues](https://github.com/markhazleton/ConcurrentProcessing/issues)
- **Documentation**: [Project Documentation](https://github.com/markhazleton/ConcurrentProcessing#readme)

## Acknowledgments

We appreciate the security research community's efforts in making open source software safer. Security researchers who responsibly disclose vulnerabilities will be credited in our security advisories (with their permission).

---

**Last Updated**: January 21, 2026  
**Policy Version**: 1.0
