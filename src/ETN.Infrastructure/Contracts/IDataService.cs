using ETN.Application.Contracts;
using ETN.Domain.Models;
using ETN.Infrastructure.Dtos;

namespace ETN.Infrastructure.Contracts;

/// <summary>
/// Defines the contract for data service operations related to products.
/// </summary>
public interface IDataService
{
    /// <summary>
    /// Adds a new category to the system.
    /// </summary>
    /// <param name="request">Request object containing category details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created category.</returns>
    Task<IResult<Category>> AddCategoryAsync(CategoryCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves categories based on the specified request criteria.
    /// </summary>
    /// <param name="request">Request object containing filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of categories matching the criteria.</returns>
    Task<IResult<ICollection<Category>>> GetCategoriesAsync(CategoryRequest? request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new product to the system.
    /// </summary>
    /// <param name="request">Request object containing product details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created product.</returns>
    Task<IResult<Product>> AddProductAsync(ProductCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves products based on the specified request criteria.
    /// </summary>
    /// <param name="request">Request object containing filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of products matching the criteria.</returns>
    Task<IResult<ICollection<Product>>> GetProductsAsync(ProductRequest? request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product in the system.
    /// </summary>
    /// <param name="request">Request object containing updated product details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated product.</returns>
    Task<IResult<Product>> UpdateProductAsync(ProductUpdateRequest request, CancellationToken cancellationToken = default);
}
