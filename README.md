# LibraryDesk — Library Management System

A lightweight Windows desktop application developed in **C# and .NET Windows Forms** for managing library books, tracking borrowing activity, and organizing daily library operations.

## Download Windows Application

**[Download LibraryDesk v1.0.0](https://github.com/Tafseer-ibn-Abbas/librarydesk-winforms/releases/tag/v1.0.0)**

Download the Windows ZIP from GitHub Releases, extract it, and run `LibraryDesk.exe`.

## Key Features

- **Dashboard:** View total, available, and issued books.
- **Book Management:** Add, search, and remove library books.
- **Book Search:** Find books by title or author.
- **Borrowing System:** Issue and return books with borrower details.
- **Due Date Tracking:** Automatic 14-day return deadlines and overdue indicators.
- **Persistent Storage:** Automatically save library records locally.
- **Demo Data:** Preloaded sample books for exploring the application.

## Technology Stack

| Component | Technology |
|---|---|
| Programming Language | C# |
| Desktop Framework | Windows Forms |
| Target Framework | .NET Framework 4.7.2 |
| Data Storage | XML |
| Platform | Windows |
| Development Tools | VS Code / Visual Studio |

## Getting Started

### Option 1 — Download the Application

1. Visit the GitHub Releases page.
2. Download `LibraryDesk-Windows-v1.0.0.zip`.
3. Extract the downloaded archive.
4. Open `LibraryDesk.exe`.

### Option 2 — Run from Source Code

Clone the repository, open the project folder in VS Code on Windows, and execute:

`powershell -NoProfile -ExecutionPolicy Bypass -File .\Build-And-Run.ps1`

Alternatively, open `LibraryDesk.sln` in Visual Studio with the .NET Framework 4.7.2 targeting pack installed.

## Demo Access

**Admin Password:** `admin123`

The application includes sample library records to demonstrate its functionality.

## Local Data Storage

Library records are automatically saved to:

`%LOCALAPPDATA%\LibraryDesk\library.xml`

Book inventory and borrowing information persist between application sessions.

## About This Project

LibraryDesk demonstrates desktop application development using C#, object-oriented programming, file-based data persistence, and basic library management workflows.

The application is intended as a functional portfolio demonstration rather than a production-ready, multi-user library platform. Demo authentication should not be used to protect sensitive information.

## Developer

**Tafseer Ibn Abbas**

[GitHub Profile](https://github.com/Tafseer-ibn-Abbas)
