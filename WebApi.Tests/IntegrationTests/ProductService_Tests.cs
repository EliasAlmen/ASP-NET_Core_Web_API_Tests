using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using WebApi.Contexts;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi.Tests.IntegrationTests;

public class ProductService_Tests
{
    private readonly ProductContext _context;
    private readonly IProductService _productService;
    private readonly IProductRepository _productRepository;

    public ProductService_Tests()
    {
        var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _context = new ProductContext(options);
        _productRepository = new ProductRepository(_context);
        _productService = new ProductService(_productRepository);
    }


    [Fact]
    public async Task CreateAsync_Should_Return_Service_Response_With_StatusCode_201_When_Created_Successfully()
    {
        //Arrange
        var schema = new ProductSchema() { Name = "Prouduct 1" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        
        //Act
        var result = await _productService.CreateAsync(request);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceResponse<Product>>(result);
        Assert.Equal(201, (int)result.StatusCode);
        Assert.Equal(schema.Name, result.Content!.Name);
    }


    [Fact]
    public async Task CreateAsync_Should_Return_Service_Response_With_StatusCode_409_When_Entity_Already_Exists()
    {
        //Arrange
        var schema = new ProductSchema() { Name = "Prouduct 1" };
        var request = new ServiceRequest<ProductSchema> { Content = schema };
        await _productService.CreateAsync(request);

        //Act
        var result = await _productService.CreateAsync(request);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceResponse<Product>>(result);
        Assert.Equal(409, (int)result.StatusCode);
        Assert.Null(result.Content);
    }
}
