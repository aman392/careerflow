# CareerFlow

A personal job application tracker built with React, ASP.NET Core and PostgreSQL.

## Features

- Add, edit, filter and search applications.
- Track status changes and review application history.
- Record interviews and follow-up dates.
- Load optional fictional records featuring real company names into an empty database.
- Keep application data in PostgreSQL between restarts.
- Check key status rules through xUnit tests.

## Technology

React, TypeScript, Vite, Tailwind CSS, ASP.NET Core 10, Entity Framework Core 10, PostgreSQL.

## Local setup (Windows)

### Database

Install PostgreSQL, remember the `postgres` user password, and create a database named `careerflow` using pgAdmin or SQL Shell.

Configure the backend connection using an environment variable in the PowerShell window (recommended):

```powershell
$env:ConnectionStrings__CareerFlow = 'Host=localhost;Port=5432;Database=careerflow;Username=postgres;Password=YOUR_PASSWORD'
```

Replace `YOUR_PASSWORD`. Don't commit passwords into Git.

### Backend

```powershell
cd backend/CareerFlow.Api
dotnet restore
dotnet run
```

The API listens on `http://localhost:5086`. Check `http://localhost:5086/api/health`.

Stage 1 creates tables automatically on initial startup (`EnsureCreated`). For production, replace this with EF Core migrations and proper credentials.

### Frontend (second PowerShell window)

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173`. Vite proxies `/api` requests to the .NET backend.

### Automated tests

```powershell
cd backend/CareerFlow.Tests
dotnet test
```

## Sample data

Use **Load sample applications** on an empty database. These records are fictional examples using company names such as Infosys, Accenture, Cognizant, and TCS. They are not claims of actual job applications or vacancies.

## Current scope

Stage 1 is a local development app. It does not yet provide accounts or multi-user access. Use only fictional demo records until authentication and hosting are implemented. Database schema initialization is intended for a fresh local database. Further stages will add migrations, stronger validation, and deployment configuration.
