# Ticket Management System

This project is a full-stack web application for managing tickets, built with .NET for the backend and React with TypeScript for the frontend.


## Backend Setup

1. Clone the repository:
   ```
   git clone https://github.com/otmanik/TicketManagement.git
   cd TicketManagement
   ```

2. Navigate to the backend directory:
   ```
   cd Backend
   ```

3. Restore the .NET packages:
   ```
   dotnet restore
   ```

4. Update the database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=TicketManagementSystem;User Id=your_username;Password=your_password;"
   }
   ```

5. Apply the database migrations:
   ```
   dotnet ef migrations add InitialCreate --project .\src\TicketManagement.Infrastructure\TicketManagement.Infrastructure.csproj --startup-project .\src\TicketManagement.API\TicketManagement.API.csproj --context TicketManagementDbContext
   dotnet ef database update --project .\src\TicketManagement.Infrastructure\TicketManagement.Infrastructure.csproj --startup-project .\src\TicketManagement.API\TicketManagement.API.csproj --context TicketManagementDbContex
   ```

6. Run the backend application:
   ```
   dotnet run
   ```

The API should now be running on `https://localhost:5001`.

## Frontend Setup

1. Navigate to the frontend directory:
   ```
   cd ../Frontend
   ```

2. Install the dependencies:
   ```
   npm install
   ```

3. Start the development server:
   ```
   npm start
   ```

The frontend application should now be running on `http://localhost:3000`.

## Running the Application

1. Ensure that both the backend and frontend are running.
2. Open your browser and navigate to `http://localhost:3000`.
3. You should see the Ticket Management System interface.

## Additional Information

- The backend API documentation is available at `https://localhost:5001/swagger` when the backend is running.
- To run tests (TBD):
  - Backend: In the Backend directory, run `dotnet test`
