namespace ETN.Infrastructure.Dtos;

/// <summary>
/// Request DTO for updating an existing product.
/// </summary>
public class ProductUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the category this product belongs to.
    /// </summary>
    public Guid? CategoryId { get; set; }
}