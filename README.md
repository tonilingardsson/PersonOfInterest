# PersonOfInterest API

A simple ASP.NET Core Web API for managing people, their interests, and links connected to those interests.

## Assignment purpose

This project was built for a school assignment, .NET system developer at Campus Varberg (Sweden) where the goal is to create a REST API that can:

- Show all people in the database
- Show a person's interests
- Show links connected to a person's interests
- Connect a new interest to a person
- Add a new link for a person's interest

## Tech stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI

## Project structure

- `Models/` contains the entity classes
- `Data/` contains `AppDbContext`
- `Controllers/` contains the API controllers
- `Migrations/` contains Entity Framework migrations

## Database model

The project uses these entities:

- `Person`
- `Interest`
- `PersonInterest` (join table for many-to-many)
- `Link`

### Relationships

- A `Person` can have many interests
- An `Interest` can belong to many persons
- `PersonInterest` connects `Person` and `Interest`
- A `Link` belongs to one `Person` and one `Interest`

## Setup

### 1. Clone the repository

```bash
git clone <your-repository-url>
cd PersonOfInterest
```

### 2. Check connection string

Update `appsettings.json` with your SQL Server connection string.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonOfInterestDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Create database

Run these commands in Package Manager Console:

```powershell
Add-Migration InitialCreate
Update-Database
```

### 4. Run the application

Start the project from Visual Studio.

Swagger will open automatically, or use the local URL shown in the browser.

## JSON cycle handling

Because Entity Framework navigation properties can create circular references, the project uses JSON settings in `Program.cs`:

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
```

This prevents serialization errors when returning related data.

## API endpoints

### GET all persons

```http
GET /api/persons
```

Returns all persons.

### GET a person's interests

```http
GET /api/persons/{personId}/interests
```

Returns all interests connected to a specific person.

Example:

```http
GET /api/persons/1/interests
```

### GET a person's links

```http
GET /api/persons/{personId}/links
```

Returns all links connected to a specific person.

Example:

```http
GET /api/persons/1/links
```

### POST connect interest to person

```http
POST /api/persons/{personId}/interests
```

Example body:

```json
{
  "interestId": 2
}
```

### POST add link to person

```http
POST /api/persons/{personId}/links
```

Example body:

```json
{
  "url": "https://example.com",
  "interestId": 2
}
```

## Example seed data

The database can contain test data such as:

- Persons: Antonio, Beatrice, Cecilia
- Interests: Music, Coding, Reading
- Links connected to selected interests

## Testing the API

The API can be tested in Swagger.

Recommended test order:

1. `GET /api/persons`
2. `GET /api/persons/1/interests`
3. `GET /api/persons/1/links`
4. `POST /api/persons/1/interests`
5. `POST /api/persons/1/links`

## Notes

- The project uses Entity Framework Core code-first migrations.
- Seed data is added in `OnModelCreating`.
- Duplicate seed IDs will cause migration errors.
- Circular JSON references were handled in `Program.cs`.

## Author

Student project for school assignment.
