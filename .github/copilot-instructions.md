# AI Agent Instructions for .NET Sample App

## Project Structure
This is a .NET 9.0 solution with a microservices architecture consisting of:
- `BackEnd/` - ASP.NET Core Web API with minimal APIs
- `FrontEnd/` - Blazor Server web application
- Both projects use nullable reference types and implicit usings

## Architecture Overview

### Service Boundaries
- **Backend API Service** (`BackEnd/`):
  - Handles data persistence and business logic
  - Exposes RESTful endpoints using minimal APIs
  - Uses in-memory storage for demonstration
  - Swagger/OpenAPI documentation with Scalar UI

- **Frontend Application** (`FrontEnd/`):
  - Blazor Server web interface
  - Communicates with backend via HTTP
  - Configuration-based service URLs
  - Bootstrap-based responsive design

### Cross-Component Communication
1. **HTTP Client Configuration**:
   ```csharp
   builder.Services.AddHttpClient<WeatherForecastClient>(c =>
   {
       var url = builder.Configuration["WEATHER_URL"] 
           ?? throw new InvalidOperationException("WEATHER_URL is not set");
       c.BaseAddress = new(url);
   });
   ```

2. **Service Integration Points**:
   - Backend exposes OpenAPI endpoints
   - Frontend consumes APIs via typed HttpClient services
   - Environment-specific service URLs in configuration

## Code Conventions

### Naming Conventions
1. **PascalCase** for:
   - Classes: `ProductController`, `OrderService`
   - Methods: `GetAllProducts()`, `CreateOrder()`
   - Properties: `Id`, `CustomerName`
   - Interfaces: `IProductRepository`
   - Enums: `OrderStatus`

2. **camelCase** for:
   - Local variables: `productId`, `currentUser`
   - Parameters: `userId`, `orderDetails`
   - Private fields: `_orderRepository`

### DTO Patterns
Data Transfer Objects should use JSON attributes for proper serialization:

```csharp
public class ProductDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("productName")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("unitPrice")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public decimal Price { get; set; }
}
```

### Code Documentation
Use clear, concise comments for:
1. Public API methods and classes
2. Complex business logic
3. Non-obvious implementation details

Example:
```csharp
// Calculates the total order amount including applicable discounts
public decimal CalculateOrderTotal(Order order)
{
    // Apply bulk discount for orders over 10 items
    var baseTotal = order.Items.Sum(item => item.Price * item.Quantity);
    var discount = order.Items.Count > 10 ? 0.1m : 0m;
    
    return baseTotal * (1 - discount);
}
```

## Domain Models and DTOs
The application implements an e-commerce domain with clear separation between domain models and DTOs:

### Domain Models
Located in `BackEnd/Models/`:
```csharp
/// <summary>
/// Represents an order in the system with customer information and ordered items
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(item => item.Quantity * item.UnitPrice);
    public OrderStatus Status { get; set; }
    
    // Domain model to DTO conversion
    public OrderDto ToDto()
    {
        return new OrderDto
        {
            Id = Id,
            CustomerName = CustomerName,
            Items = Items.Select(item => item.ToDto()).ToList()
        };
    }
}
```

### Data Transfer Objects (DTOs)
Located in `BackEnd/Models/Dto/`:
```csharp
/// <summary>
/// Data transfer object representing an order in the API
/// </summary>
public class OrderDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("items")]
    public List<OrderItemDto> Items { get; set; } = new();

    [JsonPropertyName("totalAmount")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public decimal TotalAmount { get; set; }
}
```

### Model Documentation
- Use XML comments for all public types and members
- Document domain logic and business rules
- Explain non-obvious implementation details
- Example:
```csharp
/// <summary>
/// Defines the possible states of an order in the system
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order has been created but not yet confirmed
    /// </summary>
    Pending,
    
    /// <summary>
    /// Order has been confirmed and is awaiting shipment
    /// </summary>
    Confirmed
}
```

## Key Components

### Backend API (BackEnd/)
- Uses Minimal API pattern for REST endpoints
- OpenAPI/Swagger integration with Scalar UI
- Models located in `Models/` directory
- In-memory data storage for sample implementation
- Example endpoints in `Program.cs` demonstrate REST patterns

### Frontend (FrontEnd/)
- Blazor Server architecture
- Pages in `Pages/` directory
- Shared components in `Shared/`
- HTTP client services in `Data/`
- CSS styles in `wwwroot/css/`

## Development Workflow

### Building
```bash
# Build both projects
dotnet build SampleApp/SampleApp.sln

# Build individual projects
dotnet build SampleApp/BackEnd/BackEnd.csproj
dotnet build SampleApp/FrontEnd/FrontEnd.csproj
```

### Running
Available VS Code tasks:
- `build frontend` / `build backend` - Build projects
- `watch frontend` / `watch backend` - Run with hot reload
- `publish frontend` / `publish backend` - Create production builds

### Debugging
1. Backend API:
   - Swagger UI available in development at `/swagger`
   - Scalar API Reference at `/api-reference`
   - Configure using `appsettings.Development.json`

2. Frontend:
   - Hot reload enabled for Razor/Blazor files
   - Error page at `/Error`
   - Static files served from `wwwroot/`

### API Development Patterns
1. Models and DTOs:
   - Domain models in `BackEnd/Models/`
   - DTOs in `BackEnd/Models/Dto/`
   - Use `ToDto()` methods for model conversion
   - Keep sensitive data out of DTOs

2. API Response Structure:
   ```csharp
   app.MapGet("/api/users", () =>
   {
       // Convert domain models to DTOs
       var userDtos = users.Select(user => new UserDto
       {
           Id = user.Id,
           Username = user.Username
       }).ToList();

       // Return 200 OK with DTOs
       return Results.Ok(userDtos);
   })
   .WithName("GetAllUsers")
   .WithOpenApi()
   .WithDescription("Retrieves all users")
   .WithTags("Users");
   ```

3. Endpoint Documentation:
   - Use meaningful route names with `WithName()`
   - Add descriptions with `WithDescription()`
   - Group related endpoints with `WithTags()`
   - Enable OpenAPI/Swagger documentation

Example endpoint patterns:

```csharp
// GET collection endpoint
app.MapGet("/api/products", () =>
{
    return Results.Ok(products);
})
.WithName("GetAllProducts")
.WithOpenApi();

// GET single resource endpoint
app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
        return Results.NotFound();
    return Results.Ok(product);
})
.WithName("GetProductById")
.WithOpenApi();

// POST new resource endpoint
app.MapPost("/api/products", (Product product) =>
{
    product.Id = products.Max(p => p.Id) + 1;
    products.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
})
.WithName("CreateProduct")
.WithOpenApi();
```

### Frontend Integration
1. Define data models in `Data/` folder
2. Create HTTP clients inheriting from `HttpClient`
3. Register services in `Program.cs`
4. Use `@inject` in Blazor components

## Project Conventions
- Use nullable reference types
- Prefer minimal APIs over controllers
- Follow REST API naming conventions
- Use async/await for HTTP operations
- Utilize OpenAPI documentation

## Key Files
- `BackEnd/Program.cs` - API setup and endpoints
- `FrontEnd/Program.cs` - Blazor configuration
- `BackEnd/Models/` - API data models
- `FrontEnd/Data/` - Frontend service clients
- `FrontEnd/Pages/` - Blazor pages

## Dependencies
Backend:
- Microsoft.AspNetCore.OpenApi
- Scalar.AspNetCore (for API documentation)

Frontend:
- Standard ASP.NET Core dependencies
- Bootstrap for styling