# Football Pick'em Pool App

An ASP.NET Core web application for managing a football pick'em pool where users pick against the spread for Top-25 NCAAF and all NFL games each week.

## Features

- **User Authentication**: ASP.NET Identity for secure user registration and login
- **Game Management**: Admin interface to load and manage weekly games with favorites, underdogs, and spreads
- **Pick'em System**: Users make picks against the spread before games start
- **Automatic Scoring**: System automatically scores picks when admin enters final scores
- **Standings**: 
  - Weekly standings showing user performance for each week
  - Season-long standings tracking cumulative performance
- **Role-Based Access**: Admin role for game and score management

## Technology Stack

- **ASP.NET Core 10.0**: Web framework with Razor Pages
- **Entity Framework Core 10.0**: ORM for database operations
- **ASP.NET Identity**: Authentication and authorization
- **SQLite**: Database (easily swappable for SQL Server or other providers)
- **Bootstrap 5**: Responsive UI framework

## Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- A code editor (Visual Studio, VS Code, or Rider)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/BuckBuckGoose/football-pool-app.git
   cd football-pool-app
   ```

2. Navigate to the application folder:
   ```bash
   cd FootballPoolApp
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to `https://localhost:5001` (or the URL shown in the console)

### Default Admin Account

The application creates a default admin account on first run:
- **Email**: admin@footballpool.com
- **Password**: Admin123!

**Important**: Change this password immediately in a production environment!

## Usage

### For Administrators

1. Log in with the admin account
2. Navigate to **Admin > Games** from the menu
3. Click **Add New Game** to create games:
   - Enter week number
   - Select league (NCAAF or NFL)
   - Enter favorite and underdog team names
   - Set the spread (favorite is always giving points)
   - Set game date and time
4. After games are completed, click **Score** to enter final scores
5. The system automatically calculates which picks were correct

### For Users

1. Register a new account or log in
2. Navigate to **Picks** to see current week's games
3. Select your picks for games that haven't started yet
4. Click **Save Picks** to submit
5. View **Standings** to see:
   - Weekly performance for each week
   - Season-long cumulative standings

## Database

The application uses SQLite by default with the connection string in `appsettings.json`. The database is automatically created and seeded on first run.

To use SQL Server or another database:
1. Update the connection string in `appsettings.json`
2. Modify `Program.cs` to use the appropriate database provider (e.g., `UseSqlServer` instead of `UseSqlite`)

## Project Structure

```
FootballPoolApp/
├── Areas/Identity/          # ASP.NET Identity pages
├── Data/                    # Database context and migrations
├── Models/                  # Domain models
├── Pages/                   # Razor Pages
│   ├── Admin/              # Admin-only pages
│   ├── Picks.cshtml        # User pick submission
│   ├── Standings.cshtml    # Weekly standings
│   └── StandingsSeason.cshtml  # Season standings
└── Program.cs              # Application configuration
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the MIT License.