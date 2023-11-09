using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductSchema schema)
        {
            try
            {
                var response = new ServiceResponse<Product>();

                if (!ModelState.IsValid || string.IsNullOrEmpty(schema.Name))
                {
                    return BadRequest();
                }
                else
                {
                    var request = new ServiceRequest<ProductSchema> { Content = schema };
                    response = await _productService.CreateAsync(request);

                    if (response.StatusCode == Enums.StatusCode.Conflict)
                    {
                        return Conflict();
                    }
                    return Created("", response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Problem();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _productService.GetAllAsync();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Problem();
            }
        }
    }
}
