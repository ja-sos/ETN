namespace ETN.Infrastructure.Dtos;

/// <summary>
/// Request DTO for creating a new product.
/// </summary>
public class ProductCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the category this product belongs to.
    /// </summary>
    public Guid CategoryId { get; set; }
}