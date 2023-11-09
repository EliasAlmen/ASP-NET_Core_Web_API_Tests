using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contexts;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;

namespace WebApi.Tests.IntegrationTests
{
    public class ProductsController_Tests
    {
        private readonly ProductContext _context;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ProductsController _controller;

        public ProductsController_Tests()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            _context = new ProductContext(options);
            _productRepository = new ProductRepository(_context);
            _productService = new ProductService(_productRepository);
            _controller = new ProductsController(_productService);
        }


        [Fact]
        public async Task Create_Should_Return_Conflict_When_Product_Already_Exists()
        {
            //Arrange
            var schema = new ProductSchema() { Name = "Prouduct 1" };
            var request = new ServiceRequest<ProductSchema> { Content = schema };
            await _productService.CreateAsync(request);

            //Act
            var result = await _controller.Create(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ConflictResult>(result);

            await DisposeAsync();
        }


        [Fact]
        public async Task Create_Should_Return_BadRequest_When_Schema_Is_Not_Valid()
        {
            //Arrange
            var schema = new ProductSchema() { };

            //Act
            var result = await _controller.Create(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);

            await DisposeAsync();

        }

        private async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            _context.Dispose();
        }
    }
}
