using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contexts;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Tests.IntegrationTests
{
    public class ProductRepository_Tests
    {
        private readonly ProductContext _context;
        private readonly IProductRepository _repository;

        public ProductRepository_Tests()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Entity_To_Database_And_Return_Entity()
        {
            // Arrange
            var entity = new ProductEntity { Name = "Test Product" };

            // Act
            var result = await _repository.CreateAsync(entity);  

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductEntity>(result);
            Assert.Equal(entity.Name, result.Name);
        }


        [Fact]
        public async Task ExistsAsync_Should_Return_True_When_Entity_Already_Exists()
        {
            // Arrange
            var entity = new ProductEntity { Name = "Test Product" };
            await _repository.CreateAsync(entity);

            // Act
            var result = await _repository.ExistsAsync(x => x.Name == entity.Name);

            // Assert
            Assert.True(result);

            await DisposeAsync();
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_When_Entity_Does_Not_Exists()
        {
            // Arrange
            var entity = new ProductEntity { Name = "Test Product" };

            // Act
            var result = await _repository.ExistsAsync(x => x.Name == entity.Name);

            // Assert
            Assert.False(result);

            await DisposeAsync();
        }

        private async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            _context.Dispose();
        }
    }
}
