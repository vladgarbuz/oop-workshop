# OOP Workshop

This is a workshop project demonstrating Object-Oriented Programming (OOP) principles in C#. The application simulates a Library management system where users can interact with various media types.

## Project Structure

The solution is organized into the following folders:

- **src/**: Contains the source code for the application.
  - **Domain/**: Core business logic and entities (Library, User, Media).
  - **Persistence/**: Data access layer (handling CSV storage).
  - **Presentation/**: User interface logic (console interactions).
- **test/**: Contains unit tests for the application.
- **doc/**: Project documentation and specifications.
- **var/**: Runtime data storage (e.g., `data.csv`).

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later.

## How to Run

1. Open a terminal in the root directory of the project.
2. Run the following command to start the application:

   ```bash
   dotnet run --project src/OopWorkshop.csproj
   ```

## How to Test

To run the unit tests, execute the following command in the root directory:

```bash
dotnet test
```

## Features

- **Domain Modeling**: Classes representing Library, Users, and Media.
- **Persistence**: Data storage using CSV files.
- **Architecture**: Separation of concerns using Domain, Persistence, and Presentation layers.
