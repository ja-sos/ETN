namespace ETN.Infrastructure.Dtos;

/// <summary>
/// Request DTO for retrieving categories.
/// </summary>
public class CategoryRequest
{
    /// <summary>
    /// Gets or sets the collection of category IDs to filter by.
    /// </summary>
    public ICollection<Guid>? Ids { get; set; }

    /// <summary>
    /// Gets or sets the collection of category names to filter by.
    /// </summary>
    public ICollection<string>? Names { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include categories with descriptions containing the specified text.
    /// </summary>
    public string? DescriptionContains { get; set; }
}