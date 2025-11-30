using System.Net;
using ETN.Application.Contracts;
using ETN.Domain.Models;
using ETN.Infrastructure.Contracts;
using ETN.Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ETN.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController(IDataService dataService) : ControllerBase
{
    [HttpPost("products")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<Product>>> GetProducts([FromBody] ProductRequest? request)
    {
        var result = await dataService.GetProductsAsync(request);
        return ProcessResult(result);
    }

    [HttpPut("products/add")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> AddProduct([FromBody] ProductCreateRequest request)
    {
        var result = await dataService.AddProductAsync(request);
        return ProcessResult(result);
    }

    [HttpPut("products/update")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] ProductUpdateRequest request)
    {
        var result = await dataService.UpdateProductAsync(request);
        return ProcessResult(result);
    }

    [HttpPost("categories")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<Category>>> GetCategories([FromBody] CategoryRequest? request)
    {
        var result = await dataService.GetCategoriesAsync(request);
        return ProcessResult(result);
    }

    [HttpPut("categories/add")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Category>> AddCategory([FromBody] CategoryCreateRequest request)
    {
        var result = await dataService.AddCategoryAsync(request);
        return ProcessResult(result);
    }

    private ActionResult<T> ProcessResult<T>(IResult<T> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => Ok(result.Data),
            HttpStatusCode.NotFound => NotFound(result.Messages),
            _ => StatusCode((int)result.StatusCode, result.Messages)
        };
    }
}
