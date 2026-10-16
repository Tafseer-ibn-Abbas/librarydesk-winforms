# LibraryDesk

**A clean, lightweight library management desktop app built with C# and Windows Forms.**

LibraryDesk is a polished portfolio revision of an older classroom *Library Management System*. It preserves the original add / browse / issue / return workflows, improves the experience, and adds local persistence without a database server or external dependencies.

## Features

- Home screen with total, available and issued book counts
- Admin area to add, find, and remove books (issued books cannot be deleted)
- Catalog search by title, author, or borrower in the admin area
- Public/library desk view to search titles and issue or return books
- Borrower name and automatic 14-day due date when issuing
- Overdue status highlighting
- Local XML file saved automatically after changes
- Six **clearly labeled demo records** for a useful first launch

## Run on Windows

### Option A: VS Code + built-in Windows compiler (lightweight; no SDK download)

1. Open this `LibraryDesk_Portfolio` folder in VS Code.
2. Open **Terminal > New Terminal** (PowerShell).
3. Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Build-And-Run.ps1
```

The script compiles and opens `LibraryDesk\bin\Debug\LibraryDesk.exe`. This route uses the Windows C# compiler at `%WINDIR%\Microsoft.NET\Framework[64]\v4.0.30319\csc.exe`. The project source has been made compatible with the older compiler. Requires a functioning .NET Framework 4.x Windows installation; normal project build still targets .NET Framework 4.7.2.

**Note:** This alternative build path has not been tested on a native Windows machine. If compilation fails, send the entire terminal output; do not install extra packages until we identify the error.

### Option B: Visual Studio

1. Install **Visual Studio 2022** with the **.NET desktop development** workload.
2. Make sure the **.NET Framework 4.7.2 Developer Pack / targeting pack** is installed. Visual Studio may prompt you to add it.
3. Open `LibraryDesk.sln`.
4. Choose **Debug** and press **F5** (or Build > Build Solution).
5. On the home screen, choose **Open admin** or **Browse library**.

There are **no NuGet packages, web services, or external databases** to configure.

### Demo admin access

- Password: `admin123` (demo only)
- To override the password for your local session, set a Windows user environment variable named `LIBRARYDESK_ADMIN_PASSWORD` and restart the app. For example, in PowerShell before running the app:

```powershell
$env:LIBRARYDESK_ADMIN_PASSWORD = "MyLocalDemoPassword"
```

> The authentication is intentionally lightweight and **not production-grade**. The demo password is publicly documented, there are no user accounts, and anyone with access to the local data file can edit it. Do not use this project to protect confidential library records.

## Data storage

The app creates its data file in:

`%LOCALAPPDATA%\LibraryDesk\library.xml`

Six sample books appear **only on the first launch**, when this file does not exist. Subsequent launches load saved books and loan states. The app maintains a simple `library.xml.bak` backup when replacing the saved file.

To start a fresh demo, close the app and move the XML file to a safe place (do not delete user data unless you intend to reset the demo). If stored data is corrupt, the app stops with an error rather than overwriting it.

## Tech stack

| Area | Technology |
|---|---|
| UI | C# Windows Forms |
| Runtime | .NET Framework 4.7.2 |
| Storage | XML serialization in local AppData |
| IDE | Visual Studio 2022 on Windows |

## Scope

This is intentionally a **small desktop portfolio project**, not a hosted multi-user SaaS platform. It does not implement real user permissions, audit history, fines, barcode scanning, or cloud sync. Those features would add unnecessary complexity for this use case.

## Portfolio screenshots

After running the project on Windows, capture the **home**, **admin catalog**, and **borrow/return** screens from the real application before publishing your GitHub README or freelance portfolio. No fabricated runtime screenshots are included.

## Source and release notes

- Original project: student WinForms library management exercise with in-memory records.
- Portfolio revision: `LibraryDesk`, revised layout, search, persistent storage, borrower names, due dates and basic error handling.
- Release validation: project XML and source-level checks were performed. A native Windows build and GUI run are **still required** before publishing as a tested app.
