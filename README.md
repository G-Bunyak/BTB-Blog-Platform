---

## Part 1: Blog-Platform-Server 
Contains backend API for a Blog Platform with couple of xUnit tests.

Open Package Manager Console in Visual studio.
Run command to restore all required dependencies for the project

```sh
dotnet restore
```

Open appsettings.json file located in "Blog-Platform-Server/BlogPlatform" project folder and put your database connection string:

```json
  "DatabaseConnectionString": "YourConnectionStringHere",
```

It might be any empty database.

Database migrations:
Go back to Package Manager Console
Run next command to apply it to your database

```sh
Update-Database
```

That's should be enough to run the project using visual studio in debug mode.
OR
Open "Blog-Platform-Server/BlogPlatform" folder in the PowerShell
Run next command and follow the instructions

```sh
dotnet run
```

---
## Part 2: Blog-Platform-Client
Contains frontend representation of a Blog Platform.

In order to run it you should initialize all dependencies.
Open PowerShell and run next command:

```sh
npm install
```

Then you can start project using next command:

```sh
npm run dev
```

---

## Part 3: GetAmortizationSchedule
Stored produre SQL script can be found at "AmortizationStoredProcedure" folder
You need to run this SQL script as QUERY on any database and call the stored procedure with EXEC command

```sh
EXEC [dbo].[GetAmortizationSchedule]
```