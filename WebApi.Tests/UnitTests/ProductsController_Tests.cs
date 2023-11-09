using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Tests.UnitTests
{
    public class ProductsController_Tests
    {
        private readonly Mock<IProductService> _mockproductService;
        private readonly ProductsController _controller;

        public ProductsController_Tests()
        {
            _mockproductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockproductService.Object);
        }


        [Fact]
        public async Task CreateAsync_Should_ReturnBadRequest_WhenModelStateIsNotValid()
        {
            // ARRANGE
            var schema = new ProductSchema();
            _controller.ModelState.AddModelError("Name", "Name is required");

            // ACT
            var result = await _controller.Create(schema);

            // ASSERT
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async Task CreateAsync_Should_ReturnStatusCode500_When_ErrorOccurs()
        {
            // ARRANGE
            var schema = new ProductSchema();
            var request = new ServiceRequest<ProductSchema> { Content = schema };
            var response = new ServiceResponse<Product>
            {
                StatusCode = Enums.StatusCode.Conflict,
                Content = null
            };
            _mockproductService.Setup(x => x.CreateAsync(request)).ReturnsAsync(response);

            // ACT
            var result = await _controller.Create(schema);

            // ASSERT
            Assert.IsType<ObjectResult>(result);
            var ObjectResult = result as ObjectResult;
            Assert.Equal(500, (int)ObjectResult!.StatusCode!);
        }

        //[Fact]
        //public async Task CreateAsync_Should_ReturnConflict_When_ProductAlreadyExists()
        //{
        //    // ARRANGE
        //    var schema = new ProductSchema() { Name = "p1" };
        //    var request = new ServiceRequest<ProductSchema> { Content = schema };
        //    var response = new ServiceResponse<Product>
        //    {
        //        StatusCode = Enums.StatusCode.Conflict,
        //        Content = null
        //    };
        //    _mockproductService.Setup(x => x.CreateAsync(request)).ReturnsAsync(response);

        //    // ACT
        //    var result = await _controller.Create(schema);

        //    // ASSERT
        //    Assert.IsType<ConflictResult>(result);
        //}
    }




}
