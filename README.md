# Contact Management

ASP.NET Core MVC application for managing contacts, built with .NET 6 and MariaDB 10.6.

## Requirements

- .NET 6 SDK
- MariaDB 10.6

## Installation

```bash
# Clone the repository
git clone https://github.com/gabrielspereira2604/contact-management.git
cd contact-management

# Publish to the build folder
dotnet publish -o /home/gabrielpereira-dotnet/build

# Restart the application
touch /home/gabrielpereira-dotnet/build/restart
```

The application will automatically create the database tables and seed the initial user on first run.

## Access

- **URL:** https://gabrielpereira-dotnet.recruitment.alfasoft.pt
- **Login:** admin@contacts.pt
- **Password:** Admin@123

## Features

- Public contact listing
- Authenticated users can create, edit and delete contacts
- Soft delete — records are never permanently removed
- Unique phone and email validation

## Running tests

```bash
dotnet test ContactManagement.Tests/ContactManagement.Tests.csproj
```

## CI/CD

GitHub Actions runs all tests automatically on every push and pull request to `master`.
