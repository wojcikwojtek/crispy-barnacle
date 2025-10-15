# Blazor Development Guidelines

## Component Structure

### Component Organization
- Pages in `Pages/` folder
- Shared components in `Shared/` folder
- Use partial classes for complex components
```razor
@page "/orders/{id:int}"

<h1>Order Details</h1>

@if (order == null)
{
    <Loading />
}
else
{
    <OrderSummary Order="@order" />
}

@code {
    [Parameter]
    public int Id { get; set; }

    private OrderDto? order;

    protected override async Task OnParametersSetAsync()
    {
        order = await OrderService.GetOrderAsync(Id);
    }
}
```

## Data Management

### HTTP Clients
- Define typed HTTP clients in `Data/` folder
- Use dependency injection
- Handle errors consistently
```csharp
public class OrderClient
{
    private readonly HttpClient _httpClient;

    public OrderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderDto?> GetOrderAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<OrderDto>($"api/orders/{id}");
        }
        catch (HttpRequestException)
        {
            // Handle error appropriately
            return null;
        }
    }
}
```

### Service Registration
```csharp
builder.Services.AddHttpClient<OrderClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["API_URL"]
        ?? throw new InvalidOperationException("API_URL not configured"));
});
```

## Component Practices

### Parameters
- Use nullable reference types
- Include parameter validation
- Document with XML comments
```razor
@code {
    /// <summary>
    /// The order to display
    /// </summary>
    [Parameter]
    [EditorRequired]
    public OrderDto? Order { get; set; }

    /// <summary>
    /// Callback when order is updated
    /// </summary>
    [Parameter]
    public EventCallback<OrderDto> OnOrderUpdated { get; set; }
}
```

### Event Handling
- Use EventCallback for component events
- Include proper async handling
- Provide meaningful method names
```razor
<button @onclick="HandleOrderSubmit">Submit Order</button>

@code {
    private async Task HandleOrderSubmit()
    {
        try
        {
            await OrderService.SubmitOrderAsync(Order);
            await OnOrderUpdated.InvokeAsync(Order);
        }
        catch (Exception ex)
        {
            // Handle error
        }
    }
}
```

## State Management

### Component State
- Use cascading parameters for shared state
- Implement IDisposable when needed
- Handle state changes efficiently
```razor
@implements IDisposable

<CascadingValue Value="this">
    @ChildContent
</CascadingValue>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private void Dispose()
    {
        // Cleanup resources
    }
}
```

## Forms and Validation

### Form Handling
- Use EditForm component
- Implement custom validation
- Show validation messages
```razor
<EditForm Model="@order" OnValidSubmit="HandleValidSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <InputText @bind-Value="order.CustomerName" />
    <ValidationMessage For="@(() => order.CustomerName)" />

    <button type="submit">Save</button>
</EditForm>
```

## Performance Considerations

### Component Lifecycle
- Override ShouldRender when needed
- Use async methods appropriately
- Implement proper disposal
```razor
@code {
    protected override bool ShouldRender()
    {
        // Custom rendering logic
        return true;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }
}
```

### Data Loading
- Use cascading parameters for shared data
- Implement proper loading states
- Handle errors gracefully
```razor
@if (isLoading)
{
    <LoadingSpinner />
}
else if (error != null)
{
    <ErrorDisplay Message="@error" />
}
else
{
    <DataDisplay Items="@items" />
}