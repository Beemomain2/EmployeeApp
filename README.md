Employee CRUD Application
A simple Employee CRUD application built with .NET 8 Web API following Clean Architecture principles and a React frontend.

Architecture Overview
├── Domain/                 
├── Application/            
├── Infrastructure/         
├── API/                   
├── Tests/                 # Unit tests
└── UI/              # frontend

Quick Start with Docker

Clone and navigate to the project:
git clone <repository-url>
cd EmployeeCRUD


Prerequisites

.NET 9 SDK
Node.js (for React frontend)
Docker Desktop (for containerized setup)
SQL Server (or use Docker container)

Run with Docker Compose:
docker-compose up -d

Access the application:
API: https://localhost:5001

Backend Setup

Restore dependencies:
dotnet restore

Update database connection string in API/appsettings.json:
json{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeDB;Integrated Security=true;TrustServerCertificate=true;"
  }
}

Apply database migrations:
cd API
dotnet ef database update

Run the API:
dotnet run

The API will be available at:
HTTPS: https://localhost:5001

Frontend Setup
The React frontend is provided as a single component that can be integrated into any React application.

Install required dependencies:
npm install lucide-react

Add Tailwind CSS for styling:
bashnpm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

Start the React development server:
npm start

Testing
Run unit tests:
dotnet test

Docker Configuration
The application includes a complete Docker setup:

API Container: .NET 9 application
SQL Server Container: Database with persistent storage
Network: Isolated network for container communication
Volumes: Persistent database storage

Docker Commands
bash# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api
docker-compose logs -f sqlserver

# Stop all services
docker-compose down

# Rebuild and start
docker-compose up -d --build
