# Person API

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)
[![API Version](https://img.shields.io/badge/API%20Version-v1.0-blue?style=for-the-badge)](https://localhost:5001/swagger)

A modern RESTful Web API for managing people data, built with ASP.NET Core and following clean architecture principles.

</div>

## 📋 Table of Contents

- [Features](#-features)
- [Project Structure](#-project-structure)
- [API Endpoints](#-api-endpoints)
- [Technologies](#-technologies)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Response Format](#-response-format)
- [Error Handling](#-error-handling)
- [Testing](#-testing)
- [Data Model](#-data-model)
- [Validation Rules](#-validation-rules)
- [Contributing](#-contributing)

## ✨ Features

- **RESTful API** - Full CRUD operations for person management
- **API Versioning** - URL segment, query string, and header-based versioning
- **OpenAPI Documentation** - Interactive API docs via Scalar UI at `/scalar`
- **Response Caching** - Built-in caching for improved performance
- **Health Checks** - Monitoring endpoint at `/health`
- **CORS Support** - Configurable cross-origin resource sharing
- **Fluent Validation** - Comprehensive request validation with meaningful error messages
- **Global Exception Handling** - Centralized error management middleware
- **Thread-Safe Data Store** - File-based storage with semaphore locking
- **Async/Await Pattern** - Fully asynchronous operations throughout

## 📁 Project Structure

```
├── public/
│   └── CommSchool.jpg          # Project logo
├── WebApplication/
│   ├── WebApplication.slnx      # Solution file
│   └── WebApplication/
│       ├── Controllers/
│       │   └── PersonController.cs
│       ├── DTOs/
│       │   ├── PersonCreateDto.cs
│       │   ├── PersonUpdateDto.cs
│       │   ├── PersonDto.cs
│       │   ├── PersonAddressCreateDto.cs
│       │   ├── PersonAddressDto.cs
│       │   ├── PersonFilter.cs
│       │   └── PagedResult.cs
│       ├── Extensions/
│       │   └── PersonExtensions.cs
│       ├── Mappings/
│       ├── Middleware/
│       │   └── ExceptionHandlingMiddleware.cs
│       ├── Models/
│       │   ├── Person.cs
│       │   └── PersonAddress.cs
│       ├── Options/
│       │   └── PersonStoreOptions.cs
│       ├── Properties/
│       │   └── launchSettings.json
│       ├── Responses/
│       │   └── ApiResponse.cs
│       ├── Services/
│       │   ├── Interfaces/
│       │   │   └── IPersonService.cs
│       │   └── PersonService.cs
│       ├── Validators/
│       │   ├── PersonCreateDtoValidator.cs
│       │   ├── PersonUpdateDtoValidator.cs
│       │   └── PersonAddressCreateDtoValidator.cs
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── people.json
│       ├── Program.cs
│       └── WebApplication.csproj
├── .gitignore
└── README.md
```

## 🎯 API Endpoints

### Person Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/person` | Create a new person |
| `GET` | `/api/v1/person` | Get all people (paginated, with filters) |
| `GET` | `/api/v1/person/{id}` | Get person by ID |
| `PUT` | `/api/v1/person/{id}` | Update person by ID |
| `DELETE` | `/api/v1/person/{id}` | Delete person by ID |

### Query Parameters (for GET all)

| Parameter | Type | Description |
|-----------|------|-------------|
| `page` | int | Page number (default: 1) |
| `pageSize` | int | Items per page (1-200, default: 10) |
| `minSalary` | decimal | Filter by minimum salary |
| `maxSalary` | decimal | Filter by maximum salary |
| `city` | string | Filter by city (case-insensitive) |

### Request/Response Examples

#### Create Person

```http
POST /api/v1/person
Content-Type: application/json

{
  "firstname": "John",
  "lastname": "Doe",
  "jobPosition": "Software Engineer",
  "salary": 75000,
  "workExperience": 5.5,
  "address": {
    "country": "Georgia",
    "city": "Tbilisi",
    "homeNumber": "123"
  }
}
```

#### Get All People (Paginated)

```http
GET /api/v1/person?page=1&pageSize=10&minSalary=50000&city=Tbilisi
```

Response:
```json
{
  "success": true,
  "data": {
    "data": [
      {
        "id": 1,
        "createDate": "2024-01-15T10:30:00Z",
        "firstname": "John",
        "lastname": "Doe",
        "jobPosition": "Software Engineer",
        "salary": 75000,
        "workExperience": 5.5,
        "address": {
          "country": "Georgia",
          "city": "Tbilisi",
          "homeNumber": "123"
        }
      }
    ],
    "page": 1,
    "pageSize": 10,
    "totalCount": 1,
    "totalPages": 1
  },
  "message": "Operation completed successfully",
  "errors": []
}
```

## 🛠 Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| **ASP.NET Core** | 10.0 | Web framework |
| **API Versioning** | 8.1.0 | API version management |
| **Scalar.AspNetCore** | 2.16.15 | OpenAPI documentation UI |
| **NSwag.AspNetCore** | 14.2.0 | OpenAPI generation |
| **FluentValidation** | 11.3.1 | Request validation |

## 🚀 Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension

### Installation

```bash
git clone https://github.com/GiorgiKavtaradze-prog/assignments.git
cd assignments/WebApplication/WebApplication
dotnet restore
dotnet run
```

### Running the Application

```bash
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`

### API Documentation

Once running, navigate to:
- **Scalar UI**: `https://localhost:5001/scalar`
- **OpenAPI JSON**: `https://localhost:5001/openapi/v1`

## ⚙️ Configuration

### appsettings.json

```json
{
  "PersonStore": {
    "FilePath": "people.json"
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://localhost:3000"
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables

| Variable | Description |
|----------|-------------|
| `ASPNETCORE_ENVIRONMENT` | Application environment (Development, Production) |
| `ASPNETCORE_URLS` | URLs to bind the server to |

## 📊 Response Format

All API responses follow a standardized format:

```json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully",
  "errors": []
}
```

### Paged Response

```json
{
  "data": [...],
  "page": 1,
  "pageSize": 10,
  "totalCount": 100,
  "totalPages": 10
}
```

## ❌ Error Handling

The API uses Problem Details (RFC 7807) for error responses:

```json
{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "Bad Request",
  "status": 400,
  "detail": "Validation failed",
  "instance": "/api/v1/person"
}
```

### HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request successful |
| `201` | Created - Resource created |
| `204` | No Content - Resource deleted |
| `400` | Bad Request - Validation error |
| `404` | Not Found - Resource not found |
| `499` | Client Closed Request - Request cancelled |
| `500` | Internal Server Error |

## 🧪 Testing

### Using curl

```bash
# Create a person
curl -X POST https://localhost:5001/api/v1/person \
  -H "Content-Type: application/json" \
  -d '{"firstname":"John","lastname":"Doe","jobPosition":"Developer","salary":60000,"workExperience":3,"address":{"country":"Georgia","city":"Tbilisi","homeNumber":"123"}}'

# Get all people
curl https://localhost:5001/api/v1/person

# Get person by ID
curl https://localhost:5001/api/v1/person/1

# Update person
curl -X PUT https://localhost:5001/api/v1/person/1 \
  -H "Content-Type: application/json" \
  -d '{"firstname":"Jane","lastname":"Smith","jobPosition":"Senior Developer","salary":80000,"workExperience":5,"address":{"country":"Georgia","city":"Batumi","homeNumber":"456"}}'

# Delete person
curl -X DELETE https://localhost:5001/api/v1/person/1
```

### Using the HTTP file

Use the included `WebApplication.http` file in VS Code with the REST Client extension.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📊 Data Model

### Person Entity

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `id` | int | Yes | Unique identifier |
| `createDate` | DateTime | Yes | Creation timestamp (defaults to UTC now) |
| `firstname` | string | Yes | First name (max 50 chars) |
| `lastname` | string | Yes | Last name (max 50 chars) |
| `jobPosition` | string | Yes | Job position (max 50 chars) |
| `salary` | decimal | Yes | Salary (0 - 100,000) |
| `workExperience` | double | Yes | Years of experience (≥ 0) |
| `address` | PersonAddress | Yes | Address details |

### Address Entity

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `country` | string | Yes | Country name |
| `city` | string | Yes | City name |
| `homeNumber` | string | Yes | Home/apartment number |

## ✅ Validation Rules

| Field | Rule |
|-------|------|
| `Firstname` | Required, max 50 characters |
| `Lastname` | Required, max 50 characters |
| `JobPosition` | Required, max 50 characters |
| `Salary` | Between 0 and 100,000 |
| `WorkExperience` | Greater than or equal to 0 |
| `CreateDate` | Cannot be in the future |
| `Address` | Required, all nested fields required |

## 📄 License

This project is part of an assignment and is licensed under the MIT License.
