# Sønderborg Library Digital Platform

This project is a digital platform for **Sønderborg’s Library**, designed to manage and share a diverse collection of digital media. It demonstrates **Object-Oriented Programming (OOP)** principles in C# and provides a console-based interface for users to interact with the system.

## Project Overview

The system supports multiple types of media:

- **E-books**
- **Movies**
- **Songs**
- **Video Games**
- **Apps**
- **Podcasts**
- **Images**

Users can borrow all media items, and only users who have borrowed an item may rate it.

## User Roles

The system supports three categories of users:

- **Borrower**: Interacts with the collection by listing items by type, selecting and previewing details, rating items (only if borrowed), and performing actions specific to the media type.
- **Employee**: Manages the collection with the ability to add or remove media items.
- **Admin**: Has all Employee rights and can additionally manage Borrowers and Employees. Management includes viewing, creating, deleting, and updating the personal information of users.

## Project Structure

- **src/**: Source code for the application.
  - **Domain/**: Core business logic and entities (Library, User, Media, Interfaces).
  - **Persistence/**: Data access layer (handling CSV storage).
  - **Presentation/**: User interface logic (console interactions).
- **test/**: Unit tests for the application.
- **doc/**: Project documentation and specifications.
- **var/**: Runtime data storage (e.g., `data.csv`).


## How to Run

1. Open a terminal in the root directory of the project.
2. Run the following command:

```bash
dotnet run --project src/OopWorkshop.csproj
```
## Features

- **Domain Modeling**: The system models media items, users, and their behaviors using Object-Oriented Programming (OOP) principles.  
- **Interfaces**: Media actions such as play, read, watch, download, execute, complete, and display are defined through interfaces and implemented by the corresponding media classes.  
- **User Roles**: Supports three user roles—Borrower, Employee, and Admin—each with clearly defined responsibilities.  
- **Persistence**: Data is stored and managed using CSV files, enabling simple and persistent storage of media and user information.  
- **Presentation**: A console-based interface guides users with clear instructions, validates inputs, and ensures proper execution of actions.  
- **Extensibility**: Designed for future growth, allowing new media types or user roles to be added without affecting existing functionality.

![Domain Class Diagram](diagram.svg)