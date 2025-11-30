using System.Net;
using ETN.Application.Contracts;
using ETN.Application.Results;
using ETN.Domain.Models;
using ETN.Infrastructure.Contracts;
using ETN.Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ETN.Infrastructure.Services;

/// <summary>
/// Implements data service operations related to products and categories.
/// </summary>
public class DataService(ILogger<DataService> logger, IDbContextFactory<EtnDbContext> dbContextFactory) : IDataService
{
    /// <inheritdoc />
    public async Task<IResult<Category>> AddCategoryAsync(CategoryCreateRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            _ = await dbContext.Categories.AddAsync(category, cancellationToken);
            _ = await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency error while adding category.");
            return new Result<Category>("Unable to add category.", statusCode: HttpStatusCode.InternalServerError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update error while adding category.");
            return new Result<Category>("Unable to add category", statusCode: HttpStatusCode.InternalServerError);
        }


        return new Result<Category>(category);
    }

    /// <inheritdoc />
    public async Task<IResult<Product>> AddProductAsync(ProductCreateRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        if (await dbContext.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken: cancellationToken) is false)
        {
            logger.LogWarning("Attempted to add product with non-existent category Id {CategoryId}", request.CategoryId);
            return new Result<Product>($"Category with Id {request.CategoryId} does not exist.", statusCode: HttpStatusCode.NotFound);
        }

        try
        {
            _ = await dbContext.Products.AddAsync(product, cancellationToken);
            _ = await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency error while adding category.");
            return new Result<Product>("Unable to add category.", statusCode: HttpStatusCode.InternalServerError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update error while adding category.");
            return new Result<Product>("Unable to add category", statusCode: HttpStatusCode.InternalServerError);
        }

        return new Result<Product>(product);
    }

    /// <inheritdoc />
    public async Task<IResult<ICollection<Category>>> GetCategoriesAsync(CategoryRequest? request, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        if (request is null)
        {
            var allCategories = await dbContext.Categories.ToListAsync(cancellationToken);
            return new Result<ICollection<Category>>(allCategories);
        }

        var categories = await dbContext.Categories
            .Where(x => (request.Ids == null || request.Ids.Contains(x.Id)) &&
            (request.Names == null || request.Names.Contains(x.Name)) &&
            (request.DescriptionContains == null || x.Description.Contains(request.DescriptionContains)))
            .ToListAsync(cancellationToken);

        return new Result<ICollection<Category>>(categories);
    }

    /// <inheritdoc />
    public async Task<IResult<ICollection<Product>>> GetProductsAsync(ProductRequest? request, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        if (request is null)
        {
            var allProducts = await dbContext.Products.ToListAsync(cancellationToken);
            return new Result<ICollection<Product>>(allProducts);
        }

        var products = await dbContext.Products
             .Where(x => (request.Ids == null || request.Ids.Contains(x.Id)) &&
             (request.Names == null || request.Names.Contains(x.Name)) &&
             (request.MinPrice == null || x.Price > request.MinPrice) &&
             (request.MaxPrice == null || x.Price < request.MaxPrice) &&
             (request.CategoryIds == null || request.CategoryIds.Contains(x.CategoryId)))
             .ToListAsync(cancellationToken);

        return new Result<ICollection<Product>>(products);
    }

    /// <inheritdoc />
    public async Task<IResult<Product>> UpdateProductAsync(ProductUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var product = await dbContext.Products.FindAsync([request.Id], cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Attempted to update non-existent product with Id {ProductId}", request.Id);
            return new Result<Product>($"Product with Id {request.Id} does not exist.", statusCode: HttpStatusCode.NotFound);
        }

        if (request.CategoryId is not null && await dbContext.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken: cancellationToken) is false)
        {
            logger.LogWarning("Attempted to update product with non-existent category Id {CategoryId}", request.CategoryId);
            return new Result<Product>($"Category with Id {request.CategoryId} does not exist.", statusCode: HttpStatusCode.NotFound);
        }

        product.Name = request.Name ?? product.Name;
        product.Price = request.Price ?? product.Price;
        product.CategoryId = request.CategoryId ?? product.CategoryId;

        try
        {
            _ = await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency error while updating product with Id {ProductId}.", request.Id);
            return new Result<Product>("Unable to update product.", statusCode: HttpStatusCode.InternalServerError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update error while updating product with Id {ProductId}.", request.Id);
            return new Result<Product>("Unable to update product.", statusCode: HttpStatusCode.InternalServerError);
        }

        return new Result<Product>(product);
    }
}
