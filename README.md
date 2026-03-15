# Robotico.Specification

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![GitHub Packages](https://img.shields.io/badge/GitHub%20Packages-Robotico.Specification-blue?logo=github)](https://github.com/robotico-dev/robotico-specification/packages)
[![Build](https://github.com/robotico-dev/robotico-specification/actions/workflows/publish.yml/badge.svg)](https://github.com/robotico-dev/robotico-specification/actions/workflows/publish.yml)

**Specification pattern** for .NET 8 and .NET 10. Composable query criteria as objects: `ISpecification<T>`, And, Or, Not. Use with repositories and EF Core. Zero dependencies.

## Pattern

Reference **Robotico.Specification** when you use the **Specification pattern**: reusable, composable query criteria instead of ad-hoc predicates.

## Features

- **ISpecification<T>**: `IsSatisfiedBy(T candidate)`, optional `ToExpression()` for EF Core
- **Composition**: And, Or, Not specifications
- **Zero dependencies**: no EF or other packages required

## Installation

```bash
dotnet add package Robotico.Specification
```

## Quick start

```csharp
using Robotico.Specification;

// Define a specification
public sealed class ActiveUsersSpec : ISpecification<User>
{
    public bool IsSatisfiedBy(User candidate) => candidate.IsActive;
    public System.Linq.Expressions.Expression<Func<User, bool>>? ToExpression() => u => u.IsActive;
}

// Compose
var spec = new ActiveUsersSpec().And(new VerifiedEmailSpec());
bool ok = spec.IsSatisfiedBy(user);
```

## License

See repository license file.
