# Recipe API

A simple ASP.NET Core Web API for managing recipes,
built with clean architecture principles and pursuing REST API best practices.

## Prerequisites

- .NET 8.0 SDK
- MongoDB (local or cloud instance)
- Docker (optional)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Lucasbc47/recipe-api/
cd Recipe.API
```

### 2. Configure MongoDB

Update the MongoDB connection string in `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RecipeDb",
    "CollectionName": "Recipes"
  }
}
```

### 3. Run the application

```bash
dotnet run
```

The API will be available at:

- API: https://localhost:5000
- Swagger UI: https://localhost:5000 (root)
- Health Check: https://localhost:500/health

## API Endpoints

### Recipes

| Method | Endpoint           | Description       |
| ------ | ------------------ | ----------------- |
| GET    | `/api/recipe`      | Get all recipes   |
| GET    | `/api/recipe/{id}` | Get recipe by ID  |
| POST   | `/api/recipe`      | Create new recipe |
| PUT    | `/api/recipe/{id}` | Update Recipe     |
| DELETE | `/api/recipe/{id}` | Delete Recipe     |

### Examples

**Request: Create Recipe:**

```json
POST /api/recipe
{
  "Name": "Spaghetti Carbonara",
  "Ingredients": ["pasta", "eggs", "bacon", "parmesan"],
  "Description": "Classic Italian pasta dish",
  "Steps": ["Boil pasta", "Cook bacon", "Mix with eggs"],
  "Images": [],
  "Videos": []
}
```

## Project Structure

```
Recipe.API/
├── Controllers/                    # API endpoints
│   ├── RecipeController.cs        # Recipe CRUD operations
│   └── ErrorController.cs         # Global error handling
├── Models/                        # Data models and DTOs
│   ├── Recipe.cs                  # Core Recipe entity
│   ├── RecipeDto.cs               # Data Transfer Objects
│   ├── BaseResponse.cs            # Response wrapper classes
│   └── MongoDbSettings.cs         # MongoDB configuration
├── Application/                   # Business logic layer
│   ├── Services/                  # Application services
│   │   └── RecipeService.cs       # Recipe business logic
│   └── Interfaces/                # Service contracts
│       └── IRecipeRepository.cs   # Repository interface
├── Infrastructure/                # Data access layer
│   └── Data/                     # Repository implementations
│       ├── RecipeRepository.cs    # MongoDB repository
│       └── MongoDbContext.cs      # MongoDB context
├── Filters/                      # Global filters
│   └── GlobalExceptionFilter.cs  # Exception handling
├── Properties/                   # Project properties
│   └── launchSettings.json       # Launch configurations
├── Program.cs                    # Application startup
├── appsettings.json              # Configuration
├── appsettings.Development.json  # Development settings
├── Recipe.API.csproj             # Project file
├── Dockerfile                    # Docker configuration
├── .dockerignore                 # Docker ignore rules
├── Recipe.API.http               # API testing file
└── README.md                     # Project documentation
```

## Development

### Running with Docker

```bash
docker build -t recipe-api .
docker run -p 5000:80 recipe-api
```

### Testing the API

Use the included `Recipe.API.http` file with VS Code REST Client extension or import into Postman.

### Health Check

Monitor application health at `/health` endpoint.

## Configuration

### Environment Variables

- `MongoDbSettings__ConnectionString`: MongoDB connection string
- `MongoDbSettings__DatabaseName`: Database name
- `MongoDbSettings__CollectionName`: Collection name

### Logging

Configured with different levels for development and production environments.

## License

MIT License
