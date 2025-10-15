# Architecture Documentation

[English](#english) | [Polski](#polski)

---

## English

## System Architecture

This document describes the architecture, design patterns, and technical decisions used in the Crispy Barnacle sample application.

### Overview

The application follows a **microservices architecture** with clear separation between frontend and backend services. Each service has a specific responsibility and communicates via HTTP/REST APIs.

```
┌─────────────────────────────────────────────────┐
│                   Client Browser                │
└────────────────────┬────────────────────────────┘
                     │ HTTPS
                     │
┌────────────────────▼────────────────────────────┐
│            Frontend (Blazor Server)             │
│  ┌──────────────────────────────────────────┐  │
│  │  Razor Components (.razor files)         │  │
│  │  - FetchData.razor (Weather Display)     │  │
│  │  - Contact.razor (Contact Form)          │  │
│  └──────────────────┬───────────────────────┘  │
│                     │                           │
│  ┌──────────────────▼───────────────────────┐  │
│  │  HTTP Clients (Data/)                    │  │
│  │  - WeatherForecastClient                 │  │
│  │  - ContactClient                         │  │
│  └──────────────────┬───────────────────────┘  │
└────────────────────┬┴───────────────────────────┘
                     │ HTTP/REST API
                     │
┌────────────────────▼────────────────────────────┐
│             Backend (ASP.NET Core)              │
│  ┌──────────────────────────────────────────┐  │
│  │  API Endpoints (Program.cs)              │  │
│  │  - Minimal APIs                          │  │
│  │  - OpenAPI Documentation                 │  │
│  └──────────────────┬───────────────────────┘  │
│                     │                           │
│  ┌──────────────────▼───────────────────────┐  │
│  │  Business Logic                          │  │
│  │  - Validation (Validation/)              │  │
│  │  - Domain Models (Models/)               │  │
│  └──────────────────┬───────────────────────┘  │
│                     │                           │
│  ┌──────────────────▼───────────────────────┐  │
│  │  Data Layer (In-Memory)                  │  │
│  │  - Products, Users, Orders, Contacts     │  │
│  └──────────────────────────────────────────┘  │
└─────────────────────────────────────────────────┘
```

### Architecture Patterns

#### 1. Microservices Architecture

**Services:**
- **Frontend Service**: Blazor Server application responsible for user interface
- **Backend Service**: ASP.NET Core Web API responsible for business logic and data

**Benefits:**
- Independent deployment and scaling
- Technology independence for each service
- Clear separation of concerns
- Easier maintenance and testing

#### 2. Minimal API Pattern

The backend uses ASP.NET Core Minimal APIs for a lightweight, functional approach:

```csharp
app.MapGet("/api/products", () => Results.Ok(products))
   .WithName("GetAllProducts")
   .WithOpenApi();
```

**Benefits:**
- Less boilerplate code
- Faster development
- Better performance
- Easy to understand and maintain

#### 3. DTO (Data Transfer Object) Pattern

Domain models are separated from DTOs to:
- Exclude sensitive data (e.g., password hashes)
- Control API response structure
- Decouple internal models from external contracts

**Example:**
```csharp
// Domain Model
public class User {
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }  // Sensitive!
}

// DTO
public class UserDto {
    public int Id { get; set; }
    public string Username { get; set; }
    // No password hash exposed
}
```

#### 4. Repository Pattern (In-Memory)

For this sample, data is stored in-memory using simple lists:
```csharp
var products = new List<Product>();
var users = new List<User>();
var orders = new List<Order>();
```

**Note:** In production, this would be replaced with:
- Entity Framework Core
- Dapper
- Direct database access
- External data services

### Component Architecture

#### Backend Components

**1. Program.cs**
- Application entry point
- Service configuration
- Endpoint registration
- Middleware configuration

**2. Models/**
- `Product.cs` - Product domain model
- `User.cs` - User domain model with authentication
- `Order.cs` - Order and OrderItem models with business logic
- `OrderStatus` - Enum for order lifecycle

**3. Models/Dto/**
- `UserDto.cs` - Safe user representation
- `OrderDto.cs` - Order response format
- `ContactDto.cs` - Contact message structure

**4. Models/Validation/**
- `OrderValidation.cs` - Order validation rules and status transitions

#### Frontend Components

**1. Program.cs**
- Application configuration
- HTTP client setup with base URLs
- Dependency injection

**2. Pages/**
- `FetchData.razor` - Weather forecast display
- `Contact.razor` - Contact form with validation
- `_Host.cshtml` - App shell

**3. Data/**
- `WeatherForecastClient.cs` - HTTP client for weather API
- `ContactClient.cs` - HTTP client for contact API
- `WeatherForecast.cs` - Weather data model

**4. Shared/**
- Shared Blazor components
- Layout components

### Communication Flow

#### Example: Submitting Contact Form

```
1. User fills form in Contact.razor
   ↓
2. Form validation (DataAnnotations)
   ↓
3. ContactClient.SendAsync() called
   ↓
4. HTTP POST to Backend /api/contact
   ↓
5. Backend validates and stores message
   ↓
6. Returns 201 Created with ContactDto
   ↓
7. Frontend shows success message
```

#### Example: Displaying Weather Forecast

```
1. FetchData.razor loads (OnInitializedAsync)
   ↓
2. WeatherForecastClient.GetForecastAsync() called
   ↓
3. HTTP GET to Backend /weatherforecast
   ↓
4. Backend generates random forecast data
   ↓
5. Returns array of WeatherForecast
   ↓
6. Frontend displays in table
```

### Data Flow

```
User Input → Validation → HTTP Request → API Endpoint → 
Business Logic → Data Storage → Response DTO → 
HTTP Response → Client Processing → UI Update
```

### Configuration Management

#### Backend Configuration

**appsettings.json:**
- Logging levels
- Application settings
- Environment-specific values

**appsettings.Development.json:**
- Development overrides
- Debug settings

#### Frontend Configuration

**appsettings.json:**
- `WEATHER_URL`: Backend API URL for weather
- `API_URL`: Backend API base URL

**Environment Variables:**
Can override configuration values for different environments.

### Validation Strategy

#### Client-Side Validation (Frontend)
- DataAnnotations attributes
- Blazor EditForm validation
- Real-time feedback

**Example:**
```csharp
[Required]
[EmailAddress]
public string Email { get; set; }
```

#### Server-Side Validation (Backend)
- Business rule validation
- Custom validation logic
- OrderValidation class for complex rules

**Example:**
```csharp
public static List<string> ValidateOrder(Order order)
{
    var errors = new List<string>();
    
    if (string.IsNullOrWhiteSpace(order.CustomerName))
        errors.Add("Customer name is required");
        
    if (!order.Items.Any())
        errors.Add("Order must contain at least one item");
        
    return errors;
}
```

### Security Considerations

#### Current Implementation (Demo)
- In-memory data storage (not persistent)
- No authentication/authorization
- Password hashes stored but not verified
- No HTTPS enforcement in development

#### Production Recommendations
- Implement JWT authentication
- Add authorization policies
- Use HTTPS only
- Implement rate limiting
- Add CORS policies
- Use secrets management (Azure Key Vault, etc.)
- Add SQL injection protection
- Implement input sanitization
- Use Entity Framework for SQL safety

### API Documentation

The application uses **OpenAPI/Swagger** with **Scalar UI** for interactive API documentation:

**Features:**
- Automatic endpoint discovery
- Request/response examples
- Interactive testing
- Type definitions
- Error responses

**Access:**
Navigate to `/scalar` when backend is running.

### Error Handling

#### Backend
- Returns appropriate HTTP status codes
- Structured error responses
- Validation error details

**Example:**
```csharp
if (validationErrors.Any())
{
    return Results.BadRequest(new { errors = validationErrors });
}
```

#### Frontend
- Try-catch blocks in HTTP clients
- User-friendly error messages
- Success/failure notifications

### Performance Considerations

#### Blazor Server Benefits
- Small download size (no large SPA bundle)
- Server-side rendering
- Fast initial load

#### Blazor Server Considerations
- Requires SignalR connection
- Server resources per user
- Network latency for interactions

#### API Performance
- Minimal APIs for reduced overhead
- In-memory storage for fast access
- No database query overhead (in this demo)

### Scalability

#### Current Architecture
- Single instance design
- In-memory state (not scalable)

#### Production Scaling Recommendations
- Use external database (SQL Server, PostgreSQL)
- Implement caching (Redis)
- Use load balancer
- Consider Blazor WebAssembly for reduced server load
- Implement message queue for async operations
- Use CDN for static assets

### Testing Strategy

#### Recommended Testing Approach

**Unit Tests:**
- Business logic validation
- Domain model behavior
- DTO conversions

**Integration Tests:**
- API endpoint testing
- HTTP client behavior
- End-to-end flows

**UI Tests:**
- Blazor component testing
- Form validation
- User interactions

### Deployment Architecture

#### Local Development
```
localhost:5001 (Backend)
localhost:5002 (Frontend)
```

#### GitHub Codespaces
```
Forwarded Port (Backend)
Forwarded Port (Frontend)
```

#### Production (Recommended)
```
Azure App Service / Container Apps
- Backend API
- Frontend App
- Azure SQL Database
- Application Insights
```

### Technology Choices

#### Why .NET 9.0?
- Latest performance improvements
- Modern C# features
- Minimal API support
- Native AOT compilation support

#### Why Blazor Server?
- C# for both frontend and backend
- Real-time UI updates via SignalR
- Server-side rendering
- Smaller client-side footprint

#### Why Minimal APIs?
- Reduced boilerplate
- Performance benefits
- Modern development approach
- Easy to learn

#### Why Scalar?
- Modern API documentation UI
- Better UX than Swagger UI
- Interactive testing
- Built-in request examples

---

## Polski

## Architektura Systemu

Ten dokument opisuje architekturę, wzorce projektowe i decyzje techniczne zastosowane w przykładowej aplikacji Crispy Barnacle.

### Przegląd

Aplikacja wykorzystuje **architekturę mikroserwisów** z wyraźnym rozdziałem między serwisami frontend i backend. Każdy serwis ma określoną odpowiedzialność i komunikuje się za pomocą HTTP/REST API.

### Wzorce Architektoniczne

#### 1. Architektura Mikroserwisów

**Serwisy:**
- **Serwis Frontendowy**: Aplikacja Blazor Server odpowiedzialna za interfejs użytkownika
- **Serwis Backendowy**: ASP.NET Core Web API odpowiedzialne za logikę biznesową i dane

**Korzyści:**
- Niezależne wdrażanie i skalowanie
- Niezależność technologiczna dla każdego serwisu
- Wyraźne rozdzielenie odpowiedzialności
- Łatwiejsza konserwacja i testowanie

#### 2. Wzorzec Minimal API

Backend wykorzystuje ASP.NET Core Minimal APIs dla lekkiego, funkcjonalnego podejścia.

#### 3. Wzorzec DTO (Data Transfer Object)

Modele domenowe są oddzielone od DTO aby:
- Wykluczyć wrażliwe dane (np. hashe haseł)
- Kontrolować strukturę odpowiedzi API
- Oddzielić modele wewnętrzne od kontraktów zewnętrznych

#### 4. Wzorzec Repository (W Pamięci)

W tej przykładowej aplikacji dane są przechowywane w pamięci przy użyciu prostych list.

**Uwaga:** W produkcji zostałoby to zastąpione:
- Entity Framework Core
- Dapper
- Bezpośredni dostęp do bazy danych
- Zewnętrzne serwisy danych

### Komponenty

Szczegółowy opis komponentów znajduje się w sekcji angielskiej powyżej.

### Przepływ Komunikacji

Szczegółowe przykłady przepływu znajdują się w sekcji angielskiej powyżej.

### Strategia Walidacji

#### Walidacja po Stronie Klienta (Frontend)
- Atrybuty DataAnnotations
- Walidacja Blazor EditForm
- Natychmiastowa informacja zwrotna

#### Walidacja po Stronie Serwera (Backend)
- Walidacja reguł biznesowych
- Niestandardowa logika walidacji
- Klasa OrderValidation dla złożonych reguł

### Kwestie Bezpieczeństwa

#### Aktualna Implementacja (Demo)
- Przechowywanie danych w pamięci (nietrwałe)
- Brak uwierzytelniania/autoryzacji
- Hashe haseł przechowywane ale nie weryfikowane
- Brak wymuszania HTTPS w środowisku deweloperskim

#### Rekomendacje dla Produkcji
- Implementacja uwierzytelniania JWT
- Dodanie polityk autoryzacji
- Użycie tylko HTTPS
- Implementacja ograniczania częstotliwości żądań
- Dodanie polityk CORS
- Użycie zarządzania sekretami (Azure Key Vault, itp.)
- Dodanie ochrony przed SQL injection
- Implementacja sanityzacji danych wejściowych

### Dokumentacja API

Aplikacja używa **OpenAPI/Swagger** z interfejsem **Scalar UI** dla interaktywnej dokumentacji API.

### Wydajność

Szczegółowe informacje o wydajności znajdują się w sekcji angielskiej powyżej.

### Skalowalność

#### Obecna Architektura
- Projekt jednoinstancyjny
- Stan w pamięci (nieskalowalny)

#### Rekomendacje Skalowania dla Produkcji
- Użycie zewnętrznej bazy danych (SQL Server, PostgreSQL)
- Implementacja cache'owania (Redis)
- Użycie load balancera
- Rozważenie Blazor WebAssembly dla zmniejszenia obciążenia serwera
- Implementacja kolejki komunikatów dla operacji asynchronicznych
- Użycie CDN dla zasobów statycznych

### Wybory Technologiczne

#### Dlaczego .NET 9.0?
- Najnowsze ulepszenia wydajności
- Nowoczesne funkcje C#
- Wsparcie Minimal API
- Wsparcie natywnej kompilacji AOT

#### Dlaczego Blazor Server?
- C# zarówno dla frontendu jak i backendu
- Aktualizacje UI w czasie rzeczywistym przez SignalR
- Renderowanie po stronie serwera
- Mniejszy ślad po stronie klienta

#### Dlaczego Minimal APIs?
- Zredukowany boilerplate
- Korzyści wydajnościowe
- Nowoczesne podejście do rozwoju
- Łatwe do nauczenia

#### Dlaczego Scalar?
- Nowoczesny interfejs dokumentacji API
- Lepszy UX niż Swagger UI
- Interaktywne testowanie
- Wbudowane przykłady żądań
