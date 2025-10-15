namespace BackEnd.Models.Validation;

/// <summary>
/// Contains validation rules for orders in the system
/// </summary>
public static class OrderValidation
{
    /// <summary>
    /// Maximum number of items allowed in a single order
    /// </summary>
    public const int MaxItemsPerOrder = 20;

    /// <summary>
    /// Minimum total amount for an order to be valid
    /// </summary>
    public const decimal MinOrderAmount = 10.0m;

    /// <summary>
    /// Maximum total amount for an order without special approval
    /// </summary>
    public const decimal MaxOrderAmount = 50000.0m;

    /// <summary>
    /// Maximum quantity of a single item that can be ordered
    /// </summary>
    public const int MaxQuantityPerItem = 100;

    /// <summary>
    /// Validates an order and returns a list of validation errors
    /// </summary>
    /// <param name="order">The order to validate</param>
    /// <returns>List of validation error messages, empty if order is valid</returns>
    public static List<string> ValidateOrder(Order order)
    {
        var errors = new List<string>();

        // Basic order validation
        if (order == null)
        {
            errors.Add("Order cannot be null");
            return errors;
        }

        // Customer information validation
        if (string.IsNullOrWhiteSpace(order.CustomerName))
        {
            errors.Add("Customer name is required");
        }
        else if (order.CustomerName.Length < 2)
        {
            errors.Add("Customer name must be at least 2 characters long");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            errors.Add("Customer email is required");
        }
        else if (!order.CustomerEmail.Contains("@"))
        {
            errors.Add("Invalid email format");
        }

        // Order items validation
        if (order.Items == null || !order.Items.Any())
        {
            errors.Add("Order must contain at least one item");
            return errors;
        }

        if (order.Items.Count > MaxItemsPerOrder)
        {
            errors.Add($"Order cannot contain more than {MaxItemsPerOrder} items");
        }

        // Validate individual items
        foreach (var item in order.Items)
        {
            if (item.Quantity <= 0)
            {
                errors.Add($"Item {item.ProductName}: Quantity must be greater than 0");
            }
            else if (item.Quantity > MaxQuantityPerItem)
            {
                errors.Add($"Item {item.ProductName}: Quantity cannot exceed {MaxQuantityPerItem}");
            }

            if (item.UnitPrice < 0)
            {
                errors.Add($"Item {item.ProductName}: Price cannot be negative");
            }
        }

        // Total amount validation
        var totalAmount = order.TotalAmount;
        if (totalAmount < MinOrderAmount)
        {
            errors.Add($"Order total amount must be at least {MinOrderAmount:C}");
        }
        if (totalAmount > MaxOrderAmount)
        {
            errors.Add($"Order total amount cannot exceed {MaxOrderAmount:C} without special approval");
        }

        // Order date validation
        if (order.OrderDate == default)
        {
            errors.Add("Order date is required");
        }
        if (order.OrderDate > DateTime.UtcNow)
        {
            errors.Add("Order date cannot be in the future");
        }

        return errors;
    }

    /// <summary>
    /// Validates order status transition
    /// </summary>
    /// <param name="currentStatus">Current order status</param>
    /// <param name="newStatus">Requested new status</param>
    /// <returns>True if transition is valid, false otherwise</returns>
    public static bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            // Valid transitions
            (OrderStatus.Pending, OrderStatus.Confirmed) => true,
            (OrderStatus.Confirmed, OrderStatus.Shipped) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
            // All other transitions are invalid
            _ => false
        };
    }
}