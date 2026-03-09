# Management User API

A RESTful API for user management built with ASP.NET Core 9, following clean architecture principles with vertical slice organization. Supports user CRUD operations, JWT authentication, Redis caching, RabbitMQ event publishing, and an Outbox pattern for reliable messaging.

## Architecture

```
management-user/
├── Domain/
│   └── User/
│       ├── User.cs               # User aggregate root
│       └── Enum/AccountStatus.cs # Active / Inactive enum
├── Features/
│   └── User/
│       ├── Create/               # POST /v1/users
│       ├── Delete/               # DELETE /v1/users/{id}
│       ├── GetAll/               # GET /v1/users
│       ├── GetById/              # GET /users/{id}
│       ├── Login/                # POST /v1/auth/login
│       └── Update/               # PUT /v1/users/{id}
├── Infrastructure/
│   ├── cache/                    # Redis cache service
│   ├── database/                 # EF Core (PostgreSQL), entities, mappers, repository
│   ├── queue/rabbit/             # RabbitMQ publisher
│   └── worker/outBox/            # Outbox processor & cleaner
└── Shared/
    ├── Interfaces/               # IUserRepository, ICacheService
    ├── JsonApi/                  # JSON:API response helpers and middleware
    ├── OutBox/                   # OutboxService
    ├── Result/                   # Result<T> monad
    ├── Security/                 # PasswordHasher (PBKDF2)
    └── Validation/               # IValidator, ValidationResult, ValidationError
```

Each feature follows the same pattern:
- **Request** — record with input fields
- **Validator** — implements `IValidator<TRequest>`, returns `ValidationResult`
- **Handler** — business logic, returns `Result<T>`
- **Endpoint** — maps route, validates, calls handler, returns JSON:API response
- **Response** — record with output fields

## Technology Stack

| Component         | Technology                          |
|-------------------|-------------------------------------|
| Framework         | ASP.NET Core 9 (Minimal APIs)       |
| Database          | PostgreSQL via EF Core 9 (Npgsql)   |
| Cache             | Redis (StackExchange)               |
| Message Broker    | RabbitMQ                            |
| Auth              | JWT Bearer tokens                   |
| API Docs          | NSwag (Swagger UI at `/docs`)       |
| Password Hashing  | PBKDF2-SHA256 (built-in .NET)       |
| Outbox Pattern    | Custom background workers           |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL
- Redis
- RabbitMQ

Or use Docker:

```bash
docker run -d -p 5432:5432 -e POSTGRES_DB=appdb -e POSTGRES_USER=app -e POSTGRES_PASSWORD=app postgres:16
docker run -d -p 6379:6379 redis:7
docker run -d -p 5672:5672 rabbitmq:3
```

## Configuration

`appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=appdb;Username=app;Password=app"
  },
  "Redis": {
    "Configuration": "localhost:6379",
    "InstanceName": "api_cache"
  },
  "Rabbitmq": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-that-is-at-least-32-characters-long",
    "Issuer": "management-user-api",
    "Audience": "management-user-api-clients",
    "ExpirationMinutes": 60
  }
}
```

> **Important:** Replace `Jwt:Secret` with a strong random key in production. Never commit secrets to source control.

## Running the API

```bash
# Apply database migrations
cd management-user
dotnet ef database update

# Run the API
dotnet run
```

API is available at `https://localhost:7xxx` (port shown in console output).
Swagger UI: `https://localhost:7xxx/docs`

## API Endpoints

### Authentication

#### POST /v1/auth/login
Authenticate a user and receive a JWT token.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-03-09T13:00:00Z"
}
```

---

### Users

All user responses follow [JSON:API](https://jsonapi.org/) format (`Content-Type: application/vnd.api+json`).

#### POST /v1/users — Create User

**Request:**
```json
{
  "name": "John Doe",
  "cpf": "12345678901",
  "email": "john@example.com",
  "password": "password123"
}
```

**Response (201):**
```json
{
  "data": {
    "type": "users",
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "attributes": {
      "name": "John Doe",
      "cpf": "12345678901",
      "accountStatusCode": 0,
      "accountStatus": "Active"
    }
  }
}
```

#### GET /users/{id} — Get User by ID

**Response (200):** JSON:API user object.

#### GET /v1/users — Get Users with Filters

**Query parameters:** `name`, `cpf`, `page` (default 1), `pageSize` (default 10)

**Response (200):** JSON:API collection of users.

#### PUT /v1/users/{id} — Update User

**Request:**
```json
{
  "name": "John Updated",
  "cpf": "12345678901",
  "accountStatus": 1
}
```

**Response (200):** JSON:API updated user object.

#### DELETE /v1/users/{id} — Delete User

**Response (204):** No content.

---

### Error Responses

All errors follow JSON:API error format:

```json
{
  "errors": [
    {
      "status": "400",
      "title": "Validation Error",
      "detail": "Name is required"
    }
  ]
}
```

## AccountStatus Values

| Value | Name     |
|-------|----------|
| 0     | Active   |
| 1     | Inactive |

## Caching

User data is cached in Redis with a 30-minute TTL. Cache is:
- **Set** on create, update, and get-by-id
- **Invalidated** on delete

## Outbox Pattern

Write operations publish domain events (`user.created`, `user.updated`, `user.deleted`) to an outbox table within the same database transaction. Background workers (`OutboxProcessor`, `OutboxCleaner`) relay messages to RabbitMQ and clean up processed entries.

## Running Tests

```bash
cd management-user-tests
dotnet test
```

The test suite covers:

| Category   | Tests                                                   |
|------------|---------------------------------------------------------|
| Handlers   | CreateUser, GetUserById, GetAllUsers, Login             |
| Validators | CreateUser, UpdateUser, Login                          |
| Mapper     | UserMapper (ToEntity, ToDomain, round-trip)             |
| Security   | PasswordHasher (hash, verify, tamper detection)         |

## Project Structure

```
management-user-api/
├── management-user/          # Main API project
├── management-user-tests/    # xUnit test project
└── management-user-api.sln   # Solution file
```
