
# To-Do App with Azure SQL Edge, .NET Core Web API, and React.js

This project is a To-Do application that uses **Azure SQL Edge**, **.NET Core Web API**, and **React.js**. The application follows a **Database-First Approach** and can be run in Docker containers. The backend and frontend are both equipped with unit and integration testing to ensure quality and reliability.

## Project Structure

- **Backend**: The .NET Core Web API for handling business logic and database interaction.
- **Frontend**: The React.js app that allows users to interact with the To-Do list.
- **Database**: Azure SQL Edge running in a Docker container to store the application's data.

## Getting Started

### Prerequisites

- Docker (for running the application in containers)
- .NET Core (for the backend)
- Node.js and npm (for the frontend)

### Steps to Run the Application

1. **Clone the repository**  
   Clone this project to your local machine:

   ```bash
   git clone https://github.com/yourusername/todo-app.git
   cd todo-app
   ```

2. **Run the Docker Containers**  
   After cloning the repository, use Docker Compose to start the containers for the backend, frontend, and SQL Server:

   ```bash
   docker-compose up --build
   ```

   This will:
   - Start the **Azure SQL Edge** container (database).
   - Build and start the **.NET Core Web API** backend.
   - Build and start the **React.js** frontend.

   **Note**: If the SQL Server container is not running, the `docker-compose` command will automatically start it.

3. **Create the Database and Table**  
   Once the Docker containers are up and running, connect to the SQL Server instance:

   ```bash
   sqlcmd -S localhost,1433 -U SA -P 'MyPass@word'
   ```

   Then, run the following SQL commands to create the `TodoApp` database and the `Task` table:

   ```sql
   CREATE DATABASE TodoApp;
   USE TodoApp;

   CREATE TABLE Task (
       TaskId INT IDENTITY(1,1) PRIMARY KEY,
       Title NVARCHAR(255) NOT NULL,
       Description NVARCHAR(1000) NOT NULL,
       IsCompleted BIT NOT NULL DEFAULT 0,
       CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
   );
   ```

4. **Verify Database and Table Creation**  
   You can verify the creation of the database and table with the following queries:

   ```sql
   SELECT * FROM sys.databases;
   USE TodoApp;
   SELECT * FROM sys.tables;
   ```

5. **Run the Backend and Frontend**  
   After setting up the database, you can run the backend and frontend applications.

   - **Backend**: The backend can be run in a Docker container or directly on your local machine using .NET Core.
   - **Frontend**: The frontend can be run in a Docker container or directly on your local machine using React.js.

   **Note**: Ensure the connection string in the backend points to the SQL Server container. The connection string should look like:

   ```text
   Server=sql,1433;Database=TodoApp;User=sa;Password=MyPass@word;TrustServerCertificate=true
   ```

6. **Access the Application**  
   After the database is set up, you can access the application:
   - **Frontend**: Navigate to [http://localhost:3000](http://localhost:3000) to view the React.js app.
   - **Backend**: The backend API will be accessible at [http://localhost:5001](http://localhost:5001).
   - **Database**: The database can be accessed through SQL Server at `localhost:1433`.

### Environment Variables

- **Backend**:
  - `ConnectionStrings__DefaultConnection`: Connection string for the database.
- **Frontend**:
  - `VITE_API_URL`: URL for the backend API.

## Testing

- **Backend Testing**: Unit and integration tests for the .NET Core Web API are included.
- **Frontend Testing**: Unit tests for the React.js app are also included.

To run the tests for both the frontend and backend, simply use the following commands:

- For the backend (in the `backend/to-do-api` directory):

  ```bash
  dotnet test
  ```

- For the frontend (in the `frontend/to-do-web` directory):

  ```bash
  npm test
  ```

## Conclusion

This application provides a simple To-Do app with Azure SQL Edge as the database, .NET Core Web API for the backend, and React.js for the frontend. It is containerized using Docker, and the database-first approach allows for easy integration with SQL Server.
