using ETN.API.Controllers;
using ETN.API.Tests.Helpers;
using ETN.Domain.Models;
using ETN.Infrastructure;
using ETN.Infrastructure.Dtos;
using ETN.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ETN.API.Tests.Unit;
public class DataControllerTests
{
    private readonly Mock<IDbContextFactory<EtnDbContext>> _mockDbContextFactory;
    private readonly DataController _controller;

    public DataControllerTests()
    {
        _mockDbContextFactory = new Mock<IDbContextFactory<EtnDbContext>>();
        _controller = new DataController(new DataService(Mock.Of<ILogger<DataService>>(), _mockDbContextFactory.Object));
    }

    [Fact]
    public async Task GetProducts_ReturnsFilteredResponse()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        await using var dbFicture = ConfigureDbContextFactory(dbName);
        var category1Entry = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 1", Description = "Desc 1" });
        var category2Entry = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 2", Description = "Desc 2" });
        await dbFicture.DbContext.Products.AddRangeAsync(
        [
            new Product { Name = "Product A", Price = 10.0m, CategoryId = category1Entry.Entity.Id },
            new Product { Name = "Product B", Price = 20.0m, CategoryId = category1Entry.Entity.Id },
            new Product { Name = "Product C", Price = 30.0m, CategoryId = category2Entry.Entity.Id }
        ]);
        await dbFicture.DbContext.SaveChangesAsync();
        var filter1 = new ProductRequest
        {
            CategoryIds = [category1Entry.Entity.Id],
        };
        var filter2 = new ProductRequest
        {
            MinPrice = 15.0m,
        };
        var filter3 = new ProductRequest
        {
            Names = ["Product C"],
        };
        var filter4 = new ProductRequest
        {
            Ids = [Guid.NewGuid()],
        };
        var filter5 = new ProductRequest
        {
            MinPrice = 15.0m,
            MaxPrice = 25.0m,
        };

        // Act
        var response1 = await _controller.GetProducts(filter1);
        var response2 = await _controller.GetProducts(filter2);
        var response3 = await _controller.GetProducts(filter3);
        var response4 = await _controller.GetProducts(filter4);
        var response5 = await _controller.GetProducts(filter5);

        // Assert
        if (response1.Result is not OkObjectResult result1 || result1.Value is not ICollection<Product> products1)
        {
            Assert.Fail("Expected OkObjectResult for response1");
            return;
        }

        if (response2.Result is not OkObjectResult result2 || result2.Value is not ICollection<Product> products2)
        {
            Assert.Fail("Expected OkObjectResult for response2");
            return;
        }

        if (response3.Result is not OkObjectResult result3 || result3.Value is not ICollection<Product> products3)
        {
            Assert.Fail("Expected OkObjectResult for response3");
            return;
        }

        if (response4.Result is not OkObjectResult result4 || result4.Value is not ICollection<Product> products4)
        {
            Assert.Fail("Expected OkObjectResult for response4");
            return;
        }

        if (response5.Result is not OkObjectResult result5 || result5.Value is not ICollection<Product> products5)
        {
            Assert.Fail("Expected OkObjectResult for response5");
            return;
        }

        Assert.NotNull(result1.Value);
        Assert.NotNull(result2.Value);
        Assert.NotNull(result3.Value);
        Assert.NotNull(result4);
        Assert.NotNull(result5.Value);

        Assert.Equal(2, products1.Count);
        Assert.Equal(2, products2.Count);
        Assert.Single(products3);
        Assert.Empty(products4);
        Assert.Single(products5);

        Assert.Contains(products1, p => p.Name == "Product A");
        Assert.Contains(products1, p => p.Name == "Product B");
        Assert.Contains(products2, p => p.Name == "Product B");
        Assert.Contains(products2, p => p.Name == "Product C");
        Assert.Contains(products3, p => p.Name == "Product C");
        Assert.Contains(products5, p => p.Name == "Product B");
    }

    [Fact]
    public async Task AddProduct_CreatesNewProduct()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        await using var dbFicture = ConfigureDbContextFactory(dbName);
        var categoryEntry = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 1", Description = "Desc 1" });
        _ = await dbFicture.DbContext.SaveChangesAsync();
        var productRequest = new ProductCreateRequest
        {
            Name = "New Product",
            Price = 15.0m,
            CategoryId = categoryEntry.Entity.Id
        };

        // Act
        var response = await _controller.AddProduct(productRequest);

        // Assert
        if (response.Result is not OkObjectResult result || result.Value is not Product createdProduct)
        {
            Assert.Fail("Expected OkObjectResult");
            return;
        }

        Assert.Equal(productRequest.Name, createdProduct.Name);
        Assert.Equal(productRequest.Price, createdProduct.Price);
        Assert.Equal(productRequest.CategoryId, createdProduct.CategoryId);
        Assert.True(dbFicture.DbContext.Products.Any(p => p.Id == createdProduct.Id));
    }

    [Fact]
    public async Task AddProduct_Returns404_WhenCategoryDoesNotExits()
    {
        // Arrange
        await using var dbFicture = ConfigureDbContextFactory(Guid.NewGuid().ToString());
        var productRequest = new ProductCreateRequest
        {
            Name = "New Product",
            Price = 15.0m,
            CategoryId = Guid.NewGuid(),
        };

        // Act
        var response = await _controller.AddProduct(productRequest);

        // Assert
        if (response.Result is not NotFoundObjectResult result || result.Value is not ICollection<string> errors)
        {
            Assert.Fail("Expected NotFoundObjectResult");
            return;
        }

        Assert.Single(errors);
        Assert.Contains($"Category with Id {productRequest.CategoryId} does not exist.", errors);
    }

    [Fact]
    public async Task UpdateProduct_UpdatesExistingProduct()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        await using var dbFicture = ConfigureDbContextFactory(dbName);
        var categoryEntry1 = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 1", Description = "Desc 1" });
        var categoryEntry2 = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 2", Description = "Desc 2" });
        var productEntry = await dbFicture.DbContext.Products.AddAsync(new Product { Name = "Old Product", Price = 10.0m, CategoryId = categoryEntry1.Entity.Id });
        _ = await dbFicture.DbContext.SaveChangesAsync();
        var productRequest = new ProductUpdateRequest
        {
            Id = productEntry.Entity.Id,
            Name = "New Product",
            Price = 15.0m,
            CategoryId = categoryEntry2.Entity.Id
        };

        // Act
        var response = await _controller.UpdateProduct(productRequest);

        // Assert
        if (response.Result is not OkObjectResult result || result.Value is not Product updatedProduct)
        {
            Assert.Fail("Expected OkObjectResult");
            return;
        }

        Assert.Equal(productRequest.Name, updatedProduct.Name);
        Assert.Equal(productRequest.Price, updatedProduct.Price);
        Assert.Equal(productRequest.CategoryId, updatedProduct.CategoryId);
        Assert.True(dbFicture.DbContext.Products.Any(p => p.Id == updatedProduct.Id));
    }

    [Fact]
    public async Task UpdateProduct_Returns404_WhenProductDoesNotExits()
    {
        // Arrange
        await using var dbFicture = ConfigureDbContextFactory(Guid.NewGuid().ToString());
        var productRequest = new ProductUpdateRequest { Id = Guid.NewGuid() };

        // Act
        var response = await _controller.UpdateProduct(productRequest);

        // Assert
        if (response.Result is not NotFoundObjectResult result || result.Value is not ICollection<string> errors)
        {
            Assert.Fail("Expected NotFoundObjectResult");
            return;
        }

        Assert.Single(errors);
        Assert.Contains($"Product with Id {productRequest.Id} does not exist.", errors);
    }

    [Fact]
    public async Task GetCategories_ReturnsFilteredResponse()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        await using var dbFicture = ConfigureDbContextFactory(dbName);
        var category1Entry = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 1", Description = "Desc 1" });
        var category2Entry = await dbFicture.DbContext.Categories.AddAsync(new Category { Name = "Category 2", Description = "Desc 2" });
        await dbFicture.DbContext.SaveChangesAsync();
        var filter1 = new CategoryRequest
        {
            Names = ["Category 1"],
        };
        var filter2 = new CategoryRequest
        {
            DescriptionContains = "2",
        };
        var filter3 = new CategoryRequest
        {
            Ids = [category1Entry.Entity.Id],
        };
        var filter4 = new CategoryRequest
        {
            Ids = [Guid.NewGuid()],
        };

        // Act
        var response1 = await _controller.GetCategories(filter1);
        var response2 = await _controller.GetCategories(filter2);
        var response3 = await _controller.GetCategories(filter3);
        var response4 = await _controller.GetCategories(filter4);

        // Assert
        if (response1.Result is not OkObjectResult result1 || result1.Value is not ICollection<Category> categories1)
        {
            Assert.Fail("Expected OkObjectResult for response1");
            return;
        }

        if (response2.Result is not OkObjectResult result2 || result2.Value is not ICollection<Category> categories2)
        {
            Assert.Fail("Expected OkObjectResult for response2");
            return;
        }

        if (response3.Result is not OkObjectResult result3 || result3.Value is not ICollection<Category> categories3)
        {
            Assert.Fail("Expected OkObjectResult for response3");
            return;
        }

        if (response4.Result is not OkObjectResult result4 || result4.Value is not ICollection<Category> categories4)
        {
            Assert.Fail("Expected OkObjectResult for response4");
            return;
        }

        Assert.NotNull(result1.Value);
        Assert.NotNull(result2.Value);
        Assert.NotNull(result3.Value);
        Assert.NotNull(result4);

        Assert.Single(categories1);
        Assert.Single(categories2);
        Assert.Single(categories3);
        Assert.Empty(categories4);

        Assert.Contains(categories1, p => p.Name == "Category 1");
        Assert.Contains(categories2, p => p.Name == "Category 2");
        Assert.Contains(categories3, p => p.Name == "Category 1");
    }

    [Fact]
    public async Task AddCategory_CreatesNewCategory()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        await using var dbFicture = ConfigureDbContextFactory(dbName);
        var categoryRequest = new CategoryCreateRequest
        {
            Name = "New category",
            Description = "New description",
        };

        // Act
        var response = await _controller.AddCategory(categoryRequest);

        // Assert
        if (response.Result is not OkObjectResult result || result.Value is not Category createdCategory)
        {
            Assert.Fail("Expected OkObjectResult");
            return;
        }

        Assert.Equal(categoryRequest.Name, createdCategory.Name);
        Assert.Equal(categoryRequest.Description, createdCategory.Description);
        Assert.True(dbFicture.DbContext.Categories.Any(p => p.Id == createdCategory.Id));
    }

    private DbFixture ConfigureDbContextFactory(string dbName)
    {
        var options = new DbContextOptionsBuilder<EtnDbContext>().UseInMemoryDatabase(dbName).Options;
        _mockDbContextFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new EtnDbContext(options));
        var dbContext = new EtnDbContext(options);
        dbContext.Database.EnsureCreated();
        return new DbFixture(dbContext);
    }
}
