using System.Text.Json.Serialization;

namespace BackEnd.Models.Dto;

/// <summary>
/// Data transfer object representing an order in the system
/// </summary>
public class OrderDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonPropertyName("orderDate")]
    public DateTime OrderDate { get; set; }

    [JsonPropertyName("items")]
    public List<OrderItemDto> Items { get; set; } = new();

    [JsonPropertyName("totalAmount")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
}

/// <summary>
/// Data transfer object representing an item within an order
/// </summary>
public class OrderItemDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitPrice")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("totalPrice")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public decimal TotalPrice => Quantity * UnitPrice;
}