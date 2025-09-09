# EasyGames 🏀📖🎮

A simple web application for selling books, games, and toys built with ASP.NET Core MVC.

## Features 🛒

- **Product Management**: Admin users can create, edit, and delete products (Books, Games, Toys)
- **Shopping Cart**: Authenticated users can add items to cart with quantity selection
- **User Authentication**: Identity-based authentication with role-based authorization
- **Responsive Design**: Bootstrap-powered UI that works on all devices

## Tech Stack 💻

- ASP.NET Core MVC
- Entity Framework Core
- SQLite Database
- Bootstrap 5
- ASP.NET Core Identity

## Getting Started ✅

### Prerequisites

- .NET 8.0 SDK or later
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/EasyGames.git
cd EasyGames/EasyGames
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Create and seed the database:
```bash
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

5. Open your browser and navigate to `https://localhost:5001`

## User Roles 🧑‍💼

- **Admin**: Can manage products (CRUD operations), no cart access
- **User**: Can browse products and use shopping cart functionality

## Project Structure 📁

- `Models/` - Data models and entities
- `Controllers/` - MVC controllers
- `Views/` - Razor view templates
- `Services/` - Business logic services
- `Data/` - Entity Framework context and migrations

## Database 🗄️

The application uses SQLite for simplicity. The database file (`easygames.db`) is created automatically when you run the first migration.
