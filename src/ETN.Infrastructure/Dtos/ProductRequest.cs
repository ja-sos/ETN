namespace ETN.Infrastructure.Dtos;

/// <summary>
/// Request DTO for retrieving products.
/// </summary>
public class ProductRequest
{
    /// <summary>
    /// Gets or sets the collection of product IDs to filter by.
    /// </summary>
    public ICollection<Guid>? Ids { get; set; }

    /// <summary>
    /// Gets or sets the collection of product names to filter by.
    /// </summary>
    public ICollection<string>? Names { get; set; }

    /// <summary>
    /// Gets or sets the minimum price to filter by.
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Gets or sets the maximum price to filter by.
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Gets or sets the collection of category IDs to filter by.
    /// </summary>
    public ICollection<Guid>? CategoryIds { get; set; }
}