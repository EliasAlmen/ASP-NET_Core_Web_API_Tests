using Azure.Core;
using System.Diagnostics;
using System.Linq.Expressions;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<Product>> CreateAsync(ServiceRequest<ProductSchema> request);
        Task<ServiceResponse<Product>> GetByArtricleNumberAsync(int articleNumber);
        Task<ServiceResponse<List<Product>>> GetAllAsync();
    }
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ServiceResponse<Product>> CreateAsync(ServiceRequest<ProductSchema> request)
        {
            var response = new ServiceResponse<Product>();

            try
            {
                if (request.Content != null)
                {
                    if (!await _productRepository.ExistsAsync(x => x.Name == request.Content!.Name))
                    {
                        response.Content = await _productRepository.CreateAsync(request.Content!);
                        response.StatusCode = StatusCode.Created;
                    }
                    else
                    {
                        response.StatusCode = StatusCode.Conflict;
                        response.Content = null;
                    }
                }
                else
                {
                    response.StatusCode = StatusCode.BadRequest;
                    response.Content = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.StatusCode = StatusCode.InternalServerError;
                response.Content = null;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetAllAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                StatusCode = StatusCode.Ok,
                Content = new List<Product>()
            };
            try
            {
                var result = await _productRepository.ReadAllAsync();
                foreach (var entity in result)
                {
                    response.Content.Add(entity);   
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.StatusCode = StatusCode.InternalServerError;
                response.Content = null;
            }

            return response;

        }

        public async Task<ServiceResponse<Product>> GetByArtricleNumberAsync(int articleNumber)
        {
            throw new NotImplementedException();
        }
    }
}
