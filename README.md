# Company Dashboard

A multi-layered ASP.NET Core MVC application for comprehensive company management. The system supports secure authentication, CRUD operations for departments, employees, projects, tasks, dashboards, and meeting scheduling. Built with Entity Framework Core and Bootstrap for a modern, responsive UI.

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Database Structure](#database-structure)
- [Authentication & Authorization](#authentication--authorization)
- [Setup & Installation](#setup--installation)
- [Usage](#usage)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

---

## Features

### Authentication & Authorization
- **User Registration & Login:** Secure registration and login with hashed passwords.
- **Role-based Access:** Employees have roles (e.g., Admin, Employee) that control access to features.
- **Session Management:** Uses cookie authentication for persistent sessions.

### Department Management
- **CRUD Operations:** Create, view, update, and delete departments.
- **Employee Association:** View all employees in a department.

### Employee Management
- **CRUD Operations:** Manage employee records, including department assignment and role.
- **Role Assignment:** Assign roles to employees for access control.

### Project Management
- **CRUD Operations:** Create, view, update, and delete projects.
- **Department Association:** Assign projects to one or more departments.
- **Meeting Linkage:** Link projects to meetings for better coordination.

### Task Management
- **CRUD Operations:** Manage tasks, assign them to employees or projects.
- **Status Tracking:** Track task progress and completion.

### Meeting Scheduling
- **Meeting Creation:** Schedule meetings, set participants, and link to projects/departments.
- **Participant Management:** Add employees, departments, or projects to meetings.

### Dashboard
- **Overview:** Visual dashboard with key company metrics (departments, employees, projects, meetings, tasks).
- **Quick Links:** Access to most-used features.

### Responsive UI
- **Bootstrap-based:** Modern, mobile-friendly design.
- **Validation:** Client-side and server-side validation for forms.

---

## Architecture

This project follows a clean, layered architecture:

- **Presentation Layer (`Company.PL`):**  
  ASP.NET Core MVC web application. Handles HTTP requests, user interface, and user input validation. Contains controllers, views, and view models.

- **Business Logic Layer (`Company.BLL`):**  
  Implements the repository pattern. Contains business rules, validation, and logic for manipulating data. Interfaces and concrete repository classes abstract data access.

- **Data Access Layer (`Company.DAL`):**  
  Entity Framework Core for ORM. Contains the `DbContext`, entity classes, and database migrations.

**Data Flow Example:**  
User submits a form → Controller (PL) → Repository (BLL) → DbContext (DAL) → Database  
Results/data flow back up the stack to the user.

---

## Project Structure

```
Final Company.DAL/
│
├── Company.PL/         # Presentation Layer (MVC)
│   ├── Controllers/    # Account, Department, Employee, Project, Task, Meeting, Home
│   ├── Models/         # ViewModels and DTOs for data transfer and validation
│   ├── Views/          # Razor Views for all features (organized by entity)
│   ├── wwwroot/        # Static files (CSS, JS, images, Bootstrap)
│   └── Program.cs      # Application entry point and configuration
│
├── Company.BLL/        # Business Logic Layer
│   ├── Interface/      # Repository interfaces (e.g., IDepartmentRepostery)
│   └── Repostery/      # Repository implementations (e.g., DepartmentRepostery)
│
├── Company.DAL/        # Data Access Layer
│   ├── Data/           # DbContext and Migrations
│   └── Entity/         # Entity classes (Department, Employee, Project, Task, Meeting, User, etc.)
│
└── Company.DAL.sln     # Solution file
```

---

## Database Structure

The application uses SQL Server via Entity Framework Core.  
**Main Entities:**
- `Department`: Name, Code, CreatedAt, etc.
- `Employee`: Name, Email, Role, DepartmentId, etc.
- `User`: Email, PasswordHash (for authentication)
- `Project`: Name, Description, etc.
- `Task`: Title, Description, Status, AssignedTo, etc.
- `Meeting`: Title, Date, etc.

**Join Entities (for many-to-many relationships):**
- `ProjectDepartment`
- `MeetingParticipant`
- `MeetingDepartment`
- `MeetingProject`

**DbContext Example:**
```csharp
public DbSet<Department> Departments { get; set; }
public DbSet<Employee> Employees { get; set; }
public DbSet<User> Users { get; set; }
public DbSet<Project> Projects { get; set; }
public DbSet<Task> Tasks { get; set; }
public DbSet<Meeting> Meetings { get; set; }
```

**Migrations:**  
Database schema is managed via EF Core migrations in `Company.DAL/Data/Migrations/`.

---

## Authentication & Authorization

- **Registration:** Users register with email and password. Passwords are hashed using ASP.NET Core's `PasswordHasher`.
- **Login:** Users log in with email and password. On success, a cookie is issued for authentication.
- **Roles:** Employee roles (e.g., Admin, Employee) are used for access control.
- **Authorization:** Controllers and actions are protected with `[Authorize]` attributes. Only authorized users can access certain features.
- **Logout:** Users can log out, which clears the authentication cookie.

---

## Setup & Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio 2022+ or VS Code

### Steps

1. **Clone the repository:**
   ```sh
   git clone <your-repo-url>
   cd Final\ Company.DAL
   ```

2. **Restore NuGet packages:**
   ```sh
   dotnet restore
   ```

3. **Configure the database:**
   - Ensure SQL Server is running.
   - Update the connection string in `Company.PL/Program.cs` and `appsettings.json` if needed.

4. **Apply migrations and create the database:**
   ```sh
   dotnet ef database update --project Company.DAL --startup-project Company.PL
   ```

5. **Build and run the application:**
   ```sh
   dotnet run --project Company.PL
   ```

6. **Access the app:**
   - Open your browser and go to `https://localhost:5001` (or the port shown in the console).

---

## Usage

### Example Flows

#### Register & Login
- Go to `/Account/Register` to create a new user.
- Log in at `/Account/Login`.

#### Department Management
- Navigate to `/Department` to view all departments.
- Create, edit, or delete departments as needed.

#### Employee Management
- Go to `/Employee` to manage employees.
- Assign employees to departments and set their roles.

#### Project & Task Management
- Use `/Project` and `/Task` to manage projects and tasks.
- Assign tasks to employees or projects.

#### Meeting Scheduling
- Go to `/Meetings` to schedule and manage meetings.
- Add participants, link to projects/departments.

#### Dashboard
- The home page provides a dashboard overview of all key metrics.

---

## Troubleshooting

- **Database Connection Issues:**  
  Ensure your SQL Server is running and the connection string is correct.

- **Migrations Not Applying:**  
  Make sure you run the migration command from the solution root and specify the correct startup project.

- **Static Files Not Loading:**  
  Check that `app.UseStaticFiles();` is present in `Program.cs`.

---

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

- Follow the existing code style.
- Write clear commit messages.
- Add tests for new features where possible.

---

## License

This project is licensed under the MIT License.
