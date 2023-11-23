# ReceivablesAPI.API

This ASP.NET Core API is a simple HTTP api which tries to follow best practices to provide the following capabilities:
- Accept a payload containing receivables data and store it (Sample payload example available in Misc folder)
- Return summary statistics about the stored receivables data, specifically the value of open and closed invoices

## Installation

1. Clone the repository.
2. Navigate to the project directory.
3. Build solution and run it (prior .NET Core framework 7 has to be installed).

## Usage

To build the project, use the following command: 
- dotnet build .\ReceivablesAPI.API\ReceivablesAPI.API.csproj
To run the project, use the following command: 
- dotnet run --no-build --no-restore --project .\ReceivablesAPI.API\ReceivablesAPI.API.csproj -lp https

## Configuration

The only thing that should be set up is DB connection present in appsettings.json file.
If one would like to use InMemory EF Core database option then he should confirm that following packages are installed within the project:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.InMemory

Usefull commands would be:
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.InMemory