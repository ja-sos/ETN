using System.ComponentModel.DataAnnotations;

namespace ETN.Domain.Models;

/// <summary>
/// Represents a category for products.
/// </summary>
public class Category
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    [MaxLength(10000)]
    public string Description { get; set; } = string.Empty;
}
