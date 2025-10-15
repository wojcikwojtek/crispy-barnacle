using System.Text.Json.Serialization;
using BackEnd.Models.Dto;

namespace BackEnd.Models;

/// <summary>
/// Represents an order in the system with customer information and ordered items
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier for the order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the customer who placed the order
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the customer for order notifications
    /// </summary>
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the order was placed
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Collection of items included in the order
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// Calculates the total amount of the order based on item quantities and prices
    /// </summary>
    public decimal TotalAmount => Items.Sum(item => item.Quantity * item.UnitPrice);

    /// <summary>
    /// Current status of the order in the fulfillment process
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Date and time when the order was last modified
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Converts the domain model to a DTO for API responses
    /// </summary>
    public OrderDto ToDto()
    {
        return new OrderDto
        {
            Id = Id,
            CustomerName = CustomerName,
            CustomerEmail = CustomerEmail,
            OrderDate = OrderDate,
            Items = Items.Select(item => item.ToDto()).ToList(),
            TotalAmount = TotalAmount,
            Status = Status
        };
    }
}

/// <summary>
/// Represents an individual item within an order
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Unique identifier for the order item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Reference to the product being ordered
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Name of the product at the time of ordering
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the product ordered
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price per unit at the time of ordering
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Converts the domain model to a DTO for API responses
    /// </summary>
    public OrderItemDto ToDto()
    {
        return new OrderItemDto
        {
            Id = Id,
            ProductId = ProductId,
            ProductName = ProductName,
            Quantity = Quantity,
            UnitPrice = UnitPrice
        };
    }
}

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
    Confirmed,

    /// <summary>
    /// Order has been shipped to the customer
    /// </summary>
    Shipped,

    /// <summary>
    /// Order has been delivered to the customer
    /// </summary>
    Delivered,

    /// <summary>
    /// Order has been cancelled and will not be processed
    /// </summary>
    Cancelled
}