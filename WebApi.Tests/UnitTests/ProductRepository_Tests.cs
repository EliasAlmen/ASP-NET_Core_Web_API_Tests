using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Tests.UnitTests
{
    public class ProductRepository_Tests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public ProductRepository_Tests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task CreateAsync_Should_Return_ProductEntity_When_Created_Successfully()
        {
            // Arrange
            var entity = new ProductEntity { ArticleNumber = 1, Name = "Test", Description = "Description" };
            _productRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(entity);
            // Act
            var result = await _productRepositoryMock.Object.CreateAsync(entity);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductEntity>(result);   
            Assert.Equal(1, result.ArticleNumber);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_True_When_Entity_Already_Exists()
        {
            // Arrange
            //var entity = new ProductEntity { ArticleNumber = 1, Name = "Test", Description = "Description" };
            _productRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(true);
            // Act
            var result = await _productRepositoryMock.Object.ExistsAsync(x => x.ArticleNumber == 1);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_When_Entity_Not_Exists()
        {
            // Arrange
            //var entity = new ProductEntity { ArticleNumber = 1, Name = "Test", Description = "Description" };
            _productRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(false);
            // Act
            var result = await _productRepositoryMock.Object.ExistsAsync(x => x.ArticleNumber == 1);
            // Assert
            Assert.False(result);
        }
    }
}
