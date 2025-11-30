namespace ETN.Infrastructure.Dtos;

/// <summary>
/// Request DTO for creating a new category.
/// </summary>
public class CategoryCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}