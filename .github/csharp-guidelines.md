# C# Coding Guidelines

## Naming Conventions

### PascalCase
- Classes: `OrderService`, `ProductController`
- Methods: `GetOrderById()`, `CalculateTotal()`
- Properties: `Id`, `OrderDate`
- Interfaces: `IOrderRepository`
- Enums: `OrderStatus`
```csharp
public class OrderService
{
    public decimal CalculateTotal(Order order)
    {
        // Implementation
    }
}
```

### camelCase
- Local variables: `orderTotal`, `customerName`
- Method parameters: `orderId`, `updateRequest`
- Private fields: `_orderRepository`, `_logger`
```csharp
public void ProcessOrder(int orderId, OrderRequest orderRequest)
{
    var currentUser = GetCurrentUser();
    decimal orderTotal = _orderService.CalculateTotal(orderRequest);
}
```

## Model Structure

### Domain Models
- Located in `Models/` folder
- Use nullable reference types
- Include XML documentation
- Implement validation logic
```csharp
/// <summary>
/// Represents an order in the system
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
}
```

### DTOs (Data Transfer Objects)
- Located in `Models/Dto/` folder
- Include JSON attributes
- Exclude sensitive data
- Use proper number handling
```csharp
public class OrderDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("totalAmount")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public decimal TotalAmount { get; set; }
}
```

## API Patterns

### Minimal API Endpoints
- Group by functionality
- Use meaningful route names
- Include OpenAPI documentation
- Return proper status codes
```csharp
app.MapGet("/api/orders/{id}", (int id) =>
{
    var order = await _orderService.GetByIdAsync(id);
    if (order == null)
        return Results.NotFound();
    return Results.Ok(order.ToDto());
})
.WithName("GetOrderById")
.WithDescription("Retrieves an order by ID")
.WithOpenApi()
.WithTags("Orders");
```

### Validation
- Use dedicated validation classes
- Return descriptive error messages
- Validate at domain model level
```csharp
public static class OrderValidation
{
    public static List<string> ValidateOrder(Order order)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrEmpty(order.CustomerName))
            errors.Add("Customer name is required");
            
        if (!order.Items.Any())
            errors.Add("Order must contain at least one item");
            
        return errors;
    }
}
```

## Error Handling
- Use consistent error response structure
- Include appropriate HTTP status codes
- Provide meaningful error messages
```csharp
app.MapPost("/api/orders", (Order order) =>
{
    var errors = OrderValidation.ValidateOrder(order);
    if (errors.Any())
        return Results.BadRequest(new { errors });
        
    // Process valid order
    return Results.Created($"/api/orders/{order.Id}", order.ToDto());
});
```

## Async/Await Practices
- Use async/await consistently
- Include proper cancellation token support
- Name async methods with "Async" suffix
```csharp
public async Task<Order?> GetOrderByIdAsync(
    int id, 
    CancellationToken cancellationToken = default)
{
    return await _context.Orders
        .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
}