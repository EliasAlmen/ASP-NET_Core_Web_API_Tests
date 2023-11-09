using WebApi.Contexts;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface IProductRepository : IRepo<ProductEntity, ProductContext>
    {
    }

    public class ProductRepository : Repo<ProductEntity, ProductContext>, IProductRepository
    {
        public ProductRepository(ProductContext dbContext) : base(dbContext)
        {
        }


    }
}
