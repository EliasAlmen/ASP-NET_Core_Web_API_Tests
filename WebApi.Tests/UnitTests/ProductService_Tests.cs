using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi.Tests.UnitTests
{
    public class ProductService_Tests
    {
        private readonly Mock<IProductService> _productServiceMock;

        public ProductService_Tests()
        {
            _productServiceMock = new Mock<IProductService>();
        }

        [Fact]
        public async Task CreateAsync_Should_ReturnServiceResponseWithStatusCode201_WhenCreatedSuccessfully()
        {
            // Arrange
            var schema = new ProductSchema() { Name = "Product 1", Description = "Product 1 Description" };
            var request = new ServiceRequest<ProductSchema> { Content = schema };
            var entity = new ProductEntity() { ArticleNumber = 1, Name = "Product 1", Description = "Product 1 Description" };
            var response = new ServiceResponse<Product>
            {
                StatusCode = StatusCode.Created,
                Content = entity
            };

            _productServiceMock.Setup(x => x.CreateAsync(It.IsAny<ServiceRequest<ProductSchema>>())).ReturnsAsync(response);

            // Act
            var result = await _productServiceMock.Object.CreateAsync(request);

            // Assert
            Assert.NotNull(result.Content);
            Assert.Equal(StatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task CreateAsync_Should_ReturnServiceResponseWithStatusCode409_When_ProductAlreadyExists()
        {
            // Arrange
            var schema = new ProductSchema() { Name = "Product 1", Description = "Product 1 Description" };
            var request = new ServiceRequest<ProductSchema> { Content = schema };
            var response = new ServiceResponse<Product>
            {
                StatusCode = StatusCode.Conflict,
                Content = null
            };

            _productServiceMock.Setup(x => x.CreateAsync(It.IsAny<ServiceRequest<ProductSchema>>())).ReturnsAsync(response);

            // Act
            var result = await _productServiceMock.Object.CreateAsync(request);

            // Assert
            Assert.Null(result.Content);
            Assert.Equal(StatusCode.Conflict, result.StatusCode);
        }
    }
}
