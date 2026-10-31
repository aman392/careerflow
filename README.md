# CareerFlow

CareerFlow is a job application tracking web application built with React, TypeScript, ASP.NET Core, and PostgreSQL.

It brings job applications, interview details, status updates, and follow-up dates into one place, making it easier to keep track of opportunities throughout the hiring process.

## Features

Application tracking: Add, edit, search, and filter job applications by company, role, and status.

Application history: Track changes as applications move through different recruitment stages.

Interview records: Save interview rounds, dates, modes, and notes for each application.

Follow-up tracking: Set follow-up dates and identify pending actions.

Dashboard: View application totals, active applications, interview-stage applications, and follow-ups due.

Sample data: Load example applications to explore the system

## Technology

React, TypeScript, Vite, Tailwind CSS, ASP.NET Core 10, Entity Framework Core 10, PostgreSQL.

## How It Works

CareerFlow uses a React frontend connected to an ASP.NET Core Web API. The backend handles application data, validates requests, manages status changes, and stores records in PostgreSQL through Entity Framework Core.

Application status changes are recorded in a history log, while interview details and follow-up dates remain associated with the relevant application.

The dashboard displays information from stored application records rather than hardcoded statistics.

## Running Locally

Requirements: .NET 10 SDK, Node.js, npm, and PostgreSQL.

Create a PostgreSQL database named careerflow and configure the CareerFlow connection string for your local environment.

Backend

From backend/CareerFlow.Api:

dotnet restore
dotnet run

The API runs at http://localhost:5086.

Frontend

From frontend:

npm install
npm run dev

Open http://localhost:5173 to access CareerFlow.

Backend tests

From backend/CareerFlow.Tests:

dotnet test
## Development Status

CareerFlow currently supports application management, status history, interview records, follow-up dates, and PostgreSQL persistence.

The application has been tested locally with sample records. Authentication, multi-user accounts, and public hosting are planned improvements.

The sample records use real company names for demonstration purposes and do not represent actual job applications.
