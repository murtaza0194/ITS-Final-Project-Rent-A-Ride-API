#  Rent-A-Ride API & Client

Welcome to the **Rent-A-Ride** project! This is a comprehensive full-stack solution for a vehicle rental management system. It features a robust **.NET 9 Clean Architecture** backend and a modern, high-performance **React 19** frontend.

##  Features

*   **Vehicle Management**: CRUD operations for various vehicle types and maintenance logs.
*   **Rental System**: Complete booking flow with rental tracking and history.
*   **User Management**: Secure authentication (JWT) and role-based access.
*   **Audit Logging**: Comprehensive tracking of system changes/actions.
*   **Background Jobs**: Integrated Hangfire for scheduled tasks (Maintenance reminders, etc.).
*   **Modern UI**: Responsive, animated interface built with Tailwind CSS v4 and Framer Motion.

---

##  Technology Stack

### Backend 
Built with **.NET 9.0** following **Clean Architecture** principles.
*   **Core**: ASP.NET Core Web API
*   **Database**: SQLite (via Entity Framework Core 9)
*   **Authentication**: JWT (JSON Web Tokens)
*   **Background Jobs**: Hangfire (Memory Storage)
*   **Documentation**: Swagger / OpenAPI
*   **Key Libraries**: `Unmanaged` (Architecture), `MediatR` (CQRS pattern - inferred), `FluentValidation`.

### Frontend 
Built with **React 19** and **Vite**.
*   **Styling**: Tailwind CSS v4, PostCSS
*   **Routing**: React Router v7
*   **State & API**: Axios, React Hooks
*   **UI/UX**: Framer Motion (Animations), Lucide React (Icons), React Hot Toast (Notifications)
*   **Utilities**: Date-fns

---

##  Project Structure

```
├── client/                 # Frontend (React + Vite) Application
│   ├── src/
│   │   ├── components/     # Reusable UI components (Navbar, Modal, Cards)
│   │   ├── pages/          # Application views (Home, Dashboard, Login)
│   │   ├── api.js          # API integration layer
│   │   └── ...
│   ├── package.json        # Frontend dependencies
│   └── vite.config.js      # Vite configuration
│
├── src/                    # Backend (.NET) Solution
│   ├── RentARide.Api/              # Entry point (Controllers, Config)
│   ├── RentARide.Application/      # Business Logic (Services, DTOs)
│   ├── RentARide.Domain/           # Core Entities (User, Vehicle, Rental)
│   └── RentARide.Infrastructure/   # DB Context, Repositories, Migrations
```

---

##  Getting Started

### Prerequisites
*   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [Node.js](https://nodejs.org/) (Latest LTS recommended)
*   [Git](https://git-scm.com/)

###  Backend Setup (API)

1.  Navigate to the API directory:
    ```bash
    cd src/RentARide.Api
    ```

2.  Restore dependencies:
    ```bash
    dotnet restore
    ```

3.  Apply Database Migrations (creates `app.db` locally):
    ```bash
    dotnet ef database update
    ```
    *(Note: Ensure you have `dotnet-ef` tool installed globally if needed)*

4.  Run the server:
    ```bash
    dotnet run
    ```
    The API will start at `http://localhost:5038` (or similar, check console output).
    Swagger UI is available at `/swagger/index.html`.

###  Frontend Setup (Client)

1.  Open a new terminal and navigate to the client directory:
    ```bash
    cd client
    ```

2.  Install dependencies:
    ```bash
    npm install
    ```

3.  Start the development server:
    ```bash
    npm run dev
    ```
    The application will run at `http://localhost:5173`.

---

##  Credentials & Configuration

*   **Database**: Uses SQLite by default. The database file will be created in the API folder.
*   **Authentication**: The system uses JWT. Default connection string and JWT settings are located in `src/RentARide.Api/appsettings.json`.

##  License
This project is for educational (ITS Final Project) purposes.
