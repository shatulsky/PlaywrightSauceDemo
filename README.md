# Getting Started

## Prerequisites
- .NET 8.0

## Installation

1. Clone the repository: `git clone <repository-url>`
2. Navigate to the project directory: `cd <project-directory>`
3. Build the project: `dotnet build`
4. Install required browsers `pwsh /Tests/bin/Debug/net8.0/playwright.ps1 install --with-deps`

## Running Tests

Run tests: `dotnet test`

## Configuration

For local debugging - create `appsettings.Development.json` file in the built directory (e.g. `/Tests/bin/Debug/net8.0/`) with the following content:

```json
{
  "Playwright": {
    "Headless": false,
    "SlowMo": 100
  }
}
```
