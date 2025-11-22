This project is a **Multi-Tier ASP.NET Core MVC Application** designed to manage internal company resources (Employees, Departments) and administrative tasks (Users, Roles) with secure authentication and third-party integrations.

-----

# 🏢 Company Management System

A robust, **Human Resource Management System** built with **ASP.NET Core 8.0 MVC**. This application streamlines the management of company departments and employees while providing a secure administrative dashboard for user and role management.

It adheres to **Clean Architecture** principles, utilizing the **Repository & Unit of Work** patterns to ensure code maintainability and scalability.

-----

## 🔭 Project Overview

The **Company Management System** is a full-stack web application designed to digitize HR operations. It allows administrators and moderators to perform CRUD operations on company entities, manage secure access levels, and handle communications via Email and SMS.

**Core Capabilities:**

  * **Resource Management:** specialized handling of Departments and Employees.
  * **Security:** Robust Identity system with Role-Based Access Control (RBAC).
  * **Communication:** Integrated services for sending transactional Emails and SMS.
  * **Performance:** Server-side filtering and optimized database queries.

-----

## 🛠 Tech Stack

### Frameworks & Languages

  * **C\# 12** & **.NET 8.0**
  * **ASP.NET Core MVC** (Razor Views)
  * **HTML5, CSS3, Bootstrap 5** (Frontend UI)
  * **jQuery** & **jQuery Validation** (Client-side scripting)

### Data & Persistence

  * **Entity Framework Core 8.0** (Code-First ORM)
  * **SQL Server** (Relational Database)
  * **AutoMapper** (Object-Object Mapping)

### Security & Identity

  * **ASP.NET Core Identity** (User & Role Management)
  * **Google Authentication** (OAuth 2.0)
  * **Twilio** (2FA / SMS Notifications)

### Third-Party Integrations

  * **MailKit & MimeKit** (SMTP Email Service)
  * **Twilio SDK** (SMS Service)

-----

## 🏗 Architecture

The solution follows a **Layered (N-Tier) Architecture**, promoting separation of concerns:

1.  **Company.DAL (Data Access Layer):**

      * Contains the database context (`AppDbContext`) and entities (`Employee`, `Department`, `AppUser`).
      * Implements the **Repository Pattern** (`GenericRepository`) to abstract database operations.
      * Manages Database Migrations.
      * Defines the **Unit of Work** (`IUnitOfWork`) to coordinate transactions.

2.  **Company.BLL (Business Logic Layer):**

      * Contains Service Interfaces (`IEmployeeService`, `IDepartmentService`) and their implementations.
      * Handles DTO mappings and business validation logic.

3.  **Company.PL (Presentation Layer):**

      * The entry point (`Program.cs`) containing DI configurations.
      * **Controllers:** Handle HTTP requests (`AccountController`, `EmployeesController`, etc.).
      * **Views:** Render the UI using Razor syntax and Tag Helpers.
      * **ViewModels:** Data containers specific to UI requirements.

-----

## ✨ Features

### 🏢 Department & Employee Management

  * **CRUD Operations:** Create, Read, Update, and Delete departments and employees.
  * [cite\_start]**Image Handling:** Upload and manage employee profile pictures[cite: 43247].
  * [cite\_start]**Search & Filter:** Real-time search functionality for employees by name[cite: 43252].
  * [cite\_start]**Soft Delete:** "IsDeleted" flag implementation to preserve data integrity[cite: 43706].

### 🔐 Authentication & Authorization

  * **Secure Identity:** Registration, Login, and Logout functionality.
  * [cite\_start]**Role Management:** Dynamic creation of Roles and assignment to Users[cite: 44011].
  * [cite\_start]**Google Login:** Integrated OAuth allowing users to sign in with their Google accounts[cite: 43961].
  * [cite\_start]**Password Recovery:** Forgot Password flows supported by Email links and SMS[cite: 43969].

### 📨 Communication Services

  * **Email Service:** Automated emails using MailKit (e.g., for password resets).
  * [cite\_start]**SMS Service:** Integration with Twilio to send notifications and secure links[cite: 43973].

-----

## 📂 Folder Structure

```
CompanyMvcRepo
├── Company.DAL             # Data Access Layer
│   ├── Data                # DbContext & Migrations
│   ├── Models              # Database Entities
│   └── Repositories        # Generic & Specific Repositories
├── Company.BLL             # Business Logic Layer
│   ├── Interfaces          # Service Contracts
│   ├── Services            # Business Logic Implementations
│   └── Mapping             # AutoMapper Profiles
└── Company.PL              # Presentation Layer (Web)
    ├── Controllers         # MVC Controllers
    ├── Views               # Razor Views
    ├── ViewModels          # UI Data Models
    ├── wwwroot             # Static Assets (CSS, JS, Images)
    └── appsettings.json    # Configuration (DbStrings, API Keys)
```

-----

## 🚀 Getting Started

### Prerequisites

  * [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
  * [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  * Visual Studio 2022 (or VS Code)

### Configuration

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/Abdulrahman00130/CompanyMvcRepo.git
    ```
2.  **Update `appsettings.json`:**
    Navigate to `Company.PL` and configure your connection strings and API keys:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=CompanyMVC;Trusted_Connection=True;..."
    },
    "MailSettings": {
      "Email": "your-email@gmail.com",
      "Password": "your-app-password"
    },
    "TwilioSettings": {
      "AccountSID": "your-sid",
      "AuthToken": "your-token"
    },
    "Authentication": {
      "Google": { "ClientId": "...", "ClientSecret": "..." }
    }
    ```

### Running the Application

1.  **Apply Migrations:**
    Open Package Manager Console (PMC) and run:
    ```powershell
    Update-Database
    ```
2.  **Run the Project:**
    ```bash
    cd Company.PL
    dotnet run
    ```
3.  Open your browser and navigate to `https://localhost:7018` (or the port specified in your launch profile).

-----

## 🧪 Testing

  * **Manual Testing:** The project utilizes strict Model Validation (Data Annotations) on ViewModels to ensure data integrity during manual testing via the UI.
  * **Debugging:** The solution includes `launchSettings.json` profiles for IIS Express and Kestrel for easy debugging.

-----

## 📸 Screenshots


<img width="1920" height="987" alt="image" src="https://github.com/user-attachments/assets/f6d05591-6b17-4217-8d7a-dfbb56c4eafc" />
Login page

<img width="1920" height="987" alt="image" src="https://github.com/user-attachments/assets/0c987b99-5a7b-4a9c-b1ce-709644faf986" />
Register Page

<img width="1913" height="977" alt="image" src="https://github.com/user-attachments/assets/b17b4aaa-a710-439e-a092-5ea5c22e5d29" />
Users Page

<img width="1920" height="977" alt="image" src="https://github.com/user-attachments/assets/e6184243-3ba1-4934-bfb7-650271d2ebed" />
Access denied for the departments page without an admin role

-----
