using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using BackEnd.Models;
using BackEnd.Models.Dto;
using BackEnd.Models.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // current workaround for port forwarding in codespaces
    // https://github.com/dotnet/aspnetcore/issues/57332
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Servers = [];
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Sample in-memory products list
var products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 1299.99m, StockQuantity = 10 },
    new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone model", Price = 799.99m, StockQuantity = 15 }
};

// GET all products
app.MapGet("/api/products", () =>
{
    return Results.Ok(products);
})
.WithName("GetAllProducts")
.WithOpenApi();

// GET product by id
app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
        return Results.NotFound();
    
    return Results.Ok(product);
})
.WithName("GetProductById")
.WithOpenApi();

// POST new product
app.MapPost("/api/products", (Product product) =>
{
    product.Id = products.Max(p => p.Id) + 1;
    products.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
})
.WithName("CreateProduct")
.WithOpenApi();

// PUT update product
app.MapPut("/api/products/{id}", (int id, Product updatedProduct) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
        return Results.NotFound();
    
    product.Name = updatedProduct.Name;
    product.Description = updatedProduct.Description;
    product.Price = updatedProduct.Price;
    product.StockQuantity = updatedProduct.StockQuantity;
    
    return Results.Ok(product);
})
.WithName("UpdateProduct")
.WithOpenApi();

// DELETE product
app.MapDelete("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
        return Results.NotFound();
    
    products.Remove(product);
    return Results.NoContent();
})
.WithName("DeleteProduct")
.WithOpenApi();

// Sample in-memory users list
var users = new List<User>
{
    new User 
    { 
        Id = 1, 
        Username = "john.doe", 
        Email = "john.doe@example.com",
        PasswordHash = "hashed_password_1",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    }
};

// Sample in-memory orders list
var orders = new List<Order>();
// In-memory storage for contact messages
var contacts = new List<BackEnd.Models.Dto.ContactDto>();

// GET all users
// Returns a list of all users with sensitive data excluded
app.MapGet("/api/users", () =>
{
    // Convert domain models to DTOs, excluding sensitive information
    var userDtos = users.Select(user => new UserDto
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    }).ToList();

    // Return 200 OK with the list of users
    return Results.Ok(userDtos);
})
.WithName("GetAllUsers")
.WithOpenApi()
.WithDescription("Retrieves all users with pagination")
.WithTags("Users");

// GET user by id
app.MapGet("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();
    
    var safeUser = new
    {
        user.Id,
        user.Username,
        user.Email,
        user.CreatedAt,
        user.UpdatedAt
    };
    
    return Results.Ok(safeUser);
})
.WithName("GetUserById")
.WithOpenApi();

// POST new user
app.MapPost("/api/users", (User user) =>
{
    // Validate email and username
    if (users.Any(u => u.Email == user.Email))
        return Results.BadRequest("Email already exists");
    
    if (users.Any(u => u.Username == user.Username))
        return Results.BadRequest("Username already exists");
    
    user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
    user.CreatedAt = DateTime.UtcNow;
    user.UpdatedAt = DateTime.UtcNow;
    
    users.Add(user);
    
    var safeUser = new
    {
        user.Id,
        user.Username,
        user.Email,
        user.CreatedAt,
        user.UpdatedAt
    };
    
    return Results.Created($"/api/users/{user.Id}", safeUser);
})
.WithName("CreateUser")
.WithOpenApi();

// PUT update user
app.MapPut("/api/users/{id}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();
    
    // Validate email and username if they are being changed
    if (user.Email != updatedUser.Email && users.Any(u => u.Email == updatedUser.Email))
        return Results.BadRequest("Email already exists");
    
    if (user.Username != updatedUser.Username && users.Any(u => u.Username == updatedUser.Username))
        return Results.BadRequest("Username already exists");
    
    user.Username = updatedUser.Username;
    user.Email = updatedUser.Email;
    user.UpdatedAt = DateTime.UtcNow;
    
    // Only update password if provided
    if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
        user.PasswordHash = updatedUser.PasswordHash;
    
    var safeUser = new
    {
        user.Id,
        user.Username,
        user.Email,
        user.CreatedAt,
        user.UpdatedAt
    };
    
    return Results.Ok(safeUser);
})
.WithName("UpdateUser")
.WithOpenApi();

// DELETE user
app.MapDelete("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();
    
    users.Remove(user);
    return Results.NoContent();
})
.WithName("DeleteUser")
.WithOpenApi();

// POST new order
app.MapPost("/api/orders", (Order order) =>
{
    // Validate order
    var validationErrors = OrderValidation.ValidateOrder(order);
    if (validationErrors.Any())
    {
        return Results.BadRequest(new { errors = validationErrors });
    }

    // Set order properties
    order.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
    order.OrderDate = DateTime.UtcNow;
    order.Status = OrderStatus.Pending;
    order.UpdatedAt = DateTime.UtcNow;

    orders.Add(order);
    return Results.Created($"/api/orders/{order.Id}", order.ToDto());
})
.WithName("CreateOrder")
.WithDescription("Creates a new order with validation")
.WithOpenApi()
.WithTags("Orders");

// PUT update order status
app.MapPut("/api/orders/{id}/status", (int id, OrderStatus newStatus) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null)
        return Results.NotFound();

    // Validate status transition
    if (!OrderValidation.IsValidStatusTransition(order.Status, newStatus))
    {
        return Results.BadRequest(new { error = $"Invalid status transition from {order.Status} to {newStatus}" });
    }

    order.Status = newStatus;
    order.UpdatedAt = DateTime.UtcNow;

    return Results.Ok(order.ToDto());
})
.WithName("UpdateOrderStatus")
.WithDescription("Updates order status with validation of allowed transitions")
.WithOpenApi()
.WithTags("Orders");

// GET order by id
app.MapGet("/api/orders/{id}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null)
        return Results.NotFound();

    return Results.Ok(order.ToDto());
})
.WithName("GetOrderById")
.WithDescription("Retrieves order by ID")
.WithOpenApi()
.WithTags("Orders");

// GET all orders
app.MapGet("/api/orders", () =>
{
    return Results.Ok(orders.Select(o => o.ToDto()));
})
.WithName("GetAllOrders")
.WithDescription("Retrieves all orders")
.WithOpenApi()
.WithTags("Orders");

// POST contact message
app.MapPost("/api/contact", (BackEnd.Models.Dto.ContactDto contact) =>
{
    contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
    contact.CreatedAt = DateTime.UtcNow;
    contacts.Add(contact);
    return Results.Created($"/api/contact/{contact.Id}", contact);
})
.WithName("CreateContact")
.WithDescription("Submit a contact message")
.WithOpenApi()
.WithTags("Contact");

System.Console.WriteLine("Testing");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
