using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace WebApi.Models
{
    public class ProductEntity
    {
        [Key]
        public int ArticleNumber{ get; set; }
        public string? SupplierArticleNumber {  get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        public static implicit operator ProductEntity(ProductSchema product)
        {
            try
            {
                return new ProductEntity
                {
                    Name = product.Name,
                    SupplierArticleNumber = product.SupplierArticleNumber,
                    Description = product.Description,
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}
